using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [Header("Tilemap Target")]
    public Tilemap tilemap;

    [Header("Text Assets")]
    public TextAsset mapFile;
    public TextAsset tileDataFile;

    [Header("Available Tile Assets (Assign in Inspector)")]
    public List<TileBase> availableTiles;

    [ContextMenu("Load Map")]
    public void LoadMap()
    {
        if (tilemap == null)
        {
            Debug.LogError("MapLoader: Tilemap target is not assigned!");
            return;
        }

        if (mapFile == null)
        {
            Debug.LogError("MapLoader: Map file (TextAsset) is not assigned!");
            return;
        }

        if (tileDataFile == null)
        {
            Debug.LogError("MapLoader: Tile Data file (TextAsset) is not assigned!");
            return;
        }

        Debug.Log("MapLoader: Starting load. Available Tiles in list: " + (availableTiles != null ? availableTiles.Count : 0));

        // 1. Parse Tile Data mapping
        Dictionary<int, string> indexToTileName = ParseTileData();
        Debug.Log("MapLoader: Parsed tile data mapping. Total mappings found in tileData file: " + indexToTileName.Count);

        // 2. Clear current tiles
        tilemap.ClearAllTiles();

        // 3. Parse and set tiles
        string[] rows = mapFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int rowCount = rows.Length;
        Debug.Log("MapLoader: Found rows in map file: " + rowCount);

        int placedCount = 0;
        int failedCount = 0;

        for (int r = 0; r < rowCount; r++)
        {
            string[] cols = rows[r].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int colCount = cols.Length;

            for (int c = 0; c < colCount; c++)
            {
                if (int.TryParse(cols[c], out int tileIndex))
                {
                    TileBase tileToSet = FindTileByIndex(tileIndex, indexToTileName);
                    if (tileToSet != null)
                    {
                        // Place tile in grid coordinate (c, -r, 0) to align top-left at (0,0)
                        tilemap.SetTile(new Vector3Int(c, -r, 0), tileToSet);
                        placedCount++;
                    }
                    else
                    {
                        failedCount++;
                        if (failedCount <= 5) // Limit warning spam to first 5
                        {
                            string targetName = indexToTileName.ContainsKey(tileIndex) ? indexToTileName[tileIndex] : "Fallback Index";
                            Debug.LogWarning($"MapLoader: Could not find tile asset matching index {tileIndex} (Name: {targetName})");
                        }
                    }
                }
            }
        }

        tilemap.RefreshAllTiles();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(tilemap);
        if (tilemap.gameObject.scene.IsValid())
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(tilemap.gameObject.scene);
        }
#endif

        Debug.Log($"MapLoader: Completed! Placed Tiles: {placedCount}, Failed Matches: {failedCount}");
    }

    private Dictionary<int, string> ParseTileData()
    {
        Dictionary<int, string> mapping = new Dictionary<int, string>();
        string[] lines = tileDataFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int index = 0;
        // In tileData1.txt: Line 2*i is PNG filename, Line 2*i+1 is Solid flag
        for (int i = 0; i < lines.Length; i += 2)
        {
            if (i < lines.Length)
            {
                string pngName = lines[i].Trim();
                mapping[index] = pngName;
                index++;
            }
        }

        return mapping;
    }

    private TileBase FindTileByIndex(int index, Dictionary<int, string> mapping)
    {
        string targetTileName = "";

        // Check if index matches an entry in tileData1.txt
        if (mapping.ContainsKey(index))
        {
            targetTileName = mapping[index].Replace(".png", "").ToLower();
        }
        else
        {
            // Fallback for missing indices in tileData1.txt (indices 11-16)
            switch (index)
            {
                case 11:
                    targetTileName = "sand";
                    break;
                case 12:
                    targetTileName = "table";
                    break;
                case 13:
                    targetTileName = "wall";
                    break;
                case 14:
                    targetTileName = "water";
                    break;
                case 15:
                    targetTileName = "water_2";
                    break;
                case 16:
                    targetTileName = "water_3";
                    break;
                default:
                    Debug.LogWarning("MapLoader: Unknown tile index: " + index);
                    return null;
            }
        }

        // Try exact match with suffix conventions (e.g. dirt_0, sand_0, grass_0_0)
        foreach (var tile in availableTiles)
        {
            if (tile != null)
            {
                string tileName = tile.name.ToLower();
                if (tileName == targetTileName + "_0" || tileName == targetTileName + "_0_0" || tileName == targetTileName)
                {
                    return tile;
                }
            }
        }

        // Secondary fallback: check if the tile asset name contains the string
        foreach (var tile in availableTiles)
        {
            if (tile != null && tile.name.ToLower().Contains(targetTileName))
            {
                return tile;
            }
        }

        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Populate and Load")]
    public void AutoPopulateAndLoad()
    {
        // Search globally in Assets folder for any TileBase type
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:TileBase", new[] { "Assets" });
        availableTiles = new List<TileBase>();
        
        System.Text.StringBuilder tileNames = new System.Text.StringBuilder();
        
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            TileBase tile = UnityEditor.AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null)
            {
                availableTiles.Add(tile);
                tileNames.Append(tile.name + ", ");
            }
        }
        
        Debug.Log($"MapLoader: Auto-populated {availableTiles.Count} tiles globally. Tiles: {tileNames.ToString()}");
        LoadMap();
    }
#endif
}
