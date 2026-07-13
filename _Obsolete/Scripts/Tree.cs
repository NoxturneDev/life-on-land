using UnityEngine;

public class Tree : WorldObject
{
    [Header("Tree Settings")]
    [SerializeField] private string treeTypeID;
    [SerializeField] private GrowthState currentFSMState = GrowthState.Seed;
    [SerializeField] private float localO2EmissionRate;
    [SerializeField] private int ticksSinceLastWatered;
    [SerializeField] private int thresholdWaterRequirement = 3;

    [Header("Visuals (State Sprites)")]
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private Sprite sproutSprite;
    [SerializeField] private Sprite matureSprite;
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

    // Call this to initialize the tree dynamically from a Profile
    public void Initialize(TreeProfile profile)
    {
        treeTypeID = profile.treeTypeID;
        localO2EmissionRate = profile.o2EmissionRate;
        thresholdWaterRequirement = profile.waterRequirement;
        currentFSMState = GrowthState.Seed;
        ticksSinceLastWatered = 0;
        UpdateVisuals();
    }

    // Call this to water the tree, resetting the timer
    public void Water()
    {
        if (currentFSMState == GrowthState.Withered) return;
        ticksSinceLastWatered = 0;
    }

    // Progress the growth cycle (typically called by EnvironmentManager on state ticks)
    public void ProgressGrowthCycle()
    {
        if (currentFSMState == GrowthState.Withered) return;

        // Check water requirement: if the tree has been neglected for too many ticks, it withers
        if (ticksSinceLastWatered >= thresholdWaterRequirement)
        {
            TransitionToWitheredState();
            return;
        }

        // Otherwise, progress growth state
        switch (currentFSMState)
        {
            case GrowthState.Seed:
                currentFSMState = GrowthState.Sprout;
                break;
            case GrowthState.Sprout:
                currentFSMState = GrowthState.MatureTree;
                break;
            case GrowthState.MatureTree:
                // Already fully grown
                break;
        }

        // Increment tick tracker
        ticksSinceLastWatered++;

        UpdateVisuals();
    }

    // Emits O2 output into the atmosphere
    public void InjectAtmosphericO2()
    {
        if (currentFSMState == GrowthState.Withered) return;

        // O2 output scale based on growth stage
        float outputFactor = 0f;
        switch (currentFSMState)
        {
            case GrowthState.Seed:
                outputFactor = 0.1f; // minimal output
                break;
            case GrowthState.Sprout:
                outputFactor = 0.5f; // half output
                break;
            case GrowthState.MatureTree:
                outputFactor = 1.0f; // full output
                break;
        }

        float actualO2Injected = localO2EmissionRate * outputFactor;
        
        Debug.Log($"{ObjectID} (Type: {treeTypeID}) injected {actualO2Injected} O2 into atmosphere.");
    }

    public void TransitionToWitheredState()
    {
        currentFSMState = GrowthState.Withered;
        localO2EmissionRate = 0f;
        UpdateVisuals();
        Debug.LogWarning($"{ObjectID} (Type: {treeTypeID}) has withered due to lack of water.");
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null) return;

        switch (currentFSMState)
        {
            case GrowthState.Seed:
                spriteRenderer.sprite = seedSprite;
                break;
            case GrowthState.Sprout:
                spriteRenderer.sprite = sproutSprite;
                break;
            case GrowthState.MatureTree:
                spriteRenderer.sprite = matureSprite;
                break;
            case GrowthState.Withered:
                spriteRenderer.sprite = witheredSprite;
                break;
        }
    }
}
