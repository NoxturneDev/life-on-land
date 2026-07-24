import os
import subprocess
from PIL import Image

def generate_diagram_images():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    diagrams_dir = os.path.join(sub_dir, "diagrams")
    os.makedirs(diagrams_dir, exist_ok=True)

    chrome_path = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
    if not os.path.exists(chrome_path):
        chrome_path = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

    print(f"Generating 5 UML & Architecture Diagram Image files...")

    # HTML 1: Use Case Diagram
    html_use_case = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1400px; height: 950px;
    background: #0f172a; color: #f8fafc;
    font-family: 'Inter', sans-serif;
    padding: 30px; display: flex; flex-direction: column; justify-content: space-between;
  }
  .title-bar {
    background: #1e293b; border: 2px solid #10b981; padding: 12px 24px; border-radius: 8px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #34d399; }
  .title-bar span { font-size: 14px; color: #fbbf24; font-weight: bold; }

  .diagram-container {
    display: flex; gap: 40px; align-items: center; justify-content: center; flex: 1; margin-top: 20px;
  }

  .actor-box {
    background: #1e293b; border: 3px solid #f59e0b; border-radius: 12px; padding: 24px; text-align: center;
    box-shadow: 0 8px 16px rgba(0,0,0,0.4); width: 220px;
  }
  .actor-icon { font-size: 50px; margin-bottom: 8px; }
  .actor-name { font-family: 'Chakra Petch', sans-serif; font-size: 20px; font-weight: bold; color: #fef08a; }

  .system-boundary {
    background: rgba(16, 185, 129, 0.05); border: 3px dashed #10b981; border-radius: 16px; padding: 24px;
    flex: 1; display: grid; grid-template-columns: 1fr 1fr; gap: 14px; position: relative;
  }
  .boundary-title {
    position: absolute; top: -14px; left: 24px; background: #10b981; color: #064e3b;
    font-family: 'Chakra Petch', sans-serif; font-size: 14px; font-weight: bold; padding: 4px 12px; border-radius: 4px;
  }

  .use-case {
    background: #1e293b; border: 2px solid #38bdf8; border-radius: 30px; padding: 12px 18px;
    font-size: 14px; font-weight: 600; text-align: center; color: #e2e8f0;
    box-shadow: 0 4px 8px rgba(0,0,0,0.3); display: flex; align-items: center; justify-content: center; gap: 8px;
  }
  .use-case.include { border-color: #f59e0b; background: rgba(245, 158, 11, 0.1); }
  .use-case.extend { border-color: #ec4899; background: rgba(236, 72, 153, 0.1); }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>USE CASE DIAGRAM — GAME "LIFE ON LAND"</h1>
    <span>F.1 UML DIAGRAM SPECIFICATION</span>
  </div>

  <div class="diagram-container">
    <div class="actor-box">
      <div class="actor-icon">🧑‍🌾</div>
      <div class="actor-name">PEMAIN (PLAYER)</div>
      <div style="font-size: 12px; color: #94a3b8; margin-top: 6px;">Restorer Utama</div>
    </div>

    <div class="system-boundary">
      <div class="boundary-title">SYSTEM BOUNDARY: LIFE ON LAND GAME</div>
      <div class="use-case">🎮 Memulai Permainan</div>
      <div class="use-case">🏃 Menggerakkan &amp; Dash Karakter</div>
      <div class="use-case">🎒 Memilih Slot Hotbar (1-6)</div>
      <div class="use-case include">🪏 Menyekop Ubin Corrupted</div>
      <div class="use-case include">💧 Menyiram &amp; Memurnikan Ubin</div>
      <div class="use-case">🌊 Mengisi Ulang Gembor Air</div>
      <div class="use-case include">🌱 Menanam Benih Vegetasi</div>
      <div class="use-case">🏗️ Membangun Infrastruktur</div>
      <div class="use-case extend">💬 Berinteraksi dengan NPC</div>
      <div class="use-case extend">📜 Mengelola Quest Checklist</div>
      <div class="use-case">⏸️ Menjeda Permainan (Pause)</div>
      <div class="use-case">🏆 Melihat Pencapaian (Achievements)</div>
      <div class="use-case include" style="grid-column: span 2;">⚡ &lt;&lt;include&gt;&gt; Konsumsi Stamina Pemain (5 Stamina per Aksi)</div>
    </div>
  </div>
</body>
</html>"""

    # HTML 2: Class Diagram
    html_class = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=JetBrains+Mono:wght@400;600&family=Inter:wght@400;600&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1600px; height: 1100px; background: #0b130e; color: #f8fafc;
    font-family: 'Inter', sans-serif; padding: 30px; display: flex; flex-direction: column; justify-content: space-between;
  }
  .title-bar {
    background: #0d1a12; border: 2px solid #10b981; padding: 12px 24px; border-radius: 8px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #4ade80; }
  .title-bar span { font-size: 14px; color: #facc15; font-weight: bold; }

  .class-grid {
    display: grid; grid-template-columns: repeat(4, 1fr); gap: 16px; margin-top: 20px; flex: 1;
  }

  .class-box {
    background: #122217; border: 2px solid #34d399; border-radius: 8px; overflow: hidden;
    box-shadow: 0 4px 10px rgba(0,0,0,0.5); font-family: 'JetBrains Mono', monospace; font-size: 12px;
  }
  .class-box.so { border-color: #f59e0b; background: #1c180e; }
  .class-box.mgr { border-color: #38bdf8; background: #0a1120; }

  .class-header {
    background: #064e3b; color: #ffffff; padding: 8px 12px; font-weight: bold;
    font-family: 'Chakra Petch', sans-serif; font-size: 15px; border-bottom: 2px solid #34d399;
  }
  .class-box.so .class-header { background: #78350f; border-color: #f59e0b; }
  .class-box.mgr .class-header { background: #1e3a8a; border-color: #38bdf8; }

  .class-body { padding: 10px 12px; }
  .field { color: #a7f3d0; margin-bottom: 4px; }
  .method { color: #fef08a; margin-bottom: 4px; }
  .divider { border-top: 1px solid #1e3a2b; margin: 6px 0; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>CLASS DIAGRAM — GAME "LIFE ON LAND" (C# UNITY CLASS CONTRACTS)</h1>
    <span>F.2 UML DIAGRAM SPECIFICATION</span>
  </div>

  <div class="class-grid">
    <div class="class-box">
      <div class="class-header">Player</div>
      <div class="class-body">
        <div class="field">- currentStamina : float</div>
        <div class="field">- localO2Buffer : float</div>
        <div class="field">- activeHotbarSlot : int</div>
        <div class="field">- inventory : List&lt;Item&gt;</div>
        <div class="divider"></div>
        <div class="method">+ UseActiveTool(Vector2)</div>
        <div class="method">+ ExecutePlantAction()</div>
        <div class="method">+ ConsumeStamina(float)</div>
      </div>
    </div>

    <div class="class-box">
      <div class="class-header">PlayerController</div>
      <div class="class-body">
        <div class="field">+ moveSpeed : float</div>
        <div class="field">+ dashSpeed : float</div>
        <div class="divider"></div>
        <div class="method">- PerformDash()</div>
        <div class="method">+ ProcessMovement()</div>
      </div>
    </div>

    <div class="class-box mgr">
      <div class="class-header">EnvironmentManager</div>
      <div class="class-body">
        <div class="field">- globalO2Percentage : float</div>
        <div class="field">+ currentLevel : int</div>
        <div class="divider"></div>
        <div class="method">+ RecalculateO2()</div>
        <div class="method">+ ExecuteStateTick()</div>
        <div class="method">+ EvaluateVictory() : bool</div>
      </div>
    </div>

    <div class="class-box mgr">
      <div class="class-header">GridWorldMatrix</div>
      <div class="class-body">
        <div class="field">+ TilesPurifiedCount : int</div>
        <div class="divider"></div>
        <div class="method">+ PurifyTileShovel() : bool</div>
        <div class="method">+ PurifyTileWater() : bool</div>
        <div class="method">+ GetCell(Vector2Int)</div>
      </div>
    </div>

    <div class="class-box">
      <div class="class-header">GridCell</div>
      <div class="class-body">
        <div class="field">+ moisture : float</div>
        <div class="field">+ localO2 : float</div>
        <div class="field">+ corruptionState : int</div>
        <div class="field">+ placedObject : WorldObject</div>
      </div>
    </div>

    <div class="class-box">
      <div class="class-header">WorldObject</div>
      <div class="class-body">
        <div class="field">+ ObjectID : string</div>
        <div class="field">+ GridCoordinates : Vector2Int</div>
      </div>
    </div>

    <div class="class-box">
      <div class="class-header">Tree : WorldObject</div>
      <div class="class-body">
        <div class="field">- currentFSMState : GrowthState</div>
        <div class="divider"></div>
        <div class="method">+ ProgressGrowthCycle()</div>
        <div class="method">+ TransitionToWithered()</div>
        <div class="method">+ Revive()</div>
      </div>
    </div>

    <div class="class-box so">
      <div class="class-header">&lt;&lt;SO&gt;&gt; TreeProfile</div>
      <div class="class-body">
        <div class="field">+ treeTypeID : string</div>
        <div class="field">+ o2EmissionRate : float</div>
        <div class="field">+ waterRequirement : int</div>
      </div>
    </div>
  </div>
</body>
</html>"""

    # HTML 3: Activity Diagram
    html_activity = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Inter:wght@400;600&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1400px; height: 950px; background: #0f172a; color: #f8fafc;
    font-family: 'Inter', sans-serif; padding: 30px; display: flex; flex-direction: column; justify-content: space-between;
  }
  .title-bar {
    background: #1e293b; border: 2px solid #10b981; padding: 12px 24px; border-radius: 8px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #34d399; }
  .title-bar span { font-size: 14px; color: #fbbf24; font-weight: bold; }

  .flow-container {
    display: flex; flex-direction: column; align-items: center; justify-content: center; flex: 1; margin-top: 20px; gap: 16px;
  }
  .node-start { background: #10b981; color: #064e3b; font-weight: bold; border-radius: 20px; padding: 8px 24px; }
  .node-act { background: #1e293b; border: 2px solid #38bdf8; border-radius: 8px; padding: 10px 20px; font-size: 15px; }
  .node-dec { background: #78350f; border: 2px solid #f59e0b; transform: rotate(0deg); padding: 10px 20px; border-radius: 6px; color: #fef08a; font-weight: bold; }
  .flow-branches { display: flex; gap: 60px; width: 100%; justify-content: center; }
  .branch { display: flex; flex-direction: column; align-items: center; gap: 12px; background: rgba(30, 41, 59, 0.5); padding: 18px; border-radius: 8px; border: 1px solid #334155; flex: 1; }
  .arrow { color: #f59e0b; font-size: 20px; font-weight: bold; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>ACTIVITY DIAGRAM — ALUR PURIFIKASI DUA TAHAP</h1>
    <span>F.3 UML DIAGRAM SPECIFICATION</span>
  </div>

  <div class="flow-container">
    <div class="node-start">● START</div>
    <div class="arrow">↓</div>
    <div class="node-act">Pemain Memilih Slot Hotbar &amp; Mengarahkan Kursor ke Tile</div>
    <div class="arrow">↓</div>
    <div class="node-dec">Keputusan Slot Hotbar yang Dipilih?</div>

    <div class="flow-branches">
      <div class="branch">
        <div style="color: #34d399; font-weight: bold;">SLOT 1: SEKOP (SHOVEL)</div>
        <div class="arrow">↓</div>
        <div class="node-act">Cek Stamina &gt;= 5 &amp; Tile = Corrupted Burnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Jalankan PurifyTileShovel() ➔ Tile = DugBurnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Kurangi 5 Stamina &amp; Toast Notifikasi "Tile Cleared"</div>
      </div>

      <div class="branch">
        <div style="color: #38bdf8; font-weight: bold;">SLOT 2: GEMBOR AIR (WATERING CAN)</div>
        <div class="arrow">↓</div>
        <div class="node-act">Cek Target = Sumber Air atau Tile = DugBurnt</div>
        <div class="arrow">↓</div>
        <div class="node-act">Jalankan PurifyTileWater() ➔ Tile = Normal Soil (Moisture 1.0)</div>
        <div class="arrow">↓</div>
        <div class="node-act">TilesPurifiedCount++ &amp; Toast Notifikasi "Tile Purified!"</div>
      </div>
    </div>
    <div class="arrow">↓</div>
    <div class="node-start" style="background:#ef4444; color:#fff;">◉ END</div>
  </div>
</body>
</html>"""

    # HTML 4: Component Diagram
    html_component = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Inter:wght@400;600&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1500px; height: 1000px; background: #0b130e; color: #f8fafc;
    font-family: 'Inter', sans-serif; padding: 30px; display: flex; flex-direction: column; justify-content: space-between;
  }
  .title-bar {
    background: #0d1a12; border: 2px solid #10b981; padding: 12px 24px; border-radius: 8px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #4ade80; }
  .title-bar span { font-size: 14px; color: #facc15; font-weight: bold; }

  .comp-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; margin-top: 20px; flex: 1; }
  .pkg-box {
    background: #122217; border: 2px solid #34d399; border-radius: 8px; padding: 16px; box-shadow: 0 4px 10px rgba(0,0,0,0.5);
  }
  .pkg-title { font-family: 'Chakra Petch', sans-serif; font-size: 16px; font-weight: bold; color: #facc15; margin-bottom: 12px; border-bottom: 1px solid #1e3a2b; padding-bottom: 6px; }
  .comp-item { background: #064e3b; color: #e2e8f0; border: 1px solid #10b981; border-radius: 6px; padding: 8px 12px; margin-bottom: 8px; font-size: 13px; font-weight: 600; display: flex; align-items: center; gap: 8px; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>COMPONENT DIAGRAM — GAME "LIFE ON LAND" LAYER ARCHITECTURE</h1>
    <span>F.4 UML DIAGRAM SPECIFICATION</span>
  </div>

  <div class="comp-grid">
    <div class="pkg-box">
      <div class="pkg-title">📦 PRESENTATION LAYER (UI)</div>
      <div class="comp-item">🧩 UIManager</div>
      <div class="comp-item">🧩 DialogueManager</div>
      <div class="comp-item">🧩 QuestChecklistUI</div>
      <div class="comp-item">🧩 NotificationManager</div>
      <div class="comp-item">🧩 MainMenuManager</div>
      <div class="comp-item">🧩 PauseMenu &amp; VictoryUI</div>
    </div>

    <div class="pkg-box" style="border-color:#38bdf8;">
      <div class="pkg-title" style="color:#38bdf8;">📦 GAMEPLAY LOGIC LAYER</div>
      <div class="comp-item" style="background:#1e3a8a; border-color:#38bdf8;">⚙️ Player &amp; PlayerController</div>
      <div class="comp-item" style="background:#1e3a8a; border-color:#38bdf8;">⚙️ EnvironmentManager</div>
      <div class="comp-item" style="background:#1e3a8a; border-color:#38bdf8;">⚙️ GridWorldMatrix</div>
      <div class="comp-item" style="background:#1e3a8a; border-color:#38bdf8;">⚙️ Tree FSM Component</div>
      <div class="comp-item" style="background:#1e3a8a; border-color:#38bdf8;">⚙️ Stage1Manager</div>
    </div>

    <div class="pkg-box" style="border-color:#f59e0b;">
      <div class="pkg-title" style="color:#f59e0b;">📦 DATA &amp; PERSISTENCE LAYER</div>
      <div class="comp-item" style="background:#78350f; border-color:#f59e0b;">📄 TreeProfile (ScriptableObject)</div>
      <div class="comp-item" style="background:#78350f; border-color:#f59e0b;">📄 BuildingBlueprint (ScriptableObject)</div>
      <div class="comp-item" style="background:#78350f; border-color:#f59e0b;">💾 Local JSON SaveData</div>
      <div class="comp-item" style="background:#78350f; border-color:#f59e0b;">☁️ PlayFab Cloud BaaS Integration</div>
    </div>
  </div>
</body>
</html>"""

    # HTML 5: Deployment Diagram
    html_deployment = """<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Inter:wght@400;600&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    width: 1500px; height: 950px; background: #0f172a; color: #f8fafc;
    font-family: 'Inter', sans-serif; padding: 30px; display: flex; flex-direction: column; justify-content: space-between;
  }
  .title-bar {
    background: #1e293b; border: 2px solid #10b981; padding: 12px 24px; border-radius: 8px;
    display: flex; justify-content: space-between; align-items: center;
  }
  .title-bar h1 { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #34d399; }
  .title-bar span { font-size: 14px; color: #fbbf24; font-weight: bold; }

  .deploy-grid { display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 24px; margin-top: 30px; flex: 1; align-items: center; }
  .node-box { background: #1e293b; border: 3px solid #38bdf8; border-radius: 12px; padding: 24px; box-shadow: 0 8px 16px rgba(0,0,0,0.4); }
  .node-title { font-family: 'Chakra Petch', sans-serif; font-size: 18px; font-weight: bold; color: #38bdf8; margin-bottom: 14px; text-align: center; }
  .artifact { background: #0f172a; border: 1px solid #475569; padding: 10px; border-radius: 6px; font-size: 13px; text-align: center; margin-bottom: 8px; }
  .conn-arrow { font-family: 'Chakra Petch', sans-serif; font-size: 24px; color: #f59e0b; text-align: center; font-weight: bold; }
</style>
</head>
<body>
  <div class="title-bar">
    <h1>DEPLOYMENT DIAGRAM — TWO-TIER HYBRID ARCHITECTURE</h1>
    <span>F.5 UML DIAGRAM SPECIFICATION</span>
  </div>

  <div class="deploy-grid">
    <div class="node-box">
      <div class="node-title">🖥️ CLIENT DEVICE (PC / BROWSER)</div>
      <div class="artifact">🎮 Unity Runtime (Standalone .exe)</div>
      <div class="artifact">🌐 Unity WebGL Build</div>
      <div class="artifact">⚙️ Monolithic Simulation Engine</div>
    </div>

    <div class="conn-arrow">
      ⇄ Local Reads/Writes ⇄<br>
      <small style="color:#cbd5e1; font-size:12px;">Offline Native Persistence</small>
    </div>

    <div class="node-box" style="border-color:#f59e0b;">
      <div class="node-title" style="color:#f59e0b;">☁️ BACKEND &amp; LOCAL STORAGE</div>
      <div class="artifact" style="border-color:#f59e0b;">💾 Local Storage (SaveData.json / PlayerPrefs)</div>
      <div class="artifact" style="border-color:#f59e0b;">☁️ Microsoft PlayFab BaaS Title Data</div>
      <div class="artifact" style="border-color:#f59e0b;">🏆 Global Real-Time Leaderboards</div>
    </div>
  </div>
</body>
</html>"""

    html_files = [
        ("1_USE_CASE_DIAGRAM", html_use_case, 1400, 950),
        ("2_CLASS_DIAGRAM", html_class, 1600, 1100),
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
            print(f"SUCCESS: Generated PNG -> {png_path}")
            # Convert PNG to JPG
            img = Image.open(png_path).convert('RGB')
            img.save(jpg_path, 'JPEG', quality=95)
            print(f"SUCCESS: Generated JPG -> {jpg_path}")

if __name__ == "__main__":
    generate_diagram_images()
