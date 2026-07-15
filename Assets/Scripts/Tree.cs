using UnityEngine;

public class Tree : WorldObject
{
    [Header("Tree Settings")]
    [SerializeField] private string treeTypeID;
    [SerializeField] private GrowthState currentFSMState = GrowthState.Seed;
    [SerializeField] private float localO2EmissionRate;
    [SerializeField] private int ticksSinceLastWatered;
    [SerializeField] private int thresholdWaterRequirement = 3;

    // Track previous growth state before withering to restore on revive
    private GrowthState previousFSMState = GrowthState.Seed;
    private float originalO2EmissionRate;

    [Header("Visuals (State Sprites)")]
    [SerializeField] private Sprite[] stageSprites; // Seed..MatureTree by GrowthState.StageIndex()
    [SerializeField] private Sprite witheredSprite;

    private SpriteRenderer spriteRenderer;

    // Public properties
    public string TreeTypeID => treeTypeID;
    public GrowthState CurrentFSMState => currentFSMState;
    public float LocalO2EmissionRate => localO2EmissionRate;
    public int TicksSinceLastWatered => ticksSinceLastWatered;
    public int ThresholdWaterRequirement => thresholdWaterRequirement;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateVisuals();
    }

    // Initialize the tree dynamically from a Profile
    public void Initialize(TreeProfile profile)
    {
        treeTypeID = profile.treeTypeID;
        localO2EmissionRate = profile.o2EmissionRate;
        originalO2EmissionRate = profile.o2EmissionRate;
        thresholdWaterRequirement = profile.waterRequirement;
        currentFSMState = GrowthState.Seed;
        previousFSMState = GrowthState.Seed;
        ticksSinceLastWatered = 0;

        // Load sprites from profile
        stageSprites = profile.growthStageSprites;
        witheredSprite = profile.witheredSprite;

        UpdateVisuals();
    }

    // Water the tree directly to reset the neglect timer
    public void Water()
    {
        if (currentFSMState == GrowthState.Withered)
        {
            Revive();
            return;
        }
        ticksSinceLastWatered = 0;
    }

    // Revives a withered tree back to its previous growth state
    public void Revive()
    {
        if (currentFSMState == GrowthState.Withered)
        {
            currentFSMState = previousFSMState;
            localO2EmissionRate = originalO2EmissionRate;
            ticksSinceLastWatered = 0;
            UpdateVisuals();
            Debug.Log($"{ObjectID} (Type: {treeTypeID}) revived back to state: {currentFSMState}");
        }
    }

    // Progress the growth cycle (called by EnvironmentManager on state ticks)
    public void ProgressGrowthCycle()
    {
        if (currentFSMState == GrowthState.Withered) return;

        // Check neglect limits
        if (ticksSinceLastWatered >= thresholdWaterRequirement)
        {
            TransitionToWitheredState();
            return;
        }

        // Progress growth state: Seed -> Sprout -> Sapling -> Young -> MatureTree
        switch (currentFSMState)
        {
            case GrowthState.Seed:
                currentFSMState = GrowthState.Sprout;
                break;
            case GrowthState.Sprout:
                currentFSMState = GrowthState.Sapling;
                break;
            case GrowthState.Sapling:
                currentFSMState = GrowthState.Young;
                break;
            case GrowthState.Young:
                currentFSMState = GrowthState.MatureTree;
                break;
            case GrowthState.MatureTree:
                // Already fully grown
                break;
        }
        previousFSMState = currentFSMState;

        ticksSinceLastWatered++;
        UpdateVisuals();
    }

    // Emits O2 output into the atmosphere (handled by EnvironmentManager calculations)
    public void InjectAtmosphericO2()
    {
        if (currentFSMState == GrowthState.Withered) return;

        float actualO2Injected = localO2EmissionRate * GrowthO2Factor(currentFSMState);
        Debug.Log($"{ObjectID} (Type: {treeTypeID}) emitting {actualO2Injected} O2.");
    }

    // O2 output scales with growth stage (shared by EnvironmentManager's atmospheric calc).
    public static float GrowthO2Factor(GrowthState state)
    {
        switch (state)
        {
            case GrowthState.Seed: return 0.1f;
            case GrowthState.Sprout: return 0.3f;
            case GrowthState.Sapling: return 0.55f;
            case GrowthState.Young: return 0.8f;
            case GrowthState.MatureTree: return 1.0f;
            default: return 0f;
        }
    }

    public void TransitionToWitheredState()
    {
        previousFSMState = currentFSMState; // Save previous state
        currentFSMState = GrowthState.Withered;
        localO2EmissionRate = 0f;
        UpdateVisuals();
        Debug.LogWarning($"{ObjectID} (Type: {treeTypeID}) has withered.");
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null) return;

        if (currentFSMState == GrowthState.Withered)
        {
            if (witheredSprite != null) spriteRenderer.sprite = witheredSprite;
            return;
        }

        if (stageSprites != null && stageSprites.Length > 0)
        {
            int idx = Mathf.Clamp(currentFSMState.StageIndex(), 0, stageSprites.Length - 1);
            if (stageSprites[idx] != null) spriteRenderer.sprite = stageSprites[idx];
        }
    }
}
