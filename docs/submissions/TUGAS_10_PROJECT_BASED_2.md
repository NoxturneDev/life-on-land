# LAPORAN PROGRESS PROJECT BASED 2 (TUGAS 10)

**Mata Kuliah:** Game Development  
**Sesi / Pertemuan:** 10  
**Judul Project Game:** Life on Land — Top-Down Tactical Eco-Restoration Simulator (Build Demo)  
**Tim Pengembang:** Kelompok Game Development  
**Progress Project:** 75% (Tahap Prototype Lanjutan & Integrasi PlayFab)

## 1. IDENTITAS PROJECT DAN ABSTRAK

- **Nama Game:** Life on Land
- **Genre:** Top-Down Cozy Stage-Based Forest Ecosystem Simulator
- **Platform:** PC / Windows / macOS / Web (Unity Engine - C#)
- **Perspektif:** Fixed 2D Orthographic Top-Down (32 PPU, Pixel Art)
- **Ringkasan Progress (75%):** Pada tahap Project Based 2 ini, game Life on Land telah berhasil dikembangkan hingga tahap prototype lanjutan (75% selesai) dengan fokus pada **Stage 1: Red Region (The Arid Oasis)**. Sistem mekanik utama seperti pergerakan karakter top-down WASD/Panah, sistem 6 slot hotbar, purifikasi tanah 2 tahap (sekop lalu siram), matriks kelembapan tanah (soil moisture decay), serta siklus pertumbuhan tanaman (Seed -> Sprout -> Mature -> Withered -> Revive) telah 100% berfungsi di Unity. Integrasi PlayFab Game Manager dan serialisasi data lokal (SaveData.json) telah mencapai 50% untuk autentikasi user dan sinkronisasi progress stage.

## 2. KONSEP GAME DAN METODE GAMIFIKASI (STAGE 1 DEMO)

### 2.1 Model Pembelajaran dan Gamifikasi
Game Life on Land mengusung model pembelajaran ekologi dan restorasi lingkungan (Eco-Restoration) berbasis gamifikasi Quest-Driven Ecosystem Loop. Pemain diajarkan dampak degradasi tanah, pentingnya retensi air, serta keseimbangan kadar oksigen (O2) atmosfer melalui mekanisme permainan yang interaktif.

Tingkatan Gamifikasi (Stage 1 Red Oasis Demo):

1. **Stage 1 — Red Region (The Arid Oasis):**
   - **Aesthetic:** Tanah liat merah, pasir kering, tunggul merah mati.
   - **Tujuan Ekologi:** Mengembalikan tingkat Oksigen lokal dari 15.0% menjadi 50.0%.
   - **NPC:** Maliz (Bear Wrath Barbarian) — Karakter berotot tangguh yang sedih karena oasis terbakarnya.
   - **NPC Mini-Quest:** Mengambil 10 unit air dari kolam dalam (Deep Pond) menggunakan ember.
   - **Reward:** Desert Shrub Seeds (Tanaman Tipe B — konsumsi air rendah, meningkatkan retensi kelembapan tanah di sekitarnya).
   - **Main Quest:** Membersihkan 5 ubin terbakar (corrupted tiles), menanam dan menumbuhkan 5 Semak Gurun (Desert Shrubs) hingga dewasa, mencapai O2 50.0%.

2. **Rencana Ekspansi Stage (Peta Jalan Masa Depan):**
   - **Stage 2 (Scorched Grove):** Rencana implementasi Pohon Pinus dan Soil Purifier.
   - **Stage 3 (Pink Bloom):** Rencana implementasi Pipa Irigasi, Heatwave disaster, dan Biosphere Dome.

## 3. ASET GAME DAN SPESIFIKASI MULTIMEDIA (PROGRESS 70-80%)

### 3.1 Karakter dan Aset Visual
- **Protagonis (Umbra):** Karakter utama (Sloth Monk / Restorer) dibuat sendiri 100% menggunakan sprite sheet 2D pixel art dengan animasi 4 arah (Idle, Walk, Dig, Water, Plant).
- **NPC dan Antagonis:** Sprite Maliz (Bear) dan Villain dirancang khusus dengan gaya pixel art 32 PPU.
- **Aset Lingkungan (Tileset):** Kombinasi aset buatan sendiri (ubin tanah terbakar, ubin dug soil, ubin normal) dan aset Unity Asset Store (<50%) untuk variasi pohon mati dan vegetasi dekoratif.
- **Visual Shader Transition:** Menggunakan custom Unity Sprite Shader untuk mentransisikan warna lingkungan dari nuansa kecokelatan/terbakar menjadi hijau cerah secara dinamis seiring peningkatan O2 dari 15% ke 50%.

### 3.2 Background Musik dan Effect (BGM & SFX)
- **BGM Stage 1:** Musik melankolis padang pasir dengan sentuhan alat musik petik.
- **SFX:** Efek suara langkah kaki pada tanah kering, suara cangkul menyentuh tanah terbakar, cipratan air disiramkan, dan efek suara penanaman benih.

## 4. ARSITEKTUR KODE DAN KONEKSI DATABASE / PLAYFAB (PROGRESS 50%)

### 4.1 Teknologi dan Bahasa Pemrograman
- **Engine:** Unity 6 (2D Render Pipeline)
- **Bahasa Pemrograman:** C# (.NET Standard 2.1)
- **Format Persistence:** Local JSON Serialization (SaveData.json) dan SQLite Database backend.

### 4.2 Integrasi PlayFab Game Manager (Progress 50%)
Untuk memenuhi ketentuan pengkoneksian database/PlayFab:
1. **PlayFab User Authentication:** Fitur Sign-In / Register menggunakan PlayFab Auth (Custom ID & Email Login) telah aktif.
2. **Cloud Save dan Player Title Data:** Data status progress pemain (Stage aktif, jumlah O2 terkumpul, status inventaris hotbar) disinkronkan dari struct SaveData ke PlayFab User Data API.
3. **Leaderboard Structure:** Menyiapkan struktur leaderboard nilai restorasi O2 global.

```csharp
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabAuthManager : MonoBehaviour {
    public void LoginCustomID(string customId) {
        var request = new LoginWithCustomIDRequest {
            CustomId = customId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    private void OnSuccess(LoginResult result) {
        Debug.Log("PlayFab Login Berhasil! ID: " + result.PlayFabId);
    }
    private void OnError(PlayFabError error) {
        Debug.LogError("PlayFab Login Gagal: " + error.GenerateErrorReport());
    }
}
```

## 5. TAMPILAN INTERFACE DAN SCREENSHOT PROTOTYPE (PROGRESS 75%)

Berikut adalah dokumentasi tampilan antarmuka dan visualisasi fitur game Life on Land yang telah diimplementasikan di Unity:

### 1. Tampilan Main Menu dan PlayFab Auth
`[PLACEHOLDER SCREENSHOT: Main Menu Game Life on Land dengan tombol Start, Options, PlayFab Login Panel, dan Logo Game]`  
*Keterangan:* Tampilan menu utama game dengan latar belakang animasi hutan yang perlahan pulih dan form login PlayFab.

### 2. Gameplay Stage 1 — Hotbar dan Purifikasi Tanah
`[PLACEHOLDER SCREENSHOT: Gameplay Stage 1 Red Region memperlihatkan Umbra dengan 6 slot Hotbar, ubin burnt, ubin dug, dan status O2 HUD]`  
*Keterangan:* Antarmuka permainan top-down memperlihatkan slot hotbar 1-6, bar stamina, indikator O2 (15.0%), dan proses purifikasi tanah menggunakan sekop dan gembor air.

### 3. Tampilan Dialog NPC (Maliz the Bear)
`[PLACEHOLDER SCREENSHOT: UI Dialog Box bawah layar menampilkan portrait Maliz Bear dan percakapan quest air]`  
*Keterangan:* Sistem dialog visual novel di bagian bawah layar saat Maliz memberikan quest mengambil 10 unit air dari kolam.

### 4. Tampilan Pertumbuhan Tanaman dan Perubahan Visual Environment
`[PLACEHOLDER SCREENSHOT: Vegetasi Desert Shrub yang tumbuh dari Sprout ke Mature, serta perubahan warna tanah di sekitarnya]`  
*Keterangan:* Perubahan state FSM tanaman Semak Gurun dan efek perbaikan lingkungan visual dari warna tanah liat merah menjadi area yang segar.

## 6. RENCANA PENYELESAIAN MENUJU 100% DEMO BUILD (NEXT STEPS)

1. Menyelesaikan polis visual efek perubahan O2 di Stage 1 Red Region.
2. Finalisasi quest pengambilan air Maliz the Bear dan penyerahan benih Desert Shrub.
3. Penyempurnaan sinkronisasi data PlayFab Cloud Save untuk pencapaian Achievements dan Leaderboard Global.
4. Polis grafis visual shader dan efek audio responsif.
