using UnityEngine;
using UnityEngine.Tilemaps;

// Single source of truth for world <-> grid-cell conversions so the cursor, tile
// painting, and planted objects all snap to the same tile the mouse is over.
// gridPos = (col, row) with row increasing downward; tilemap cell = (col, -row).
public static class GridUtil
{
    private static Tilemap tilemap;

    private static Tilemap TM
    {
        get
        {
            if (tilemap == null)
            {
                var go = GameObject.Find("Ground");
                if (go != null) tilemap = go.GetComponent<Tilemap>();
            }
            return tilemap;
        }
    }

    // World point -> grid cell that visually contains it (floor, via the tilemap).
    public static Vector2Int WorldToGrid(Vector3 world)
    {
        if (TM != null)
        {
            Vector3Int c = TM.WorldToCell(world);
            return new Vector2Int(c.x, -c.y);
        }
        return new Vector2Int(Mathf.FloorToInt(world.x), -Mathf.FloorToInt(world.y));
    }

    public static Vector2Int WorldToGrid(Vector2 world) => WorldToGrid(new Vector3(world.x, world.y, 0f));

    // Grid cell -> world position of that cell's centre (where objects/cursor should sit).
    public static Vector3 GridToWorldCenter(Vector2Int g)
    {
        if (TM != null) return TM.GetCellCenterWorld(new Vector3Int(g.x, -g.y, 0));
        return new Vector3(g.x + 0.5f, -g.y + 0.5f, 0f);
    }
}
