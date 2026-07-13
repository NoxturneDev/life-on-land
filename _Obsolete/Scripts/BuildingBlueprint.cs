using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingBlueprint", menuName = "LifeOnLand/BuildingBlueprint")]
public class BuildingBlueprint : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    public int waterCost;
    public int seedCost;
}
