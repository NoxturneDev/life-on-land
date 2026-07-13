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

    [Header("Victory Targets & Stages")]
    public int currentLevel = 1; // Stage 1 (Red), Stage 2 (Orange), Stage 3 (Pink Bloom)
    public bool isUniqueBuildingConstructed = false;

    [Header("Stage Configs")]
    [SerializeField] private float stage1O2Goal = 18.0f;
    [SerializeField] private int stage1TreeGoal = 5;
    [SerializeField] private float stage2O2Goal = 21.0f;
    [SerializeField] private int stage2TreeGoal = 8;
    [SerializeField] private float stage3O2Goal = 21.0f;
    [SerializeField] private int stage3TreeGoal = 15;

    [Header("Disasters")]
    public bool isHeatwaveActive = false;

    [Header("Simulation Settings")]
    public float tickInterval = 5.0f; // Seconds per tick
    private float tickTimer;

    private GridWorldMatrix environmentGrid;

    public static EnvironmentManager Instance { get; private set; }

    // Getters
    public float GlobalO2Percentage => globalO2Percentage;
    public float TargetSafeO2 => GetCurrentO2Goal();
    public int AbsoluteActiveTreeCount => absoluteActiveTreeCount;
    public float GlobalSoilQualityMean => globalSoilQualityMean;
    public bool StateHabitableFlag => stateHabitableFlag;
    public GridWorldMatrix EnvironmentGrid => environmentGrid;

    void Awake()
    {
        Instance = this;
        environmentGrid = GetComponent<GridWorldMatrix>();
    }

    void Start()
    {
        RecalculateAtmosphericComposition();
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

    public float GetCurrentO2Goal()
    {
        if (currentLevel == 1) return stage1O2Goal;
        if (currentLevel == 2) return stage2O2Goal;
        return stage3O2Goal;
    }

    public int GetCurrentTreeGoal()
    {
        if (currentLevel == 1) return stage1TreeGoal;
        if (currentLevel == 2) return stage2TreeGoal;
        return stage3TreeGoal;
    }

    public string GetCurrentNPCName()
    {
        if (currentLevel == 1) return "Maliz (Bear)";
        if (currentLevel == 2) return "Oryel (Fox)";
        return "Pyper (Moth)";
    }

    // Recalculates atmospheric metrics
    public void RecalculateAtmosphericComposition()
    {
        if (environmentGrid == null) return;

        int activeTrees = 0;
        float totalO2Emitted = 0f;
        float soilQualitySum = 0f;
        int cellCount = 0;

        foreach (Vector2Int coord in environmentGrid.GetAllCoordinates())
        {
            GridCell cell = environmentGrid.GetCell(coord);
            cellCount++;
            soilQualitySum += cell.soilQuality;

            if (cell.placedObject != null && cell.placedObject is Tree)
            {
                Tree tree = (Tree)cell.placedObject;
                if (tree.CurrentFSMState != GrowthState.Withered)
                {
                    activeTrees++;

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
        globalSoilQualityMean = cellCount > 0 ? (soilQualitySum / cellCount) : 0.5f;

        // atmospheric O2 logic
        globalO2Percentage = Mathf.Min(100f, 15.0f + totalO2Emitted);
        stateHabitableFlag = (globalO2Percentage >= GetCurrentO2Goal());
    }

    // Tick execution
    [ContextMenu("Execute State Tick")]
    public void ExecuteStateTick()
    {
        if (environmentGrid == null) return;

        List<Vector2Int> coords = new List<Vector2Int>(environmentGrid.GetAllCoordinates());
        float moistureDecayAmount = isHeatwaveActive ? 0.10f : 0.05f; // Evaporation rate doubles under heatwave

        foreach (Vector2Int pos in coords)
        {
            GridCell cell = environmentGrid.GetCell(pos);

            if (cell.placedObject != null && cell.placedObject is Tree)
            {
                Tree tree = (Tree)cell.placedObject;

                if (cell.moisture <= 0f)
                {
                    // Dehydrating tick
                }
                else
                {
                    // Tree absorbs moisture
                    cell.moisture = Mathf.Max(0f, cell.moisture - 0.1f);
                    tree.Water();
                }

                tree.ProgressGrowthCycle();
                tree.InjectAtmosphericO2();
            }
            else
            {
                // Passive soil moisture evaporation
                cell.moisture = Mathf.Max(0f, cell.moisture - moistureDecayAmount);
            }
        }

        DiffuseOxygen();
        RecalculateAtmosphericComposition();

        Debug.Log($"State Tick: Level {currentLevel}, O2 = {globalO2Percentage:F2}%, Target = {GetCurrentO2Goal()}%, Trees = {absoluteActiveTreeCount}/{GetCurrentTreeGoal()}");

        if (EvaluateVictoryState())
        {
            Debug.Log("EnvironmentManager: Stage Victory conditions satisfied!");
        }
    }

    private void DiffuseOxygen()
    {
        if (environmentGrid == null) return;

        Dictionary<Vector2Int, float> nextO2 = new Dictionary<Vector2Int, float>();
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

            nextO2[pos] = sum / count;
        }

        foreach (Vector2Int pos in coords)
        {
            environmentGrid.GetCell(pos).localO2 = nextO2[pos];
        }
    }

    public void DeployLocalizedDisasterEvent(int levelMilestone)
    {
        Debug.LogWarning($"ENVIRONMENT ALERT: Disaster Event triggered!");
        
        // Heatwave (doubles moisture decay, can be set globally)
        int disasterType = Random.Range(0, 2);

        if (disasterType == 0)
        {
            isHeatwaveActive = true;
            Debug.LogWarning("DISASTER: Extreme Heatwave! Evaporation rates doubled.");
        }
        else
        {
            // Pest Infestation
            Debug.LogWarning("DISASTER: Pest Infestation! Trees damaged.");
            List<Vector2Int> coords = new List<Vector2Int>(environmentGrid.GetAllCoordinates());
            int affected = 0;
            foreach (Vector2Int pos in coords)
            {
                GridCell cell = environmentGrid.GetCell(pos);
                if (cell.placedObject != null && cell.placedObject is Tree)
                {
                    Tree tree = (Tree)cell.placedObject;
                    if (tree.CurrentFSMState != GrowthState.Withered)
                    {
                        tree.TransitionToWitheredState();
                        affected++;
                        if (affected >= 2) break;
                    }
                }
            }
        }
        RecalculateAtmosphericComposition();
    }

    public bool EvaluateVictoryState()
    {
        bool targetO2Met = globalO2Percentage >= GetCurrentO2Goal();
        bool targetTreesMet = absoluteActiveTreeCount >= GetCurrentTreeGoal();

        if (currentLevel == 3)
        {
            // Stage 3 requires the Biosphere Dome
            return targetO2Met && targetTreesMet && isUniqueBuildingConstructed;
        }
        
        return targetO2Met && targetTreesMet;
    }

    // Call this to advance stage
    public void AdvanceStage()
    {
        if (EvaluateVictoryState())
        {
            if (currentLevel < 3)
            {
                currentLevel++;
                isUniqueBuildingConstructed = false;
                Debug.Log($"EnvironmentManager: Advanced to Stage {currentLevel}!");
            }
            else
            {
                Debug.Log("EnvironmentManager: Campaign Completed!");
            }
        }
        else
        {
            Debug.LogWarning("EnvironmentManager: Cannot advance stage, goals not met yet.");
        }
    }
}
