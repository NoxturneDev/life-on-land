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

    [Header("Prefabs for Spawning")]
    public GameObject waterSourcePrefab; // Prefab with WaterSourceNode component
    public Sprite staticTreeSprite;

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

        Debug.Log("MapLoader: Starting load. Available Tiles: " + (availableTiles != null ? availableTiles.Count : 0));

        // Clear previous static trees
        var allGOs = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in allGOs)
        {
            if (go != null && go.name.StartsWith("StaticTree_"))
            {
                DestroyImmediate(go);
            }
        }

        // 1. Parse Tile Data mapping
        Dictionary<int, string> indexToTileName = ParseTileData();

        // 2. Clear current tiles
        tilemap.ClearAllTiles();

        // 3. Clear previous GridWorldMatrix data if available
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            EnvironmentManager.Instance.EnvironmentGrid.ClearGrid();
        }

        // 4. Parse and set tiles
        string[] rows = mapFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int rowCount = rows.Length;

        for (int r = 0; r < rowCount; r++)
        {
            string[] cols = rows[r].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int colCount = cols.Length;

            for (int c = 0; c < colCount; c++)
            {
                if (int.TryParse(cols[c], out int tileIndex))
                {
                    // Introduce random variety for redgrass tiles (indices 2 to 6)
                    if (tileIndex >= 2 && tileIndex <= 6)
                    {
                        tileIndex = UnityEngine.Random.Range(2, 7);
                    }

                    TileBase tileToSet = FindTileByIndex(tileIndex, indexToTileName);
                    if (tileToSet != null)
                    {
                        string tileName = tileToSet.name.ToLower();
                        if (tileName.Contains("tree"))
                        {
                            // Trees are separate GameObjects, not tiles - place grass underneath
                            // first so there isn't a blank hole in the tilemap under the canopy.
                            TileBase groundTile = FindTileByIndex(2, indexToTileName);
                            Vector3Int tilemapPos = new Vector3Int(c, -r, 0);
                            if (groundTile != null)
                            {
                                tilemap.SetTile(tilemapPos, groundTile);
                                ConfigureGridCell(new Vector2Int(c, r), groundTile.name.ToLower());
                            }
                            SpawnStaticTree(new Vector2Int(c, r));
                        }
                        else
                        {
                            // Place tile in tilemap coordinate (c, -r, 0)
                            Vector3Int tilemapPos = new Vector3Int(c, -r, 0);
                            tilemap.SetTile(tilemapPos, tileToSet);

                            // Set up corresponding GridCell in GridWorldMatrix
                            ConfigureGridCell(new Vector2Int(c, r), tileName);
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

        Debug.Log("MapLoader: Map loading and GridWorldMatrix population complete.");
    }

    private void ConfigureGridCell(Vector2Int gridPos, string tileName)
    {
        if (EnvironmentManager.Instance == null || EnvironmentManager.Instance.EnvironmentGrid == null) return;

        GridWorldMatrix gridMatrix = EnvironmentManager.Instance.EnvironmentGrid;
        GridCell cell = gridMatrix.GetCell(gridPos);

        if (tileName.Contains("water"))
        {
            cell.moisture = 1.0f;
            cell.soilQuality = 0.0f;
            cell.corruptionState = 0;
            cell.localO2 = 18.0f;

            // Spawn WaterSourceNode dynamically if prefab is assigned
            if (waterSourcePrefab != null)
            {
                Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
                GameObject waterGO = Instantiate(waterSourcePrefab, worldPos, Quaternion.identity);
                waterGO.name = $"WaterSource_{gridPos.x}_{gridPos.y}";
                WaterSourceNode node = waterGO.GetComponent<WaterSourceNode>();
                if (node != null)
                {
                    cell.placedObject = waterGO.AddComponent<WorldObject>();
                    cell.placedObject.GridCoordinates = gridPos;
                    cell.placedObject.ObjectID = waterGO.name;
                }
            }
        }
        else if (tileName.Contains("wall") || tileName.Contains("table"))
        {
            cell.moisture = 0.0f;
            cell.soilQuality = 0.0f;
            cell.corruptionState = 0;
            cell.localO2 = 15.0f;
        }
        else if (tileName == "dirt_0" || tileName == "dirt_1_0" || tileName == "dirt")
        {
            // Only pure dirt path tiles are initially corrupted burnt tiles left by the villain
            cell.moisture = 0.0f;
            cell.soilQuality = 0.3f;
            cell.corruptionState = 1; // Corrupted
            cell.localO2 = 15.0f;
        }
        else if (tileName.Contains("sand"))
        {
            cell.moisture = 0.1f;
            cell.soilQuality = 0.2f;
            cell.corruptionState = 0; // Sand is dry but normal, can be planted on directly if watered
            cell.localO2 = 15.0f;
        }
        else if (tileName.Contains("grass"))
        {
            cell.moisture = 0.7f;
            cell.soilQuality = 0.8f;
            cell.corruptionState = 0; // Already purified and healthy grass
            cell.localO2 = 21.0f;
        }
        else
        {
            // Default fallback
            cell.moisture = 0.3f;
            cell.soilQuality = 0.5f;
            cell.corruptionState = 0;
            cell.localO2 = 15.0f;
        }
    }

    private Dictionary<int, string> ParseTileData()
    {
        Dictionary<int, string> mapping = new Dictionary<int, string>();
        string[] lines = tileDataFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int index = 0;
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

        if (mapping.ContainsKey(index))
        {
            targetTileName = mapping[index].Replace(".png", "").ToLower();
        }
        else
        {
            switch (index)
            {
                case 11: targetTileName = "sand"; break;
                case 12: targetTileName = "table"; break;
                case 13: targetTileName = "wall"; break;
                case 14: targetTileName = "water"; break;
                case 15: targetTileName = "water_2"; break;
                case 16: targetTileName = "water_3"; break;
                default:
                    Debug.LogWarning("MapLoader: Unknown tile index: " + index);
                    return null;
            }
        }

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
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:TileBase", new[] { "Assets" });
        availableTiles = new List<TileBase>();
        
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            TileBase tile = UnityEditor.AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null)
            {
                availableTiles.Add(tile);
            }
        }
        
        LoadMap();
    }
#endif

    private void SpawnStaticTree(Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
        GameObject treeGO = new GameObject($"StaticTree_{gridPos.x}_{gridPos.y}");
        treeGO.transform.position = worldPos;

        // Add SpriteRenderer
        var sr = treeGO.AddComponent<SpriteRenderer>();
        sr.sprite = staticTreeSprite != null ? staticTreeSprite : UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Assets/redtree_large.png");
        sr.spriteSortPoint = SpriteSortPoint.Pivot;

        // Add BoxCollider2D for the trunk
        var col = treeGO.AddComponent<BoxCollider2D>();
        col.size = new Vector2(0.8f, 0.4f);
        col.offset = new Vector2(0f, 0.2f);

        // Add WorldObject
        var wo = treeGO.AddComponent<WorldObject>();
        wo.GridCoordinates = gridPos;
        wo.ObjectID = treeGO.name;

        // Configure GridCell
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            var cell = EnvironmentManager.Instance.EnvironmentGrid.GetCell(gridPos);
            cell.placedObject = wo;
            cell.corruptionState = 0;
            cell.moisture = 0.5f;
            cell.soilQuality = 0.5f;
            cell.localO2 = 18.0f;
        }
    }
}
