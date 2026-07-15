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

    [Header("Runtime Loading Mode")]
    public bool loadFromTextFile = false;
    public bool autoTileOnStart = false;

    [Header("Prefabs for Spawning")]
    public GameObject waterSourcePrefab; // Prefab with WaterSourceNode component
    public Sprite staticTreeSprite;
    public GameObject deadTreePrefab; // Ready-to-use dead tree prefab

    [System.Serializable]
    public class TreeVariant
    {
        public Sprite sprite;
        public bool hasCollider = true;
        public Vector2 colliderSize = new Vector2(0.8f, 0.4f);
        public Vector2 colliderOffset = new Vector2(0f, 0.2f);
        [Range(0f, 10f)] public float weight = 1f;
    }

    [Header("Static Tree/Decoration Variety (falls back to red_region assets if empty)")]
    public List<TreeVariant> treeVariants;

    private void Start()
    {
        // GridWorldMatrix's dictionary is runtime-only (not serialized), so the map must
        // be rebuilt on play to populate cell data (water sources, corruption states).
        if (loadFromTextFile)
        {
            LoadMap();
        }
        else
        {
            InitializeGridFromTilemap();
        }
    }

    public void InitializeGridFromTilemap()
    {
        if (tilemap == null)
        {
            Debug.LogError("MapLoader: Tilemap target is not assigned!");
            return;
        }

        Debug.Log("MapLoader: Starting scan and initialization from existing Tilemap.");

        // 1. Clear previous static trees
        var allGOs = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in allGOs)
        {
            if (go != null && go.name.StartsWith("StaticTree_"))
            {
                if (Application.isPlaying) Destroy(go);
                else DestroyImmediate(go);
            }
        }

        // 2. Clear previous GridWorldMatrix data if available
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            EnvironmentManager.Instance.EnvironmentGrid.ClearGrid();
        }

        TerrainVisualManager.Instance?.ClearTracking();

        // 3. Scan the tilemap bounds for painted tiles
        var bounds = tilemap.cellBounds;
        var allSoilCells = new HashSet<Vector2Int>();
        var paintDirtCells = new HashSet<Vector2Int>();
        var waterCells = new HashSet<Vector2Int>();
        var grassCells = new HashSet<Vector2Int>(); // Keep track of grass cells for dead trees

        // Define programmatic Dirt Clearing Bounds (grid coordinates)
        int minX = -12, maxX = 32;
        int minY = 5, maxY = 25;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                TileBase tile = tilemap.GetTile(pos);
                if (tile == null) continue;

                string tileName = tile.name.ToLower();
                Vector2Int gridPos = new Vector2Int(pos.x, -pos.y);

                if (tileName.Contains("tree"))
                {
                    // Trees are separate GameObjects, not tiles - place grass underneath
                    TileBase grassTile = null;
                    foreach (var t in availableTiles)
                    {
                        if (t != null && (t.name.ToLower() == "redgrass_0" || t.name.ToLower().Contains("grass")))
                        {
                            grassTile = t;
                            break;
                        }
                    }
                    if (grassTile != null)
                    {
                        tilemap.SetTile(pos, grassTile);
                        ConfigureGridCell(gridPos, grassTile.name.ToLower());
                    }
                    SpawnStaticTree(gridPos);
                }
                else if (tileName.StartsWith("water"))
                {
                    ConfigureGridCell(gridPos, tileName);
                    waterCells.Add(gridPos);
                }
                else
                {
                    // Apply programmatic layout: check if this cell falls in the clearing
                    bool isWithinClearing = (gridPos.x >= minX && gridPos.x <= maxX && gridPos.y >= minY && gridPos.y <= maxY);
                    
                    if (isWithinClearing)
                    {
                        allSoilCells.Add(gridPos);

                        // 4 quadrants of burnt soil, adjusted to prevent overlapping with grass/water borders
                        bool isWithinBurntQuadrant = 
                            (gridPos.x >= -6 && gridPos.x <= 5 && gridPos.y >= 8 && gridPos.y <= 12) ||    // Top-Left
                            (gridPos.x >= 14 && gridPos.x <= 25 && gridPos.y >= 8 && gridPos.y <= 12) ||   // Top-Right
                            (gridPos.x >= -6 && gridPos.x <= 5 && gridPos.y >= 17 && gridPos.y <= 21) ||   // Bottom-Left
                            (gridPos.x >= 14 && gridPos.x <= 25 && gridPos.y >= 17 && gridPos.y <= 21);  // Bottom-Right

                        if (isWithinBurntQuadrant)
                        {
                            ConfigureGridCell(gridPos, "burnt");
                        }
                        else
                        {
                            ConfigureGridCell(gridPos, "dirt");
                            paintDirtCells.Add(gridPos);

                            // Repaint as dirt_0 on the tilemap
                            TileBase dirtTile = null;
                            foreach (var t in availableTiles)
                            {
                                if (t != null && t.name == "dirt_0")
                                {
                                    dirtTile = t;
                                    break;
                                }
                            }
                            if (dirtTile != null)
                            {
                                tilemap.SetTile(pos, dirtTile);
                            }
                        }
                    }
                    else
                    {
                        // Outside the clearing: keep original grass / red grass
                        ConfigureGridCell(gridPos, tileName);
                        
                        // Keep track of grass cells to spawn dead trees on
                        if (tileName.Contains("grass"))
                        {
                            grassCells.Add(gridPos);
                        }
                    }
                }
            }
        }

        // 3.5 Spawn Dead Trees dynamically on the surrounding grass cells
        Vector3 playerPos = Vector3.zero;
        var player = FindFirstObjectByType<Player>();
        if (player != null) playerPos = player.transform.position;

        Vector3 malizPos = Vector3.zero;
        var malizObj = GameObject.Find("Maliz");
        if (malizObj != null) malizPos = malizObj.transform.position;

        if (deadTreePrefab != null)
        {
            Debug.Log($"MapLoader: Spawning dead trees on surrounding grass area. Prefab: {deadTreePrefab.name}");
            // Clear any previous runtime dead trees
            var allSceneObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var go in allSceneObjects)
            {
                if (go != null && go.name.StartsWith("RuntimeDeadTree_"))
                {
                    if (Application.isPlaying) Destroy(go);
                    else DestroyImmediate(go);
                }
            }

            int spawnCount = 0;
            foreach (var gridPos in grassCells)
            {
                Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
                
                // Avoid spawning too close to player spawn or Maliz NPC
                if (Vector3.Distance(worldPos, playerPos) < 3.0f) continue;
                if (Vector3.Distance(worldPos, malizPos) < 3.0f) continue;

                // Ensure we don't spawn on top of an existing static tree
                bool hasStaticTree = false;
                var staticTree = GameObject.Find($"StaticTree_{gridPos.x}_{gridPos.y}");
                if (staticTree != null) hasStaticTree = true;

                if (!hasStaticTree && UnityEngine.Random.value < 0.22f) // 22% density for a rich look
                {
                    GameObject dt = Instantiate(deadTreePrefab, worldPos, Quaternion.identity);
                    dt.name = $"RuntimeDeadTree_{gridPos.x}_{gridPos.y}";
                    spawnCount++;
                }
            }
            Debug.Log($"MapLoader: Successfully spawned {spawnCount} dead trees on grass.");
        }

        // 4. Auto-tile static regions if enabled
        if (autoTileOnStart)
        {
            var tvm = TerrainVisualManager.Instance;
            if (tvm != null)
            {
                tvm.PaintStaticRegion(paintDirtCells, allSoilCells.Contains, tvm.dirtStatic);
                tvm.PaintStaticRegion(waterCells, tvm.waterStatic);
            }
        }

        TerrainVisualManager.Instance?.RefreshAll();
        tilemap.RefreshAllTiles();

        Debug.Log("MapLoader: Tilemap scan and GridWorldMatrix population complete.");
    }

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
                if (Application.isPlaying) Destroy(go);
                else DestroyImmediate(go);
            }
        }

        // 1. Parse Tile Data mapping
        Dictionary<int, string> indexToTileName = ParseTileData();

        // 2. Clear current tiles
        tilemap.ClearAllTiles();
        TerrainVisualManager.Instance?.ClearTracking();

        // 3. Clear previous GridWorldMatrix data if available
        if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
        {
            EnvironmentManager.Instance.EnvironmentGrid.ClearGrid();
        }

        // 4. Parse and set tiles
        string[] rows = mapFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int rowCount = rows.Length;

        int startRowIndex = 0;
        int offsetCol = 0;
        int offsetRow = 0;
        if (rowCount > 0 && rows[0].StartsWith("#offset"))
        {
            string[] offsetParts = rows[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (offsetParts.Length >= 3 && int.TryParse(offsetParts[1], out int oCol) && int.TryParse(offsetParts[2], out int oRow))
            {
                offsetCol = oCol;
                offsetRow = oRow;
                startRowIndex = 1;
            }
        }

        // Track base-terrain region membership so the dirt path / pond can be auto-tiled
        // (edges + outer/inner corners) from adjacency instead of relying on whichever
        // specific edge tile the map author manually placed in the text file.
        var allSoilCells = new HashSet<Vector2Int>();
        var paintDirtCells = new HashSet<Vector2Int>();
        var waterCells = new HashSet<Vector2Int>();

        for (int r = startRowIndex; r < rowCount; r++)
        {
            string[] cols = rows[r].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int colCount = cols.Length;
            int relativeRow = r - startRowIndex;

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
                        int targetX = offsetCol + c;
                        int targetY = offsetRow - relativeRow;
                        Vector3Int tilemapPos = new Vector3Int(targetX, targetY, 0);
                        Vector2Int gridPos = new Vector2Int(targetX, -targetY);

                        if (tileName.Contains("tree"))
                        {
                            // Trees are separate GameObjects, not tiles - place grass underneath
                            // first so there isn't a blank hole in the tilemap under the canopy.
                            TileBase groundTile = FindTileByIndex(2, indexToTileName);
                            if (groundTile != null)
                            {
                                tilemap.SetTile(tilemapPos, groundTile);
                                ConfigureGridCell(gridPos, groundTile.name.ToLower());
                            }
                            SpawnStaticTree(gridPos);
                        }
                        else
                        {
                            // Place tile in tilemap coordinate
                            tilemap.SetTile(tilemapPos, tileToSet);

                            // Set up corresponding GridCell in GridWorldMatrix
                            ConfigureGridCell(gridPos, tileName);

                            if (tileName.StartsWith("dirt") || tileName.Contains("burnt") || tileName.Contains("dug")) 
                            {
                                allSoilCells.Add(gridPos);
                                if (!tileName.Contains("burnt") && !tileName.Contains("dug"))
                                {
                                    paintDirtCells.Add(gridPos);
                                }
                            }
                            else if (tileName.StartsWith("water")) 
                            {
                                waterCells.Add(gridPos);
                            }
                        }
                    }
                }
            }
        }

        // Auto-tile the static dirt path / pond regions (edges + corners from adjacency),
        // then paint the burnt/corrupted and wet overlays on top.
        var tvm = TerrainVisualManager.Instance;
        if (tvm != null)
        {
            tvm.PaintStaticRegion(paintDirtCells, allSoilCells.Contains, tvm.dirtStatic);
            tvm.PaintStaticRegion(waterCells, tvm.waterStatic);
        }
        TerrainVisualManager.Instance?.RefreshAll();

        tilemap.RefreshAllTiles();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(tilemap);
            if (tilemap.gameObject.scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(tilemap.gameObject.scene);
            }
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
            cell.isWaterSource = true;
        }
        else if (tileName.Contains("wall") || tileName.Contains("table"))
        {
            cell.moisture = 0.0f;
            cell.soilQuality = 0.0f;
            cell.corruptionState = 0;
            cell.localO2 = 15.0f;
        }
        else if (tileName.Contains("burnt"))
        {
            cell.moisture = 0.0f;
            cell.soilQuality = 0.3f;
            cell.corruptionState = 1; // Authoritative burnt spot
            cell.localO2 = 15.0f;
        }
        else if (tileName.Contains("dug"))
        {
            cell.moisture = 0.0f;
            cell.soilQuality = 0.3f;
            cell.corruptionState = 2; // DugBurnt
            cell.localO2 = 15.0f;
        }
        else if (tileName == "dirt_0" || tileName == "dirt_1_0" || tileName == "dirt")
        {
            cell.moisture = 0.0f;
            cell.soilQuality = 0.3f;
            cell.corruptionState = 0; // Clean tilled soil
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

    [ContextMenu("Export painted Tilemap to map_1.txt")]
    public void ExportMap()
    {
        if (tilemap == null)
        {
            Debug.LogError("MapLoader: Tilemap target is not assigned!");
            return;
        }
        
        // Parse tile data to build name-to-index reverse mapping
        Dictionary<string, int> nameToIndex = new Dictionary<string, int>();
        string[] dataLines = tileDataFile.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        int index = 0;
        for (int i = 0; i < dataLines.Length; i += 2)
        {
            if (i < dataLines.Length)
            {
                string pngName = dataLines[i].Trim().Replace(".png", "").ToLower();
                nameToIndex[pngName] = index;
                index++;
            }
        }

        // Find bounds of actual non-empty tiles
        var bounds = tilemap.cellBounds;
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;
        bool hasAnyTile = false;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                hasAnyTile = true;
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;
            }
        }

        // Fallback if empty
        if (!hasAnyTile)
        {
            minX = 0; maxX = 44;
            minY = -29; maxY = 0;
        }

        int columns = (maxX - minX) + 1;
        int rows = (maxY - minY) + 1;
        
        List<string> rowStrings = new List<string>();
        // Write offset header as first line
        rowStrings.Add($"#offset {minX} {maxY}");

        for (int r = 0; r < rows; r++)
        {
            List<string> colIndices = new List<string>();
            for (int c = 0; c < columns; c++)
            {
                // Aligning with standard Coordinate: top-left starts at minX, maxY and row goes downwards
                Vector3Int pos = new Vector3Int(minX + c, maxY - r, 0);
                TileBase tile = tilemap.GetTile(pos);
                
                int tileIndex = 0; // Default to dirt_0 (index 0) if null
                if (tile != null)
                {
                    string tileName = tile.name.ToLower();
                    
                    // Clean names like "dirt_0 (Instance)" to "dirt_0"
                    if (tileName.Contains(" (instance)"))
                    {
                        tileName = tileName.Replace(" (instance)", "");
                    }
                    
                    string key = tileName;
                    
                    if (nameToIndex.ContainsKey(key))
                    {
                        tileIndex = nameToIndex[key];
                    }
                    else
                    {
                        bool found = false;
                        foreach (var pair in nameToIndex)
                        {
                            if (key == pair.Key || key.Contains(pair.Key) || pair.Key.Contains(key))
                            {
                                tileIndex = pair.Value;
                                found = true;
                                break;
                            }
                        }
                        
                        if (!found)
                        {
                            if (key.Contains("sand")) tileIndex = 7;
                            else if (key.Contains("wall")) tileIndex = 8;
                            else if (key.Contains("water_2")) tileIndex = 10;
                            else if (key.Contains("water")) tileIndex = 9;
                            else if (key.Contains("tree")) tileIndex = 11;
                        }
                    }
                }
                
                colIndices.Add(tileIndex.ToString());
            }
            rowStrings.Add(string.Join(" ", colIndices));
        }
        
        string exportedText = string.Join("\r\n", rowStrings) + "\r\n";
        
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(mapFile);
        if (string.IsNullOrEmpty(assetPath))
        {
            assetPath = "Assets/Assets/maps/map_1.txt"; // fallback
        }
        
        System.IO.File.WriteAllText(assetPath, exportedText);
        UnityEditor.AssetDatabase.ImportAsset(assetPath);
        UnityEditor.AssetDatabase.Refresh();
        
        Debug.Log($"MapLoader: Exported {columns}x{rows} map to {assetPath} successfully with offset ({minX}, {maxY})!");
    }
