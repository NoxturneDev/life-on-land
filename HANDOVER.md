# Life on Land — Developer Handover

> Snapshot of everything built so far, so a NEW SESSION can continue seamlessly.
> Read order: `GEMINI.md` (design spec) → this file → the auto-memory notes (see bottom).

**Project:** Unity 6 (6000.4.2f1), URP 2D. Main scene: `Assets/Scenes/BasicScene.unity`.
**Access:** MCP-for-Unity bridge (HTTP `127.0.0.1:8080`). The "Working with this project" section at the bottom lists gotchas that will save you hours — read it before touching anything.

---

## Current state: Stage 1 (Red Region) is fully playable end-to-end

Verified in Play Mode:
1. Opening dialogue (Villain burns the oasis, Maliz begs for help).
2. **Quest "Help Maliz"** (checklist): collect 10 water from the pond → deliver to Maliz → receive Desert Shrub seeds.
3. **Quest "Restore the Oasis"** (checklist): purify 5 corrupted tiles (shovel → water), grow 5 Desert Shrubs to maturity, raise O2 to 18%.
4. Completion → villain farewell dialogue → `AdvanceStage()` (stage counter → 2) → "Stage 1 Complete" victory screen.

Stage 2 (Orange Region) and Stage 3 (Obsidian Caldera) are **not built** — completion shows an "Orange Region — coming soon" screen. All systems below are stage-agnostic and reusable.

---

## Core gameplay systems (all implemented & working)

