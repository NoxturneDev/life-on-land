# DOKUMENTASI TEKNIS APLIKASI GAME "LIFE ON LAND"
## (TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR)

**Program Studi Teknik Informatika, Fakultas Ilmu Komputer**  
**Universitas Esa Unggul — 2026**

---

## BAB I: PENDAHULUAN DAN ARSITEKTUR SISTEM

### 1.1 Identitas dan Ringkasan Aplikasi
* **Nama Game:** Life on Land
* **Genre:** Top-Down Cozy Stage-Based Forest Ecosystem Simulator
* **Platform:** PC Standalone (Windows 10/11 x86_64, macOS, Linux) & WebGL
* **Engine & Language:** Unity 6 (6000.0.x) / Unity 2022.3 LTS & C# (.NET Core)
* **Backend & Persistence:** Local JSON (`SaveData.json`) / SQLite & PlayFab Cloud Title Data Services

### 1.2 Topologi Arsitektur (Two-Tier Hybrid Architecture)
Game *Life on Land* menerapkan arsitektur dua tingkat (Two-Tier Hybrid Architecture) yang memisahkan logika komputasi klien (*Client-Side Monolithic Simulation Engine*) dengan layanan penyimpanan awan (*Cloud Persistence & Global Leaderboard Services*):

1. **Client-Side Simulation Layer (Unity C#):**
   * **Input & Controller Module (`PlayerController.cs`):** Mengelola masukan pergerakan WASD/Arrow, penggunaan stamina, buffer oksigen lokal, serta pemilihan slot Hotbar (1–6).
   * **Environmental Matrix Module (`EnvironmentManager.cs` & `GridWorldMatrix.cs`):** Menjalankan simulasi spasial 2D untuk kelembapan tanah (*soil moisture*), tingkat kontaminasi lahan (*corruption state*), dan difusi oksigen atmosfer ($O_2$) berbasis interval waktu (*tick* 5 detik).
   * **Plant Lifecycle FSM Module (`Tree.cs`):** Mengontrol siklus hidup biologis tanaman dari *Seed*, *Sprout*, *Young Tree*, *Mature Tree*, hingga *Withered* menggunakan Finite State Machine (FSM).
2. **Backend Services Layer (Microsoft PlayFab SDK):**
   * **PlayFab Authentication & Title Data:** Mengautentikasi identitas unik pemain (Custom ID / Login Anonymous) serta men-serialize status progres permainan ke dalam PlayFab Title Data.
   * **Global Leaderboard Service:** Menjadwalkan dan mengagregasi skor pemulihan oksigen global (`globalO2Percentage`) serta waktu penyelesaian tahap (*stage completion time*) ke dalam papan peringkat global secara *real-time*.

---

## BAB II: SETUP LINGKUNGAN PENGEMBANGAN DAN KOMPILASI BUILD

### 2.1 Persyaratan Perangkat Lunak (Prerequisites)
Sebelum membuka dan meng-compile source code proyek gim, lingkungan pengembangan berikut harus disiapkan:

1. **Unity Hub & Engine:** Unity 6 (6000.0.x) atau Unity 2022.3 LTS dengan modul:
   * Microsoft Visual Studio / JetBrains Rider Integration
   * Windows Build Support (IL2CPP / Mono)
   * WebGL Build Support
2. **Git Version Control System:** Terinstal pada OS untuk manajemen repositori.
3. **PlayFab SDK Package:** SDK PlayFab Unity v2.x terintegrasi di folder `Assets/PlayFabSDK`.

### 2.2 Panduan Setup dan Pengoperasian Project
1. **Clone Repositori:**
   ```bash
   git clone https://github.com/username/life-on-land.git
   cd "My project"
   ```
2. **Buka di Unity Editor:**
   * Jalankan Unity Hub -> pilih **Add project from disk**.
   * Arahkan ke folder root `My project`.
   * Pilih Editor Version Unity 6 / 2022.3 LTS dan tunggu proses *asset importing* selesai.
3. **Konfigurasi PlayFab Manager:**
   * Buka menu `Window` -> `PlayFab` -> `EdEx` (Editor Extensions).
   * Masukkan **Title ID** PlayFab (misal: `1A2B3`).
   * Pastikan aset `PlayFabSharedSettings` di `Assets/PlayFabSDK/Shared/Public/Resources/` telah terhubung dengan Title ID yang valid.

### 2.3 Prosedur Kompilasi (Build Pipeline)
1. **Pilih Scene Permainan:**
   * Buka window **File** -> **Build Settings**.
   * Tambahkan scene ke dalam daftar *Scenes In Build* sesuai urutan:
     1. `Assets/Scenes/MainMenu.unity` (Index 0)
     2. `Assets/Scenes/Stage1_RedRegion.unity` (Index 1)
     3. `Assets/Scenes/Stage2_OrangeRegion.unity` (Index 2)
     4. `Assets/Scenes/Stage3_PinkBloom.unity` (Index 3)
2. **Kompilasi Standalone Windows (PC):**
   * Target Platform: **Windows**, Architecture: **x86_64**.
   * Klik tombol **Build** -> Buat folder tujuan `Builds/Windows/` -> Eksekusi `LifeOnLand.exe`.
3. **Kompilasi WebGL (Browser):**
   * Target Platform: **WebGL**.
   * Color Space: **Gamma** (atau Linear jika URP WebGL diaktifkan).
   * Klik tombol **Build** -> Pilih folder tujuan `Builds/WebGL/`.

---

## BAB III: SPESIFIKASI SKRIP LOGIKA UTAMA (C# CLASS CONTRACTS)

### 3.1 Skrip `Player.cs` / `PlayerController.cs`
Mengontrol pergerakan fisik 2D, pemakaian stamina, buffer udara, dan interaksi perkakas hotbar.

```csharp
public class Player : MonoBehaviour {
    [SerializeField] private string playerName = "Restorer";
    [SerializeField] private float baseMovementSpeed = 5.0f;
    [SerializeField] private float currentStamina = 100.0f;
    [SerializeField] private float localO2Buffer = 10.0f;
    [SerializeField] private int currentWaterInventory = 0;
    [SerializeField] private int activeHotbarSlot = 0; // Slot 0 s.d 5 (Tombol 1-6)

    public void ProcessMovementInput(float horizontal, float vertical);
    public void SelectHotbarSlot(int slotIndex);
    public void UseActiveTool(Vector2 targetGridCoordinates);
    public void ExecutePlantAction(TreeProfile profile, Vector2 targetGridCoordinates);
    public void ExtractWater(WaterSourceNode source);
    public void ConstructInfrastructure(BuildingBlueprint blueprint, Vector2 originCoordinates);
    private void EvaluateCalculatedDebuffs();
}
```

### 3.2 Skrip `EnvironmentManager.cs`
Mengelola siklus detak lingkungan (*environment tick*), difusi oksigen spasial, dan penguapan kelembapan tanah.

```csharp
public class EnvironmentManager : MonoBehaviour {
    [SerializeField] private float globalO2Percentage = 15.0f;
    [SerializeField] private float tickInterval = 5.0f;
    [SerializeField] private bool isHeatwaveActive = false;
    
    private GridWorldMatrix environmentGrid;

    public void RecalculateAtmosphericComposition();
    public void ExecuteStateTick();
    public void DiffuseOxygen();
    public void DeployLocalizedDisasterEvent(int levelMilestone);
    public bool EvaluateVictoryState();
}
```

### 3.3 Skrip `GridWorldMatrix.cs` dan `GridCell.cs`
Mewakili dunia 2D berbasis sel matriks terstruktur untuk variabel tanah dan kontaminasi.

```csharp
public class GridCell {
    public float moisture;          // 0.0f (kering) s.d 1.0f (basah)
    public int corruptionState;     // 0 = Normal, 1 = Burnt, 2 = Dug Burnt
    public float localO2;           // Kadar O2 pada sel ini
    public Tree plantedTree;        // Referensi komponen tanaman
}

public class GridWorldMatrix : MonoBehaviour {
    public int TilesPurifiedCount { get; private set; }
    
    public bool PurifyTileShovel(Vector2Int gridPosition);
    public bool PurifyTileWater(Vector2Int gridPosition);
    public GridCell GetCell(Vector2Int gridPosition);
}
```

### 3.4 Skrip `Tree.cs` dan `GrowthState.cs`
Mengontrol mesin status berhingga (*Finite State Machine*) pertumbuhan vegetasi.

```csharp
public enum GrowthState { Seed, Sprout, Sapling, Young, MatureTree, Withered }

public class Tree : WorldObject {
    [SerializeField] private GrowthState currentFSMState = GrowthState.Seed;
    [SerializeField] private float localO2EmissionRate = 0.5f;
    [SerializeField] private int ticksSinceLastWatered = 0;
    [SerializeField] private int thresholdWaterRequirement = 3;

    public void ProgressGrowthCycle();
    public void InjectAtmosphericO2();
    public void TransitionToWitheredState();
    public void Revive();
}
```

---

## BAB IV: SKEMA DATABASE DAN PERSISTENSI DATA

### 4.1 Skema Kontrak Data JSON (`SaveData.json`)
Seluruh state permainan yang perlu dipertahankan disimpan dalam format terstruktur berbasis JSON:

| Field Key | Tipe Data | Deskripsi Fitur |
|---|---|---|
| `playerId` | string | Identifier unik pemain (PlayFab Login GUID / Local ID) |
| `playerName` | string | Nama akun pemain (default: "Restorer") |
| `currentStamina` | float | Sisa nilai stamina pemain (0.0 – 100.0) |
| `localO2Buffer` | float | Buffer cadangan oksigen lokal (0.0 – 10.0) |
| `currentLevel` | int | Tahap stage aktif (1 = Red, 2 = Orange, 3 = Pink Bloom) |
| `inventory` | Array\<Object\> | Daftar item inventoris `{ itemID, quantity }` |
| `gridCells` | Array\<Object\> | Array koordinat sel dimodifikasi `{ x, y, moisture, corruptionState, growthState }` |
| `achievements` | Object | Flag pencapaian `{ firstSteps: bool, waterBearer: bool, greenOasis: bool }` |
| `globalO2Percentage` | float | Nilai persentase oksigen global bumi terakhir tersimpan |
| `lastSavedAt` | string (ISO 8601) | Stempel waktu penyimpanan untuk validasi konflik cloud save |

---

## BAB V: PIPELINE ASSET DAN SISTEM DIALOG NOVEL VISUAL

### 5.1 Standar Pengolahan Grafik Piksel (Pixel Art Standards)
* **Pixels Per Unit (PPU):** Seluruh aset sprite dan tileset diset tepat ke **32 PPU**.
* **Filter Mode:** Wajib menggunakan **Point (no filter)** agar piksel tidak buram saat di-scale.
* **Compression:** **Uncompressed** (RGBA 32-bit).
* **Pivot Location:** Menggunakan **BottomCenter** pada prefab objek interaktif agar Y-sorting depth rendering bekerja sempurna terhadap posisi pemain.

### 5.2 Sistem Dialog Novel Visual (`DialogueManager.cs`)
* Memiliki panel UI overlay di bagian bawah layar dengan dukungan potret ekspresi karakter (Umbra, Maliz, Oryel, Pyper, Blaze).
* Berjalan otomatis saat pemain mendekati NPC atau memicu milestone tahap (*Stage Completion Gate*).
