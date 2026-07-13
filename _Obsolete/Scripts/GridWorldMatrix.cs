using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    [Range(0f, 1f)] public float moisture = 0.5f;        // 0.0 (bone dry) to 1.0 (waterlogged)
    public float localO2 = 15.0f;                        // Local oxygen percentage
    [Range(0f, 1f)] public float soilQuality = 0.5f;     // 0.0 (sterile/sandy) to 1.0 (rich soil)
    public WorldObject placedObject;                     // Tree, Building, etc. placed on this tile
}

public class GridWorldMatrix : MonoBehaviour
{
    private Dictionary<Vector2Int, GridCell> grid = new Dictionary<Vector2Int, GridCell>();

    // Retrieve cell data, automatically initializing if not previously accessed
    public GridCell GetCell(Vector2Int coordinates)
    {
        if (!grid.ContainsKey(coordinates))
        {
            grid[coordinates] = new GridCell();
        }
        return grid[coordinates];
    }

    // Bind a placed GameObject (Tree/Building) to a coordinate
    public void SetPlacedObject(Vector2Int coordinates, WorldObject worldObject)
    {
        GridCell cell = GetCell(coordinates);
        cell.placedObject = worldObject;
        if (worldObject != null)
        {
            worldObject.GridCoordinates = coordinates;
        }
    }

    public WorldObject GetPlacedObject(Vector2Int coordinates)
    {
        if (!grid.ContainsKey(coordinates)) return null;
        return grid[coordinates].placedObject;
    }

    public bool HasCell(Vector2Int coordinates)
    {
        return grid.ContainsKey(coordinates);
    }

    public Dictionary<Vector2Int, GridCell>.KeyCollection GetAllCoordinates()
    {
        return grid.Keys;
    }

    public void ClearGrid()
    {
        grid.Clear();
    }
}
