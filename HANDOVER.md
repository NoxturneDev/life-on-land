# Life on Land — Developer Handover

> Snapshot of everything built so far, so a new agent can continue seamlessly.
> Companion to `GEMINI.md` (the design spec). Read `GEMINI.md` first for game vision, then this.

**Project:** Unity 6 (6000.4.2f1), URP 2D. Main scene: `Assets/Scenes/BasicScene.unity`.
**Working through:** MCP-for-Unity bridge (HTTP `127.0.0.1:8080`). See "Working with this project" at the bottom — it has critical gotchas that will save you hours.

---

## Current state: Stage 1 (Red Region) is fully playable end-to-end

The full Stage 1 loop works and is verified in Play Mode:

1. Opening dialogue (Villain burns the oasis, Maliz begs for help).
2. **Quest "Help Maliz":** collect 10 water from the pond → deliver to Maliz → receive Desert Shrub seeds.
3. **Quest "Restore the Oasis":** purify 5 corrupted tiles (shovel → water), grow 5 Desert Shrubs to maturity, raise O2 to 18%.
4. On completion → villain farewell dialogue → `AdvanceStage()` (stage counter → 2) → "Stage 1 Complete" victory screen.

Stage 2 (Orange Region) and Stage 3 (Obsidian Caldera) are **not built** — completion currently shows an "Orange Region — coming soon" screen. The systems below are stage-agnostic and reusable for Stage 2.

---

## Core gameplay systems (all implemented & working)

### Terrain / grid
- `GridWorldMatrix.cs` — runtime dictionary of `GridCell` (moisture, localO2, soilQuality, `corruptionState` 0=normal/1=corrupted/2=dug, `isWaterSource`). **Not serialized** — rebuilt each play from the map file. Has `TilesPurifiedCount` (for quest tracking).
- `MapLoader.cs` — reads `Assets/Assets/maps/map_1.txt` (+ `tileData1.txt`) and paints the tilemap on `Start()`. Trees are spawned as separate `StaticTree_x_y` GameObjects, not tiles. Map is generated with **square** dirt-clearing + pond regions (axis-aligned rectangles = clean autotile corners). To regenerate: see `[[redregion_terrain_system]]` memory or re-run the generator logic in `MapLoader`/execute_code.
- `TerrainVisualManager.cs` — **neighbor-aware autotiling** for state feedback. When you shovel/water a cell, it repaints that cell + 4 neighbors picking fill/edge/corner variants so burnt/wet regions get organic shadowed borders (not flat squares). Burnt tiles = `cb_*`, wet tiles = `ws_*`, dug = `dug_0`, dry = `dirt_0`.
- `GridUtil.cs` — **single source of truth** for world↔grid conversion. `WorldToGrid()` uses the tilemap's `WorldToCell` (floor-based, robust). `GridToWorldCenter()` returns the cell center. Cursor, planting, and tile-painting all use this so everything snaps to the tile the mouse is over. **Always use GridUtil for any new world↔cell math** — do not hand-roll `round()`.

### Player
- `Player.cs` — inventory, stamina/O2 buffer, hotbar tools, planting, water extraction, building.
- `PlayerController.cs` — top-down Rigidbody2D movement (zeroes velocity during dialogue).
- **Hotbar layout** (slots 0–5, keys 1–6): `Shovel · Watering Can · Food · Seed · Build · (reserved)`. Food eats a ration (+stamina). Seed slot is stage-aware via `Player.CurrentStageSeedProfile` (Stage 1 = Desert Shrub).
- **Starting inventory:** 10 food (rations), 10 of the current stage's seed. Only populates if the serialized inventory is empty.
- Planting requires `corruptionState==0` AND `moisture>0` (must water the soil first).

