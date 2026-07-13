using UnityEngine;

public class WaterSourceNode : MonoBehaviour
{
    [SerializeField] private string sourceName = "Water Pond";
    [SerializeField] private int maxWaterCapacity = 100;
    [SerializeField] private int currentWaterAvailable = 100;

    public string SourceName => sourceName;
    public int CurrentWaterAvailable => currentWaterAvailable;

    // Withdraws water from the source, capped by availability
    public int ExtractWater(int requestedAmount)
    {
        int extracted = Mathf.Min(requestedAmount, currentWaterAvailable);
        currentWaterAvailable -= extracted;
        return extracted;
    }

    // Replenishes pond reserves (e.g. from rain or passive regeneration)
    public void Refill(int amount)
    {
        currentWaterAvailable = Mathf.Min(maxWaterCapacity, currentWaterAvailable + amount);
    }
}
