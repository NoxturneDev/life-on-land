import os
import subprocess
import base64

def get_base64_image(image_path):
    if not os.path.exists(image_path):
        return ""
    with open(image_path, "rb") as f:
        data = f.read()
    ext = os.path.splitext(image_path)[1].lower().replace('.', '')
    if ext == 'jpg': ext = 'jpeg'
    return f"data:image/{ext};base64,{base64.b64encode(data).decode('utf-8')}"

def build_pixel_poster():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    screenshots_dir = r"c:\Users\galih\Documents\Projects\Game\My project\Assets\Screenshots"
    charts_dir = os.path.join(sub_dir, "charts")
    
    html_path = os.path.join(sub_dir, "poster_pixelated.html")
    output_png = os.path.join(sub_dir, "POSTER_PIXELATED_LIFE_ON_LAND.png")

    img_maliz = get_base64_image(os.path.join(screenshots_dir, "maliz_dialogs.png"))
    img_trees = get_base64_image(os.path.join(screenshots_dir, "grown_trees.png"))
    img_complete = get_base64_image(os.path.join(screenshots_dir, "restoration_complete.png"))
    img_chart = get_base64_image(os.path.join(charts_dir, "gform_chart_5_sus_scores.png"))

    html_content = f"""<!DOCTYPE html>
<html lang="id">
<head>
<meta charset="UTF-8">
<title>Poster Life on Land - Pixel Edition</title>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Pixelify+Sans:wght@400;600;700&family=Press+Start+2P&family=VT323&display=swap" rel="stylesheet">
<style>
  * {{ box-sizing: border-box; margin: 0; padding: 0; }}
  body {{
    width: 1920px;
    height: 1357px;
    background: #08100b;
    color: #e2e8f0;
    font-family: 'Pixelify Sans', 'VT323', sans-serif;
    overflow: hidden;
    position: relative;
    padding: 24px;
    background-image: 
      radial-gradient(circle at 50% 50%, #142a1b 0%, #050b07 100%),
      linear-gradient(rgba(18, 16, 16, 0) 50%, rgba(0, 0, 0, 0.3) 50%);
    background-size: 100% 100%, 100% 4px;
  }}

  /* CRT Scanline Effect Overlay */
  body::before {{
    content: " ";
    display: block;
    position: absolute;
    top: 0; left: 0; bottom: 0; right: 0;
    background: linear-gradient(rgba(18, 16, 16, 0) 50%, rgba(0, 0, 0, 0.15) 50%);
    background-size: 100% 4px;
    z-index: 100;
    pointer-events: none;
  }}

  /* Pixel Boxes Styling */
  .pixel-box {{
    background: #0d1a12;
    border: 4px solid #10b981;
    box-shadow: 
      inset -4px -4px 0px #042f2e,
      inset 4px 4px 0px #6ee7b7,
      6px 6px 0px #000000;
    padding: 16px;
  }}

  .pixel-box-gold {{
    background: #18150a;
    border: 4px solid #f59e0b;
    box-shadow: 
      inset -4px -4px 0px #451a03,
      inset 4px 4px 0px #fde68a,
      6px 6px 0px #000000;
    padding: 16px;
  }}

  .pixel-box-blue {{
    background: #0a1120;
    border: 4px solid #38bdf8;
    box-shadow: 
      inset -4px -4px 0px #1e3a8a,
      inset 4px 4px 0px #bae6fd,
      6px 6px 0px #000000;
    padding: 16px;
  }}

  /* Header Banner */
  .header-banner {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #0f1c13;
    border: 5px solid #10b981;
    box-shadow: 8px 8px 0px #000;
    padding: 14px 28px;
    margin-bottom: 18px;
  }}

  .header-title h1 {{
    font-family: 'Press Start 2P', cursive;
    font-size: 21px;
    color: #4ade80;
    text-shadow: 3px 3px 0px #064e3b, 5px 5px 0px #000;
    margin-bottom: 8px;
    letter-spacing: 1px;
  }}

  .header-title h2 {{
    font-family: 'VT323', monospace;
    font-size: 26px;
    color: #fef08a;
    letter-spacing: 2px;
  }}

  .dosen-badge {{
    background: #1e293b;
    border: 4px solid #f59e0b;
    box-shadow: 4px 4px 0 #000;
    padding: 10px 20px;
    text-align: right;
  }}
  .dosen-badge .label {{
    font-family: 'Press Start 2P', cursive;
    font-size: 9px;
    color: #fbbf24;
    display: block;
    margin-bottom: 4px;
  }}
  .dosen-badge .name {{
    font-family: 'VT323', monospace;
    font-size: 26px;
    color: #ffffff;
    font-weight: bold;
  }}

  /* Poster Grid 3 Columns */
  .poster-grid {{
    display: grid;
    grid-template-columns: 1fr 1.15fr 1fr;
    gap: 18px;
    height: 1200px;
  }}

  .column {{
    display: flex;
    flex-direction: column;
    gap: 16px;
  }}

  .section-header {{
    font-family: 'Press Start 2P', cursive;
    font-size: 11px;
    color: #facc15;
    background: #064e3b;
    padding: 8px 12px;
    border: 3px solid #34d399;
    box-shadow: 3px 3px 0 #000;
    margin-bottom: 10px;
    display: flex;
    align-items: center;
    gap: 8px;
  }}

  /* Team Member Cards */
  .team-member {{
    background: #052217;
    border: 2px solid #10b981;
    padding: 8px 12px;
    margin-bottom: 8px;
    box-shadow: 3px 3px 0 #000;
  }}
  .team-member .top-row {{
    display: flex;
    justify-content: space-between;
    align-items: center;
  }}
  .team-member .name {{ font-size: 19px; font-weight: bold; color: #ffffff; }}
  .team-member .nim {{ font-family: 'VT323', monospace; font-size: 21px; color: #a7f3d0; }}
  .team-member .role {{ font-size: 15px; color: #fbbf24; margin-top: 2px; }}

  p, li {{
    font-size: 18px;
    line-height: 1.35;
    color: #cbd5e1;
  }}

  ul {{ list-style: none; padding: 0; }}
  ul li {{
    margin-bottom: 6px;
    position: relative;
    padding-left: 18px;
  }}
  ul li::before {{
    content: "▶";
    position: absolute;
    left: 0;
    color: #4ade80;
    font-size: 12px;
  }}

  .highlight {{ color: #4ade80; font-weight: bold; }}
  .highlight-gold {{ color: #fbbf24; font-weight: bold; }}
  .highlight-blue {{ color: #38bdf8; font-weight: bold; }}

  /* Screenshot Frames */
  .screenshot-frame {{
    border: 4px solid #475569;
    box-shadow: 5px 5px 0 #000;
    background: #000;
    overflow: hidden;
    margin-bottom: 8px;
  }}
  .screenshot-frame img {{
    width: 100%;
    height: auto;
    display: block;
    image-rendering: pixelated;
  }}
  .screenshot-caption {{
    background: #0f172a;
    font-family: 'VT323', monospace;
    font-size: 19px;
    color: #38bdf8;
    padding: 3px 8px;
    text-align: center;
    border-top: 2px solid #334155;
  }}

  /* Metric Badges */
  .metric-badge {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #022c22;
    border: 3px solid #34d399;
    padding: 8px 12px;
    margin-bottom: 8px;
    box-shadow: 4px 4px 0 #000;
  }}
  .metric-badge .title {{ font-size: 17px; font-weight: bold; color: #e2e8f0; }}
  .metric-badge .value {{ font-family: 'Press Start 2P', cursive; font-size: 11px; color: #facc15; }}

  /* HUD Simulation Bar */
  .hud-status-bar {{
    display: flex;
    gap: 12px;
    background: #000;
    border: 3px solid #475569;
    padding: 6px 12px;
    margin-top: 6px;
    justify-content: space-around;
  }}
  .hud-stat {{ font-family: 'VT323', monospace; font-size: 20px; }}
  .hud-stat span {{ color: #4ade80; font-weight: bold; }}

  .hotbar-box {{
    display: flex;
    gap: 6px;
    justify-content: center;
    background: #000;
    border: 3px solid #64748b;
    padding: 6px;
    margin-top: 6px;
  }}
  .hotbar-slot {{
    width: 42px;
    height: 42px;
    background: #1e293b;
    border: 2px solid #94a3b8;
    display: flex;
    align-items: center;
    justify-content: center;
    font-family: 'Press Start 2P', cursive;
    font-size: 10px;
    color: #fbbf24;
  }}
  .hotbar-slot.active {{
    border-color: #4ade80;
    background: #064e3b;
    box-shadow: 0 0 8px #4ade80;
  }}
</style>
</head>
<body>

  <!-- Header Banner -->
  <div class="header-banner">
    <div class="header-title">
      <h1>LIFE ON LAND: TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR</h1>
      <h2>POSTER PROYEK AKHIR GAME DEVELOPMENT (CIE 725) — UNIVERSITAS ESA UNGGUL (2026)</h2>
    </div>
    <div class="dosen-badge">
      <span class="label">DOSEN PENGAMPU:</span>
      <span class="name">Ir. Sawali Wahyu, S.Kom., M.Kom.</span>
    </div>
  </div>

  <!-- Poster 3 Column Grid -->
  <div class="poster-grid">

    <!-- COLUMN 1: Identitas & Konsep & Tujuan -->
    <div class="column">
      
      <!-- Identitas Kelompok -->
      <div class="pixel-box-gold">
        <div class="section-header">👥 IDENTITAS TIM KELOMPOK</div>
        
        <div class="team-member">
          <div class="top-row">
            <span class="name">Galih Adhi Kusuma</span>
            <span class="nim">20230801198</span>
          </div>
          <div class="role">Lead Programmer & Backend Engineer</div>
        </div>

        <div class="team-member">
          <div class="top-row">
            <span class="name">Firschanya Alula R.</span>
            <span class="nim">20230801201</span>
          </div>
          <div class="role">Art Director & Narrative Designer</div>
        </div>

        <div class="team-member">
          <div class="top-row">
            <span class="name">Defanda Yeremia C. R.</span>
            <span class="nim">20230801205</span>
          </div>
          <div class="role">System Analyst & QA Tester</div>
        </div>
      </div>

      <!-- Overview Narasi & Abstrak -->
      <div class="pixel-box">
        <div class="section-header">🌱 OVERVIEW & NARASI GAME</div>
        <p>
          Bumi pasca-apokaliptik mengalami krisis atmosfer masif dengan kadar oksigen tersisa <span class="highlight-gold">15.0%</span>. 
          Pemain berperan sebagai <span class="highlight">Restorer terakhir (Umbra)</span> yang sabar memulihkan biosfer tanah demi tanah 
          sambil mengejar antagonis <span class="highlight-blue">Blaze</span> yang aktif membakar hutan melintasi 3 region (Red, Orange, & Pink Bloom).
        </p>
      </div>

      <!-- Tujuan & Manfaat -->
      <div class="pixel-box-blue">
        <div class="section-header">🎯 TUJUAN & MANFAAT APLIKASI</div>
        <ul>
          <li><span class="highlight-gold">Tujuan Utama:</span> Memulihkan O2 atmosferik hingga >= 21.0% dan memurnikan tanah terpolusi 2-tahap.</li>
          <li><span class="highlight">Bagi Pemain:</span> Media hiburan simulasi taktis bercocok tanam sekaligus edukasi siklus air tanah.</li>
          <li><span class="highlight-blue">Bagi Instansi/Masyarakat:</span> Gamifikasi edukasi kesadaran pelestarian lingkungan pasca-bencana.</li>
          <li><span class="highlight-gold">Bagi Reviewer:</span> Bukti integrasi FSM vegetasi, Grid Matrix, & PlayFab Cloud Save.</li>
        </ul>
      </div>

    </div>

    <!-- COLUMN 2: Metode, Rancangan & Screenshot Output -->
    <div class="column">

      <!-- Metode & Rancangan Game -->
      <div class="pixel-box">
        <div class="section-header">🛠️ METODE & RANCANGAN ENGINE</div>
        <ul>
          <li><span class="highlight-gold">Game Genre:</span> Top-Down Tactical Eco-Restoration Simulator.</li>
          <li><span class="highlight">Engine & Bahasa:</span> Unity 6 (6000.0.x) / 2022.3 LTS & C# (.NET Core).</li>
          <li><span class="highlight-blue">Metode Pengembangan:</span> GDLC (Game Development Life Cycle - 6 Tahapan Iteratif).</li>
          <li><span class="highlight-gold">Finite State Machine (FSM):</span> Siklus vegetasi (Seed -> Sprout -> Young -> Mature -> Withered).</li>
          <li><span class="highlight">Grid World Matrix:</span> Tracking kelembapan tanah (moisture 0-1) & difusi emisi O2.</li>
        </ul>
        
        <!-- HUD Simulation -->
        <div class="hud-status-bar">
          <div class="hud-stat">O2: <span>50.0%</span></div>
          <div class="hud-stat">STAMINA: <span>100/100</span></div>
          <div class="hud-stat">WATER: <span>10 UNITS</span></div>
        </div>
        <div class="hotbar-box">
          <div class="hotbar-slot active">1:🪏</div>
          <div class="hotbar-slot">2:💧</div>
          <div class="hotbar-slot">3:🌱</div>
          <div class="hotbar-slot">4:🌲</div>
          <div class="hotbar-slot">5:🌿</div>
          <div class="hotbar-slot">6:🏗️</div>
        </div>
      </div>

      <!-- Tampilan Output Screenshots -->
      <div class="pixel-box-gold" style="flex: 1;">
        <div class="section-header">🖼️ TAMPILAN OUTPUT GAMEPLAY</div>
        
        <div class="screenshot-frame">
          <img src="{img_maliz}" alt="Purifikasi & Dialog Maliz">
          <div class="screenshot-caption">Output 1: Dialog Novel Visual Maliz & Purifikasi Lahan 2-Tahap</div>
        </div>

        <div class="screenshot-frame">
          <img src="{img_trees}" alt="Vegetasi Pohon Dewasa">
          <div class="screenshot-caption">Output 2: Vegetasi Pohon Dewasa & Emisi O2 Atmosferik</div>
        </div>
      </div>

    </div>

    <!-- COLUMN 3: Hasil Testing & Kesimpulan -->
    <div class="column">

      <!-- Hasil Testing Aplikasi -->
      <div class="pixel-box-blue">
        <div class="section-header">📊 HASIL TESTING APLIKASI</div>
        
        <div class="metric-badge">
          <span class="title">Alpha Testing (SUS Usability):</span>
          <span class="value">SKOR: 63.45 (GRADE D / OK)</span>
        </div>

        <div class="metric-badge">
          <span class="title">Beta Testing (UAT Acceptance):</span>
          <span class="value">82.4% (SANGAT LAYAK)</span>
        </div>

        <div class="screenshot-frame" style="margin-top: 6px;">
          <img src="{img_chart}" alt="Grafik Testing SUS">
          <div class="screenshot-caption">Output 3: Grafik Distribusi Skor Testing SUS (21 Responden)</div>
        </div>
      </div>

      <!-- Kesimpulan & Saran -->
      <div class="pixel-box" style="flex: 1;">
        <div class="section-header">📌 KESIMPULAN & SARAN</div>
        <p style="margin-bottom: 8px;">
          <span class="highlight-gold">Kesimpulan:</span> Game Life on Land Stage 1 berhasil dibangun 100% menggunakan Unity C# 
          dengan mekanik purifikasi 2-tahap, FSM daur hidup vegetasi, quest air, dan recovery O2 50.0%. 
          Hasil UAT 82.4% membuktikan game <span class="highlight">SANGAT LAYAK</span> untuk dimainkan.
        </p>
        <p>
          <span class="highlight-blue">Saran:</span> Realisasi Stage 2 (Soil Purifier) & Stage 3 (Pipa Irigasi, Heatwave, & penangkapan Blaze), 
          serta pengimbangan kontrol touch-screen joystick untuk Android/iOS.
        </p>
      </div>

    </div>

  </div>

</body>
</html>
"""

    with open(html_path, "w", encoding="utf-8") as f:
        f.write(html_content)
    print(f"Generated pixelated HTML poster: {html_path}")

    # Invoke Headless Chrome or Edge to capture screenshot
    chrome_path = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
    if not os.path.exists(chrome_path):
        chrome_path = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

    cmd = [
        chrome_path,
        "--headless=new",
        "--disable-gpu",
        "--hide-scrollbars",
        f"--window-size=1920,1357",
        f"--screenshot={output_png}",
        f"file:///{html_path.replace('\\', '/')}"
    ]

    print(f"Capturing headless browser screenshot using {chrome_path}...")
    res = subprocess.run(cmd, capture_output=True, text=True)
    if res.returncode == 0 and os.path.exists(output_png):
        print(f"SUCCESS! High-Resolution Pixelated Poster PNG saved at:\n{output_png}")
    else:
        print(f"Screenshot Error: {res.stderr}")

if __name__ == "__main__":
    build_pixel_poster()
