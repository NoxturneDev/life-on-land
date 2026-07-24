import os
import urllib.request
import zlib
import base64
import string
from PIL import Image

plantuml_alphabet = string.ascii_uppercase + string.ascii_lowercase + string.digits + '-_'
base64_alphabet  = string.ascii_uppercase + string.ascii_lowercase + string.digits + '+/'
translation_table = str.maketrans(base64_alphabet, plantuml_alphabet)

def encode_plantuml(text):
    compressor = zlib.compressobj(level=9, method=zlib.DEFLATED, wbits=-15)
    deflated = compressor.compress(text.encode('utf-8')) + compressor.flush()
    b64 = base64.b64encode(deflated).decode('ascii')
    return b64.translate(translation_table)

def render_diagram(name, puml_code, diagrams_dir):
    png_path = os.path.join(diagrams_dir, f"{name}.png")
    jpg_path = os.path.join(diagrams_dir, f"{name}.jpg")

    data = None

    # Method 1: Kroki POST Endpoint
    try:
        url = 'https://kroki.io/plantuml/png'
        req = urllib.request.Request(url, data=puml_code.encode('utf-8'), headers={
            'Content-Type': 'text/plain; charset=utf-8',
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'
        })
        with urllib.request.urlopen(req) as resp:
            data = resp.read()
            print(f"Kroki POST Success for {name}! ({len(data)} bytes)")
    except Exception as e:
        print(f"Kroki POST failed for {name}: {e}")

    # Method 2: PlantUML Official Server with Raw Deflate (wbits=-15)
    if not data or len(data) < 1000:
        try:
            encoded = encode_plantuml(puml_code)
            url = f"http://www.plantuml.com/plantuml/png/{encoded}"
            req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
            with urllib.request.urlopen(req) as resp:
                data = resp.read()
                print(f"PlantUML Raw Deflate GET Success for {name}! ({len(data)} bytes)")
        except Exception as e:
            print(f"PlantUML GET failed for {name}: {e}")

    if data:
        with open(png_path, "wb") as f:
            f.write(data)
        print(f"SUCCESS: Saved PNG -> {png_path}")

        # Convert to high-resolution JPG with white background
        img = Image.open(png_path)
        if img.mode in ('RGBA', 'LA') or (img.mode == 'P' and 'transparency' in img.info):
            bg = Image.new('RGB', img.size, (255, 255, 255))
            bg.paste(img, mask=img.split()[-1] if img.mode == 'RGBA' else None)
            bg.save(jpg_path, 'JPEG', quality=95)
        else:
            img.convert('RGB').save(jpg_path, 'JPEG', quality=95)
        print(f"SUCCESS: Saved JPG -> {jpg_path}")
    else:
        print(f"ERROR: Could not render diagram {name}")