### Plants / growth
- `Tree.cs` + `GrowthState.cs` — 5-stage FSM: `Seed → Sprout → Sapling → Young → MatureTree` (+ `Withered`). Grows from a small crop leaf into a full red tree. O2 output scales per stage via `Tree.GrowthO2Factor()` (shared with `EnvironmentManager`).
- `TreeProfile.cs` (ScriptableObjects in `Assets/Assets/profiles/`) — has `growthStageSprites[5]` + `witheredSprite`. Wired: DesertShrub, PineTree, SilkmothFern. Growth art in `Assets/Assets/plants/` (crops extracted 1:1 from `ref/Farm/Objects/Spring Crops.png`, final stages use `redtree_large_treeonly`/scaled `young_tree`).
- `TreePlant.prefab` — SpriteRenderer (spriteSortPoint=Pivot) + Tree; assigned to all 3 profiles.

### Environment
- `EnvironmentManager.cs` — atmospheric O2 calc, tick loop (moisture evaporation, tree growth, O2 diffusion every 5s), stage config (O2/tree goals), `AdvanceStage()`, victory eval.

### UI
- `UIManager.cs` — stat panel (stamina/O2 bars), hotbar icons/labels, stage name.
- `DialogueManager.cs` — VN-style bottom dialogue box; auto-swaps EventSystem to InputSystemUIInputModule.
- `NotificationManager.cs` — toast messages (top-center, fade in/out).
- `VictoryUI.cs` — full-screen stage-complete panel.
- `QuestChecklistUI.cs` + `QuestObjective.cs` — **live quest checklist** (top-right): titled panel, one row per objective (checkbox + label + progress), ticks green as completed. Driven by `Stage1Manager`.
- `TileCursor.cs` — gold highlight frame that snaps to the tile under the mouse (uses GridUtil).
- All UI art is procedural pixel-art in `Assets/UI/Pixel/` (panel_frame, slot_frame, bar_*, checkbox_*).

### Stage flow
- `Stage1Manager.cs` — owns the Stage 1 script: opening dialogue, both quest phases (defines `QuestObjective` lists + drives the checklist), Maliz interaction, and completion → `AdvanceStage()` + victory.

---

## Known bugs / things left to fix (for the next agent)

> The user specifically wants these finished. Verify each in Play Mode.

1. **Editor keeps auto-entering Play Mode between MCP calls.** Not a game bug per se, but it repeatedly caused scene-edit calls (`MarkSceneDirty`/`SaveOpenScenes`) to throw "cannot be used during play mode." Always check `Application.isPlaying` and `manage_editor stop` before Edit-Mode scene changes. (Might be an Editor setting the user has — worth confirming with them.)
2. **Opening dialogue re-triggers on some Play sessions** — the Villain/Maliz opening sometimes replays or its callback doesn't fire cleanly, leaving the water quest inactive until dialogue is advanced. `Stage1Manager` uses an `openingPlayed` guard + a one-frame `dialogueFreeLastFrame` gate (added to fix a dialogue↔interaction infinite loop that froze the player). Re-verify the opening → water-quest handoff works from a clean Play start with real keyboard/mouse input (most verification so far drove it via reflection, which bypasses the input path).
3. **"Missing script (Unknown)" console warnings** — several pre-existing, unrelated to recent work. Some GameObject in the scene references a deleted script. Worth hunting down and cleaning (`find_gameobjects` + inspect components) but non-blocking.
4. **`redtree_large.png` is a broken multi-object sheet** — contains the tree PLUS stray flower/berry decorations in one un-sliced sprite. Use `redtree_large_treeonly.png` instead (already done for planted trees & map trees). Don't reference `redtree_large.png`, `redtree_small.png`, or `redtree_medium.png` directly — they're contaminated. See `[[unity-mcp-gotchas]]`.
5. **Tile ColliderType trap** — any `Tile` asset created via `CreateInstance<Tile>()` defaults to `ColliderType.Sprite`. If the Ground tilemap has a `TilemapCollider2D`, that makes the tile a **solid wall** and can trap the player. All ground/state tiles must be `ColliderType.None`; only `wall_0` and `water_*` should be solid (`ColliderType.Grid`). This already bit us once. Re-check if you add new ground tiles.
6. **Pond/dirt corner tiles** — the water edge/corner tiles were deskewed + synthesized from the ref sheet; a couple of diagonal corner seams may be slightly thin where two corner tiles meet. Cosmetic. Square regions mostly avoid this now.

