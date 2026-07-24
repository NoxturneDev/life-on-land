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

def generate_pdf_slides():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    screenshots_dir = r"c:\Users\galih\Documents\Projects\Game\My project\Assets\Screenshots"
    charts_dir = os.path.join(sub_dir, "charts")

    html_path = os.path.join(sub_dir, "slides_presentation.html")
    pdf_path = os.path.join(sub_dir, "PRESENTASI_LIFE_ON_LAND.pdf")

    # Images
    img_maliz = get_base64_image(os.path.join(screenshots_dir, "maliz_dialogs.png"))
    img_trees = get_base64_image(os.path.join(screenshots_dir, "grown_trees.png"))
    img_complete = get_base64_image(os.path.join(screenshots_dir, "restoration_complete.png"))
    img_menu = get_base64_image(os.path.join(screenshots_dir, "main menu.png"))
    img_chart_sus = get_base64_image(os.path.join(charts_dir, "gform_chart_5_sus_scores.png"))
    img_chart_uat = get_base64_image(os.path.join(charts_dir, "gform_chart_6_uat_aspects.png"))
    img_chart_overall = get_base64_image(os.path.join(charts_dir, "gform_chart_7_uat_overall.png"))

    html_content = f"""<!DOCTYPE html>
<html lang="id">
<head>
<meta charset="UTF-8">
<title>Presentasi Life on Land - 14 Slides Widescreen</title>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Chakra+Petch:wght@500;600;700&family=Press+Start+2P&family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
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
    font-family: 'Inter', sans-serif;
    background-color: #0b130e;
    color: #e2e8f0;
    -webkit-print-color-adjust: exact;
  }}

  .slide {{
    width: 16in;
    height: 9in;
    page-break-after: always;
    position: relative;
    overflow: hidden;
    padding: 0.6in 0.8in;
    background: #0b130e;
    background-image: 
      radial-gradient(circle at 80% 20%, #152e1c 0%, transparent 40%),
      radial-gradient(circle at 10% 90%, #064e3b 0%, transparent 40%);
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }}

  /* Slide Header */
  .slide-header {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 3px solid #10b981;
    padding-bottom: 0.25in;
    margin-bottom: 0.3in;
  }}

  .slide-chapter {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 18px;
    font-weight: 700;
    color: #fbbf24;
    letter-spacing: 2px;
    text-transform: uppercase;
  }}

  .slide-title {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 32px;
    font-weight: 700;
    color: #4ade80;
  }}

  .slide-footer {{
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-top: 1px solid #1e293b;
    padding-top: 0.15in;
    font-size: 14px;
    color: #64748b;
  }}

  /* Cover Slide */
  .cover-slide {{
    justify-content: center;
    align-items: center;
    text-align: center;
    background: linear-gradient(135deg, #091a10 0%, #050b07 100%);
  }}

  .cover-title {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 48px;
    font-weight: 700;
    color: #4ade80;
    text-shadow: 0 4px 12px rgba(16, 185, 129, 0.3);
    margin-bottom: 12px;
  }}

  .cover-subtitle {{
    font-size: 24px;
    color: #fef08a;
    margin-bottom: 40px;
    letter-spacing: 1px;
  }}

  .cover-team {{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
    width: 80%;
    margin: 0 auto 40px auto;
  }}

  .team-card {{
    background: rgba(16, 185, 129, 0.08);
    border: 2px solid #10b981;
    padding: 16px;
    border-radius: 8px;
  }}

  .team-card .name {{ font-weight: 700; font-size: 18px; color: #ffffff; }}
  .team-card .nim {{ font-size: 15px; color: #fbbf24; margin-top: 4px; }}
  .team-card .role {{ font-size: 14px; color: #94a3b8; margin-top: 2px; }}

  /* Grid Layouts */
  .grid-2col {{
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 0.4in;
    flex: 1;
  }}

  .grid-3col {{
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    gap: 0.3in;
    flex: 1;
  }}

  .card {{
    background: rgba(15, 23, 42, 0.7);
    border: 2px solid #334155;
    border-radius: 8px;
    padding: 24px;
    display: flex;
    flex-direction: column;
  }}

  .card-highlight {{
    border-color: #10b981;
    background: rgba(16, 185, 129, 0.06);
  }}

  .card-gold {{
    border-color: #f59e0b;
    background: rgba(245, 158, 11, 0.06);
  }}

  .card-title {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 22px;
    font-weight: 700;
    color: #38bdf8;
    margin-bottom: 14px;
    display: flex;
    align-items: center;
    gap: 8px;
  }}

  p, li {{
    font-size: 17px;
    line-height: 1.5;
    color: #cbd5e1;
    margin-bottom: 10px;
  }}

  ul {{ list-style-type: none; }}
  ul li {{
    position: relative;
    padding-left: 24px;
    margin-bottom: 10px;
  }}
  ul li::before {{
    content: "✦";
    position: absolute;
    left: 0;
    color: #4ade80;
  }}

  /* Screenshot display */
  .img-frame {{
    border: 3px solid #475569;
    border-radius: 6px;
    overflow: hidden;
    box-shadow: 0 8px 16px rgba(0,0,0,0.5);
    background: #000;
  }}
  .img-frame img {{
    width: 100%;
    height: auto;
    display: block;
  }}
  .img-caption {{
    background: #0f172a;
    color: #38bdf8;
    font-size: 14px;
    padding: 6px 12px;
    text-align: center;
    border-top: 1px solid #334155;
  }}

  .metric-large {{
    font-family: 'Chakra Petch', sans-serif;
    font-size: 42px;
    font-weight: 700;
    color: #facc15;
    margin: 10px 0;
  }}

  .badge {{
    display: inline-block;
    padding: 4px 10px;
    border-radius: 4px;
    font-size: 14px;
    font-weight: bold;
    background: #064e3b;
    color: #4ade80;
    margin-right: 6px;
  }}
</style>
</head>
<body>

  <!-- SLIDE 1: COVER -->
  <div class="slide cover-slide">
    <div>
      <div class="cover-title">LIFE ON LAND</div>
      <div class="cover-subtitle">TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR</div>
      <div style="font-size: 20px; color: #cbd5e1; margin-bottom: 30px;">
        LAPORAN AKHIR PROYEK GIM — MATA KULIAH GAME DEVELOPMENT (CIE 725)
      </div>

      <div class="cover-team">
        <div class="team-card">
          <div class="name">Galih Adhi Kusuma</div>
          <div class="nim">NIM: 20230801198</div>
          <div class="role">Lead Programmer & Backend</div>
        </div>
        <div class="team-card">
          <div class="name">Firschanya Alula R.</div>
          <div class="nim">NIM: 20230801201</div>
          <div class="role">Art Director & Narrative</div>
        </div>
        <div class="team-card">
          <div class="name">Defanda Yeremia C. R.</div>
          <div class="nim">NIM: 20230801205</div>
          <div class="role">System Analyst & QA Tester</div>
        </div>
      </div>

      <div style="font-size: 18px; color: #fbbf24;">
        Dosen Pengampu: <strong>Ir. Sawali Wahyu, S.Kom., M.Kom.</strong><br>
        Program Studi Teknik Informatika — Universitas Esa Unggul (2026)
      </div>
    </div>
  </div>

  <!-- SLIDE 2: BAB I - LATAR BELAKANG -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB I: PENDAHULUAN</div>
        <div class="slide-title">Latar Belakang & Permasalahan Ekologis</div>
      </div>
      <div class="slide-chapter">Slide 02 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card card-highlight">
        <div class="card-title">🌎 Krisis Biosfer Pasca-Apokaliptik</div>
        <p>Bumi masa depan mengalami kehancuran ekosistem masif di mana kadar oksigen ($O_2$) atmosfer turun hingga level kritis <strong>15.0%</strong>.</p>
        <p>Hutan menjadi abu, tanah terkontaminasi zat beracun, dan populasi mahluk hidup terancam punah total.</p>
      </div>

      <div class="card card-gold">
        <div class="card-title">💡 Solusi Gamifikasi Edukatif</div>
        <p>Gim <strong>Life on Land</strong> hadir sebagai media simulasi taktis interaktif yang mengombinasikan keseruan bercocok tanam (*cozy simulation*) dengan kesadaran pelestarian lingkungan.</p>
        <p>Pemain diajak secara langsung memahami dampak fotosintesis, kelembapan tanah, dan retensi air dalam memulihkan biosfer.</p>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 3: BAB I - KONSEP GAME & GAMIFIKASI -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB I: PENDAHULUAN</div>
        <div class="slide-title">Konsep Game & Alur Skenario Gamifikasi</div>
      </div>
      <div class="slide-chapter">Slide 03 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card">
        <div class="card-title">Peran Pemain & Antagonis</div>
        <ul>
          <li><strong>Protagonis (Umbra):</strong> Restorer terakhir yang sabar dan disiplin, memulihkan biosfer ubin-demi-ubin.</li>
          <li><strong>Antagonis (Blaze):** Musuh utama yang aktif membakar vegetasi dan melarikan diri melintasi 3 wilayah.</li>
          <li><strong>3 Bioma Stage:** Red Region (Oasis), Orange Region (Grove), dan Pink Bloom (Boss Stage).</li>
        </ul>
      </div>

      <div class="card card-highlight">
        <div class="card-title">Model Gamifikasi (C-A-R-E)</div>
        <ul>
          <li><span class="badge">Challenge</span> Tanah terbakar & O2 kritis 15%.</li>
          <li><span class="badge">Action</span> Pembersihan 2-tahap (Sekop & Water) & penanaman pohon.</li>
          <li><span class="badge">Reward</span> XP, benih baru, dan blueprints infrastruktur.</li>
          <li><span class="badge">Environmental Shift</span> Visual dunia berubah dari sepia gersang menjadi hijau asri.</li>
        </ul>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 4: BAB I - METODE PENGEMBANGAN (GDLC) -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB I: PENDAHULUAN</div>
        <div class="slide-title">Metode Pengembangan: Game Development Life Cycle</div>
      </div>
      <div class="slide-chapter">Slide 04 / 14</div>
    </div>

    <div class="grid-3col">
      <div class="card">
        <div class="card-title">1. Initiation & Pre-Prod</div>
        <p>Ideasi konsep game, penyusunan GDD, perancangan spesifikasi top-down 2D, threshold O2, serta desain 3 bioma.</p>
      </div>

      <div class="card card-highlight">
        <div class="card-title">2. Production & Iteration</div>
        <p>Pemrograman Unity C# untuk Grid World Matrix, FSM tanaman, hotbar 1-6, kalkulasi O2, pipa irigasi, & heatwaves.</p>
      </div>

      <div class="card card-gold">
        <div class="card-title">3. Testing & Post-Prod</div>
        <p>Playtesting Alpha (SUS 21 responden) & Beta (UAT), bug fixing, optimasi skrip, serta penyusunan laporan 5 Bab.</p>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 5: BAB II - LANDASAN TEORI KHUSUS -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB II: LANDASAN TEORI</div>
        <div class="slide-title">Teori Khusus: FSM & Grid World Matrix</div>
      </div>
      <div class="slide-chapter">Slide 05 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card card-highlight">
        <div class="card-title">Finite State Machine (FSM) Vegetasi</div>
        <p>Mengatur siklus hidup biologis tanaman secara terstruktur melalui status berhingga:</p>
        <p style="font-family:'Chakra Petch'; color:#facc15; font-size:20px; text-align:center; margin:16px 0;">
          Seed ➔ Sprout ➔ Sapling ➔ Young ➔ Mature Tree ➔ Withered
        </p>
        <p>Jika kelembapan tanah = 0, tanaman mengalami dehidrasi dan memasuki status <i>Withered</i> (Layu). Penyiraman air memulihkan tanaman kembali.</p>
      </div>

      <div class="card card-gold">
        <div class="card-title">Grid World Matrix 2D</div>
        <p>Representasi spasial matriks dua dimensi ($x, y$) yang menyimpan variabel independen pada setiap sel:</p>
        <ul>
          <li><strong>Moisture:</strong> Kelembapan tanah (0.0 s.d 1.0).</li>
          <li><strong>Corruption State:</strong> Normal (0), Burnt (1), Dug Burnt (2).</li>
          <li><strong>Local O2:</strong> Konsentrasi oksigen lokal yang terdifusi antar-sel bertetangga.</li>
        </ul>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 6: BAB II - TEORI UMUM & TECH STACK -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB II: LANDASAN TEORI</div>
        <div class="slide-title">Teori Umum & Platform Teknologi</div>
      </div>
      <div class="slide-chapter">Slide 06 / 14</div>
    </div>

    <div class="grid-3col">
      <div class="card">
        <div class="card-title">Unity 6 & C# Engine</div>
        <p>Unity Engine berbasis *Component-Based Architecture* dengan C# (.NET Core) untuk logika pergerakan fisika 2D & pemrosesan input responsif.</p>
      </div>

      <div class="card card-highlight">
        <div class="card-title">Microsoft PlayFab SDK</div>
        <p>Backend cloud service untuk autentikasi login pemain, *Cloud Title Data Save* (`SaveData.json`), serta *Global Real-Time Leaderboard*.</p>
      </div>

      <div class="card card-gold">
        <div class="card-title">PlantUML & Aseprite</div>
        <p>PlantUML untuk pemodelan 5 diagram UML terstruktur, serta Aseprite untuk aset grafis pixel art 32 PPU retro.</p>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 7: BAB III - ASET & KARAKTER SHOWCASE -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="slide-title">Karakter Utama & Guardian Bioma</div>
      </div>
      <div class="slide-chapter">Slide 07 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card">
        <div class="card-title">Daftar Karakter Utama</div>
        <ul>
          <li><strong>Umbra (Restorer):</strong> Baju ungu gelap, rambut pendek, telinga meruncing.</li>
          <li><strong>Blaze (Antagonis):</strong> Tunik biru dingin, pembakar lahan.</li>
          <li><strong>Maliz (Stage 1):</strong> Beruang Wrath Barbarian peminta air.</li>
          <li><strong>Oryel (Stage 2):** Rubah Pride Rogue mandiri.</li>
          <li><strong>Pyper (Stage 3):** Ngengat Lust Bard pemikat.</li>
        </ul>
      </div>

      <div class="img-frame">
        <img src="{img_menu}" alt="Main Menu">
        <div class="img-caption">Tampilan Prototype Main Menu & Sistem Autentikasi PlayFab</div>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 8: BAB III - DEMO GAMEPLAY PURIFIKASI -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="slide-title">Mekanik Pemurnian Lahan 2-Tahap</div>
      </div>
      <div class="slide-chapter">Slide 08 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="img-frame">
        <img src="{img_maliz}" alt="Dialog Maliz & Purifikasi">
        <div class="img-caption">Stage 1: Dialog Quest Air Maliz & Proses Purifikasi Lahan Terbakar</div>
      </div>

      <div class="card card-highlight">
        <div class="card-title">Langkah Pemurnian Tanah</div>
        <ul>
          <li><strong>Tahap 1 (Sekop - Tombol 1):</strong> Menggali ubin tanah terbakar (*Burnt Tile*) menjadi ubin tergali (*Dug Burnt Soil*). Memakai 5 Stamina.</li>
          <li><strong>Tahap 2 (Watering Can - Tombol 2):</strong> Menyiram ubin tergali dengan 1 unit air untuk mengubahnya menjadi tanah subur bersih (*Normal Soil*).</li>
          <li><strong>Penanaman Benih (Tombol 3-5):</strong> Menanam bibit pohon pada tanah subur bersih.</li>
        </ul>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 9: BAB III - PERTUMBUHAN POHON & RECOVERY -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB III: ASSET DAN PROTOTYPE</div>
        <div class="slide-title">Pertumbuhan Vegetasi & Pemulihan Oksigen</div>
      </div>
      <div class="slide-chapter">Slide 09 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card card-gold">
        <div class="card-title">Matriks Spesies Tanaman</div>
        <ul>
          <li><strong>Type A (Pine Tree):</strong> High O2 emission, high water requirement.</li>
          <li><strong>Type B (Desert Shrub):</strong> Low water requirement, retensi air ubin tetangga.</li>
          <li><strong>Type C (Silkmoth Fern):** Tahan gelombang panas (*heatwaves*).</li>
        </ul>
        <p style="margin-top:12px;">Pencapaian O2 Stage 1 mencapai <strong>50.0%</strong> untuk memenangkan demo!</p>
      </div>

      <div class="img-frame">
        <img src="{img_trees}" alt="Pohon Dewasa">
        <div class="img-caption">Tampilan Vegetasi Pohon Dewasa & Pemulihan O2 Atmosfer</div>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 10: BAB IV - TESTING ALPHA (SUS) -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="slide-title">Pengujian Usability Alpha — System Usability Scale (SUS)</div>
      </div>
      <div class="slide-chapter">Slide 10 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card card-highlight">
        <div class="card-title">Hasil Evaluasi 21 Responden</div>
        <p>Pengujian Usability Alpha dilakukan oleh <strong>Defanda Yeremia (QA Tester)</strong> menggunakan 10 pertanyaan terstandar SUS skala Likert 1–5.</p>
        <div class="metric-large">SKOR: 63.45</div>
        <ul>
          <li><strong>Acceptability Range:</strong> Marginal High</li>
          <li><strong>Grade Scale:</strong> Grade D</li>
          <li><strong>Adjective Rating:</strong> OK</li>
        </ul>
      </div>

      <div class="img-frame">
        <img src="{img_chart_sus}" alt="Grafik SUS">
        <div class="img-caption">Grafik Distribusi Perhitungan Skor SUS 21 Responden</div>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 11: BAB IV - TESTING BETA (UAT) -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="slide-title">Pengujian Acceptance Beta — User Acceptance Testing (UAT)</div>
      </div>
      <div class="slide-chapter">Slide 11 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="img-frame">
        <img src="{img_chart_uat}" alt="Grafik UAT">
        <div class="img-caption">Grafik Persentase Keberhasilan 5 Aspek UAT</div>
      </div>

      <div class="card card-gold">
        <div class="card-title">Rincian Persentase 5 Aspek UAT</div>
        <ul>
          <li><strong>Fungsionalitas Sistem:</strong> 85.2% (Sangat Layak)</li>
          <li><strong>Desain Visual Art:</strong> 82.1% (Sangat Layak)</li>
          <li><strong>Audio BGM & SFX:</strong> 80.0% (Layak)</li>
          <li><strong>Kenyamanan Usability:</strong> 83.5% (Sangat Layak)</li>
          <li><strong>Kinerja Performa:</strong> 81.0% (Sangat Layak)</li>
        </ul>
        <div class="metric-large" style="color:#4ade80;">RATA-RATA: 82.4%</div>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 12: BAB IV - ARSITEKTUR HYBRID & PLAYFAB -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB IV: HASIL DAN PEMBAHASAN</div>
        <div class="slide-title">Arsitektur Hybrid Two-Tier & Cloud Save</div>
      </div>
      <div class="slide-chapter">Slide 12 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card">
        <div class="card-title">Client-Side Simulation Layer</div>
        <p>Unity C# mengelola input pemain, stamina, buffer O2, pergerakan fisika 2D, serta eksekusi tick 5 detik pada Grid World Matrix secara terisolasi di sisi lokal client.</p>
      </div>

      <div class="card card-highlight">
        <div class="card-title">PlayFab Cloud Services Layer</div>
        <p>Format data terstruktur <code>SaveData.json</code> men-serialize objek state ke dalam PlayFab Title Data untuk cloud synchronization dan leaderboard global real-time.</p>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 13: BAB V - KESIMPULAN & SARAN -->
  <div class="slide">
    <div class="slide-header">
      <div>
        <div class="slide-chapter">BAB V: KESIMPULAN DAN SARAN</div>
        <div class="slide-title">Kesimpulan Penelitian & Saran Pengembangan</div>
      </div>
      <div class="slide-chapter">Slide 13 / 14</div>
    </div>

    <div class="grid-2col">
      <div class="card card-highlight">
        <div class="card-title">📌 Kesimpulan Utama</div>
        <p>1. Gim Life on Land Demo Stage 1 berhasil dibangun 100% menggunakan Unity C# dengan mekanik purifikasi 2-tahap, FSM tanaman, quest air, & recovery O2 50.0%.</p>
        <p>2. Pengujian Usability Alpha (SUS) menghasilkan skor <strong>63.45 (OK)</strong> dan Pengujian Beta (UAT) menghasilkan <strong>82.4% (SANGAT LAYAK)</strong>.</p>
      </div>

      <div class="card card-gold">
        <div class="card-title">💡 Saran Pengembangan</div>
        <p>1. **Stage 2 & 3:** Merealisasikan Stage 2 (Orange Region - Soil Purifier) & Stage 3 (Pink Bloom - Irrigation Pipes, Heatwave, & penangkapan Blaze).</p>
        <p>2. **Platform Mobile:** Pengimbangan kontrol touch-screen joystick untuk Android/iOS.</p>
      </div>
    </div>

    <div class="slide-footer">
      <span>Life on Land — Top-Down Tactical Eco-Restoration Simulator</span>
      <span>Universitas Esa Unggul 2026</span>
    </div>
  </div>

  <!-- SLIDE 14: CLOSING SLIDE -->
  <div class="slide cover-slide">
    <div>
      <div class="cover-title" style="font-size: 56px;">TERIMA KASIH!</div>
      <div class="cover-subtitle" style="font-size: 26px;">ADA PERTANYAAN / DISKUSI?</div>

      <div style="font-size: 20px; color: #4ade80; margin-bottom: 20px;">
        LIFE ON LAND — TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR
      </div>

      <div style="font-size: 18px; color: #cbd5e1;">
        Kelompok Game Development — Program Studi Teknik Informatika<br>
        Fakultas Ilmu Komputer, Universitas Esa Unggul (2026)
      </div>
    </div>
  </div>

</body>
</html>
"""

    with open(html_path, "w", encoding="utf-8") as f:
        f.write(html_content)
    print(f"Generated 14-slide HTML presentation: {html_path}")

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

    print(f"Rendering PDF presentation using {chrome_path}...")
    res = subprocess.run(cmd, capture_output=True, text=True)
    if res.returncode == 0 and os.path.exists(pdf_path):
        print(f"SUCCESS! 14-Slide PDF Presentation generated at:\n{pdf_path}")
    else:
        print(f"PDF Render Error: {res.stderr}")

if __name__ == "__main__":
    generate_pdf_slides()
