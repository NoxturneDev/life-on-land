using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private Vector2Int gridCoordinates;

    public string ObjectID
    {
        get => objectID;
        set => objectID = value;
    }
    
    public Vector2Int GridCoordinates 
    {
        get => gridCoordinates;
        set => gridCoordinates = value;
    }
}
