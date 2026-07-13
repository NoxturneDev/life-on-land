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
}
