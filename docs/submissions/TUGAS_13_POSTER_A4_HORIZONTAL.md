# POSTER PROYEK AKHIR GAME DEVELOPMENT (UKURAN A4 HORIZONTAL / LANDSCAPE)
# LIFE ON LAND: TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR

**Mata Kuliah: Game Development (CIE 725)**  
**Fakultas Ilmu Komputer, Program Studi Teknik Informatika — Universitas Esa Unggul (2026)**  
**Dosen Pengampu: Ir. Sawali Wahyu, S.Kom., M.Kom.**

---

## 👥 IDENTITAS TIM KELOMPOK & DOSEN PENGAMPU

### Dosen Pengampu Mata Kuliah:
* **Ir. Sawali Wahyu, S.Kom., M.Kom.** (Dosen Pengampu Game Development)

### Anggota Tim Kelompok:
1. **Galih Adhi Kusuma** (NIM: `20230801198`) — *Lead Programmer & Backend Engineer*
2. **Firschanya Alula Rietmadhanty** (NIM: `20230801201`) — *Art Director & Narrative Designer*
3. **Defanda Yeremia Christian Rompas** (NIM: `20230801205`) — *System Analyst & QA Tester*

---

## 🌱 OVERVIEW NARASI, ABSTRAK, DAN KONSEP GAME

Bumi masa depan mengalami kerusakan biosfer masif dengan kadar oksigen ($O_2$) atmosferik tersisa $15.0\%$. Pemain berperan sebagai **Umbra** (Restorer terakhir) yang berkelana memulihkan vegetasi dan kelembapan tanah gersang tile-demi-tile sambil mengejar antagonis **Blaze** yang aktif membakar hutan. Permainan menggabungkan ketenangan bercocok tanam (*cozy simulation*) dengan ketegangan batas oksigen dan survival pasca-apokaliptik.

---

## 🎯 TUJUAN, FUNGSI, DAN MANFAAT APLIKASI

* **Tujuan Utama:** Memulihkan kadar $O_2$ dari $15.0\%$ ke zona layak huni $\ge 21.0\%$ (mencapai $50.0\%$ di Stage 1 Demo), memurnikan tanah terpolusi 2-tahap, serta menanam vegetasi pendukung ekosistem.
* **Manfaat Bagi User / Pemain:** Media hiburan edukatif yang melatih kesadaran lingkungan, pemahaman siklus air tanah, dan perencanaan taktis penanaman vegetasi.
* **Manfaat Bagi Masyarakat & Instansi:** Model gamifikasi interaktif untuk kampanye pelestarian alam dan simulasi edukasi ekologi pasca-bencana.
* **Manfaat Bagi Reviewer / Penguji:** Bukti terintegrasi implementasi arsitektur rekayasa perangkat lunak gim (Finite State Machine, Grid World Matrix, PlayFab Cloud Save, serta evaluasi terstandar SUS/UAT).

---

## 🛠️ METODE GAME, RANCANGAN GAME, GENRE, DAN ENGINE

* **Game Genre:** *Top-Down Tactical Eco-Restoration Simulator*
* **Game Engine & Pemrograman:** Unity 6 (6000.0.x) / 2022.3 LTS & C# (.NET Core)
* **Metode Pengembangan:** *Game Development Life Cycle* (GDLC - 6 Tahapan Iteratif: *Initiation, Pre-Production, Production, Testing, Beta Release, Post-Production*)
* **Rancangan Sistem Inti:**
  1. **Finite State Machine (FSM):** Mengontrol siklus hidup vegetasi dari `Seed` $\rightarrow$ `Sprout` $\rightarrow$ `Sapling` $\rightarrow$ `Young` $\rightarrow$ `MatureTree` $\rightarrow$ `Withered`.
  2. **Grid World Matrix:** Struktur spasial 2D untuk mengukur kelembapan tanah (*soil moisture* 0.0–1.0), status tanah terbakar/tergali (*corruptionState*), dan difusi oksigen lokal.
  3. **Gamification Model:** Kerangka *Challenge* (Tanah Terbakar) $\rightarrow$ *Action* (Shovel & Water) $\rightarrow$ *Reward* (XP & Benih) $\rightarrow$ *Environmental Shift* (Visual Dunia Hijau).

---

## 📊 HASIL TESTING APLIKASI (ALPHA & BETA TESTING)

* **Pengujian Usability Alpha (System Usability Scale / SUS):**  
  Dilakukan terhadap 21 responden oleh QA Tester (Defanda Yeremia). Menghasilkan **Rata-rata Skor SUS: 63.45** (*Acceptability Range:* **Marginal High**, *Grade Scale:* **Grade D**, *Adjective Rating:* **OK**).
* **Pengujian Acceptance Beta (User Acceptance Testing / UAT):**  
  Pengujian terhadap 5 aspek utama (Fungsionalitas, Visual Art, Audio BGM/SFX, Usability, Performa). Menghasilkan **Persentase Keberhasilan Rata-Rata: 82.4%** (Kategori **SANGAT LAYAK**).

---

## 🖼️ TAMPILAN OUTPUT GAME (SCREENSHOTS HASHIL IMPLEMENASI)

1. **Tampilan Main Menu & Autentikasi PlayFab Cloud:**  
   `Assets/Screenshots/main menu.png`
2. **Tampilan Dialogue Panel Visual Novel & Hotbar 1-6:**  
   `Assets/Screenshots/maliz_dialogs.png` / `Assets/Screenshots/quest_dialog.png`
3. **Tampilan Purifikasi Lahan 2-Tahap & Vegetasi Pohon Dewasa:**  
   `Assets/Screenshots/grown_trees.png` / `Assets/Screenshots/restoration_complete.png`
4. **Grafik Hasil Pengujian SUS & UAT Google Form:**  
   `docs/submissions/charts/gform_chart_5_sus_scores.png`

---

## 📌 KESIMPULAN DAN SARAN

* **Kesimpulan:** Game *Life on Land* Demo Stage 1 berhasil dibangun 100% menggunakan Unity C# dengan mekanik purifikasi 2-tahap, FSM daur hidup vegetasi, quest air Maliz, dan pemulihan oksigen 50.0%. Hasil pengujian UAT 82.4% membuktikan game sangat layak untuk dimainkan.
* **Saran:** 
  1. Pengembangan Stage 2 (*Orange Region* - *Soil Purifier*) & Stage 3 (*Pink Bloom* - *Irrigation Pipes*, *Heatwave*, dan penangkapan Blaze).
  2. Pengimbangan kontrol *touch-screen joystick* untuk adaptasi ke platform mobile (Android/iOS).