#endif

#if UNITY_EDITOR
    // Fallback pool used only when `treeVariants` hasn't been wired up in the Inspector.
    private static readonly (string path, bool hasCollider, float w)[] FallbackTreeVariantPaths = new[]
    {
        ("Assets/Assets/redtree_large_treeonly.png", true, 25f),
        ("Assets/Assets/tiles/red_region/tree_big.png", true, 30f),
        ("Assets/Assets/tiles/red_region/tree_bush_small.png", true, 20f),
        ("Assets/Assets/tiles/red_region/tree_pine_small.png", true, 15f),
        ("Assets/Assets/tiles/red_region/bush_cluster.png", false, 6f),
        ("Assets/Assets/tiles/red_region/mushroom_cluster.png", false, 4f),
        ("Assets/Assets/tiles/red_region/rock_cluster.png", true, 5f),
        ("Assets/Assets/tiles/red_region/grass_tufts.png", false, 8f),
        ("Assets/Assets/tiles/red_region/mushroom_single.png", false, 5f),
        ("Assets/Assets/tiles/red_region/flower_red.png", false, 6f),
        ("Assets/Assets/tiles/red_region/flower_pink.png", false, 6f),
    };

    private List<TreeVariant> BuildFallbackTreeVariants()
    {
        var list = new List<TreeVariant>();
        foreach (var (path, hasCollider, w) in FallbackTreeVariantPaths)
        {
            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) continue;

            // Scale trunk collider footprint proportionally to sprite size (small
            // decorations like clusters get no collider - they're walkable ground clutter).
            Bounds b = sprite.bounds;
            list.Add(new TreeVariant
            {
                sprite = sprite,
                hasCollider = hasCollider,
                colliderSize = new Vector2(b.size.x * 0.22f, b.size.y * 0.09f),
                colliderOffset = new Vector2(0f, b.size.y * 0.05f),
                weight = w
            });
        }
        return list;
    }
