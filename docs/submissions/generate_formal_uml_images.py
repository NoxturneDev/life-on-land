import os
import subprocess
from PIL import Image

def generate_formal_uml():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    diagrams_dir = os.path.join(sub_dir, "diagrams")
    os.makedirs(diagrams_dir, exist_ok=True)

    chrome_path = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
    if not os.path.exists(chrome_path):
        chrome_path = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

    print(f"Generating 5 Formal UML & Architecture Diagram Image files...")

    # HTML 1: Formal Use Case Diagram
    html_use_case = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;600;700&family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1500px; height: 1000px;
    background: #f8fafc; color: #0f172a;
    font-family: 'JetBrains Mono', monospace;
    padding: 36px; display: flex; flex-direction: column; justify-content: space-between;
    background-image: radial-gradient(#e2e8f0 1px, transparent 1px);
    background-size: 20px 20px;
  }
  .title-bar {
    border-bottom: 3px solid #0f172a; padding-bottom: 12px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-size: 24px; font-weight: 700; color: #0f172a; letter-spacing: -0.5px; }
  .title-bar span { font-size: 13px; color: #475569; font-weight: 600; }

  .diagram-container {
    display: flex; gap: 50px; align-items: center; justify-content: center; flex: 1; margin-top: 30px;
  }

  .actor-box {
    background: #ffffff; border: 2px solid #0f172a; border-radius: 8px; padding: 24px 20px;
    text-align: center; width: 220px; box-shadow: 4px 4px 0px #0f172a;
  }
  .actor-svg { width: 60px; height: 90px; margin: 0 auto 12px auto; }
  .actor-name { font-size: 16px; font-weight: 700; color: #0f172a; }

  .system-boundary {
    background: #ffffff; border: 2px solid #0f172a; border-radius: 12px; padding: 30px;
    flex: 1; display: grid; grid-template-columns: 1fr 1fr; gap: 16px; position: relative;
    box-shadow: 6px 6px 0px #cbd5e1;
  }
  .boundary-title {
    position: absolute; top: -14px; left: 30px; background: #93c5fd; color: #1e3a8a;
    font-size: 13px; font-weight: 700; padding: 2px 14px; border: 2px solid #0f172a; border-radius: 4px;
  }

  .use-case {
    background: #ffffff; border: 2px solid #0f172a; border-radius: 30px; padding: 10px 16px;
    font-size: 13px; font-weight: 600; text-align: center; color: #0f172a;
    display: flex; align-items: center; justify-content: center; height: 50px;
    box-shadow: 2px 2px 0px #94a3b8;
  }
  .use-case.inc { background: #fef08a; border-color: #854d0e; }
  .use-case.ext { background: #fbcfe8; border-color: #9d174d; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>Use Case Diagram — Life on Land System</h1>
    <span>FIGURE F.1: FORMAL UML USE CASE SPECIFICATION</span>
  </div>

  <div class="diagram-container">
    <div class="actor-box">
      <svg class="actor-svg" viewBox="0 0 60 90">
        <circle cx="30" cy="15" r="12" fill="none" stroke="#0f172a" stroke-width="3"/>
        <line x1="30" y1="27" x2="30" y2="60" stroke="#0f172a" stroke-width="3"/>
        <line x1="10" y1="40" x2="50" y2="40" stroke="#0f172a" stroke-width="3"/>
        <line x1="30" y1="60" x2="12" y2="85" stroke="#0f172a" stroke-width="3"/>
        <line x1="30" y1="60" x2="48" y2="85" stroke="#0f172a" stroke-width="3"/>
      </svg>
      <div class="actor-name">Player</div>
      <div style="font-size: 11px; color: #64748b; margin-top: 4px;">&lt;&lt;actor&gt;&gt;</div>
    </div>

    <div class="system-boundary">
      <div class="boundary-title">System Boundary: Life on Land</div>
      <div class="use-case">UC1: Memulai Permainan</div>
      <div class="use-case">UC2: Menggerakkan &amp; Dash Karakter</div>
      <div class="use-case">UC3: Memilih Slot Hotbar</div>
      <div class="use-case inc">UC4: Menyekop Ubin Corrupted</div>
      <div class="use-case inc">UC5: Menyiram/Memurnikan Ubin</div>
      <div class="use-case">UC6: Mengisi Ulang Gembor Air</div>
      <div class="use-case inc">UC7: Menanam Benih Vegetasi</div>
      <div class="use-case">UC8: Membangun Infrastruktur</div>
      <div class="use-case ext">UC9: Berinteraksi dengan NPC</div>
      <div class="use-case ext">UC10: Mengelola Quest Checklist</div>
      <div class="use-case">UC11: Menjeda Permainan (Pause)</div>
      <div class="use-case">UC12: Melihat Pencapaian</div>
      <div class="use-case inc" style="grid-column: span 2;">&lt;&lt;include&gt;&gt; UCe1: Konsumsi Stamina Pemain (5 Stamina)</div>
    </div>
  </div>
</body>
</html>"""

    # HTML 2: Formal Class Diagram (Exact look of User's sample image)
    html_class = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1650px; height: 1150px;
    background: #f8fafc; color: #0f172a;
    font-family: 'JetBrains Mono', monospace;
    padding: 36px; display: flex; flex-direction: column; justify-content: space-between;
    background-image: radial-gradient(#cbd5e1 1px, transparent 1px);
    background-size: 20px 20px;
  }
  .title-bar {
    border-bottom: 3px solid #0f172a; padding-bottom: 12px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-size: 24px; font-weight: 700; color: #0f172a; }
  .title-bar span { font-size: 13px; color: #475569; font-weight: 600; }

  .class-grid {
    display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px; margin-top: 24px; flex: 1;
  }

  .class-card {
    background: #ffffff; border: 2px solid #0f172a; border-radius: 12px; overflow: hidden;
    box-shadow: 4px 4px 0px #0f172a; display: flex; flex-direction: column;
  }

  .class-header {
    background: #93c5fd; color: #0f172a; padding: 10px 14px; font-weight: 700; font-size: 15px;
    border-bottom: 2px solid #0f172a; text-align: center;
  }
  .class-card.purple .class-header { background: #c084fc; }
  .class-card.yellow .class-header { background: #fde047; }
  .class-card.green .class-header { background: #86efac; }

  .class-section { padding: 10px 12px; font-size: 12px; line-height: 1.5; }
  .class-section.methods { border-top: 2px solid #0f172a; background: #fafafa; flex: 1; }

  .attr { color: #0f172a; }
  .meth { color: #0369a1; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>Class Diagram — Life on Land Core Architecture</h1>
    <span>FIGURE F.2: FORMAL UML CLASS SPECIFICATION</span>
  </div>

  <div class="class-grid">

    <!-- Player -->
    <div class="class-card blue">
      <div class="class-header">Player</div>
      <div class="class-section">
        <div class="attr">+currentStamina: float</div>
        <div class="attr">+localO2Buffer: float</div>
        <div class="attr">+activeHotbarSlot: int</div>
        <div class="attr">+inventory: List&lt;Item&gt;</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+UseActiveTool()</div>
        <div class="meth">+ExecutePlantAction()</div>
        <div class="meth">+ConsumeStamina()</div>
      </div>
    </div>

    <!-- PlayerController -->
    <div class="class-card blue">
      <div class="class-header">PlayerController</div>
      <div class="class-section">
        <div class="attr">+moveSpeed: float</div>
        <div class="attr">+dashSpeed: float</div>
      </div>
      <div class="class-section methods">
        <div class="meth">-PerformDash()</div>
        <div class="meth">+ProcessMovement()</div>
      </div>
    </div>

    <!-- EnvironmentManager -->
    <div class="class-card purple">
      <div class="class-header">EnvironmentManager</div>
      <div class="class-section">
        <div class="attr">-globalO2Percentage: float</div>
        <div class="attr">+currentLevel: int</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+RecalculateAtmosphere()</div>
        <div class="meth">+ExecuteStateTick()</div>
        <div class="meth">+EvaluateVictoryState()</div>
      </div>
    </div>

    <!-- GridWorldMatrix -->
    <div class="class-card purple">
      <div class="class-header">GridWorldMatrix</div>
      <div class="class-section">
        <div class="attr">+TilesPurifiedCount: int</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+PurifyTileShovel(): bool</div>
        <div class="meth">+PurifyTileWater(): bool</div>
        <div class="meth">+GetCell(): GridCell</div>
      </div>
    </div>

    <!-- GridCell -->
    <div class="class-card green">
      <div class="class-header">GridCell</div>
      <div class="class-section">
        <div class="attr">+moisture: float</div>
        <div class="attr">+localO2: float</div>
        <div class="attr">+corruptionState: int</div>
        <div class="attr">+placedObject: WorldObject</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+SetMoisture()</div>
      </div>
    </div>

    <!-- WorldObject -->
    <div class="class-card green">
      <div class="class-header">WorldObject</div>
      <div class="class-section">
        <div class="attr">+ObjectID: string</div>
        <div class="attr">+GridCoordinates: Vector2</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+Initialize()</div>
      </div>
    </div>

    <!-- Tree -->
    <div class="class-card green">
      <div class="class-header">Tree</div>
      <div class="class-section">
        <div class="attr">-currentFSMState: GrowthState</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+ProgressGrowthCycle()</div>
        <div class="meth">+TransitionToWithered()</div>
        <div class="meth">+Revive()</div>
      </div>
    </div>

    <!-- ScriptableObject -->
    <div class="class-card yellow">
      <div class="class-header">&lt;&lt;SO&gt;&gt; TreeProfile</div>
      <div class="class-section">
        <div class="attr">+treeTypeID: string</div>
        <div class="attr">+o2EmissionRate: float</div>
        <div class="attr">+waterRequirement: int</div>
      </div>
      <div class="class-section methods">
        <div class="meth">+GetProfileData()</div>
      </div>
    </div>

  </div>
</body>
</html>"""

    # HTML 3: Formal Activity Diagram
    html_activity = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1400px; height: 950px; background: #f8fafc; color: #0f172a;
    font-family: 'JetBrains Mono', monospace; padding: 36px; display: flex; flex-direction: column; justify-content: space-between;
    background-image: radial-gradient(#cbd5e1 1px, transparent 1px); background-size: 20px 20px;
  }
  .title-bar { border-bottom: 3px solid #0f172a; padding-bottom: 12px; display: flex; justify-content: space-between; align-items: center; }
  .title-bar h1 { font-size: 24px; font-weight: 700; color: #0f172a; }

  .flow-container { display: flex; flex-direction: column; align-items: center; justify-content: center; flex: 1; margin-top: 20px; gap: 14px; }
  .node-start { background: #0f172a; color: #ffffff; font-weight: 700; border-radius: 20px; padding: 6px 20px; font-size: 13px; }
  .node-act { background: #ffffff; border: 2px solid #0f172a; border-radius: 8px; padding: 10px 18px; font-size: 13px; font-weight: 600; box-shadow: 3px 3px 0px #94a3b8; }
  .node-dec { background: #fde047; border: 2px solid #0f172a; padding: 8px 18px; border-radius: 6px; font-weight: 700; font-size: 13px; }
  .flow-branches { display: flex; gap: 50px; width: 100%; justify-content: center; }
  .branch { display: flex; flex-direction: column; align-items: center; gap: 10px; background: #ffffff; padding: 18px; border-radius: 10px; border: 2px solid #0f172a; flex: 1; box-shadow: 4px 4px 0px #cbd5e1; }
  .arrow { color: #0f172a; font-weight: 700; font-size: 16px; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>Activity Diagram — Two-Step Tile Purification Flow</h1>
    <span>FIGURE F.3: FORMAL UML ACTIVITY SPECIFICATION</span>
  </div>

  <div class="flow-container">
    <div class="node-start">● START</div>
    <div class="arrow">↓</div>
    <div class="node-act">Select Hotbar Slot &amp; Target Grid Tile</div>
    <div class="arrow">↓</div>
    <div class="node-dec">Decision: Active Hotbar Slot?</div>

    <div class="flow-branches">
      <div class="branch">
        <div style="font-weight: 700; color: #0369a1;">[Slot = 1 (Shovel)]</div>
        <div class="arrow">↓</div>
        <div class="node-act">Check Stamina &gt;= 5 &amp; State = CorruptedBurnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Execute PurifyTileShovel() ➔ Tile = DugBurnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Deduct 5 Stamina &amp; Display Toast "Tile Cleared"</div>
      </div>

      <div class="branch">
        <div style="font-weight: 700; color: #047857;">[Slot = 2 (Watering Can)]</div>
        <div class="arrow">↓</div>
        <div class="node-act">Check Target = Deep Pond or State = DugBurnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Execute PurifyTileWater() ➔ Tile = Normal Soil</div>
        <div class="arrow">↓</div>
        <div class="node-act">Increment TilesPurifiedCount &amp; Display Toast</div>
      </div>
    </div>
    <div class="arrow">↓</div>
    <div class="node-start" style="background:#0f172a;">◉ END</div>
  </div>
</body>
</html>"""

    # HTML 4: Formal Component Diagram
    html_component = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1500px; height: 1000px; background: #f8fafc; color: #0f172a;
    font-family: 'JetBrains Mono', monospace; padding: 36px; display: flex; flex-direction: column; justify-content: space-between;
    background-image: radial-gradient(#cbd5e1 1px, transparent 1px); background-size: 20px 20px;
  }
  .title-bar { border-bottom: 3px solid #0f172a; padding-bottom: 12px; display: flex; justify-content: space-between; align-items: center; }
  .title-bar h1 { font-size: 24px; font-weight: 700; color: #0f172a; }

  .comp-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; margin-top: 24px; flex: 1; }
  .pkg-box {
    background: #ffffff; border: 2px solid #0f172a; border-radius: 10px; padding: 18px; box-shadow: 4px 4px 0px #0f172a;
  }
  .pkg-title { font-size: 14px; font-weight: 700; color: #0f172a; margin-bottom: 14px; border-bottom: 2px solid #0f172a; padding-bottom: 6px; }
  .comp-item { background: #f1f5f9; border: 2px solid #0f172a; border-radius: 6px; padding: 10px 12px; margin-bottom: 10px; font-size: 12px; font-weight: 600; }
  .comp-item::before { content: "«component» "; color: #64748b; font-size: 10px; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>Component Diagram — Life on Land Architecture Layers</h1>
    <span>FIGURE F.4: FORMAL UML COMPONENT SPECIFICATION</span>
  </div>

  <div class="comp-grid">
    <div class="pkg-box">
      <div class="pkg-title">package Presentation Layer</div>
      <div class="comp-item">UIManager</div>
      <div class="comp-item">DialogueManager</div>
      <div class="comp-item">QuestChecklistUI</div>
      <div class="comp-item">NotificationManager</div>
      <div class="comp-item">MainMenuManager</div>
      <div class="comp-item">PauseMenu &amp; VictoryUI</div>
    </div>

    <div class="pkg-box">
      <div class="pkg-title">package Gameplay Logic Layer</div>
      <div class="comp-item">Player &amp; PlayerController</div>
      <div class="comp-item">EnvironmentManager</div>
      <div class="comp-item">GridWorldMatrix</div>
      <div class="comp-item">Tree (FSM Component)</div>
      <div class="comp-item">Stage1Manager</div>
    </div>

    <div class="pkg-box">
      <div class="pkg-title">package Persistence &amp; Data Layer</div>
      <div class="comp-item">TreeProfile (ScriptableObject)</div>
      <div class="comp-item">BuildingBlueprint (SO)</div>
      <div class="comp-item">Local JSON SaveData</div>
      <div class="comp-item">PlayFab Cloud BaaS</div>
    </div>
  </div>
</body>
</html>"""

    # HTML 5: Formal Deployment Diagram
    html_deployment = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1500px; height: 950px; background: #f8fafc; color: #0f172a;
    font-family: 'JetBrains Mono', monospace; padding: 36px; display: flex; flex-direction: column; justify-content: space-between;
    background-image: radial-gradient(#cbd5e1 1px, transparent 1px); background-size: 20px 20px;
  }
  .title-bar { border-bottom: 3px solid #0f172a; padding-bottom: 12px; display: flex; justify-content: space-between; align-items: center; }
  .title-bar h1 { font-size: 24px; font-weight: 700; color: #0f172a; }

  .deploy-grid { display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 30px; margin-top: 40px; flex: 1; align-items: center; }
  .node-box { background: #ffffff; border: 2px solid #0f172a; border-radius: 12px; padding: 24px; box-shadow: 6px 6px 0px #0f172a; }
  .node-title { font-size: 15px; font-weight: 700; color: #0f172a; margin-bottom: 14px; text-align: center; border-bottom: 2px solid #0f172a; padding-bottom: 6px; }
  .artifact { background: #f1f5f9; border: 1px solid #0f172a; padding: 10px; border-radius: 6px; font-size: 12px; text-align: center; margin-bottom: 10px; font-weight: 600; }
  .conn-arrow { font-size: 14px; color: #0f172a; text-align: center; font-weight: 700; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>Deployment Diagram — Two-Tier Hybrid Architecture</h1>
    <span>FIGURE F.5: FORMAL UML DEPLOYMENT SPECIFICATION</span>
  </div>

  <div class="deploy-grid">
    <div class="node-box">
      <div class="node-title">&lt;&lt;device&gt;&gt; Client Device</div>
      <div class="artifact">&lt;&lt;artifact&gt;&gt; Unity Executable (.exe)</div>
      <div class="artifact">&lt;&lt;artifact&gt;&gt; Unity WebGL Build</div>
      <div class="artifact">&lt;&lt;component&gt;&gt; Monolithic Engine</div>
    </div>

    <div class="conn-arrow">
      ━━━━ 💬 ━━━━<br>
      Local I/O Reads/Writes<br>
      <small style="color:#64748b; font-size:11px;">(Native Serialization)</small>
    </div>

    <div class="node-box">
      <div class="node-title">&lt;&lt;execution env&gt;&gt; Storage &amp; Cloud</div>
      <div class="artifact">&lt;&lt;database&gt;&gt; SaveData.json / PlayerPrefs</div>
      <div class="artifact">&lt;&lt;cloud&gt;&gt; PlayFab Title Data</div>
      <div class="artifact">&lt;&lt;service&gt;&gt; Global Leaderboards</div>
    </div>
  </div>
</body>
</html>"""

    html_files = [
        ("1_USE_CASE_DIAGRAM", html_use_case, 1500, 1000),
        ("2_CLASS_DIAGRAM", html_class, 1650, 1150),
        ("3_ACTIVITY_DIAGRAM", html_activity, 1400, 950),
        ("4_COMPONENT_DIAGRAM", html_component, 1500, 1000),
        ("5_DEPLOYMENT_ARCHITECTURE", html_deployment, 1500, 950),
    ]

    for name, content, w, h in html_files:
        h_path = os.path.join(diagrams_dir, f"{name}.html")
        png_path = os.path.join(diagrams_dir, f"{name}.png")
        jpg_path = os.path.join(diagrams_dir, f"{name}.jpg")

        with open(h_path, "w", encoding="utf-8") as f:
            f.write(content)

        cmd = [
            chrome_path,
            "--headless=new",
            "--disable-gpu",
            "--hide-scrollbars",
            f"--window-size={w},{h}",
            f"--screenshot={png_path}",
            f"file:///{h_path.replace('\\', '/')}"
        ]
        res = subprocess.run(cmd, capture_output=True, text=True)
        if res.returncode == 0 and os.path.exists(png_path):
            print(f"SUCCESS FORMAL UML: Generated PNG -> {png_path}")
            # Convert PNG to JPG
            img = Image.open(png_path).convert('RGB')
            img.save(jpg_path, 'JPEG', quality=95)
            print(f"SUCCESS FORMAL UML: Generated JPG -> {jpg_path}")

if __name__ == "__main__":
    generate_formal_uml()
