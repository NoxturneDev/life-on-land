# Handover Document — Phase 2: Refinements & Sizing

This document summarizes the changes, asset updates, and structural configurations implemented during this session for the next agent to pick up.

---

## 1. Summary of Accomplishments

### A. Sizing & Sizing Fixes (Character Assets)
- **Real Maliz Asset Integration:** Assigned the newly added **`maliz.png`** character sprite to the `SpriteRenderer` of the `Maliz` NPC in the scene and set it as `Stage1Manager.malizPortrait` for dialogue.
- **Pixel-Perfect Scaling Fix:** Corrected a bug where Maliz looked tiny and the player character (**Umbra**) shrunk to 32% of their size when walking.
  - **Cause:** `maliz.png` and the walking frame textures (`Umbra-front-walk1/2/3.png`) were imported at default **100 PPU** and **Bilinear** filtering, whereas standard characters use **32 PPU** and **Point (no filter)**.
  - **Resolution:** Reconfigured the texture import settings on all four assets to `32 PPU`, `Point (no filter)`, and `Uncompressed`. They now render at their correct pixel-perfect scale and do not glitch or shrink when transitioning between standing still and walking.

### B. Crisp UI Font Rendering
- **Restoration Progress Fix:** The dynamic "Restoration Progress: 0%" text was very blurry due to low font resolution rendering and stretching.
- **Resolution:** Modified **[UIManager.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/UIManager.cs)** (`CreateProgressUI()` and existing panel checks) to render the font at **33px** (3x resolution) and scaled down the local scale of the `RectTransform` to **0.3333f** (with shadow offsets scaled to `(3, -3)`). The text is now rendered high-res and is perfectly crisp and legible at any scale.

### C. Direct Scene Tilemap Loading Mode
- Modified **[MapLoader.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/MapLoader.cs)** to add non-destructive scene loading:
  - **`loadFromTextFile`** (defaults to `false` in scene): When `false`, the game directly scans the pre-existing tiles painted on the `Ground` tilemap inside the active scene rather than wiping and rebuilding it from `map_1.txt` on startup.
  - **`autoTileOnStart`** (defaults to `false` in scene): Prevents the autotiler from overriding custom borders painted in the editor.
  - **`InitializeGridFromTilemap()`**: Loops over the tilemap's actual bounds, populates the runtime `GridWorldMatrix` cell database, spawns static tree gameobjects from tree tile placements, and initializes progress.

### D. Protected Autotiling for Burnt & Dug Tiles
- Discovered that the autotiler was overwriting burnt (`burnt_1`) and dug (`dug_0`) tiles with normal dirt tiles on startup because it treated all soil states as generic dirt cells.
- Modified **[TerrainVisualManager.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/TerrainVisualManager.cs)** to support a new overload of `PaintStaticRegion()` that accepts a neighbor-check predicate.
- Modified **[MapLoader.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/MapLoader.cs)** to separate soil cells into:
  - `allSoilCells`: Used for neighbor adjacency (dirt, dug, burnt).
  - `paintDirtCells`: Only the tiles to be painted as tilled dirt.
- This ensures dirt borders autotile cleanly against burnt/dug areas, while preserving the actual burnt and dug tiles on the tilemap.
- **Fixed Burnt Visuals:** Assigned `burnt_0` and `burnt_1` tile assets to `TerrainVisualManager` in the scene (they were previously unset, causing the tiles to be cleared and rendering as blue background gaps).

### E. Dynamic Tilemap Bounds & Offset Support
- Fixed a bug where **"Export painted Tilemap to map_1.txt"** only exported a hardcoded `45x30` grid starting at `(0,0)`, ignoring tiles painted outside this box (such as the actual bounds `[-30, 66]`, `[-49, 16]`).
- Modified `ExportMap()` to dynamically detect the bounding box of non-empty tiles, write an `#offset minX maxY` header to the first line of `map_1.txt`, and export the entire bounding region.
- Modified `LoadMap()` to parse the `#offset` header, shifting the loaded tiles relative to that offset.