#endif

    private TreeVariant PickTreeVariant(Vector2Int gridPos)
    {
        List<TreeVariant> pool = treeVariants;
#if UNITY_EDITOR
        if (pool == null || pool.Count == 0)
        {
            pool = BuildFallbackTreeVariants();
        }
#endif
        if (pool == null || pool.Count == 0)
        {
            return new TreeVariant { sprite = staticTreeSprite, hasCollider = true };
        }

        // Deterministic per-cell hash so re-running LoadMap doesn't reshuffle decorations.
        int seed = gridPos.x * 73856093 ^ gridPos.y * 19349663;
        System.Random rng = new System.Random(seed);

        float totalWeight = 0f;
        foreach (var v in pool) totalWeight += Mathf.Max(0f, v.weight);
        if (totalWeight <= 0f) return pool[0];

        float roll = (float)(rng.NextDouble() * totalWeight);
        float acc = 0f;
        foreach (var v in pool)
        {
            acc += Mathf.Max(0f, v.weight);
            if (roll <= acc) return v;
        }
        return pool[pool.Count - 1];
    }

    private void SpawnStaticTree(Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y, 0f);
        GameObject treeGO = new GameObject($"StaticTree_{gridPos.x}_{gridPos.y}");
        treeGO.transform.position = worldPos;

        TreeVariant variant = PickTreeVariant(gridPos);

        // Add SpriteRenderer
        var sr = treeGO.AddComponent<SpriteRenderer>();
        sr.sprite = variant.sprite != null ? variant.sprite : staticTreeSprite;
#if UNITY_EDITOR
        if (sr.sprite == null)
        {
            sr.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Assets/redtree_large_treeonly.png");
        }
#endif
        sr.spriteSortPoint = SpriteSortPoint.Pivot;

        // Add BoxCollider2D for the trunk (skipped for small decorative clusters)
        if (variant.hasCollider)
        {
            var col = treeGO.AddComponent<BoxCollider2D>();
            col.size = variant.colliderSize != Vector2.zero ? variant.colliderSize : new Vector2(0.8f, 0.4f);
            col.offset = variant.colliderOffset;
        }

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