---

## Suggested next work (per GEMINI.md)

- **Stage 2 (Orange Region):** Oryel (Fox) NPC, orange tileset recolor, her mini-quest (purify 5 tiles to prove skill) + main quest (build 1 Soil Purifier, grow 8 Pine Trees, O2→21%). The quest checklist, growth FSM, and terrain systems are all reusable — mainly need: a Stage2Manager (mirror Stage1Manager), orange tile art, and a scene/area for it.
- **Buildings (hotbar slot 4):** `Player.ConstructInfrastructure` exists but buildings spawn invisible (no prefab/art). Soil Purifier / Irrigation Pipes / Biosphere Dome need prefabs + art. These are Stage 2/3 rewards.
- **Save/load:** none exists. `GEMINI.md` §4 specifies JSON (`SaveData.json`) with save_meta/environment_state/player_state/instantiated_objects.

---

## Working with this project (MCP-for-Unity gotchas — READ THIS)

These cost real time; they're documented in the auto-memory too (`~/.claude/.../memory/unity_mcp_gotchas.md`):

- **`execute_code` runs CodeDom (C# 6), not Roslyn.** No local functions. Fully-qualify UI types (`UnityEngine.UI.Image`, not `Image`). Avoid `Func<>` with many type params. Don't shadow outer variable names inside lambdas.
- **Screenshots are unreliable in Edit Mode** (crop to a corner / serve stale frames). Enter Play Mode to screenshot game visuals. For terrain-only checks, `capture_source:"scene_view"` works in Edit Mode.
- **Scene edits must be in Edit Mode** and followed by `MarkSceneDirty` + `SaveOpenScenes`, or they don't persist. Newly-created GameObjects can vanish if a domain reload happens before saving — save immediately.
- **After changing a `TreeProfile`/scene ref during Play Mode, it won't persist** — redo it in Edit Mode.
- **URP 2D sorting:** the Ground `TilemapRenderer` is set to `sortingOrder = -10` so sprites (trees/player, order 0+) always render above the ground. Transparency Sort Mode is Custom Axis (0,1,0) for Y-sorting. Don't revert these.
- **Verify with real gameplay, not just reflection.** Much of the quest verification drove managers via reflection, which skips the input/dialogue path — the bugs above (#2) live there.

## Key file map
```
Assets/Scripts/         all gameplay code (Player, Tree, EnvironmentManager, *Manager, *UI, GridUtil, ...)
Assets/Assets/maps/     map_1.txt, tileData1.txt
Assets/Assets/tiles/    ground + state tiles (redgrass_*, dirt_*, water_*, cb_* burnt, ws_* wet, dug_0, ...)
Assets/Assets/plants/   crop growth sprites, young_tree, icon_food, TreePlant.prefab
Assets/Assets/profiles/ TreeProfile ScriptableObjects (DesertShrub, PineTree, SilkmothFern)
Assets/Assets/blueprints/ BuildingBlueprint ScriptableObjects
Assets/UI/Pixel/        procedural UI sprites (panels, bars, slots, checkboxes)
Assets/Assets/ref/      REFERENCE ART (Farm crops, red/green woods tilesets, map screenshots) — extract 1:1 from here
Assets/Scenes/BasicScene.unity   the game
```

There's also a persistent auto-memory at `~/.claude/projects/C--Users-galih-.../memory/` with 3 notes: `redregion_terrain_system`, `pixel_ui_kit`, `unity_mcp_gotchas`. Read those for deeper detail on the art/terrain generation approach.