def main():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    diagrams_dir = os.path.join(sub_dir, "diagrams")
    os.makedirs(diagrams_dir, exist_ok=True)

    print("Rendering 5 Real PlantUML Diagrams via Guaranteed Kroki/PlantUML Engine...")

    puml_use_case = """@startuml UseCase_LifeOnLand
left to right direction
actor Pemain as Player

rectangle "Life on Land" {
  usecase "Memulai Permainan" as UC1
  usecase "Menggerakkan & Dash Karakter" as UC2
  usecase "Memilih Slot Hotbar" as UC3
  usecase "Menyekop Ubin Corrupted" as UC4
  usecase "Menyiram/Memurnikan Ubin" as UC5
  usecase "Mengisi Ulang Gembor" as UC6
  usecase "Menanam Benih" as UC7
  usecase "Membangun Infrastruktur" as UC8
  usecase "Berinteraksi dengan NPC" as UC9
  usecase "Mengelola Kuesti" as UC10
  usecase "Menjeda Permainan" as UC11
  usecase "Melihat Pencapaian" as UC12
  usecase "Konsumsi Stamina" as UCe1
  usecase "Memicu Dialog" as UCe2
}

Player --> UC1
Player --> UC2
Player --> UC3
Player --> UC4
Player --> UC5
Player --> UC6
Player --> UC7
Player --> UC8
Player --> UC9
Player --> UC11
Player --> UC12

UC4 ..> UCe1 : <<include>>
UC5 ..> UCe1 : <<include>>
UC7 ..> UCe1 : <<include>>
UC9 ..> UCe2 : <<extend>>
UC9 ..> UC10 : <<extend>>
@enduml"""

    puml_class = """@startuml Class_LifeOnLand
class Player {
  -currentStamina : float
  -localO2Buffer : float
  -activeHotbarSlot : int
  -inventory : List<InventoryItem>
  +UseActiveTool(Vector2)
  +ExecutePlantAction(TreeProfile, Vector2)
  +ConsumeStamina(float)
}
class PlayerController {
  +moveSpeed : float
  +dashSpeed : float
  -PerformDash()
}
class EnvironmentManager {
  -globalO2Percentage : float
  +currentLevel : int
  +RecalculateAtmosphericComposition()
  +ExecuteStateTick()
  +EvaluateVictoryState() : bool
}
class GridWorldMatrix {
  +TilesPurifiedCount : int
  +PurifyTileShovel(Vector2Int) : bool
  +PurifyTileWater(Vector2Int) : bool
}
class GridCell {
  +moisture : float
  +localO2 : float
  +corruptionState : int
  +placedObject : WorldObject
}
class WorldObject {
  +ObjectID : string
  +GridCoordinates : Vector2Int
}
class Tree {
  -currentFSMState : GrowthState
  +ProgressGrowthCycle()
  +TransitionToWitheredState()
  +Revive()
}
class TreeProfile <<ScriptableObject>> {
  +treeTypeID : string
  +o2EmissionRate : float
  +waterRequirement : int
}
class BuildingBlueprint <<ScriptableObject>> {
  +buildingName : string
  +waterCost : int
}
class Stage1Manager {
  +GetOverallProgressFraction() : float
}
class DialogueManager
class QuestChecklistUI
class QuestObjective {
  +current : int
  +target : int
  +IsComplete : bool
}
class UIManager
class NotificationManager
class AudioManager
class MainMenuManager
class PauseMenu
class VictoryUI

WorldObject <|-- Tree
Player *-- PlayerController
Player o-- "0..*" InventoryItem
Tree "0..*" --> "1" TreeProfile : uses
EnvironmentManager *-- GridWorldMatrix
GridWorldMatrix o-- "0..*" GridCell
GridCell o-- "0..1" WorldObject
Stage1Manager --> DialogueManager
Stage1Manager --> QuestChecklistUI
QuestChecklistUI o-- "0..*" QuestObjective
Stage1Manager --> EnvironmentManager
UIManager --> Player : observes
UIManager --> EnvironmentManager : observes
Player --> BuildingBlueprint : uses
@enduml"""

    puml_activity = """@startuml Activity_Purifikasi
start
:Pemain memilih Slot Hotbar;
if (Slot = 1 (Sekop)?) then (ya)
  if (Stamina >= 5 & Tile Corrupted?) then (ya)
    :PurifyTileShovel() -> Tile jadi DugBurnt;
    :Kurangi 5 Stamina, tampilkan notifikasi;
  else (tidak)
    :Tidak ada aksi;
  endif
else (Slot = 2 (Gembor))
  if (Target = sumber air?) then (ya)
    :Isi ulang gembor;
  else (tidak)
    if (Tile DugBurnt?) then (ya)
      :PurifyTileWater() -> Normal, moisture 1.0;
      :TilesPurifiedCount++, notifikasi "Tile Purified!";
    else (tidak)
      :Siram tanaman/tanah langsung;
    endif
  endif
endif
stop
@enduml"""

    puml_component = """@startuml Component_LifeOnLand
package "Presentation Layer" {
  [UIManager]
  [DialogueManager]
  [QuestChecklistUI]
  [NotificationManager]
  [MainMenuManager]
  [PauseMenu]
  [VictoryUI]
}
package "Gameplay Logic Layer" {
  [Player]
  [PlayerController]
  [EnvironmentManager]
  [GridWorldMatrix]
  [Tree]
  [Stage1Manager]
  [CameraFollow]
}
package "Content Data Layer" {
  [TreeProfile (ScriptableObject)]
  [BuildingBlueprint (ScriptableObject)]
}
package "Persistence Layer" {
  [Local JSON SaveData]
  [PlayFab Cloud Sync (rancangan)]
}
package "Rendering Layer" {
  [TerrainVisualManager]
  [Unity Tilemap & SpriteRenderer]
}

[Presentation Layer] --> [Gameplay Logic Layer]
[Gameplay Logic Layer] --> [Content Data Layer]
[Gameplay Logic Layer] --> [Persistence Layer]
[Gameplay Logic Layer] --> [Rendering Layer]
[Persistence Layer] ..> [PlayFab Cloud Sync (rancangan)] : sinkronisasi opsional
@enduml"""

    puml_deployment = """@startuml Deployment_LifeOnLand
node "Client Device (PC / Browser)" {
  node "Unity Runtime (Standalone / WebGL)" {
    [Life on Land Client Build]
  }
}
node "Local Storage" {
  database "SaveData.json / PlayerPrefs" as LocalDB
}
cloud "PlayFab Backend-as-a-Service\n(Rancangan Arsitektur Tahap Lanjut)" {
  database "Title Data / Leaderboard" as CloudDB
}

[Life on Land Client Build] --> LocalDB : baca/tulis progres lokal
[Life on Land Client Build] ..> CloudDB : sinkronisasi cloud save\n& leaderboard (opsional)
@enduml"""

    diagrams = [
        ("1_USE_CASE_DIAGRAM", puml_use_case),
        ("2_CLASS_DIAGRAM", puml_class),
        ("3_ACTIVITY_DIAGRAM", puml_activity),
        ("4_COMPONENT_DIAGRAM", puml_component),
        ("5_DEPLOYMENT_ARCHITECTURE", puml_deployment),
    ]

    for name, code in diagrams:
        render_diagram(name, code, diagrams_dir)

if __name__ == "__main__":
    main()
