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
    [SerializeField] private int activeHotbarSlot = 0; // 0 to 5

    [Header("Item ID Identifiers")]
    public string rationItemID = "ration";
    public string purifiedWaterItemID = "purified_water";

    [Header("Profiles & Blueprints")]
    [SerializeField] private TreeProfile desertShrubProfile;  // Slot 3 (Type B)
    [SerializeField] private TreeProfile pineTreeProfile;     // Slot 4 (Type A)
    [SerializeField] private TreeProfile silkmothFernProfile; // Slot 5 (Type C)
    [SerializeField] private BuildingBlueprint soilPurifierBlueprint; // Slot 6 - Sub-item 0
    [SerializeField] private BuildingBlueprint irrigationPipesBlueprint; // Slot 6 - Sub-item 1
    [SerializeField] private BuildingBlueprint biosphereDomeBlueprint; // Slot 6 - Sub-item 2

    private PlayerController playerController;

    // Public properties
    public string PlayerName => playerName;
    public float CurrentStamina => currentStamina;
    public float LocalO2Buffer => localO2Buffer;
    public int CurrentLevel => currentLevel;
    public int CurrentWaterInventory => currentWaterInventory;
    public int ActiveHotbarSlot => activeHotbarSlot;
    public List<InventoryItem> Inventory => inventory;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            baseMovementSpeed = playerController.moveSpeed;
        }
        
        // Populate starting inventory if empty. Each stage grants one seed type
        // (10 seeds) plus food; Stage 1 is Desert Shrub.
        if (inventory.Count == 0)
        {
            inventory.Add(new InventoryItem(rationItemID, 10));       // Food
            inventory.Add(new InventoryItem(purifiedWaterItemID, 5));
            inventory.Add(new InventoryItem("desert_shrub", 10));     // Stage 1 seed
        }
    }

    void Update()
    {
        bool useToolPressed = false;
        int targetSlot = -1;
        bool useRations = false;
        bool useWater = false;
        Vector3 mousePosition = Vector3.zero;

        #if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (kb != null)
        {
            if (kb.digit1Key.wasPressedThisFrame) targetSlot = 0;
            else if (kb.digit2Key.wasPressedThisFrame) targetSlot = 1;
            else if (kb.digit3Key.wasPressedThisFrame) targetSlot = 2;
            else if (kb.digit4Key.wasPressedThisFrame) targetSlot = 3;
            else if (kb.digit5Key.wasPressedThisFrame) targetSlot = 4;
            else if (kb.digit6Key.wasPressedThisFrame) targetSlot = 5;

            if (kb.digit7Key.wasPressedThisFrame) useRations = true;
            if (kb.digit8Key.wasPressedThisFrame) useWater = true;

            if (kb.spaceKey.wasPressedThisFrame) useToolPressed = true;
        }
        else
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) targetSlot = 0;
                else if (Input.GetKeyDown(KeyCode.Alpha2)) targetSlot = 1;
                else if (Input.GetKeyDown(KeyCode.Alpha3)) targetSlot = 2;
                else if (Input.GetKeyDown(KeyCode.Alpha4)) targetSlot = 3;
                else if (Input.GetKeyDown(KeyCode.Alpha5)) targetSlot = 4;
                else if (Input.GetKeyDown(KeyCode.Alpha6)) targetSlot = 5;

                if (Input.GetKeyDown(KeyCode.Alpha7)) useRations = true;
                if (Input.GetKeyDown(KeyCode.Alpha8)) useWater = true;

                if (Input.GetKeyDown(KeyCode.Space)) useToolPressed = true;
            }
            catch (System.Exception) { }
        }
        if (mouse != null)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                useToolPressed = true;
            }
            Vector2 mousePos = mouse.position.ReadValue();
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));
        }
        else
        {
            try
            {
                if (Input.GetMouseButtonDown(0))
                {
                    useToolPressed = true;
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            catch (System.Exception) { }
        }
        #else
        if (Input.GetKeyDown(KeyCode.Alpha1)) targetSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) targetSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) targetSlot = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) targetSlot = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) targetSlot = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) targetSlot = 5;

        if (Input.GetKeyDown(KeyCode.Alpha7)) useRations = true;
        if (Input.GetKeyDown(KeyCode.Alpha8)) useWater = true;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            useToolPressed = true;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        #endif

        if (targetSlot != -1) SelectHotbarSlot(targetSlot);
        if (useRations) ConsumeItem(rationItemID);
        if (useWater) ConsumeItem(purifiedWaterItemID);

        if (useToolPressed)
        {
            UseActiveTool(new Vector2(mousePosition.x, mousePosition.y));
        }

        EvaluateCalculatedDebuffs();
    }

    public void ProcessMovementInput(float horizontal, float vertical)
    {
        // Directional inputs are handled automatically by PlayerController.cs
    }

    public void SelectHotbarSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < 6)
        {
            activeHotbarSlot = slotIndex;
            Debug.Log($"Player: Selected Hotbar Slot {activeHotbarSlot + 1}");
        }
    }

    public void UseActiveTool(Vector2 targetGridCoordinates)
    {
        Vector2Int gridPos = GridUtil.WorldToGrid(targetGridCoordinates);
        
        switch (activeHotbarSlot)
        {
            case 0: // Slot 1: Shovel/Hoe
                ExecuteShovelAction(gridPos);
                break;
            case 1: // Slot 2: Watering Can
                ExecuteWateringAction(gridPos);
                break;
            case 2: // Slot 3: Food (eat a ration to restore stamina)
                ConsumeItem(rationItemID);
                break;
            case 3: // Slot 4: Seed (current stage's plant)
                if (CurrentStageSeedProfile != null) ExecutePlantAction(CurrentStageSeedProfile, targetGridCoordinates);
                break;
            case 4: // Slot 5: Infrastructure Blueprints
                BuildingBlueprint activeBlueprint = GetActiveBuildingBlueprint();
                if (activeBlueprint != null) ConstructInfrastructure(activeBlueprint, targetGridCoordinates);
                break;
            case 5: // Slot 6: reserved
                break;
        }
    }

    private void ExecuteShovelAction(Vector2Int gridPos)
    {
        if (currentStamina < 5f)
        {
            Debug.LogWarning("Player: Too exhausted to shovel!");
            return;
        }

        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            GridWorldMatrix gridMatrix = EnvironmentManager.Instance.EnvironmentGrid;
            if (gridMatrix.PurifyTileShovel(gridPos))
            {
                ConsumeStamina(5f);
                NotificationManager.Instance?.Show("Tile Cleared - Water it next");
            }
            else
            {
                Debug.Log("Player: Tile is not corrupted or cannot be shoveled.");
            }
        }
    }

    [Header("Water Source")]
    [SerializeField] private int maxWaterInventory = 20;
    [SerializeField] private int waterDrawnPerScoop = 5;

    private void ExecuteWateringAction(Vector2Int gridPos)
    {
        if (EnvironmentManager.Instance == null || EnvironmentManager.Instance.EnvironmentGrid == null) return;

        GridWorldMatrix gridMatrix = EnvironmentManager.Instance.EnvironmentGrid;
        GridCell cell = gridMatrix.GetCell(gridPos);

        // Case 0: Target is the pond - fill the watering can instead of consuming it
        if (cell.isWaterSource)
        {
            if (currentWaterInventory >= maxWaterInventory)
            {
                Debug.Log("Player: Watering can is already full.");
                NotificationManager.Instance?.Show("Watering can is full");
                return;
            }
            int drawn = Mathf.Min(waterDrawnPerScoop, maxWaterInventory - currentWaterInventory);
            currentWaterInventory += drawn;
            Debug.Log($"Player: Drew {drawn} water from the pond. Current: {currentWaterInventory}/{maxWaterInventory}");
            NotificationManager.Instance?.Show($"+{drawn} Water");
            return;
        }

        if (currentWaterInventory <= 0)
        {
            Debug.LogWarning("Player: Watering can is empty!");
            NotificationManager.Instance?.Show("Watering can is empty");
            return;
        }

        // Case A: Purifying DugBurnt soil to Normal
        if (cell.corruptionState == 2)
        {
            if (gridMatrix.PurifyTileWater(gridPos))
            {
                currentWaterInventory--;
                NotificationManager.Instance?.Show("Tile Purified!");
            }
            return;
        }

        // Case B: Soil is clean, check for placed tree/object
        if (cell.placedObject != null && cell.placedObject is Tree)
        {
            Tree tree = (Tree)cell.placedObject;
            tree.Water();
            currentWaterInventory--;
            Debug.Log($"Player: Watered tree at {gridPos}.");
            NotificationManager.Instance?.Show("Tree Watered");
        }
        else
        {
            // Case C: Water soil directly
            cell.moisture = 1.0f;
            currentWaterInventory--;
            TerrainVisualManager.Instance?.OnCellWatered(gridPos);
            Debug.Log($"Player: Watered soil directly at {gridPos}.");
        }
    }

    public void ExecutePlantAction(TreeProfile profile, Vector2 targetGridCoordinates)
    {
        if (currentStamina < 10f)
        {
            Debug.LogWarning("Player: Too exhausted to plant!");
            return;
        }

        Vector2Int gridPos = GridUtil.WorldToGrid(targetGridCoordinates);

        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            GridWorldMatrix gridMatrix = EnvironmentManager.Instance.EnvironmentGrid;
            GridCell cell = gridMatrix.GetCell(gridPos);

            // Cannot plant on occupied or corrupted tiles
            if (cell.placedObject != null)
            {
                Debug.LogWarning($"Player: Grid cell {gridPos} is occupied.");
                return;
            }

            if (cell.corruptionState != 0)
            {
                Debug.LogWarning($"Player: Grid cell {gridPos} must be purified before planting.");
                NotificationManager.Instance?.Show("Purify this tile first");
                return;
            }

            // Seeds only take in watered soil.
            if (cell.moisture <= 0f)
            {
                Debug.LogWarning($"Player: Grid cell {gridPos} is too dry to plant.");
                NotificationManager.Instance?.Show("Water the soil first");
                return;
            }

            // Verify inventory has the matching seed
            bool hasSeed = false;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == profile.treeTypeID && inventory[i].quantity > 0)
                {
                    hasSeed = true;
                    InventoryItem item = inventory[i];
                    item.quantity--;
                    inventory[i] = item;
                    break;
                }
            }

            if (!hasSeed)
            {
                Debug.LogWarning($"Player: No seeds of type {profile.treeTypeID} in inventory.");
                return;
            }

            // Spawn Tree at the tile centre so it aligns with the tile the player targeted.
            Vector3 worldPos = GridUtil.GridToWorldCenter(gridPos);
            GameObject treeGO = profile.prefab != null ? Instantiate(profile.prefab, worldPos, Quaternion.identity) : new GameObject(profile.treeTypeID);
            treeGO.transform.position = worldPos;
            if (profile.prefab == null) treeGO.AddComponent<SpriteRenderer>();

            Tree tree = treeGO.GetComponent<Tree>() ?? treeGO.AddComponent<Tree>();
            tree.Initialize(profile);
            tree.ObjectID = profile.treeTypeID + "_" + System.Guid.NewGuid().ToString().Substring(0, 4);

            gridMatrix.SetPlacedObject(gridPos, tree);
            ConsumeStamina(10f);

            Debug.Log($"Player: Planted {profile.treeTypeID} at {gridPos}.");
            NotificationManager.Instance?.Show($"Planted {profile.treeTypeID.Replace('_', ' ')}");
            EnvironmentManager.Instance.RecalculateAtmosphericComposition();
        }
    }

    public void ExtractWater(WaterSourceNode source)
    {
        if (source == null) return;

        int capacityLimit = 20; // fill limit
        int drawn = source.ExtractWater(capacityLimit);
        currentWaterInventory += drawn;

        Debug.Log($"Player: Extracted {drawn} water. Current Inventory: {currentWaterInventory}");
    }

    public void ConstructInfrastructure(BuildingBlueprint blueprint, Vector2 originCoordinates)
    {
        if (currentWaterInventory < blueprint.waterCost)
        {
            Debug.LogWarning($"Player: Insufficient water. Requires {blueprint.waterCost}.");
            return;
        }

        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(originCoordinates.x), Mathf.RoundToInt(-originCoordinates.y));
        
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            GridWorldMatrix gridMatrix = EnvironmentManager.Instance.EnvironmentGrid;
            
            if (gridMatrix.GetPlacedObject(gridPos) != null)
            {
                Debug.LogWarning($"Player: Coordinate {gridPos} is occupied.");
                return;
            }

            currentWaterInventory -= blueprint.waterCost;

            Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
            GameObject buildingGO = blueprint.prefab != null ? Instantiate(blueprint.prefab, worldPos, Quaternion.identity) : new GameObject(blueprint.buildingName);
            buildingGO.transform.position = worldPos;

            WorldObject worldObject = buildingGO.GetComponent<WorldObject>() ?? buildingGO.AddComponent<WorldObject>();
            worldObject.GridCoordinates = gridPos;
            worldObject.ObjectID = blueprint.buildingName + "_" + System.Guid.NewGuid().ToString().Substring(0, 4);

            gridMatrix.SetPlacedObject(gridPos, worldObject);
            Debug.Log($"Player: Constructed {blueprint.buildingName} at {gridPos}.");

            if (blueprint.buildingName.ToLower().Contains("dome"))
            {
                EnvironmentManager.Instance.isUniqueBuildingConstructed = true;
            }
        }
    }

    private void EvaluateCalculatedDebuffs()
    {
        if (playerController == null) return;

        Vector2Int playerGridPos = GridUtil.WorldToGrid(transform.position);

        float localO2 = 15.0f; // starting baseline

        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            if (EnvironmentManager.Instance.EnvironmentGrid.HasCell(playerGridPos))
            {
                localO2 = EnvironmentManager.Instance.EnvironmentGrid.GetCell(playerGridPos).localO2;
            }
        }

        float targetO2 = EnvironmentManager.Instance != null ? EnvironmentManager.Instance.TargetSafeO2 : 21.0f;
        float o2Factor = Mathf.Min(1.0f, localO2 / targetO2);

        // Modify player speed based on local oxygen level
        playerController.moveSpeed = baseMovementSpeed * o2Factor;

        // Stamina and O2 buffer depletion/replenishment
        if (o2Factor < 1.0f)
        {
            currentStamina = Mathf.Max(0f, currentStamina - (Time.deltaTime * (1.0f - o2Factor) * 2f));
            localO2Buffer = Mathf.Max(0f, localO2Buffer - Time.deltaTime);
        }
        else
        {
            currentStamina = Mathf.Min(100f, currentStamina + Time.deltaTime * 5f);
            localO2Buffer = Mathf.Min(10f, localO2Buffer + Time.deltaTime);
        }
    }

    public bool HasEnoughStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
    }

    public void AddWater(int amount)
    {
        currentWaterInventory += amount;
    }

    public void AddSeeds(string seedID, int amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == seedID)
            {
                InventoryItem item = inventory[i];
                item.quantity += amount;
                inventory[i] = item;
                Debug.Log($"Player: Added {amount} of {seedID} seeds. Total: {item.quantity}");
                return;
            }
        }
        inventory.Add(new InventoryItem(seedID, amount));
        Debug.Log($"Player: Added new seed {seedID} x{amount} to inventory.");
    }

    private void ConsumeItem(string id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == id && inventory[i].quantity > 0)
            {
                InventoryItem item = inventory[i];
                item.quantity--;
                inventory[i] = item;

                if (id == rationItemID)
                {
                    currentStamina = Mathf.Min(100f, currentStamina + 30f);
                    Debug.Log("Player: Consumed Ration. Restored 30 Stamina.");
                }
                else if (id == purifiedWaterItemID)
                {
                    localO2Buffer = Mathf.Min(10f, localO2Buffer + 5f);
                    currentStamina = Mathf.Min(100f, currentStamina + 10f);
                    Debug.Log("Player: Consumed Purified Water. Restored O2 buffer.");
                }
                return;
            }
        }
        Debug.LogWarning($"Player: No consumable of type {id} available.");
    }

    private BuildingBlueprint GetActiveBuildingBlueprint()
    {
        // Simple mock rotation: choose based on level/need
        if (EnvironmentManager.Instance != null)
        {
            int stage = EnvironmentManager.Instance.currentLevel;
            if (stage == 1) return soilPurifierBlueprint;
            if (stage == 2) return irrigationPipesBlueprint;
            return biosphereDomeBlueprint;
        }
        return soilPurifierBlueprint;
    }

    // The single seed granted for the current stage (Stage 1 = Desert Shrub).
    public TreeProfile CurrentStageSeedProfile
    {
        get
        {
            int stage = EnvironmentManager.Instance != null ? EnvironmentManager.Instance.currentLevel : 1;
            if (stage == 2) return pineTreeProfile;
            if (stage >= 3) return silkmothFernProfile;
            return desertShrubProfile;
        }
    }
}