### F. Character Walking Animation
- Created custom animation clips and controller for the Player character (**Umbra**):
  - **`Player_Idle.anim`**: Displays `Umbra-front` sprite.
  - **`Player_Walk.anim`**: Cycles through `Umbra-front-walk1`, `Umbra-front-walk2`, `Umbra-front-walk3`, and `Umbra-front-walk2` sprites at 8 FPS.
  - **`Player_Controller.controller`**: Manages transition between `Idle` and `Walk` based on the `Speed` parameter (which is updated dynamically by `PlayerController.cs`).
  - Added an **Animator** component to the **Player** GameObject and assigned the new controller.

### G. Achievements Menu & Crisp Pause UI
- **Achievements Menu:** Implemented a new, dynamic Achievements Sub-Panel inside **[PauseMenu.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/PauseMenu.cs)**. Clicking "Achievements" now seamlessly opens a dedicated Achievements view tracking three Stage 1 milestones:
  1. **First Steps:** Talk to Maliz the Bear.
  2. **Water Bearer:** Deliver 10 cans of water to Maliz.
  3. **Green Oasis:** Restore the oasis to 18% O2 and complete Stage 1.
- **Dynamic Quest Reflection:** Uses Reflection to securely read Stage 1 quest completion states from the private fields in `Stage1Manager.Instance` without breaking encapsulation.
- **Crisp Text Scaling Optimization:** Fixed the extremely small, blurry font on all Pause Menu buttons, title, back button, and achievements text. They now render at **33px / 48px** (3x font size) and are scaled down to **0.3333 local scale** on their `RectTransform` (along with 3x scaled shadows), making them crystal clear and readable at any resolution.
- **Recreation Safeguard:** Added logic to destroy the outdated pre-existing `PauseMenuPanel` in the canvas on start, allowing the manager to rebuild the updated high-res layout dynamically.

### H. Dead Tree Environment Asset Creation
- **Dead Tree Sprite (`dead_tree.png`):** Generated a custom dead tree texture at `Assets/Assets/tiles/red_region/dead_tree.png` by reading the red foliage tree sprite, desaturating the leaves to a dry ash-grey/dead-brown color, and setting its import parameters to standard **32 PPU**, **Point (no filter)**, and **BottomCenter pivot alignment**.
- **Dead Tree Prefab (`DeadTree.prefab`):** Generated a ready-to-use prefab at `Assets/Assets/tiles/red_region/DeadTree.prefab` containing a `SpriteRenderer` (with Y-sort pivot sorting enabled) and a custom `BoxCollider2D` base trunk collider (`size=(0.8, 0.4)`, `offset=(0.0, 0.2)`). Map designers can now drag-and-drop this prefab directly from the Project panel to manually paint dead trees with physical collision onto the map layout.

