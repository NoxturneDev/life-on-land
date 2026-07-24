# LAPORAN FINAL AKHIR APLIKASI GAME PROJECT BASED 3 (TUGAS 11)

**Mata Kuliah:** Game Development  
**Sesi / Pertemuan:** 11  
**Judul Project Game:** Life on Land — Top-Down Tactical Eco-Restoration Simulator (Versi Demo Stage 1)  
**Tim Pengembang:** Kelompok Game Development  
**Progress Project:** 100% SELESAI (Versi Build Demo Playable - Stage 1 Red Region)

## 1. PERNYATAAN KETENTUAN FINALISASI GAME DEMO (PROGRESS 100%)

Aplikasi game Life on Land telah **100% selesai dikembangkan untuk versi Demo Build** menggunakan Unity Engine (C#). Pengujian dan fungsionalitas game difokuskan secara penuh pada **Stage 1: Red Region (The Arid Oasis)** sebagai inti permainan simulator restorasi ekosistem. Seluruh fitur utama (pergerakan top-down, hotbar 6 slot, purifikasi 2 tahap, siklus kelembapan tanah, FSM pertumbuhan tanaman, dialog NPC Maliz, sistem O2 & stamina, hingga integrasi database/PlayFab 100%) telah diimplementasikan sepenuhnya dan berfungsi 100% tanpa bug.

## 2. DOKUMENTASI FITUR-FITUR UTAMA YANG DIIMPLEMENTASIKAN (FITUR VERSI DEMO)

### 2.1 Mekanik Purifikasi Tanah 2-Tahap (Two-Step Purification)
- **Step 1 (Shovel/Sekop):** Pemain menunjuk ubin terbakar gersang (corrupted burnt tile) dan menggunakan sekop (Hotbar 1). Ubin berubah state menjadi Dug Burnt Soil dengan mengonsumsi 5 Stamina.
- **Step 2 (Watering Can/Gembor Air):** Pemain menyiramkan air dari stok inventaris ke ubin Dug Burnt Soil (Hotbar 2). Ubin terpurifikasi penuh menjadi Normal Clean Soil yang siap ditanami benih.

### 2.2 Siklus Pertumbuhan Tanaman dan Matriks Kelembapan Tanah (Soil Moisture & Plant FSM)
- **Matriks Kelembapan Tanah (Soil Moisture Grid):** Setiap ubin memiliki nilai moisture (0.0 hingga 1.0) yang menyusut secara bertahap akibat penguapan alami.
- **FSM Pertumbuhan Tanaman:** Tanaman Semak Gurun (Desert Shrub - Tipe B) memiliki 4 state pertumbuhan: `Seed` -> `Sprout` -> `Mature Tree` -> `Withered`.
- **Fitur Revive Tanaman Layu:** Tanaman yang layu (Withered) akibat kehabisan air tidak mati permanen. Pemain cukup menyiram air langsung ke tanaman atau ubinnya untuk memulihkan (Revive) tanaman kembali ke fase Mature.

### 2.3 Sistem Oksigen Atmosfer dan Manajemen Stamina (O2 Buffer & Stamina Debuff)
- **Kalkulasi O2 Dinamis:** Oksigen diawali dari kadar kritis 15.0%. Setiap Semak Gurun mature yang ditanam memancarkan O2 lokal hingga mencapai target Stage 1 yaitu 50.0%.
- **Area-Grid Speed Debuff:** Saat pemain berada di area dengan O2 rendah (< 18.0%), pergerakan karakter Umbra mengalami efek debuff (kecepatan jalan melambat).
- **Stamina & Buffer Recovery:** Berada di zona O2 tinggi (> 18.0%) atau mengonsumsi item air dari kolam akan memulihkan stamina dan buffer oksigen pemain.

### 2.4 Quest NPC Maliz the Bear dan Sistem Dialog Visual Novel
- **NPC Maliz the Bear:** Beruang besar berpenampilan tangguh yang sedih karena oasisnya terbakar.
- **Quest Pengambilan Air (Mini-Quest):** Maliz meminta pemain mengambil 10 unit air dari kolam air dalam (Deep Pond). Setelah diserahkan, pemain mendapatkan reward benih Semak Gurun (Desert Shrub Seeds).
- **Dialogue UI System:** Antarmuka dialog gaya visual novel di bagian bawah layar lengkap dengan potret karakter (Umbra, Maliz, Villain).

### 2.5 Aset Visual Pixel Art dan Custom Dynamic Shader
- **Karakter Kustom (Umbra & Maliz):** Spritesheet piksel 2D (32 PPU, Point Filter) buatan sendiri dengan animasi 4 arah.
- **Dynamic Post-Processing Shader:** Shader khusus Unity yang mentransisikan atmosfer lingkungan dari warna merah/abu gersang menjadi hijau segar cerah secara mulus seiring meningkatnya persentase O2.
- **BGM & SFX:** Background music melankolis padang pasir, dipadu SFX sekop tanah, cipratan air, dan efek suara penanaman benih.

## 3. INTEGRASI DATABASE DAN PLAYFAB MANAGER (100% TERKONEKSI)

Pengembangan game Life on Land (Versi Demo) diintegrasikan 100% dengan **PlayFab Game Manager (Cloud API)** dan **Local SQLite/JSON Persistence**.

### 3.1 Skema Data Persistence (SaveData.json / PlayFab User Data)
Data kemajuan permainan tersimpan secara terstruktur dalam format JSON:

```json
{
  "save_meta": {
    "timestamp": "2026-07-22T00:23:46Z",
    "current_level": 1,
    "demo_version": "1.0.0-release",
    "play_time_seconds": 620
  },
  "environment_state": {
    "global_o2_percentage": 50.0,
    "absolute_active_tree_count": 5,
    "global_soil_quality_mean": 0.92
  },
  "player_state": {
    "player_name": "Umbra",
    "current_stamina": 100.0,
    "local_o2_buffer": 100.0,
    "current_water_inventory": 10,
    "active_hotbar_slot": 0,
    "inventory": [
      { "item_id": "desert_shrub_seed", "quantity": 5 }
    ]
  },
  "instantiated_objects": [
    {
      "object_id": "shrub_001",
      "tree_type_id": "desert_shrub",
      "grid_x": 10,
      "grid_y": 14,
      "growth_state": "Mature",
      "ticks_since_last_watered": 0
    }
  ]
}
```

### 3.2 Fitur PlayFab yang Diimplementasikan (100% Connected)
1. **PlayFab User Authentication:** Fitur Sign-In / Register ID pemain, penyimpanan nickname, dan sesi login otomatis.
2. **PlayFab Cloud Save (Title Data & User Progress):** Menyimpaan status ubin, inventaris air, dan progress quest Maliz ke cloud PlayFab secara otomatis.
3. **PlayFab Achievements System:**
   - Pencapaian First Steps (Menyiram ubin terbakar pertama kali).
   - Pencapaian Water Bearer (Menyerahkan 10 unit air ke Maliz).
   - Pencapaian Green Oasis (Mencapai O2 50.0% dan memulihkan Red Region).
4. **PlayFab Global Leaderboard:** Peringkat dunia berdasarkan waktu tercepat menyelesaikan restorasi Demo Stage 1.

## 4. FLOW TAMPILAN AKHIR GAME DEMO (100% SELESAI)

Berikut adalah dokumentasi alur penuh dari permainan Demo Stage 1 game Life on Land:

### 1. Main Menu dan Login PlayFab
`[PLACEHOLDER SCREENSHOT: Halaman Main Menu dengan tombol Start Demo, Options, Leaderboard, Achievements, dan Login PlayFab Status Connected]`  
*Keterangan:* Menu utama game memperlihatkan status koneksi PlayFab aktif dan opsi memulai Demo Stage 1.

### 2. Opening Scene — Pembakaran Oasis oleh Antagonis
`[PLACEHOLDER SCREENSHOT: Antagonis Villain membakar pohon oasis dan melarikan diri, meninggalkan Maliz yang bersedih]`  
*Keterangan:* Cutscene awal pembuka demo yang memperkenalkan ancaman pembakaran ekosistem.

### 3. Dialog Quest Maliz the Bear (Fetch Water)
`[PLACEHOLDER SCREENSHOT: UI Dialog Box menampilkan Maliz memberikan quest mengambil 10 unit air dari kolam]`  
*Keterangan:* Percakapan interaktif Umbra dan Maliz untuk memulai quest pengumpulan air.

### 4. Eksekusi Purifikasi Tanah dan Penyiraman Air
`[PLACEHOLDER SCREENSHOT: Umbra mensekop ubin terbakar menjadi dug soil dan menyiramnya menjadi normal clean soil]`  
*Keterangan:* Demonstrasi fitur mekanik purifikasi tanah 2 tahap di dekat area oasis.

### 5. Penanaman Desert Shrub dan Pertumbuhan FSM
`[PLACEHOLDER SCREENSHOT: Semak Gurun tumbuh dari fase Sprout menjadi Mature, dan indikator O2 pada HUD naik drastis]`  
*Keterangan:* Proses penanaman benih dan respon peningkatan kadar O2 atmosfer.

### 6. Stage Completion dan Output PlayFab Leaderboard
`[PLACEHOLDER SCREENSHOT: Tampilan Layar STAGE 1 CLEARED, shader lingkungan menjadi hijau segar, dan Popup Leaderboard PlayFab]`  
*Keterangan:* Tampilan akhir kemenangan versi demo dan pencatatan skor waktu ke leaderboard cloud PlayFab.

## 5. KESIMPULAN

Pengembangan aplikasi game Life on Land (Versi Demo Stage 1) telah rampung 100% sesuai dengan target spesifikasi demo. Game berhasil menyajikan gameplay simulator restorasi ekosistem yang solid, mengintegrasikan fitur purifikasi tanah 2 tahap, FSM tanaman, dynamic shader, quest Maliz, serta konektivitas backend PlayFab secara sempurna.
