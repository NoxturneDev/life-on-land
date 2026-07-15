using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    [Range(0f, 1f)] public float moisture = 0.5f;        // 0.0 (bone dry) to 1.0 (waterlogged)
    public float localO2 = 15.0f;                        // Local oxygen percentage
    [Range(0f, 1f)] public float soilQuality = 0.5f;     // 0.0 (sterile/sandy) to 1.0 (rich soil)
    public WorldObject placedObject;                     // Tree, Building, etc. placed on this tile
    
    // Corruption states: 0 = Normal, 1 = Corrupted, 2 = DugBurnt
    public int corruptionState = 0;

    // True for pond/water tiles - lets the watering can be refilled here instead of consumed
    public bool isWaterSource = false;
}

public class GridWorldMatrix : MonoBehaviour
{
    private Dictionary<Vector2Int, GridCell> grid = new Dictionary<Vector2Int, GridCell>();

    // Running total of corrupted tiles the player has fully purified (shovel + water).
    public int TilesPurifiedCount { get; private set; }

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

    // Two-Step Purification Mechanics:
    
    // Step 1: Shovel on Corrupted turns it into DugBurnt (Burnt soil cleared but dry/cracked)
    public bool PurifyTileShovel(Vector2Int coordinates)
    {
        GridCell cell = GetCell(coordinates);
        if (cell.corruptionState == 1) // Corrupted
        {
            cell.corruptionState = 2; // DugBurnt
            TerrainVisualManager.Instance?.OnCellChanged(coordinates);
            Debug.Log($"GridWorldMatrix: Tile at {coordinates} shoveled to DugBurnt.");
            return true;
        }
        return false;
    }

    // Step 2: Watering Can on DugBurnt turns it into Normal soil and wets it
    public bool PurifyTileWater(Vector2Int coordinates)
    {
        GridCell cell = GetCell(coordinates);
        if (cell.corruptionState == 2) // DugBurnt
        {
            cell.corruptionState = 0; // Normal
            cell.moisture = 1.0f;     // Wet the soil
            TilesPurifiedCount++;
            TerrainVisualManager.Instance?.OnCellWatered(coordinates);
            Debug.Log($"GridWorldMatrix: Tile at {coordinates} purified to Normal soil and watered.");
            return true;
        }
        return false;
    }
}