### I. Dynamic Map Randomization & Spawning
- **Soil Layout Generation:** Updated `InitializeGridFromTilemap()` in `MapLoader.cs` to dynamically construct a massive `44x20` dirt clearing centered around the player's spawn and Maliz the Bear NPC (from `x = -12` to `32`, `y = 5` to `25`).
- **4-Quadrant Burnt Soil Layout:** Within this clearing, the burnt soil is divided into **4 distinct square patches** (Top-Left, Top-Right, Bottom-Left, Bottom-Right, each `12x5` cells) instead of a single giant mass. This leaves a clean horizontal path (`y = 13..16`) and vertical path (`x = 6..13`) of clean dirt. Player spawn `(9, 8)` and Maliz `(10, 10)` are located in these clean paths, letting them stand and walk on clean soil. Additionally, the quadrants are inset (starting at `x = -6` on the left, `y = 8` at the top, and ending at `y = 21` at the bottom) to provide a clean dirt buffer zone, preventing any overlap with the surrounding grass/water tile borders.
- **Dense Forest Dead Tree Spawning:** Configured `MapLoader.cs` to dynamically spawn the collision-enabled `DeadTree` prefab on **22% of the grass cells** (outside the clearing bounds) at play start. This populates the surrounding empty red grass area with a dense forest of ~480 dead trees, resolving the "empty flat look" while keeping the planting clearing completely open and clear for walking.
- **50% Oxygen Restoration Buffer:** Increased the Stage 1 oxygen restoration target from 18% to **50% O2** in `EnvironmentManager.cs`, `Stage1Manager.cs`, `PauseMenu.cs`, and `GEMINI.md` to match the Technical Specification, updating dialogue text and quest checklist UI accordingly.
- **Fixed Speed Penalty O2 Threshold:** Resolved an issue where increasing the Stage 1 O2 target to 50% caused the player to walk in slow motion (at 30% speed) everywhere. Modified `Player.cs` to decouple the speed penalty threshold from the stage quest goal, locking the oxygen safety threshold to a fixed physical constant of **18.0% O2** (so players move at normal 83% speed in low 15% O2 environments, and full 100% speed once purified).
- **Scoping Fix (CS0136):** Resolved a local variable naming collision where `allGOs` was declared twice in `InitializeGridFromTilemap()`, fixing compilation.

---

## 2. Key Files Map

* **Scripts:**
  - **[PauseMenu.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/PauseMenu.cs)**: Custom dynamic Pause Menu and Achievements UI creation, reflection checks, and text crispness fixes.
  - **[MapLoader.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/MapLoader.cs)**: Custom scene loading, offset-aware export/load logic.
  - **[TerrainVisualManager.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/TerrainVisualManager.cs)**: Autotile overrides, neighbor-check predicate overload, burnt/dug rendering.
  - **[UIManager.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/UIManager.cs)**: Overall progress panel creation, text scaling crispness fix.
  - **[PlayerController.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/PlayerController.cs)**: Player movement, dash, and animator `Speed` updates.
  - **[Stage1Manager.cs](file:///c:/Users/galih/Documents/Projects/Game/My%20project/Assets/Scripts/Stage1Manager.cs)**: Stage 1 dialogue and quest line configurations.
* **Character & Environment Assets:**
  - `Assets/Assets/characters/maliz.png` (Assigned to Maliz NPC in the scene and dialogue portrait)
  - `Assets/Assets/characters/Umbra-front.png` (Idle sprite)
  - `Assets/Assets/characters/Umbra-front-walk1/2/3.png` (Walking frames)
  - `Assets/Assets/characters/Player_Controller.controller` (Animator controller)
  - `Assets/Assets/tiles/red_region/dead_tree.png` (Dead tree texture, 32 PPU, Point filter)
  - `Assets/Assets/tiles/red_region/DeadTree.prefab` (Collision-enabled drag-and-drop dead tree prefab)

---

## 3. Recommendations for the Next Agent

1. **Stage 2 (Orange Region) Development:**
   - Create a `Stage2Manager.cs` mirroring `Stage1Manager.cs` structure.
   - Set up the Fox NPC (**Oryel**), her mini-quest dialogue, and logic to trigger when the boundary gate to the orange region is cleared.
2. **Assign Sprite Assets via Editor / Code:**
   - Any new character sprites added (like Oryel or Stage 3's Pyper) must be imported as **Sprite (2D and UI)** with **32 Pixels Per Unit**, **Point (no filter)**, and **Uncompressed** to match the pixel-art aesthetic.
3. **Visually Implement Buildings (Soil Purifier, Irrigation Pipes):**
   - The building placement mechanics are fully functional in code, but the placed gameobjects are currently invisible placeholders. Set up custom physical art sprites and prefabs for them.
4. **Local Serialization:**
   - Set up the Flat SQLite / JSON (`SaveData.json`) parser to store/restore terrain state, O2 percentage, and player coordinates.


