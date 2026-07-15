using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

public class TileAssetGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate and Assign Inner Corner Assets")]
    public static void GenerateInnerCornerAssets()
    {
        string tilesDir = "Assets/Assets/tiles";
        string[] prefixes = { "cb", "ws" };
        string[] suffixes = { "itl", "itr", "ibl", "ibr" };

        // 1. Refresh database first so Unity sees the new PNGs
        AssetDatabase.Refresh();

        // Generate burnt_0 and burnt_1 assets
        string[] burntFiles = { "burnt_0", "burnt_1" };
        foreach (string burntName in burntFiles)
        {
            string pngPath = Path.Combine(tilesDir, $"{burntName}.png");
            string assetPath = Path.Combine(tilesDir, $"{burntName}.asset");

            if (File.Exists(pngPath))
            {
                TextureImporter importer = AssetImporter.GetAtPath(pngPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.spritePixelsPerUnit = 16;
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                }

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
                if (sprite != null)
                {
                    Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
                    if (tile == null)
                    {
                        tile = ScriptableObject.CreateInstance<Tile>();
                        AssetDatabase.CreateAsset(tile, assetPath);
                    }
                    tile.sprite = sprite;
                    tile.name = burntName;
                    EditorUtility.SetDirty(tile);
                }
            }
        }

        foreach (string prefix in prefixes)
        {
            foreach (string suffix in suffixes)
            {
                string pngName = $"{prefix}_{suffix}.png";
                string pngPath = Path.Combine(tilesDir, pngName);
                string assetName = $"{prefix}_{suffix}.asset";
                string assetPath = Path.Combine(tilesDir, assetName);

                if (!File.Exists(pngPath))
                {
                    Debug.LogWarning($"PNG not found: {pngPath}");
                    continue;
                }

                // Set Texture Importer Settings
                TextureImporter importer = AssetImporter.GetAtPath(pngPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.spritePixelsPerUnit = 16;
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                }

                // Load the imported sprite
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
                if (sprite == null)
                {
                    Debug.LogError($"Failed to load sprite from: {pngPath}");
                    continue;
                }

                // Create or load the Tile asset
                Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
                if (tile == null)
                {
                    tile = ScriptableObject.CreateInstance<Tile>();
                    AssetDatabase.CreateAsset(tile, assetPath);
                }

                tile.sprite = sprite;
                tile.name = $"{prefix}_{suffix}";
                EditorUtility.SetDirty(tile);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 2. Assign to MapLoader availableTiles and TerrainVisualManager
        MapLoader mapLoader = FindFirstObjectByType<MapLoader>();
        if (mapLoader != null)
        {
            // Auto populate availableTiles list
            string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { "Assets" });
            mapLoader.availableTiles = new List<TileBase>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                if (tile != null && !mapLoader.availableTiles.Contains(tile))
                {
                    mapLoader.availableTiles.Add(tile);
                }
            }
            EditorUtility.SetDirty(mapLoader);
            Debug.Log("TileAssetGenerator: Populated MapLoader availableTiles successfully!");
        }

        TerrainVisualManager tvm = FindFirstObjectByType<TerrainVisualManager>();
        if (tvm != null)
        {
            tvm.burnt_0 = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/burnt_0.asset");
            tvm.burnt_1 = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/burnt_1.asset");

            tvm.wet.itl = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/ws_itl.asset");
            tvm.wet.itr = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/ws_itr.asset");
            tvm.wet.ibl = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/ws_ibl.asset");
            tvm.wet.ibr = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Assets/tiles/ws_ibr.asset");

            EditorUtility.SetDirty(tvm);
            Debug.Log("TileAssetGenerator: Configured TerrainVisualManager inner corners successfully!");
        }

        // Mark the scene dirty to save changes
        if (mapLoader != null && mapLoader.gameObject.scene.IsValid())
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mapLoader.gameObject.scene);
        }

        Debug.Log("TileAssetGenerator: Inner Corner Generation and Assignment Complete!");
    }

    [MenuItem("Tools/Clean Up Redundant Scene Trees")]
    public static void CleanUpSceneTrees()
    {
        int deletedCount = 0;
        
        // Find all GameObjects in the active scene
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject go in allObjects)
        {
            string nameLower = go.name.ToLower();
            
            // Delete manual tree objects that are not spawned dynamically
            if ((nameLower.Contains("tree_0") || nameLower == "tree") && !go.name.StartsWith("StaticTree_"))
            {
                // Do not delete player components
                if (go.transform.root.name.ToLower().Contains("player"))
                    continue;
                    
                Undo.DestroyObjectImmediate(go);
                deletedCount++;
            }
        }
        
        Debug.Log($"Cleaned up {deletedCount} redundant manual trees from the scene!");
    }

    [MenuItem("Tools/Configure Sprite Settings for All Tiles")]
    public static void ConfigureAllSpriteSettings()
    {
        string[] folders = { "Assets/Assets/tiles", "Assets/AllTiles" };
        int count = 0;
        
        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder)) continue;
            
            string[] files = Directory.GetFiles(folder, "*.png", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                TextureImporter importer = AssetImporter.GetAtPath(file) as TextureImporter;
                if (importer != null)
                {
                    bool changed = false;
                    if (importer.textureType != TextureImporterType.Sprite) { importer.textureType = TextureImporterType.Sprite; changed = true; }
                    if (importer.spriteImportMode != SpriteImportMode.Single) { importer.spriteImportMode = SpriteImportMode.Single; changed = true; }
                    if (importer.spritePixelsPerUnit != 16) { importer.spritePixelsPerUnit = 16; changed = true; }
                    if (importer.filterMode != FilterMode.Point) { importer.filterMode = FilterMode.Point; changed = true; }
                    if (importer.textureCompression != TextureImporterCompression.Uncompressed) { importer.textureCompression = TextureImporterCompression.Uncompressed; changed = true; }
                    
                    if (changed)
                    {
                        importer.SaveAndReimport();
                        count++;
                    }
                }
            }
        }
        
        Debug.Log($"Successfully configured sprite import settings for {count} textures!");
    }

    [MenuItem("Tools/Create Pre-populated Tile Palette")]
    public static void CreateTilePalettePrefab()
    {
        string palettePath = "Assets/Assets/tiles/AllTilesPalette.prefab";
        
        // 1. Create temporary GameObject representing the Grid and Tilemap layer
        GameObject paletteGo = new GameObject("AllTilesPalette");
        Grid grid = paletteGo.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);
        grid.cellLayout = GridLayout.CellLayout.Rectangle;

        GameObject tilemapGo = new GameObject("Layer1", typeof(Tilemap), typeof(TilemapRenderer));
        tilemapGo.transform.SetParent(paletteGo.transform);
        Tilemap tilemap = tilemapGo.GetComponent<Tilemap>();

        // 2. Load all Tile assets under tiles directory
        string[] guids = AssetDatabase.FindAssets("t:Tile", new[] { "Assets/Assets/tiles" });
        List<Tile> allTiles = new List<Tile>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile != null && !allTiles.Contains(tile))
            {
                allTiles.Add(tile);
            }
        }

        // Sort tiles alphabetically to keep the palette organized
        allTiles.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.OrdinalIgnoreCase));

        // 3. Paint the tiles onto the layout grid
        int columns = 10;
        for (int i = 0; i < allTiles.Count; i++)
        {
            int col = i % columns;
            int row = -(i / columns);
            tilemap.SetTile(new Vector3Int(col, row, 0), allTiles[i]);
        }

        // 4. Save as a Prefab Asset
        string dir = Path.GetDirectoryName(palettePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(paletteGo, palettePath);
        DestroyImmediate(paletteGo);

        if (prefab != null)
        {
            // Create and attach the GridPalette settings sub-asset (essential for Unity recognition!)
            GridPalette paletteSettings = ScriptableObject.CreateInstance<GridPalette>();
            paletteSettings.name = "Palette Settings";
            AssetDatabase.AddObjectToAsset(paletteSettings, prefab);

            // Set GridPalette label so Unity's Tile Palette recognizes and lists it
            AssetDatabase.SetLabels(prefab, new[] { "GridPalette" });
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Successfully created pre-populated Tile Palette at {palettePath} containing {allTiles.Count} tiles!");
        }
        else
        {
            Debug.LogError("Failed to save Tile Palette prefab!");
        }
    }

    [MenuItem("Tools/Configure Player Physics and Colliders")]
    public static void ConfigurePlayerPhysics()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        if (playerGo == null)
        {
            playerGo = GameObject.Find("Player");
        }

        if (playerGo == null)
        {
            Debug.LogError("ConfigurePlayerPhysics: Could not find Player GameObject in the active scene!");
            return;
        }

        // 1. Rigidbody 2D setup
        Rigidbody2D rb = playerGo.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = playerGo.AddComponent<Rigidbody2D>();
            Debug.Log("ConfigurePlayerPhysics: Added Rigidbody2D component to Player.");
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        EditorUtility.SetDirty(rb);

        // 2. Box Collider 2D setup
        BoxCollider2D bc = playerGo.GetComponent<BoxCollider2D>();
        if (bc == null)
        {
            bc = playerGo.AddComponent<BoxCollider2D>();
            Debug.Log("ConfigurePlayerPhysics: Added BoxCollider2D component to Player.");
        }
        bc.isTrigger = false;
        EditorUtility.SetDirty(bc);

        // Mark the active scene dirty to save changes
        if (playerGo.scene.IsValid())
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(playerGo.scene);
        }

        Debug.Log("ConfigurePlayerPhysics: Player physics and constraints configured successfully!");
    }
}
#endif
