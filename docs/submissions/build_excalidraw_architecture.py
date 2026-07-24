import os
import subprocess
from PIL import Image

def build_excalidraw_architecture():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    diagrams_dir = os.path.join(sub_dir, "diagrams")
    os.makedirs(diagrams_dir, exist_ok=True)

    html_path = os.path.join(diagrams_dir, "excalidraw_architecture.html")
    png_path = os.path.join(diagrams_dir, "ARSITEKTUR_DIAGRAM_LIFE_ON_LAND.png")
    jpg_path = os.path.join(diagrams_dir, "ARSITEKTUR_DIAGRAM_LIFE_ON_LAND.jpg")

    chrome_path = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
    if not os.path.exists(chrome_path):
        chrome_path = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

    html_content = """<!DOCTYPE html>
<html lang="id">
<head>
<meta charset="UTF-8">
<title>Architecture Diagram — Life on Land System</title>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Inter:wght@400;500;600;700;800&family=JetBrains+Mono:wght@500;700&display=swap" rel="stylesheet">
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }

  body {
    width: 1600px;
    height: 1050px;
    background: #f8fafc;
    color: #0f172a;
    font-family: 'Inter', sans-serif;
    padding: 40px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    background-image: radial-gradient(#cbd5e1 1.5px, transparent 1.5px);
    background-size: 24px 24px;
    -webkit-print-color-adjust: exact;
  }

  /* Header Bar */
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 3px solid #0f172a;
    padding-bottom: 16px;
  }

  .header h1 {
    font-family: 'Chakra Petch', sans-serif;
    font-size: 30px;
    font-weight: 700;
    color: #0f172a;
    letter-spacing: -0.5px;
  }

  .header .subtitle {
    font-size: 14px;
    color: #475569;
    font-weight: 600;
    margin-top: 4px;
  }

  /* Main Grid Layout */
  .architecture-container {
    display: grid;
    grid-template-columns: 2.2fr 1fr 1fr;
    gap: 28px;
    flex: 1;
    margin-top: 24px;
  }

  /* Container Boxes */
  .sketch-box {
    background: #ffffff;
    border: 3px solid #0f172a;
    border-radius: 12px;
    padding: 24px;
    box-shadow: 6px 6px 0px #0f172a;
    display: flex;
    flex-direction: column;
  }

  .sketch-box.tier-client {
    background: #f0fdf4;
    border-color: #0f172a;
  }

  .sketch-box.tier-local {
    background: #eff6ff;
    border-color: #0f172a;
  }

  .sketch-box.tier-cloud {
    background: #faf5ff;
    border-color: #0f172a;
  }

  .box-header {
    font-family: 'Chakra Petch', sans-serif;
    font-size: 20px;
    font-weight: 700;
    margin-bottom: 18px;
    border-bottom: 2px solid #0f172a;
    padding-bottom: 10px;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  .tier-client .box-header { color: #15803d; }
  .tier-local .box-header { color: #1d4ed8; }
  .tier-cloud .box-header { color: #7e22ce; }

  /* Sub-components */
  .comp-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 14px;
    flex: 1;
  }

  .comp-card {
    background: #ffffff;
    border: 2px solid #0f172a;
    border-radius: 8px;
    padding: 14px 16px;
    box-shadow: 3px 3px 0px #0f172a;
  }

  .comp-title {
    font-family: 'Chakra Petch', sans-serif;
    font-size: 16px;
    font-weight: 700;
    color: #0f172a;
    margin-bottom: 6px;
  }

  .comp-desc {
    font-size: 12px;
    color: #334155;
    line-height: 1.45;
    font-weight: 500;
  }

  /* Connector Labels */
  .connector-label {
    font-family: 'JetBrains Mono', monospace;
    font-size: 12px;
    color: #0f172a;
    font-weight: 700;
    text-align: center;
    margin: 12px 0;
    background: #ffffff;
    border: 2px solid #0f172a;
    padding: 6px 12px;
    border-radius: 6px;
    box-shadow: 2px 2px 0px #0f172a;
  }

  /* Platforms Banner */
  .platform-banner {
    display: flex;
    justify-content: space-around;
    align-items: center;
    background: #ffffff;
    border: 3px solid #0f172a;
    border-radius: 12px;
    padding: 16px 24px;
    box-shadow: 5px 5px 0px #0f172a;
    margin-top: 20px;
  }

  .platform-item {
    font-family: 'Chakra Petch', sans-serif;
    font-size: 16px;
    font-weight: 700;
    color: #0f172a;
  }

  .tag {
    font-family: 'JetBrains Mono', monospace;
    font-size: 11px;
    font-weight: 700;
    background: #0f172a;
    color: #ffffff;
    padding: 3px 10px;
    border-radius: 4px;
  }
</style>
</head>
<body>

  <!-- Header -->
  <div class="header">
    <div>
      <h1>SYSTEM ARCHITECTURE DIAGRAM — LIFE ON LAND</h1>
      <div class="subtitle">Top-Down Tactical Eco-Restoration Simulator • Monolithic Client Engine with Two-Tier Hybrid Persistence</div>
    </div>
    <span class="tag" style="font-size: 13px; padding: 6px 14px;">FORMAL ARCHITECTURE SPECIFICATION</span>
  </div>

  <!-- Main Architecture Grid -->
  <div class="architecture-container">

    <!-- TIER 1: CLIENT ENGINE -->
    <div class="sketch-box tier-client">
      <div class="box-header">
        <span>Tier 1: Unity Monolithic Client Engine</span>
        <span class="tag" style="background:#15803d;">C# RUNTIME</span>
      </div>

      <div class="comp-grid">
        <!-- Comp 1 -->
        <div class="comp-card">
          <div class="comp-title">Player Controller &amp; Tool Input</div>
          <div class="comp-desc">WASD top-down movement, stamina evaluation (5 stamina per action), active Hotbar slot switcher (1-6).</div>
        </div>

        <!-- Comp 2 -->
        <div class="comp-card">
          <div class="comp-title">Vegetation FSM Engine</div>
          <div class="comp-desc">Finite State Machine: Seed -&gt; Sprout -&gt; Sapling -&gt; Mature Tree -&gt; Withered. Revive() via watering.</div>
        </div>

        <!-- Comp 3 -->
        <div class="comp-card">
          <div class="comp-title">Grid World Matrix System</div>
          <div class="comp-desc">2D Grid Matrix tracking tile corruption state, soil moisture decay (-0.05/5s), &amp; 4-neighbor O2 diffusion.</div>
        </div>

        <!-- Comp 4 -->
        <div class="comp-card">
          <div class="comp-title">Tile Purification Engine</div>
          <div class="comp-desc">Two-step purification: Step 1 (Shovel) -&gt; Step 2 (Watering Can) -&gt; Normal Soil + Moisture 1.0.</div>
        </div>

        <!-- Comp 5 -->
        <div class="comp-card">
          <div class="comp-title">Procedural UI Manager</div>
          <div class="comp-desc">HUD Stamina &amp; O2 bars, Dialogue Manager (Visual Novel format), Quest Checklist UI, &amp; Achievements panel.</div>
        </div>

        <!-- Comp 6 -->
        <div class="comp-card">
          <div class="comp-title">Stage &amp; Quest Managers</div>
          <div class="comp-desc">Stage 1 Manager (Maliz &amp; Arid Oasis), Stage 2 (Oryel), Stage 3 (Pyper &amp; Biosphere Dome victory gate).</div>
        </div>
      </div>
    </div>

    <!-- TIER 2: LOCAL PERSISTENCE -->
    <div class="sketch-box tier-local">
      <div class="box-header">
        <span>Tier 2: Local Persistence</span>
        <span class="tag" style="background:#1d4ed8;">NATIVE I/O</span>
      </div>

      <div style="display: flex; flex-direction: column; gap: 14px; flex: 1; justify-content: center;">
        <div class="comp-card">
          <div class="comp-title">SaveData.json Schema</div>
          <div class="comp-desc">Flat JSON schema serializing player position, stamina, water inventory, &amp; instantiated tree FSM states.</div>
        </div>

        <div class="comp-card">
          <div class="comp-title">PlayerPrefs Local Store</div>
          <div class="comp-desc">Stores unlocked Achievements (First Steps, Water Bearer, Green Oasis) &amp; local audio configuration.</div>
        </div>

        <div class="connector-label">
          &lt;-- Native File Read/Write --&gt;
        </div>
      </div>
    </div>

    <!-- TIER 3: CLOUD BAAS -->
    <div class="sketch-box tier-cloud">
      <div class="box-header">
        <span>Tier 3: PlayFab Cloud BaaS</span>
        <span class="tag" style="background:#7e22ce;">CLOUD BAAS</span>
      </div>

      <div style="display: flex; flex-direction: column; gap: 14px; flex: 1; justify-content: center;">
        <div class="comp-card">
          <div class="comp-title">Global Leaderboards</div>
          <div class="comp-desc">Real-time player global ranking by atmospheric O2 percentage restored &amp; total purified tiles count.</div>
        </div>

        <div class="comp-card">
          <div class="comp-title">Title Data &amp; User Auth</div>
          <div class="comp-desc">PlayFab Title ID (1A2B3) authenticating user sessions &amp; cross-platform cloud save synchronization.</div>
        </div>

        <div class="connector-label">
          &lt;.. Optional HTTPS API Sync ..&gt;
        </div>
      </div>
    </div>

  </div>

  <!-- Bottom Target Platforms Banner -->
  <div class="platform-banner">
    <div class="platform-item">Standalone Windows PC (.exe)</div>
    <div class="platform-item">WebGL Web Browser Runtime</div>
    <div class="platform-item">Android Cross-Platform (Stage 2 Target)</div>
  </div>

</body>
</html>
"""

    with open(html_path, "w", encoding="utf-8") as f:
        f.write(html_content)
    print(f"Generated firm architecture HTML: {html_path}")

    # Render PNG screenshot via Chrome
    cmd = [
        chrome_path,
        "--headless=new",
        "--disable-gpu",
        "--hide-scrollbars",
        "--window-size=1600,1050",
        f"--screenshot={png_path}",
        f"file:///{html_path.replace('\\', '/')}"
    ]

    print(f"Rendering Firm Architecture Diagram using {chrome_path}...")
    res = subprocess.run(cmd, capture_output=True, text=True)

    if res.returncode == 0 and os.path.exists(png_path):
        print(f"SUCCESS! Firm PNG generated at:\n{png_path}")
        # Convert to high-resolution JPG
        img = Image.open(png_path).convert('RGB')
        img.save(jpg_path, 'JPEG', quality=95)
        print(f"SUCCESS! Firm JPG generated at:\n{jpg_path}")

if __name__ == "__main__":
    build_excalidraw_architecture()