### Terrain / grid
- `GridWorldMatrix.cs` — runtime dict of `GridCell` (moisture, localO2, soilQuality, `corruptionState` 0=normal/1=corrupted/2=dug, `isWaterSource`). **Not serialized** — rebuilt each play from the map file. Has `TilesPurifiedCount` (quest tracking).
- `MapLoader.cs` — reads `Assets/Assets/maps/map_1.txt` (+ `tileData1.txt`) and paints the tilemap on `Start()`. Trees spawn as separate `StaticTree_x_y` GameObjects (sprite = `redtree_large_treeonly`), not tiles. Map uses **square** dirt-clearing + pond regions (axis-aligned rectangles = clean autotile corners).
- `TerrainVisualManager.cs` — **neighbor-aware autotiling** for state feedback. Shovel/water repaints a cell + neighbors, picking fill/edge/**corner (outer + inner concave)** variants so burnt/wet regions get organic shadowed borders. Burnt = `cb_*`, wet = `ws_*`, dug = `dug_0`, dry = `dirt_0`. (Note: `VariantSet` includes inner corners `itl/itr/ibl/ibr`; `IsBurnt/IsWet` treat out-of-bounds as same-state so no border is drawn on map edges.)
- `GridUtil.cs` — **SINGLE SOURCE OF TRUTH** for world↔grid conversion. `WorldToGrid()` uses the tilemap's `WorldToCell` (floor-based, robust); `GridToWorldCenter()` returns the cell center. Cursor, planting, and tile-painting all use it so everything snaps to the tile under the mouse. **Always use GridUtil for any new world↔cell math — never hand-roll `round()`** (round caused a half-tile misalignment bug that's now fixed).

### Player
- `Player.cs` — inventory, stamina/O2 buffer, hotbar tools, planting (at cell center via GridUtil), water extraction, building.
- `PlayerController.cs` — top-down Rigidbody2D movement (zeroes velocity during dialogue).
- **Hotbar** (slots 0–5, keys 1–6): `Shovel · Watering Can · Food · Seed · Build · (reserved)`. Food eats a ration (+stamina). Seed slot is stage-aware via `Player.CurrentStageSeedProfile` (Stage 1 = Desert Shrub).
- **Starting inventory:** 10 food (rations) + 10 of the current stage's seed. Only populates if the serialized inventory is empty.
- Planting requires `corruptionState==0` AND `moisture>0` (soil must be watered).

### Plants / growth
- `Tree.cs` + `GrowthState.cs` — **5-stage FSM**: `Seed → Sprout → Sapling → Young → MatureTree` (+ `Withered`). Grows a small crop leaf into a full red tree. O2 output scales per stage via `Tree.GrowthO2Factor()` (shared with `EnvironmentManager`).
- `TreeProfile.cs` (ScriptableObjects in `Assets/Assets/profiles/`) — `growthStageSprites[5]` + `witheredSprite`. Wired: DesertShrub, PineTree, SilkmothFern. Growth art in `Assets/Assets/plants/` (crops extracted 1:1 from `ref/Farm/Objects/Spring Crops.png`; final stages = scaled `young_tree` + `redtree_large_treeonly`).
- `TreePlant.prefab` — SpriteRenderer (spriteSortPoint=Pivot) + Tree; assigned to all 3 profiles.

### Environment
- `EnvironmentManager.cs` — atmospheric O2 calc, 5s tick loop (moisture evaporation, tree growth, O2 diffusion), stage config (O2/tree goals), `AdvanceStage()`, victory eval.

### UI (all procedural pixel-art in `Assets/UI/Pixel/`)
- `UIManager.cs` — stat panel (stamina/O2 bars), hotbar icons/labels, stage name.
- `DialogueManager.cs` — VN-style bottom box; auto-swaps EventSystem to InputSystemUIInputModule.
- `NotificationManager.cs` — toast messages (top-center, fade).
- `VictoryUI.cs` — full-screen stage-complete panel.
- `QuestChecklistUI.cs` + `QuestObjective.cs` — **live quest checklist** (top-right): titled panel, one row per objective (checkbox + label + progress), ticks green as completed. Robust immediate row-clearing when swapping quests.
- `TileCursor.cs` — gold highlight frame that snaps to the tile under the mouse (via GridUtil).

### Stage flow
- `Stage1Manager.cs` — owns Stage 1: opening dialogue, both quest phases (defines `QuestObjective` lists, drives the checklist), Maliz interaction, completion → `AdvanceStage()` + victory. Has an `openingPlayed` guard + a one-frame `dialogueFreeLastFrame` gate (fixes a dialogue↔interaction loop that froze the player).

---

## Map art / tile decision log (IMPORTANT for the next art task)

**Reference sheets** live in `Assets/Assets/ref/`:
- `ref/environtment/red_pixel_16_woods.png` and `free_pixel_16_woods.png` — **same layout, red vs green recolor.** 22×12 grid. Contains: 3 grass "blob" patches (plain / brown-rocky-rim / water-rim), a dense grass blob, and decorations (trees, rocks, mushrooms, flowers, grass tufts, reeds, lily pads, water ripples, stump, logs/bridge).
- `ref/topDown_baseTiles.png` — a DIFFERENT big sheet (33×64) that DOES have true multi-height **cliff tiles**. **The user chose NOT to use this sheet.**
- `ref/Farm/` — Spring Crops, Maple Tree, and **flowers** (user says flowers are covered here).

**Grass is stage-tinted** — every stage recolors grass (Stage 1 = red via `redgrass_*` / red_region extractions). Do NOT add green grass.

**CLIFF DECISION (settled): Option A.** `free_pixel_16_woods.png` has **no real cliff/elevation system** — the "cliff look" is just the `grass_dirt_patch` blob (grass with a brown rocky rim that wraps all sides = a decorative border, not a directional cliff face). We ALREADY have it: `Assets/Assets/tiles/red_region/grass_dirt_patch.png`. **So: no cliff tiles to create. The map stays flat; the rocky-rim blob is the accepted "raised" look.** (Real cliffs would require the excluded `topDown_baseTiles.png` + a second tilemap layer + collision — explicitly out of scope.)

**Already extracted from the woods sheet** (red, in `Assets/Assets/tiles/red_region/`): `grass_patch`, `grass_dirt_patch`, `grass_water_patch`, `tree_big`, `tree_bush_small`, `tree_pine_small`, `bush_cluster`, `mushroom_cluster`, `dirt_ground`.

**NOT yet extracted from the woods sheet (candidate future decoration work):** rocks/boulders (3 incl. rock-in-water), tree stump, grass tufts (tall grass), reeds/cattails, lily pads, water-ripple detail, dense dark-grass blob, small critter/branch. (Flowers = use `ref/Farm`; logs/bridge = skip.)

**Terrain tiles still genuinely missing** (from earlier analysis, if you want the reference's organic look): dirt→grass **inner/concave corners** (`dirt_itl/itr/ibl/ibr`) and water **inner corners** (`water_itl/…`). Outer corners + edges already exist. The autotiler already supports inner-corner slots in code.

---

## Known bugs / things to finish (user wants these done)

1. **Opening dialogue → water-quest handoff needs REAL-INPUT verification.** Most quest testing drove managers via reflection, which bypasses the actual keyboard/mouse/dialogue path. Re-verify from a clean Play start with real input that the opening plays, the water quest activates, and the checklist appears.
2. **Editor keeps auto-entering Play Mode between MCP calls** — repeatedly broke Edit-Mode scene edits. Always check `Application.isPlaying` and `manage_editor stop` before scene changes. (Possibly a user Editor setting — worth confirming.)
3. **"Missing script (Unknown)" console warnings** — several pre-existing, unrelated. Some scene GameObject references a deleted script. Non-blocking; worth cleaning.
4. **Do NOT reference** `redtree_large.png`, `redtree_small.png`, `redtree_medium.png` — contaminated multi-object sheets (contain stray decorations). Use `redtree_large_treeonly.png`.
5. **Tile ColliderType trap** — any `Tile` created via `CreateInstance<Tile>()` defaults to `ColliderType.Sprite`. If the Ground tilemap has a `TilemapCollider2D`, that tile becomes a **solid wall** and can trap the player. Ground/state tiles must be `ColliderType.None`; only `wall_0` + `water_*` are solid (`ColliderType.Grid`). This already bit us once.

---

## Suggested next work (per GEMINI.md)
- **Stage 2 (Orange Region):** Oryel (Fox) NPC, orange grass tint, her mini-quest (purify 5 tiles) + main quest (build 1 Soil Purifier, grow 8 Pine Trees, O2→21%). Reuse the checklist/growth/terrain systems; mainly need a `Stage2Manager` (mirror `Stage1Manager`) + orange recolor + area.
- **Buildings (hotbar slot 4):** `Player.ConstructInfrastructure` exists but buildings spawn invisible (no prefab/art). Soil Purifier / Irrigation Pipes / Biosphere Dome need prefabs + art (Stage 2/3 rewards).
- **Decoration pass:** extract the not-yet-extracted decorations above to enrich the map.
- **Save/load:** none exists. `GEMINI.md` §4 specifies JSON (`SaveData.json`).

---

## Working with this project (MCP-for-Unity gotchas — READ)
- **`execute_code` runs CodeDom (C# 6), not Roslyn.** No local functions. Fully-qualify UI types (`UnityEngine.UI.Image`, not `Image`). Avoid many-arg `Func<>`. Don't shadow outer var names inside lambdas.
- **Screenshots unreliable in Edit Mode** (crop to corner / stale frames). Enter Play Mode to screenshot game visuals; `capture_source:"scene_view"` works in Edit Mode for terrain.
- **Scene edits must be in Edit Mode**, followed by `MarkSceneDirty` + `SaveOpenScenes`, or they don't persist. New GameObjects can vanish if a domain reload happens before saving — save immediately.
- **Scene refs changed during Play Mode don't persist** — redo in Edit Mode.
- **URP 2D sorting:** Ground `TilemapRenderer` sortingOrder = **-10** (sprites at 0+ render above it); Transparency Sort Mode = Custom Axis (0,1,0). Don't revert.
- **Verify with REAL gameplay, not reflection** — the input/dialogue path is where remaining bugs hide (see bug #1).
- To regenerate the map or reload tiles: `GameObject.Find("Grid").GetComponent<MapLoader>().LoadMap()` (Edit Mode only).

## Key file map
```
Assets/Scripts/          gameplay code (Player, Tree, EnvironmentManager, *Manager, *UI, GridUtil, QuestObjective, ...)
Assets/Assets/maps/      map_1.txt, tileData1.txt
Assets/Assets/tiles/     ground+state tiles (redgrass_*, dirt_*, water_*, cb_* burnt, ws_* wet, dug_0, wall_0, ...)
Assets/Assets/tiles/red_region/   blob patches + tree/decoration extractions (grass_dirt_patch = the "cliff-look")
Assets/Assets/plants/    crop growth sprites, young_tree, icon_food, TreePlant.prefab
Assets/Assets/profiles/  TreeProfile SOs (DesertShrub, PineTree, SilkmothFern)
Assets/Assets/blueprints/ BuildingBlueprint SOs
Assets/UI/Pixel/         procedural UI sprites (panels, bars, slots, checkboxes)
Assets/Assets/ref/       REFERENCE ART — extract 1:1 (woods sheets, Farm crops/flowers; topDown_baseTiles = excluded)
Assets/Scenes/BasicScene.unity   the game
```

Auto-memory (deeper detail) at `~/.claude/projects/C--Users-galih-.../memory/`: `redregion_terrain_system`, `pixel_ui_kit`, `unity_mcp_gotchas`.
