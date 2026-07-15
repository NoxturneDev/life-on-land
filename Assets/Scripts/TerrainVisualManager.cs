using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Repaints ground tiles when grid-cell state changes so player actions have visible feedback.
// Burnt (corrupted) and wet (purified+watered) regions auto-tile: each cell picks a
// fill / edge / corner variant from its 4-neighbour same-state adjacency, giving the
// region an organic shadowed border instead of hard squares.
public class TerrainVisualManager : MonoBehaviour
{
    private static TerrainVisualManager instance;
    public static TerrainVisualManager Instance
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<TerrainVisualManager>();
            return instance;
        }
    }

    [Serializable]
    public class VariantSet
    {
        public TileBase fill;
        public TileBase fillAlt;
        public TileBase t, b, l, r;
        public TileBase tl, tr, bl, br;
        public TileBase itl, itr, ibl, ibr; // Inner corners (concave)
    }

    [Header("Target")]
    public Tilemap tilemap;

    [Header("Auto-tiled State Sets")]
    public VariantSet wet;

    [Header("Auto-tiled Static Base Terrain (path/pond)")]
    public VariantSet dirtStatic;
    public VariantSet waterStatic;

    [Header("Single-tile States")]
    public TileBase dugTile;
    public TileBase drySoilTile; // purified soil once moisture evaporates

    [Header("Burnt (Corrupted) Tiles")]
    public TileBase burnt_0;
    public TileBase burnt_1;

    // cells we painted wet, and cells that have completed the purify cycle (show dry soil, not original)
    private readonly HashSet<Vector2Int> wetCells = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> driedCells = new HashSet<Vector2Int>();

    private void Awake()
    {
        instance = this;
        if (dirtStatic != null)
        {
            dirtStatic.fill = dugTile;
            dirtStatic.fillAlt = dugTile;
        }
        drySoilTile = dugTile;
    }

    private GridWorldMatrix Grid =>
        EnvironmentManager.Instance != null ? EnvironmentManager.Instance.EnvironmentGrid : null;

    private Vector3Int ToTilemap(Vector2Int g) => new Vector3Int(g.x, -g.y, 0);

    private bool IsBurnt(Vector2Int c)
    {
        var grid = Grid;
        if (grid == null) return false;
        // Treat out-of-bounds as same-state (true) so we don't draw borders on the map edges
        if (!grid.HasCell(c)) return true;
        return grid.GetCell(c).corruptionState == 1;
    }

    private bool IsWet(Vector2Int c)
    {
        var grid = Grid;
        if (grid == null) return false;
        // Treat out-of-bounds as same-state (true) so we don't draw borders on the map edges
        if (!grid.HasCell(c)) return true;
        return wetCells.Contains(c);
    }

    // Convention matches MapLoader: north neighbour is (c.x, c.y-1); tile placed at (x,-y).
    private TileBase Pick(Vector2Int c, Func<Vector2Int, bool> pred, VariantSet v)
    {
        bool up = pred(new Vector2Int(c.x, c.y - 1));
        bool down = pred(new Vector2Int(c.x, c.y + 1));
        bool left = pred(new Vector2Int(c.x - 1, c.y));
        bool right = pred(new Vector2Int(c.x + 1, c.y));

        // isolated cell: no visible neighbours -> plain fill (avoids a lone corner sliver)
        if (!up && !down && !left && !right) return v.fill;

        // Inner Corners (concave)
        bool upLeft = pred(new Vector2Int(c.x - 1, c.y - 1));
        bool upRight = pred(new Vector2Int(c.x + 1, c.y - 1));
        bool downLeft = pred(new Vector2Int(c.x - 1, c.y + 1));
        bool downRight = pred(new Vector2Int(c.x + 1, c.y + 1));

        if (up && left && !upLeft && v.itl != null) return v.itl;
        if (up && right && !upRight && v.itr != null) return v.itr;
        if (down && left && !downLeft && v.ibl != null) return v.ibl;
        if (down && right && !downRight && v.ibr != null) return v.ibr;

        // Outer Corners (convex)
        if (!up && !left) return v.tl;
        if (!up && !right) return v.tr;
        if (!down && !left) return v.bl;
        if (!down && !right) return v.br;
        
        if (!up) return v.t;
        if (!down) return v.b;
        if (!left) return v.l;
        if (!right) return v.r;
        return (v.fillAlt != null && ((c.x + c.y) % 3 == 0)) ? v.fillAlt : v.fill;
    }

    // Repaint a single cell based on its current authoritative state.
    private void Repaint(Vector2Int c)
    {
        if (tilemap == null) return;
        var grid = Grid;
        if (grid == null || !grid.HasCell(c)) return;
        var cell = grid.GetCell(c);

        if (cell.corruptionState == 1)
        {
            TileBase chosenBurnt = ((c.x * 7 + c.y * 13) % 2 == 0) ? burnt_0 : burnt_1;
            tilemap.SetTile(ToTilemap(c), chosenBurnt);
        }
        else if (cell.corruptionState == 2)
            tilemap.SetTile(ToTilemap(c), dugTile);
        else if (wetCells.Contains(c))
            tilemap.SetTile(ToTilemap(c), Pick(c, IsWet, wet));
        else if (driedCells.Contains(c))
            tilemap.SetTile(ToTilemap(c), drySoilTile);
        // otherwise a normal base cell (grass/dirt border) — leave its existing tile untouched
    }

    // Repaint a cell plus its 4 neighbours (their edge variants depend on this cell).
    private void RepaintCluster(Vector2Int c)
    {
        Repaint(c);
        Repaint(new Vector2Int(c.x, c.y - 1));
        Repaint(new Vector2Int(c.x, c.y + 1));
        Repaint(new Vector2Int(c.x - 1, c.y));
        Repaint(new Vector2Int(c.x + 1, c.y));
    }

    // ---- public hooks called from gameplay ----

    // Shovel: corrupted -> dug. Neighbours' burnt edges may change.
    public void OnCellChanged(Vector2Int c) => RepaintCluster(c);

    // Watering: cell became wet (either purifying dug soil or watering normal soil).
    public void OnCellWatered(Vector2Int c)
    {
        wetCells.Add(c);
        driedCells.Remove(c);
        RepaintCluster(c);
    }

    // Evaporation: a previously-wet cell dried out.
    public void OnCellDried(Vector2Int c)
    {
        if (!wetCells.Contains(c)) return;
        wetCells.Remove(c);
        driedCells.Add(c);
        RepaintCluster(c);
    }

    // Auto-tile a static base-terrain region (dirt path / pond) so map authors only need to
    // paint solid fill cells - edges, outer corners, and concave inner corners are all picked
    // automatically from adjacency, same as the burnt/wet gameplay-state overlays.
    public void PaintStaticRegion(HashSet<Vector2Int> cellsToPaint, Func<Vector2Int, bool> neighborPred, VariantSet v)
    {
        if (tilemap == null || v == null || cellsToPaint == null) return;
        foreach (var c in cellsToPaint)
        {
            TileBase tile = Pick(c, neighborPred, v);
            if (tile != null) tilemap.SetTile(ToTilemap(c), tile);
        }
    }

    public void PaintStaticRegion(HashSet<Vector2Int> cells, VariantSet v)
    {
        PaintStaticRegion(cells, cells.Contains, v);
    }

    // Called after MapLoader finishes building the grid: paint the whole burnt region.
    public void RefreshAll()
    {
        var grid = Grid;
        if (grid == null || tilemap == null) return;
        foreach (var c in new List<Vector2Int>(grid.GetAllCoordinates()))
            Repaint(c);
    }

    public void ClearTracking()
    {
        wetCells.Clear();
        driedCells.Clear();
    }
}
