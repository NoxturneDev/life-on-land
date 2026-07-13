using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridWorldMatrix))]
public class EnvironmentManager : MonoBehaviour
{
    [Header("Atmospheric Composition")]
    [SerializeField] private float globalO2Percentage = 15.0f;
    [SerializeField] private float targetSafeO2 = 21.0f;
    [SerializeField] private int absoluteActiveTreeCount;
    [SerializeField] private float globalSoilQualityMean = 0.5f;
    [SerializeField] private bool stateHabitableFlag;

    [Header("Victory Targets")]
    public int winTreeCount = 1000;
    public bool isUniqueBuildingConstructed = false;

    [Header("Simulation settings")]
    public float tickInterval = 5.0f; // Seconds per state tick
    private float tickTimer;

    private GridWorldMatrix environmentGrid;

    public static EnvironmentManager Instance { get; private set; }

    // Getters for public systems
    public float GlobalO2Percentage => globalO2Percentage;
    public float TargetSafeO2 => targetSafeO2;
    public int AbsoluteActiveTreeCount => absoluteActiveTreeCount;
    public float GlobalSoilQualityMean => globalSoilQualityMean;
    public bool StateHabitableFlag => stateHabitableFlag;
    public GridWorldMatrix EnvironmentGrid => environmentGrid;

    void Awake()
    {
        Instance = this;
        environmentGrid = GetComponent<GridWorldMatrix>();
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickInterval)
            {
                tickTimer = 0f;
                ExecuteStateTick();
            }
        }
    }

    // Recalculates global environmental statistics based on active grid cell items
    public void RecalculateAtmosphericComposition()
    {
        int activeTrees = 0;
        float totalO2Emitted = 0f;
        float soilQualitySum = 0f;
        int populatedCells = 0;

        foreach (Vector2Int coord in environmentGrid.GetAllCoordinates())
        {
            GridCell cell = environmentGrid.GetCell(coord);
            populatedCells++;
            soilQualitySum += cell.soilQuality;

            if (cell.placedObject != null && cell.placedObject is Tree)
            {
                Tree tree = (Tree)cell.placedObject;
                if (tree.CurrentFSMState != GrowthState.Withered)
                {
                    activeTrees++;

                    // Scale emission rates based on growth state
                    float outputFactor = 0f;
                    switch (tree.CurrentFSMState)
                    {
                        case GrowthState.Seed:
                            outputFactor = 0.1f;
                            break;
                        case GrowthState.Sprout:
                            outputFactor = 0.5f;
                            break;
                        case GrowthState.MatureTree:
                            outputFactor = 1.0f;
                            break;
                    }
                    totalO2Emitted += tree.LocalO2EmissionRate * outputFactor;
                }
            }
        }

        absoluteActiveTreeCount = activeTrees;
        globalSoilQualityMean = populatedCells > 0 ? (soilQualitySum / populatedCells) : 0.5f;

        // Apply emissions to global atmospheric oxygen (starts at low threshold 15%, maxes at 100%)
        globalO2Percentage = Mathf.Min(100f, 15.0f + totalO2Emitted);

        stateHabitableFlag = (globalO2Percentage >= targetSafeO2);
    }

    // Executed every tick to progress growth cycles and evaporate local tile moisture
    [ContextMenu("Execute State Tick")]
    public void ExecuteStateTick()
    {
        List<Vector2Int> coords = new List<Vector2Int>(environmentGrid.GetAllCoordinates());

        foreach (Vector2Int pos in coords)
        {
            GridCell cell = environmentGrid.GetCell(pos);

            if (cell.placedObject != null && cell.placedObject is Tree)
            {
                Tree tree = (Tree)cell.placedObject;

                if (cell.moisture <= 0f)
                {
                    // Tree receives no water, ticksSinceLastWatered increments internally
                }
                else
                {
                    // Tree drinks water and resets water ticks
                    cell.moisture = Mathf.Max(0f, cell.moisture - 0.1f);
                    tree.Water();
                }

                tree.ProgressGrowthCycle();
                tree.InjectAtmosphericO2();
            }
            else
            {
                // Normal cell evaporation
                cell.moisture = Mathf.Max(0f, cell.moisture - 0.02f);
            }
        }

        // Run local atmospheric oxygen diffusion pass
        DiffuseOxygen();

        // Update global O2 and averages
        RecalculateAtmosphericComposition();

        // Print details to console
        Debug.Log($"State Tick Executed: Global O2 = {globalO2Percentage:F2}%, Active Trees = {absoluteActiveTreeCount}");
    }

    // Distributes oxygen between adjacent coordinates (diffusion simulation)
    private void DiffuseOxygen()
    {
        Dictionary<Vector2Int, float> nextO2Values = new Dictionary<Vector2Int, float>();
        List<Vector2Int> coords = new List<Vector2Int>(environmentGrid.GetAllCoordinates());

        foreach (Vector2Int pos in coords)
        {
            GridCell cell = environmentGrid.GetCell(pos);
            float sum = cell.localO2;
            int count = 1;

            Vector2Int[] neighbors = {
                pos + Vector2Int.up,
                pos + Vector2Int.down,
                pos + Vector2Int.left,
                pos + Vector2Int.right
            };

            foreach (Vector2Int n in neighbors)
            {
                if (environmentGrid.HasCell(n))
                {
                    sum += environmentGrid.GetCell(n).localO2;
                    count++;
                }
            }

            nextO2Values[pos] = sum / count;
        }

        // Apply calculation back to grid cells
        foreach (Vector2Int pos in coords)
        {
            environmentGrid.GetCell(pos).localO2 = nextO2Values[pos];
        }
    }

    // Spawns environmental disasters that threaten tree health or evaporate moisture
    public void DeployLocalizedDisasterEvent(int levelMilestone)
    {
        Debug.LogWarning($"ENVIRONMENT ALERT: Disaster Event Deployed for Level Milestone {levelMilestone}!");
        List<Vector2Int> coords = new List<Vector2Int>(environmentGrid.GetAllCoordinates());
        if (coords.Count == 0) return;

        // Choose a random disaster: 0 = Heatwave, 1 = Pests
        int disasterType = Random.Range(0, 2);

        if (disasterType == 0)
        {
            Debug.LogWarning("DISASTER: Extreme Heatwave! Soil moisture dropped by 50% globally.");
            foreach (Vector2Int pos in coords)
            {
                GridCell cell = environmentGrid.GetCell(pos);
                cell.moisture = Mathf.Max(0f, cell.moisture - 0.5f);
            }
        }
        else
        {
            Debug.LogWarning("DISASTER: Pest Infestation! Random active trees withered.");
            int affectedLimit = Mathf.Min(coords.Count, 3);
            for (int i = 0; i < affectedLimit; i++)
            {
                Vector2Int randomPos = coords[Random.Range(0, coords.Count)];
                GridCell cell = environmentGrid.GetCell(randomPos);
                if (cell.placedObject != null && cell.placedObject is Tree)
                {
                    Tree tree = (Tree)cell.placedObject;
                    tree.TransitionToWitheredState();
                }
            }
        }

        RecalculateAtmosphericComposition();
    }

    // Evaluates victory conditions
    public bool EvaluateVictoryState()
    {
        return (absoluteActiveTreeCount >= winTreeCount && 
                globalO2Percentage >= targetSafeO2 && 
                isUniqueBuildingConstructed);
    }
}
