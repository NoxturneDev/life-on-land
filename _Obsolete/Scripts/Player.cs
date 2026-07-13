using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Player Details")]
    [SerializeField] private string playerName = "Restorer";
    [SerializeField] private float baseMovementSpeed = 5f;
    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private float localO2Buffer = 10f;
    [SerializeField] private int currentLevel = 1;

    [Header("Inventory")]
    [SerializeField] private int currentWaterInventory = 0;
    [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();

    private PlayerController playerController;

    // Public properties
    public string PlayerName => playerName;
    public float CurrentStamina => currentStamina;
    public float LocalO2Buffer => localO2Buffer;
    public int CurrentLevel => currentLevel;
    public int CurrentWaterInventory => currentWaterInventory;
    public List<InventoryItem> Inventory => inventory;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            baseMovementSpeed = playerController.moveSpeed;
        }
    }

    void Update()
    {
        EvaluateCalculatedDebuffs();
    }

    // Required by the GEMINI specifications
    public void ProcessMovementInput(float horizontal, float vertical)
    {
        // Directional inputs are read automatically by PlayerController.cs, 
        // but this method is reserved for manual/alternative control hooks.
    }

    // Plants a seed at the target coordinates, setting up the new Tree on the Grid
    public void ExecutePlantAction(TreeProfile profile, Vector2 targetGridCoordinates)
    {
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(targetGridCoordinates.x), Mathf.RoundToInt(targetGridCoordinates.y));

        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            // Prevent planting on occupied tiles
            if (EnvironmentManager.Instance.EnvironmentGrid.GetPlacedObject(gridPos) != null)
            {
                Debug.LogWarning($"Player: Grid cell {gridPos} is already occupied!");
                return;
            }

            // Check if player has the matching seed in inventory
            bool hasSeed = false;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == profile.treeTypeID && inventory[i].quantity > 0)
                {
                    hasSeed = true;
                    // Deduct 1 seed
                    InventoryItem item = inventory[i];
                    item.quantity--;
                    inventory[i] = item;
                    break;
                }
            }

            if (!hasSeed)
            {
                Debug.LogWarning($"Player: Lacks seed in inventory to plant: {profile.treeTypeID}");
                return;
            }

            // Instantiate tree visual GameObject
            Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
            GameObject treeGO;
            if (profile.prefab != null)
            {
                treeGO = Instantiate(profile.prefab, worldPos, Quaternion.identity);
            }
            else
            {
                // Fallback for visual prototyping
                treeGO = new GameObject(profile.treeTypeID);
                treeGO.transform.position = worldPos;
                treeGO.AddComponent<SpriteRenderer>();
            }

            Tree tree = treeGO.GetComponent<Tree>();
            if (tree == null)
            {
                tree = treeGO.AddComponent<Tree>();
            }

            // Setup tree parameters
            tree.Initialize(profile);
            tree.ObjectID = profile.treeTypeID + "_" + System.Guid.NewGuid().ToString().Substring(0, 4);

            // Set coordinates and register to the environment grid
            EnvironmentManager.Instance.EnvironmentGrid.SetPlacedObject(gridPos, tree);
            Debug.Log($"Player: Successfully planted {profile.treeTypeID} at grid {gridPos}");

            // Recalculate planetary O2
            EnvironmentManager.Instance.RecalculateAtmosphericComposition();
        }
    }

    // Draws water from a natural source node, adding it to inventory
    public void ExtractWater(WaterSourceNode source)
    {
        if (source == null) return;

        int capacityLimit = 20; // max water per fill action
        int drawn = source.ExtractWater(capacityLimit);
        currentWaterInventory += drawn;

        Debug.Log($"Player: Extracted {drawn} water units from {source.SourceName}. Current Water: {currentWaterInventory}");
    }

    // Constructs infrastructure on the grid using resources
    public void ConstructInfrastructure(BuildingBlueprint blueprint, Vector2 originCoordinates)
    {
        if (currentWaterInventory < blueprint.waterCost)
        {
            Debug.LogWarning($"Player: Cannot construct {blueprint.buildingName}. Requires {blueprint.waterCost} water.");
            return;
        }

        // Deduct water cost
        currentWaterInventory -= blueprint.waterCost;

        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(originCoordinates.x), Mathf.RoundToInt(originCoordinates.y));
        Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);

        GameObject buildingGO;
        if (blueprint.prefab != null)
        {
            buildingGO = Instantiate(blueprint.prefab, worldPos, Quaternion.identity);
        }
        else
        {
            // Fallback empty object
            buildingGO = new GameObject(blueprint.buildingName);
            buildingGO.transform.position = worldPos;
        }

        WorldObject worldObject = buildingGO.GetComponent<WorldObject>();
        if (worldObject == null)
        {
            worldObject = buildingGO.AddComponent<WorldObject>();
        }
        worldObject.GridCoordinates = gridPos;

        // Register coordinates in Grid Matrix
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            EnvironmentManager.Instance.EnvironmentGrid.SetPlacedObject(gridPos, worldObject);
        }

        Debug.Log($"Player: Constructed {blueprint.buildingName} at grid coordinate {gridPos}");
    }

    // Evaluates speed debuffs based on localized coordinate O2 levels
    private void EvaluateCalculatedDebuffs()
    {
        if (playerController == null) return;

        // Map player transform coords (y is inverted on the grid)
        Vector2Int playerGridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(-transform.position.y)
        );

        float localO2 = 15.0f; // starting baseline O2

        // Retrieve local oxygen percentage from environmental matrix
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            if (EnvironmentManager.Instance.EnvironmentGrid.HasCell(playerGridPos))
            {
                localO2 = EnvironmentManager.Instance.EnvironmentGrid.GetCell(playerGridPos).localO2;
            }
        }

        // Calculate debuff factor: MovementSpeed = BaseSpeed * (Min(1.0, LocalO2 / TargetSafeO2))
        float targetO2 = EnvironmentManager.Instance != null ? EnvironmentManager.Instance.TargetSafeO2 : 21.0f;
        float o2Factor = Mathf.Min(1.0f, localO2 / targetO2);

        // Adjust the speed on the physical movement controller
        playerController.moveSpeed = baseMovementSpeed * o2Factor;

        // Stamina and air buffer decays if oxygen levels are below target threshold
        if (o2Factor < 1.0f)
        {
            currentStamina = Mathf.Max(0f, currentStamina - (Time.deltaTime * (1.0f - o2Factor) * 2f));
            localO2Buffer = Mathf.Max(0f, localO2Buffer - Time.deltaTime);
        }
        else
        {
            // Recover stamina and buffer when in a breathing zone
            currentStamina = Mathf.Min(100f, currentStamina + Time.deltaTime * 5f);
            localO2Buffer = Mathf.Min(10f, localO2Buffer + Time.deltaTime);
        }
    }
}
