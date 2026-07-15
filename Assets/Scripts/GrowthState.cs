public enum GrowthState
{
    Seed,       // freshly planted - small leaf
    Sprout,     // growing crop
    Sapling,    // full bush / large crop
    Young,      // small tree
    MatureTree, // full-fledged tree
    Withered
}

public static class GrowthStateExtensions
{
    // Index into TreeProfile.growthStageSprites (Withered handled separately).
    public static int StageIndex(this GrowthState s)
    {
        switch (s)
        {
            case GrowthState.Seed: return 0;
            case GrowthState.Sprout: return 1;
            case GrowthState.Sapling: return 2;
            case GrowthState.Young: return 3;
            case GrowthState.MatureTree: return 4;
            default: return 4;
        }
    }
}
