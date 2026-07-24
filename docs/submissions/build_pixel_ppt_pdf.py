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

def build_pixel_ppt():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    screenshots_dir = r"c:\Users\galih\Documents\Projects\Game\My project\Assets\Screenshots"
    charts_dir = os.path.join(sub_dir, "charts")

    html_path = os.path.join(sub_dir, "slides_pixelated.html")
    pdf_path = os.path.join(sub_dir, "PRESENTASI_LIFE_ON_LAND.pdf")

    # Base64 Images
    img_maliz = get_base64_image(os.path.join(screenshots_dir, "maliz_dialogs.png"))
    img_trees = get_base64_image(os.path.join(screenshots_dir, "grown_trees.png"))
    img_complete = get_base64_image(os.path.join(screenshots_dir, "restoration_complete.png"))
    img_menu = get_base64_image(os.path.join(screenshots_dir, "main menu.png"))
    img_chart_sus = get_base64_image(os.path.join(charts_dir, "gform_chart_5_sus_scores.png"))
    img_chart_uat = get_base64_image(os.path.join(charts_dir, "gform_chart_6_uat_aspects.png"))

    html_content = f"""<!DOCTYPE html>
<html lang="id">
<head>
<meta charset="UTF-8">
<title>Presentasi Life on Land - Widescreen Pixel Edition</title>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@600;700&family=Pixelify+Sans:wght@400;600;700&family=Press+Start+2P&family=VT323&display=swap" rel="stylesheet">
<style>
  @page {{
    size: 16in 9in;
    margin: 0;
  }}

  * {{
    box-sizing: border-box;
    margin: 0;
    padding: 0;
  }}

  body {{
    font-family: 'Pixelify Sans', 'VT323', sans-serif;
    background-color: #08100b;
    color: #e2e8f0;
    -webkit-print-color-adjust: exact;
  }}

  .slide {{
    width: 16in;
    height: 9in;
    page-break-after: always;
    position: relative;
    overflow: hidden;
    padding: 0.45in 0.6in;
    background: #08100b;
    background-image: 
      radial-gradient(circle at 50% 50%, #12281a 0%, #050b07 100%),
      linear-gradient(rgba(18, 16, 16, 0) 50%, rgba(0, 0, 0, 0.25) 50%);
    background-size: 100% 100%, 100% 4px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }}

  /* CRT Scanline Overlay */
  .slide::before {{
    content: " ";
    display: block;
    position: absolute;
    top: 0; left: 0; bottom: 0; right: 0;
    background: linear-gradient(rgba(18, 16, 16, 0) 50%, rgba(0, 0, 0, 0.18) 50%);
    background-size: 100% 4px;
    z-index: 10;
    pointer-events: none;
  }}

  /* Slide Header Banner */
  .header-bar {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #0d1a12;
    border: 4px solid #10b981;
    box-shadow: 5px 5px 0px #000;
    padding: 10px 20px;
    margin-bottom: 16px;
  }}

  .header-left .chapter {{
    font-family: 'Press Start 2P', cursive;
    font-size: 10px;
    color: #fbbf24;
    letter-spacing: 1px;
    margin-bottom: 4px;
  }}

  .header-left .title {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 26px;
    font-weight: 700;
    color: #4ade80;
    text-shadow: 2px 2px 0 #000;
  }}

  .slide-counter {{
    font-family: 'Press Start 2P', cursive;
    font-size: 10px;
    background: #064e3b;
    color: #facc15;
    padding: 6px 12px;
    border: 2px solid #34d399;
    box-shadow: 2px 2px 0 #000;
  }}

  /* Pixel Boxes & Panels */
  .px-box {{
    background: #0d1912;
    border: 4px solid #10b981;
    box-shadow: 
      inset -4px -4px 0px #042f2e,
      inset 4px 4px 0px #6ee7b7,
      5px 5px 0px #000000;
    padding: 16px;
  }}

  .px-box-gold {{
    background: #18150a;
    border: 4px solid #f59e0b;
    box-shadow: 
      inset -4px -4px 0px #451a03,
      inset 4px 4px 0px #fde68a,
      5px 5px 0px #000000;
    padding: 16px;
  }}

  .px-box-blue {{
    background: #0a1120;
    border: 4px solid #38bdf8;
    box-shadow: 
      inset -4px -4px 0px #1e3a8a,
      inset 4px 4px 0px #bae6fd,
      5px 5px 0px #000000;
    padding: 16px;
  }}

  .px-header {{
    font-family: 'Press Start 2P', cursive;
    font-size: 11px;
    color: #facc15;
    background: #064e3b;
    padding: 6px 10px;
    border: 2px solid #34d399;
    box-shadow: 2px 2px 0 #000;
    margin-bottom: 12px;
    display: inline-block;
  }}

  p, li {{
    font-size: 18px;
    line-height: 1.4;
    color: #cbd5e1;
  }}

  ul {{ list-style: none; }}
  ul li {{
    position: relative;
    padding-left: 20px;
    margin-bottom: 8px;
  }}
  ul li::before {{
    content: "▶";
    position: absolute;
    left: 0;
    color: #4ade80;
    font-size: 12px;
  }}

  .hl-green {{ color: #4ade80; font-weight: bold; }}
  .hl-gold {{ color: #fbbf24; font-weight: bold; }}
  .hl-blue {{ color: #38bdf8; font-weight: bold; }}
  .hl-red {{ color: #f87171; font-weight: bold; }}

  /* Footer Bar */
  .footer-bar {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #050b07;
    border-top: 2px solid #1e293b;
    padding-top: 8px;
    font-family: 'VT323', monospace;
    font-size: 20px;
    color: #64748b;
  }}

  /* Screenshot Frames */
  .screen-frame {{
    border: 4px solid #475569;
    box-shadow: 4px 4px 0 #000;
    background: #000;
    overflow: hidden;
  }}
  .screen-frame img {{
    width: 100%;
    height: auto;
    display: block;
    image-rendering: pixelated;
  }}
  .screen-caption {{
    background: #0f172a;
    font-family: 'VT323', monospace;
    font-size: 19px;
    color: #38bdf8;
    padding: 4px 8px;
    text-align: center;
    border-top: 2px solid #334155;
  }}

  /* Custom Layout Elements */
  .stepper-container {{
    display: flex;
    gap: 12px;
    margin-top: 10px;
  }}
  .step-card {{
    flex: 1;
    background: #062016;
    border: 3px solid #10b981;
    padding: 12px;
    box-shadow: 3px 3px 0 #000;
  }}
  .step-num {{
    font-family: 'Press Start 2P', cursive;
    font-size: 12px;
    color: #fbbf24;
    margin-bottom: 6px;
  }}

  .fsm-diagram {{
    display: flex;
    align-items: center;
    justify-content: space-between;
    background: #000;
    border: 3px solid #f59e0b;
    padding: 14px;
    margin-top: 12px;
  }}
  .fsm-node {{
    font-family: 'Press Start 2P', cursive;
    font-size: 10px;
    background: #1e293b;
    border: 2px solid #94a3b8;
    color: #4ade80;
    padding: 8px 12px;
    text-align: center;
  }}
  .fsm-node.active {{
    background: #064e3b;
    border-color: #4ade80;
    color: #facc15;
    box-shadow: 0 0 10px #4ade80;
  }}
  .fsm-arrow {{
    font-family: 'VT323', monospace;
    font-size: 24px;
    color: #fbbf24;
  }}

  /* Table styling */
  .px-table {{
    width: 100%;
    border-collapse: collapse;
    margin-top: 8px;
  }}
  .px-table th {{
    background: #064e3b;
    color: #facc15;
    font-family: 'Press Start 2P', cursive;
    font-size: 10px;
    padding: 8px;
    border: 2px solid #10b981;
    text-align: left;
  }}
  .px-table td {{
    background: #0a1710;
    color: #cbd5e1;
    padding: 8px;
    border: 2px solid #1e3a2b;
    font-size: 16px;
  }}

</style>
</head>
<body>

  <!-- SLIDE 1: RETRO TITLE SCREEN (COVER) -->
  <div class="slide" style="justify-content: center; align-items: center; text-align: center;">
    <div style="width: 90%;">
      <div style="font-family: 'Press Start 2P', cursive; font-size: 28px; color: #4ade80; text-shadow: 4px 4px 0 #064e3b, 7px 7px 0 #000; margin-bottom: 12px;">
        LIFE ON LAND
      </div>
      <div style="font-family: 'VT323', monospace; font-size: 34px; color: #fef08a; letter-spacing: 2px; margin-bottom: 30px;">
        TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR
      </div>

      <div style="background: #0d1a12; border: 4px solid #10b981; box-shadow: 6px 6px 0 #000; padding: 20px; margin-bottom: 30px;">
        <div style="font-family: 'Press Start 2P', cursive; font-size: 11px; color: #fbbf24; margin-bottom: 12px;">
          PROYEK AKHIR GAME DEVELOPMENT (CIE 725) — UNIVERSITAS ESA UNGGUL (2026)
        </div>
        <div style="font-size: 22px; color: #ffffff;">
          Dosen Pengampu: <strong style="color:#38bdf8;">Ir. Sawali Wahyu, S.Kom., M.Kom.</strong>
        </div>
      </div>

      <div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px;">
        <div class="px-box">
          <div style="font-family: 'Press Start 2P', cursive; font-size: 10px; color: #4ade80; margin-bottom: 4px;">PRAMAN / LEAD</div>
          <div style="font-size: 20px; font-weight: bold; color: #fff;">Galih Adhi Kusuma</div>
          <div style="font-family: 'VT323', monospace; font-size: 22px; color: #fbbf24;">NIM: 20230801198</div>
        </div>
        <div class="px-box">
          <div style="font-family: 'Press Start 2P', cursive; font-size: 10px; color: #4ade80; margin-bottom: 4px;">ART & NARRATIVE</div>
          <div style="font-size: 20px; font-weight: bold; color: #fff;">Firschanya Alula R.</div>
          <div style="font-family: 'VT323', monospace; font-size: 22px; color: #fbbf24;">NIM: 20230801201</div>
        </div>
        <div class="px-box">
          <div style="font-family: 'Press Start 2P', cursive; font-size: 10px; color: #4ade80; margin-bottom: 4px;">ANALYST & QA</div>
          <div style="font-size: 20px; font-weight: bold; color: #fff;">Defanda Yeremia C. R.</div>
          <div style="font-family: 'VT323', monospace; font-size: 22px; color: #fbbf24;">NIM: 20230801205</div>
        </div>
      </div>
    </div>
  </div>

  <!-- SLIDE 2: BAB I - LATAR BELAKANG & KRISIS BIOSFER -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB I: PENDAHULUAN</div>
        <div class="title">Latar Belakang & Permasalahan Ekologis Biosfer</div>
      </div>
      <div class="slide-counter">SLIDE 02 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1.2fr 1fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">🌎 KRISIS ATMOSFER BUMI PASCA-APOKALIPTIK</div>
        <p style="margin-bottom: 12px;">
          Bumi di masa depan mengalami kehancuran ekosistem masif akibat bencana dahsyat dan eksploitasi tanpa batas. 
          Kadar oksigen (<span class="hl-gold">O₂</span>) atmosfer terdegradasi hingga level sangat kritis yaitu <span class="hl-red">15.0%</span> (batas minimal kelangsungan hidup manusia adalah 19.5%–21.0%).
        </p>
        <p style="margin-bottom: 12px;">
          Sebagian besar wilayah tanah telah berubah menjadi <span class="hl-red">Corrupted Burnt Soil</span> — tanah hangus beracun yang tidak mampu menahan kelembapan air dan membunuh vegetasi dalam waktu singkat.
        </p>
        <p>
          Kurangnya media pembelajaran interaktif yang mampu mensimulasikan mekanisme fotosintesis dan siklus air tanah secara taktis mendorong dibuatnya game edukasi restorasi ini.
        </p>
      </div>

      <div class="px-box-gold">
        <div class="px-header">🎮 SOLUSI SIMULASI TAKTIS INTERAKTIF</div>
        <p style="margin-bottom: 12px;">
          Gim <span class="hl-green">Life on Land</span> hadir sebagai gim simulasi taktis berbasis tahap (*stage-based tactical simulator*) yang mengedukasi pemain mengenai pentingnya restorasi hutan.
        </p>
        <ul>
          <li><span class="hl-gold">Pendekatan Experiential Learning:</span> Pemain belajar langsung hubungan antara penyiraman tanah, retensi air, dan laju emisi O₂.</li>
          <li><span class="hl-green">Cozy Simulation with Stakes:</span> Menawarkan atmosfir bertani yang menenangkan namun didorong oleh batas waktu stamina dan ancaman Blaze.</li>
        </ul>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 3: BAB I - KONSEP GAME & GAMIFIKASI STEPPER -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB I: PENDAHULUAN</div>
        <div class="title">Konsep Utama & Skenario Gamifikasi (C-A-R-E)</div>
      </div>
      <div class="slide-counter">SLIDE 03 / 14</div>
    </div>

    <div style="display: flex; flex-direction: column; gap: 16px; flex: 1;">
      <div class="px-box-blue">
        <div class="px-header">🎭 SKENARIO CERITA & PARADIGMA KULTUR GAME</div>
        <p>
          Pemain berperan sebagai <span class="hl-green">Umbra (Restorer Terakhir)</span> yang sabar dan disiplin. 
          Umbra menjelajahi 3 bioma terdegradasi (Red Region, Orange Region, & Pink Bloom) untuk mengejar antagonis <span class="hl-red">Blaze</span> 
          yang aktif membakar sisa hutan terakhir.
        </p>
      </div>

      <div class="px-header" style="align-self: flex-start;">🔄 ALUR GAMIFIKASI: CHALLENGE ➔ ACTION ➔ REWARD ➔ ENVIRONMENTAL SHIFT</div>

      <div class="stepper-container">
        <div class="step-card">
          <div class="step-num">STEP 1: CHALLENGE</div>
          <p style="font-size: 15px;">Ubin terbakar beracun, O₂ kritis 15.0%, & dehidrasi tanaman.</p>
        </div>
        <div class="step-card">
          <div class="step-num">STEP 2: ACTION</div>
          <p style="font-size: 15px;">Purifikasi 2-tahap (Sekop & Water), penyiraman & penanaman pohon.</p>
        </div>
        <div class="step-card">
          <div class="step-num">STEP 3: REWARD</div>
          <p style="font-size: 15px;">Poin XP, benih baru, blueprints irigasi & unlock gate stage.</p>
        </div>
        <div class="step-card" style="border-color: #facc15;">
          <div class="step-num" style="color:#4ade80;">STEP 4: SHIFT</div>
          <p style="font-size: 15px;">Visual dunia berubah dari sepia gersang menjadi hutan hijau hidup.</p>
        </div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 4: BAB I - GDLC TIMELINE RIBBON -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB I: PENDAHULUAN</div>
        <div class="title">Metodologi Pengembangan: GDLC 6 Tahapan Iteratif</div>
      </div>
      <div class="slide-counter">SLIDE 04 / 14</div>
    </div>

    <div style="display: flex; flex-direction: column; gap: 14px; flex: 1;">
      <p style="font-size: 18px;">
        Menurut Ramadan & Widyani (2025), metodologi <span class="hl-gold">Game Development Life Cycle (GDLC)</span> digunakan untuk mengakomodasi iterasi kreatif dan playtesting mekanik secara fleksibel:
      </p>

      <div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; flex: 1;">
        <div class="px-box">
          <div class="px-header">1. INITIATION</div>
          <p style="font-size: 16px;">Ideasi konsep gim ekologi, pemetaan mekanik purifikasi, dan analisis kebutuhan awal.</p>
        </div>
        <div class="px-box">
          <div class="px-header">2. PRE-PRODUCTION</div>
          <p style="font-size: 16px;">Penyusunan GDD, perancangan sprite 32 PPU, map head offset, dan skenario 3 stage.</p>
        </div>
        <div class="px-box-gold">
          <div class="px-header">3. PRODUCTION</div>
          <p style="font-size: 16px;">Koding C# Unity untuk Grid World Matrix, FSM tanaman, hotbar 1-6, & PlayFab SDK.</p>
        </div>
        <div class="px-box-gold">
          <div class="px-header">4. TESTING</div>
          <p style="font-size: 16px;">Pengujian Usability Alpha (SUS 21 responden) dan Acceptance Beta (UAT 5 Aspek).</p>
        </div>
        <div class="px-box-blue">
          <div class="px-header">5. BETA RELEASE</div>
          <p style="font-size: 16px;">Kompilasi build Standalone PC (.exe) & WebGL, pengujian cloud save PlayFab.</p>
        </div>
        <div class="px-box-blue">
          <div class="px-header">6. POST-PROD</div>
          <p style="font-size: 16px;">Penyusunan dokumentasi teknis, user manual, poster A4, dan evaluasi laporan 5 bab.</p>
        </div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 5: BAB II - FSM STATE MACHINE DIAGRAM LAYOUT -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB II: LANDASAN TEORI</div>
        <div class="title">Teori Khusus: Finite State Machine (FSM) Vegetasi</div>
      </div>
      <div class="slide-counter">SLIDE 05 / 14</div>
    </div>

    <div style="display: flex; flex-direction: column; gap: 16px; flex: 1;">
      <div class="px-box">
        <div class="px-header">⚙️ MODEL FINITE STATE MACHINE SIKLUS HIDUP TANAMAN</div>
        <p>
          Menurut Alsveta & Haryanto (2024), <span class="hl-gold">Finite State Machine (FSM)</span> mengontrol transisi status biologis tanaman berdasarkan ketersediaan air tanah (<span class="hl-blue">moisture</span>) dan interval detak waktu (<span class="hl-green">tickInterval = 5s</span>).
        </p>

        <!-- FSM Nodes Visual -->
        <div class="fsm-diagram">
          <div class="fsm-node">SEED<br><small>(Benih)</small></div>
          <div class="fsm-arrow">➔</div>
          <div class="fsm-node">SPROUT<br><small>(Tunas)</small></div>
          <div class="fsm-arrow">➔</div>
          <div class="fsm-node">SAPLING<br><small>(Kecambah)</small></div>
          <div class="fsm-arrow">➔</div>
          <div class="fsm-node">YOUNG<br><small>(Pohon Muda)</small></div>
          <div class="fsm-arrow">➔</div>
          <div class="fsm-node active">MATURE<br><small>(Pohon Dewasa)</small></div>
        </div>
      </div>

      <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px;">
        <div class="px-box-gold">
          <div class="px-header">💧 MEKANIK DEHIDRASI & WITHERED STATE</div>
          <p style="font-size: 16px;">
            Jika ubin tempat pohon tumbuh memiliki <span class="hl-red">moisture == 0.0</span> selama 3 tick berturut-turut, FSM memicu transisi ke status <span class="hl-red">Withered (Layu)</span> dan emisi O₂ terhenti.
          </p>
        </div>

        <div class="px-box-blue">
          <div class="px-header">🔄 MEKANIK PEMULIHAN (REVIVE)</div>
          <p style="font-size: 16px;">
            Pohon layu <strong>tidak mati permanen</strong>. Menyiramkan air pada ubin layu memicu method <span class="hl-green">Revive()</span> yang memulihkan status pohon ke kondisi tumbuh sebelum layu.
          </p>
        </div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 6: BAB II - GRID WORLD MATRIX & TECH STACK -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB II: LANDASAN TEORI</div>
        <div class="title">Grid World Matrix 2D & Platform Teknologi</div>
      </div>
      <div class="slide-counter">SLIDE 06 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1.1fr 1fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">🗺️ ARSITEKTUR GRID WORLD MATRIX (HARYONO, 2026)</div>
        <p style="margin-bottom: 10px;">
          Membagi ruang simulasi 2D ke dalam matriks sel terstruktur koordinat ($x, y$) berbasis kontainer data independen:
        </p>
        <ul>
          <li><span class="hl-blue">moisture (float):</span> Nilai kelembapan tanah ($0.0$ kering s.d $1.0$ basah). Ter-evaporasi $-0.05$ setiap 5 detik (2x cepat saat *Heatwave*).</li>
          <li><span class="hl-red">corruptionState (int):</span> 0 = Normal Soil, 1 = Burnt Soil, 2 = Dug Burnt Soil.</li>
          <li><span class="hl-green">localO2 (float):</span> Konsentrasi oksigen sel yang difungsikan melalui algoritma rata-rata 4 tetangga (*Cellular Automata Diffusion*).</li>
        </ul>
      </div>

      <div class="px-box-blue">
        <div class="px-header">🧰 PLATFORM TEKNOLOGI (TECH STACK)</div>
        <ul>
          <li><span class="hl-gold">Unity 6 / 2022.3 LTS:</span> Game engine berbasis *Component-Based Architecture* & Tilemap rendering 2D.</li>
          <li><span class="hl-green">C# (.NET Core):</span> Bahasa pemrograman berorientasi objek (*strongly-typed*) untuk skrip `Player.cs`, `EnvironmentManager.cs`, & `Tree.cs`.</li>
          <li><span class="hl-blue">PlayFab SDK:</span> Title Data Cloud Save & Leaderboard real-time.</li>
          <li><span class="hl-gold">PlantUML & Aseprite:</span> Pemodelan 5 diagram UML & pembuatan aset piksel 32 PPU.</li>
        </ul>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 7: BAB III - SHOWCASE KARAKTER & PROTOTYPE MAIN MENU -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="title">Daftar Karakter & Prototype Main Menu</div>
      </div>
      <div class="slide-counter">SLIDE 07 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1fr 1.2fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">🎭 DAFTAR KARAKTER GAME</div>
        <ul style="font-size: 16px;">
          <li><span class="hl-green">Umbra (Restorer):</span> Karakter utama, rompi ungu, pembawa misi restorasi.</li>
          <li><span class="hl-blue">Blaze (Antagonis):</span> Tunik biru polos, musuh pembakar hutan.</li>
          <li><span class="hl-red">Maliz (Stage 1):</span> Beruang Wrath Barbarian peminta air.</li>
          <li><span class="hl-gold">Oryel (Stage 2):</span> Rubah Pride Rogue mandiri.</li>
          <li><span class="hl-blue">Pyper (Stage 3):</span> Ngengat Lust Bard pemikat.</li>
        </ul>
      </div>

      <div class="screen-frame">
        <img src="{img_menu}" alt="Main Menu">
        <div class="screen-caption">Prototype Scene: MainMenuScene.unity & Autentikasi Login PlayFab</div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 8: BAB III - DEMO PURIFIKASI LAHAN 2-TAHAP -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="title">Demostrasi Pemurnian Lahan 2-Tahap (Two-Step Purification)</div>
      </div>
      <div class="slide-counter">SLIDE 08 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1.2fr 1fr; gap: 20px; flex: 1;">
      <div class="screen-frame">
        <img src="{img_maliz}" alt="Dialog Maliz & Purifikasi">
        <div class="screen-caption">Tampilan Gameplay Stage 1: Dialog Novel Visual Maliz & Purifikasi Lahan Terbakar</div>
      </div>

      <div class="px-box-gold">
        <div class="px-header">🪏 PROSEDUR TEKNIS PURIFIKASI LAHAN</div>
        <p style="margin-bottom: 10px;">
          Lahan terkontaminasi tidak dapat langsung ditanami benih. Pemain wajib menjalankan prosedur 2-tahap:
        </p>
        <ul>
          <li><span class="hl-gold">Tahap 1 (Sekop - Slot 1):</span> Menyiangi ubin hitam terbakar (*Burnt Tile*) menjadi ubin tergali (*Dug Burnt Soil*). Mengonsumsi 5 Stamina.</li>
          <li><span class="hl-blue">Tahap 2 (Watering Can - Slot 2):</span> Menyiram ubin tergali dengan 1 unit air kolam untuk mengubahnya menjadi tanah subur bersih (*Normal Soil*).</li>
        </ul>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 9: BAB III - VEGETASI & MATRIKS SPESIES TANAMAN -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="title">Spesies Tanaman & Pemulihan Oksigen Atmosfer</div>
      </div>
      <div class="slide-counter">SLIDE 09 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1fr 1.2fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">🌱 MATRIKS SPESIES VEGETASI</div>
        <table class="px-table">
          <tr>
            <th>Spesies</th>
            <th>Sifat Utama</th>
            <th>Peran Strategis</th>
          </tr>
          <tr>
            <td><strong class="hl-gold">Pine Tree (Type A)</strong></td>
            <td>High O₂, High Water</td>
            <td>Mesin pemulih O₂ utama</td>
          </tr>
          <tr>
            <td><strong class="hl-green">Desert Shrub (Type B)</strong></td>
            <td>Low Water, Retensi Air</td>
            <td>Penstabil kelembapan tanah</td>
          </tr>
          <tr>
            <td><strong class="hl-blue">Silkmoth Fern (Type C)</strong></td>
            <td>Heatwave Resistant</td>
            <td>Tanaman wajib Stage 3</td>
          </tr>
        </table>
        <p style="margin-top: 12px; font-size:16px;">
          Penanaman 5 Semak Gurun di Stage 1 meningkatkan O₂ atmosferik hingga <span class="hl-green">50.0%</span> untuk menyelesaikan demo!
        </p>
      </div>

      <div class="screen-frame">
        <img src="{img_trees}" alt="Vegetasi Pohon Dewasa">
        <div class="screen-caption">Tampilan Gameplay Stage 1: Pohon Dewasa (Mature Tree) & Injeksi O₂ Atmosfer</div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 10: BAB IV - TESTING ALPHA (SUS SCOREBOARD) -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="title">Pengujian Usability Alpha — System Usability Scale (SUS)</div>
      </div>
      <div class="slide-counter">SLIDE 10 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1fr 1.2fr; gap: 20px; flex: 1;">
      <div class="px-box-blue">
        <div class="px-header">📊 EVALUASI SKOR SUS (21 RESPONDEN)</div>
        <p style="margin-bottom: 10px;">
          Pengujian Usability Alpha dilaksanakan oleh <span class="hl-gold">Defanda Yeremia (QA Tester)</span> menggunakan instrumen 10 pertanyaan terstandar SUS skala Likert 1–5.
        </p>
        
        <div style="background:#022c22; border:3px solid #34d399; padding:12px; text-align:center; margin:12px 0;">
          <div style="font-family:'Press Start 2P', cursive; font-size:24px; color:#facc15;">SKOR: 63.45</div>
        </div>

        <ul>
          <li><span class="hl-green">Acceptability Range:</span> Marginal High</li>
          <li><span class="hl-gold">Grade Scale:</span> Grade D</li>
          <li><span class="hl-blue">Adjective Rating:</span> OK</li>
        </ul>
      </div>

      <div class="screen-frame">
        <img src="{img_chart_sus}" alt="Grafik SUS">
        <div class="screen-caption">Hasil Pengujian Alpha: Grafik Perhitungan Skor SUS 21 Responden</div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 11: BAB IV - TESTING BETA (UAT 5 ASPEK) -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="title">Pengujian Acceptance Beta — User Acceptance Testing (UAT)</div>
      </div>
      <div class="slide-counter">SLIDE 11 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1.2fr 1fr; gap: 20px; flex: 1;">
      <div class="screen-frame">
        <img src="{img_chart_uat}" alt="Grafik UAT">
        <div class="screen-caption">Hasil Pengujian Beta: Grafik Persentase Keberhasilan 5 Aspek UAT</div>
      </div>

      <div class="px-box-gold">
        <div class="px-header">✅ PERSENTASE 5 ASPEK UAT</div>
        <ul>
          <li><strong>Fungsionalitas Sistem:</strong> <span class="hl-green">85.2%</span> (Sangat Layak)</li>
          <li><strong>Desain Visual Art:</strong> <span class="hl-green">82.1%</span> (Sangat Layak)</li>
          <li><strong>Audio BGM & SFX:</strong> <span class="hl-gold">80.0%</span> (Layak)</li>
          <li><strong>Kenyamanan Usability:</strong> <span class="hl-green">83.5%</span> (Sangat Layak)</li>
          <li><strong>Kinerja Performa:</strong> <span class="hl-green">81.0%</span> (Sangat Layak)</li>
        </ul>
        <div style="margin-top:14px; font-family:'Press Start 2P', cursive; font-size:14px; color:#4ade80; text-align:center; background:#064e3b; padding:8px; border:2px solid #34d399;">
          RATA-RATA: 82.4% (SANGAT LAYAK)
        </div>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 12: BAB IV - ARSITEKTUR HYBRID & PLAYFAB CLOUD -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="title">Arsitektur Hybrid Two-Tier & PlayFab Cloud Save</div>
      </div>
      <div class="slide-counter">SLIDE 12 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">💻 CLIENT-SIDE SIMULATION LAYER</div>
        <p style="margin-bottom:10px;">
          Unity Engine C# mengelola input pemain, stamina, buffer O₂, pergerakan fisika 2D, serta eksekusi tick 5 detik pada Grid World Matrix secara independen di perangkat lokal client.
        </p>
      </div>

      <div class="px-box-blue">
        <div class="px-header">☁️ PLAYFAB CLOUD SERVICES LAYER</div>
        <p style="margin-bottom:10px;">
          Skema terstruktur <span class="hl-gold">SaveData.json</span> men-serialize status pemain (`playerId`, `stamina`, `gridCells`, `achievements`, `globalO2Percentage`) ke PlayFab Title Data untuk sinkronisasi cloud save & leaderboard global real-time.
        </p>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 13: BAB V - KESIMPULAN & SARAN -->
  <div class="slide">
    <div class="header-bar">
      <div class="header-left">
        <div class="chapter">BAB V: KESIMPULAN DAN SARAN</div>
        <div class="title">Kesimpulan Penelitian & Saran Pengembangan</div>
      </div>
      <div class="slide-counter">SLIDE 13 / 14</div>
    </div>

    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; flex: 1;">
      <div class="px-box">
        <div class="px-header">📌 KESIMPULAN UTAMA</div>
        <p style="margin-bottom:10px;">
          1. Gim <span class="hl-green">Life on Land Demo Stage 1</span> berhasil dibangun 100% menggunakan Unity C# dengan mekanik purifikasi 2-tahap, FSM tanaman, quest air, & recovery O₂ 50.0%.
        </p>
        <p>
          2. Hasil Pengujian Usability Alpha (SUS) memperoleh skor <span class="hl-gold">63.45 (OK)</span> dan Pengujian Acceptance Beta (UAT) memperoleh persentase <span class="hl-green">82.4% (SANGAT LAYAK)</span>.
        </p>
      </div>

      <div class="px-box-gold">
        <div class="px-header">💡 SARAN PENGEMBANGAN</div>
        <p style="margin-bottom:10px;">
          1. <span class="hl-gold">Pengembangan Stage 2 & 3:</span> Merealisasikan Stage 2 (Soil Purifier) & Stage 3 (Pipa Irigasi, Heatwave, & penangkapan Blaze).
        </p>
        <p>
          2. <span class="hl-blue">Adaptasi Platform Mobile:</span> Pengimbangan kontrol touch-screen joystick untuk rilis Android/iOS.
        </p>
      </div>
    </div>

    <div class="footer-bar">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 14: CLOSING SLIDE / GAMEOVER CREDITS -->
  <div class="slide" style="justify-content: center; align-items: center; text-align: center;">
    <div style="width: 85%;">
      <div style="font-family: 'Press Start 2P', cursive; font-size: 42px; color: #4ade80; text-shadow: 4px 4px 0 #064e3b, 7px 7px 0 #000; margin-bottom: 20px;">
        TERIMA KASIH!
      </div>
      <div style="font-family: 'VT323', monospace; font-size: 32px; color: #fef08a; letter-spacing: 2px; margin-bottom: 30px;">
        INSERT COIN TO START Q&A SESSION
      </div>

      <div class="px-box-gold" style="padding: 24px;">
        <div style="font-size: 22px; color: #ffffff; margin-bottom: 8px;">
          LIFE ON LAND — TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR
        </div>
        <div style="font-size: 18px; color: #94a3b8;">
          Kelompok Game Development — Program Studi Teknik Informatika<br>
          Fakultas Ilmu Komputer, Universitas Esa Unggul (2026)
        </div>
      </div>
    </div>
  </div>

</body>
</html>
"""

    with open(html_path, "w", encoding="utf-8") as f:
        f.write(html_content)
    print(f"Generated Widescreen Pixelated HTML presentation: {html_path}")

    # Invoke Headless Chrome or Edge to render PDF
    chrome_path = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
    if not os.path.exists(chrome_path):
        chrome_path = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

    cmd = [
        chrome_path,
        "--headless=new",
        "--disable-gpu",
        "--no-margins",
        f"--print-to-pdf={pdf_path}",
        f"file:///{html_path.replace('\\', '/')}"
    ]

    print(f"Rendering Pixel-Styled PDF presentation using {chrome_path}...")
    res = subprocess.run(cmd, capture_output=True, text=True)
    if res.returncode == 0 and os.path.exists(pdf_path):
        print(f"SUCCESS! 14-Slide Widescreen Pixel PDF Presentation generated at:\n{pdf_path}")
    else:
        print(f"PDF Render Error: {res.stderr}")

if __name__ == "__main__":
    build_pixel_ppt()
