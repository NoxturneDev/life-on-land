using UnityEngine;

[CreateAssetMenu(fileName = "NewTreeProfile", menuName = "LifeOnLand/TreeProfile")]
public class TreeProfile : ScriptableObject
{
    public string treeTypeID;
    public float o2EmissionRate = 0.1f;
    public float moistureContribution = 0.5f;
    public int waterRequirement = 3; // Max ticks allowed without watering before withering
    public float growthTimePerStage = 10f; // Realtime seconds or ticks per growth stage
    public GameObject prefab;

    [Header("Sprites")]
    // Ordered growth-stage sprites: [0]=Seed leaf, [1]=Sprout, [2]=Sapling bush,
    // [3]=Young tree, [4]=Mature tree. Grown from small leaf into a full tree.
    public Sprite[] growthStageSprites;
    public Sprite witheredSprite;

    // --- legacy single-sprite fields (kept so old asset data still deserializes) ---
    public Sprite seedSprite;
    public Sprite sproutSprite;
    public Sprite matureSprite;
}

