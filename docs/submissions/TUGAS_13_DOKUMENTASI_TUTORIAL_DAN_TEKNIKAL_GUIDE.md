# DOKUMENTASI TUTORIAL PENGGUNAAN, INSTALASI, DAN TEKNIKAL CODING APLIKASI
# APLIKASI GAME "LIFE ON LAND" (TOP-DOWN TACTICAL ECO-RESTORATION SIMULATOR)

**Disusun Untuk Memenuhi Laporan Proyek Akhir Mata Kuliah Game Development**  
**Program Studi Teknik Informatika, Fakultas Ilmu Komputer**  
**Universitas Esa Unggul — 2026**

# BAB I: PANDUAN INSTALASI DAN SETUP LINGKUNGAN

Bab ini menjelaskan langkah-langkah teknis untuk melakukan instalasi dan konfigurasi lingkungan pengembangan perangkat lunak (development environment) pada laptop/PC untuk menjalankan dan meng-compile game Life on Land.

### 1.1 Persyaratan Sistem (Prerequisites)
Sebelum memulai proses setup dan kompilasi proyek, pastikan laptop/PC Anda telah terpasang beberapa perangkat lunak dasar berikut:

1. **Unity Hub dan Unity Engine:** Versi Unity 6 (6000.0.x) atau 2022.3 LTS dengan modul pendukung Microsoft Visual Studio / Rider Community dan Build Support Standalone (Windows/Mac) serta WebGL Build Support.
2. **Git Version Control:** Terinstal untuk mengunduh repositori proyek.
3. **PlayFab Editor Extensions / PlayFab SDK:** Terpasang di dalam proyek Unity.
4. **Spesifikasi Laptop Minimum:** Intel Core i3 / AMD Ryzen 3, RAM 8 GB, GPU Intel UHD Graphics / NVIDIA GTX 750, OS Windows 10/11 64-bit.

### 1.2 Panduan Langkah-Langkah Instalasi dan Kompilasi

#### Langkah 1: Mengunduh Source Code Proyek
Buka terminal atau Command Prompt pada laptop Anda, lalu jalankan perintah berikut:

```bash
git clone https://github.com/username/life-on-land-game.git
cd life-on-land-game
```

#### Langkah 2: Membuka Proyek di Unity Engine
1. Jalankan **Unity Hub**.
2. Klik tombol **Add** -> **Add project from disk**.
3. Pilih folder direktori root proyek My project.
4. Pilih versi editor **Unity 6** atau **Unity 2022.3 LTS**, lalu klik untuk membuka proyek.
5. Tunggu proses importing assets dan package resolution selesai (biasanya memakan waktu 2–5 menit saat pertama kali).

#### Langkah 3: Konfigurasi SDK PlayFab Manager
1. Di dalam Unity Editor, buka window menu: Window -> PlayFab -> EdEx (atau periksa komponen PlayFabAuthManager di hierarchy scene).
2. Masukkan **Title ID PlayFab** proyek Anda (misal: 1A2B3).
3. Pastikan PlayFabSharedSettings terisi API Key dan Title ID secara akurat agar fitur Cloud Save dan Leaderboard aktif.

#### Langkah 4: Menjalankan Game di Unity Editor
1. Di panel Project, navigasi ke folder Assets/Scenes/.
2. Klik ganda pada scene MainMenu.unity.
3. Tekan tombol **Play** di bagian atas Unity Editor.
4. Game siap dimainkan langsung di window Game View.

#### Langkah 5: Meng-compile Executable Build (Windows PC)
1. Pilih menu File -> Build Settings...
2. Pastikan daftar Scenes In Build mencakup:
   - Assets/Scenes/MainMenu.unity (Index 0)
   - Assets/Scenes/Stage1_RedRegion.unity (Index 1)
   - Assets/Scenes/Stage2_OrangeRegion.unity (Index 2)
   - Assets/Scenes/Stage3_PinkBloom.unity (Index 3)
3. Pilih Platform Standalone Windows (x86_64).
4. Klik tombol **Build**, lalu buat folder baru bernama Builds/.
5. Tekan **Select Folder**. Unity akan menghasilkan file LifeOnLand.exe beserta folder LifeOnLand_Data.

# BAB II: PANDUAN PENGGUNAAN APLIKASI (USER MANUAL)

Bab ini menjelaskan alur operasional penggunaan game Life on Land bagi pengguna akhir (players).

### 2.1 Skema Kontrol Keyboard (Hotbar dan Navigasi)
- **W, A, S, D / Tombol Panah:** Menggerakkan karakter Umbra ke 4 arah top-down.
- **Tombol Angka 1 – 6:** Memilih item aktif pada Hotbar:
  - **Slot 1 (Shovel/Sekop):** Menggali ubin terbakar (corrupted burnt tiles).
  - **Slot 2 (Watering Can):** Menyiram air pada tanah galian atau menyiram pohon layu.
  - **Slot 3 (Desert Shrub Seed):** Menanam benih Semak Gurun (Tanaman Tipe B).
  - **Slot 4 (Pine Tree Seed):** Menanam benih Pohon Pinus (Tanaman Tipe A).
  - **Slot 5 (Silkmoth Fern Seed):** Menanam benih Paku Ngengat (Tanaman Tipe C).
  - **Slot 6 (Blueprints Menu):** Memilih dan meletakkan bangunan (Soil Purifier / Irrigation Pipes / Biosphere Dome).
- **Tombol Spasi / Klik Kiri Mouse:** Menggunakan alat/benih aktif pada ubin yang ditunjuk kursor.
- **Tombol E:** Berinteraksi / Bicara dengan NPC (Maliz, Oryel, Pyper).
- **Tombol ESC:** Membuka Pause Menu & Panel Achievements / PlayFab Leaderboard.

### 2.2 Panduan Menjalankan Stage dan Quest

#### 1. Stage 1 — Red Region (The Arid Oasis)
1. Berbicaralah dengan NPC **Maliz the Bear** di dekat oasis mati.
2. Ambil **10 unit air** dari kolam air dalam (Deep Pond) dengan gembor air (Hotbar 2). Serahkan ke Maliz untuk mendapatkan **Desert Shrub Seeds**.
3. Pilih Sekop (Hotbar 1), klik pada 5 ubin gersang merah terbakar untuk mengubahnya menjadi Dug Burnt Soil.
4. Pilih Gembor Air (Hotbar 2), klik pada ubin galian tersebut untuk membersihkannya menjadi Normal Soil.
5. Pilih Semak Gurun (Hotbar 3), tanam benih di atas normal soil dan siram air hingga tumbuh dewasa.
6. Capai kadar oksigen 50.0%. Maliz akan membuka gerbang menuju Stage 2.

#### 2. Stage 2 — Orange Region (The Scorched Grove)
1. Temui NPC **Oryel the Fox**. Buktikan kemampuanmu dengan memurnikan 5 ubin terbakar.
2. Dapatkan **Pine Tree Seeds** dan Blueprint **Soil Purifier**.
3. Pilih Hotbar 6, letakkan Soil Purifier di tengah bioma untuk mempercepat pembersihan tanah sekitar.
4. Tanam 8 Pohon Pinus dan jaga kelembapan tanah agar tidak menguap drastis. Capai kadar O2 21.0%.

#### 3. Stage 3 — Pink Bloom (Boss Stage dan Biosphere Dome)
1. Temui **Pyper the Moth**. Ambil benih **Silkmoth Fern** dan Blueprint **Irrigation Pipes** darinya (tolak tawaran Pyper untuk membelot ke Villain!).
2. Pasang Pipa Irigasi di sekitar deretan tanaman untuk mengalirkan air otomatis saat gelombang panas (Heatwave) menyerang.
3. Setelah kadar O2 membaik, bangun **Biosphere Dome** di pusat bioma untuk mengurung sang Villain dan memenangkan permainan!

# BAB III: DOKUMENTASI TEKNIKAL CODING DAN ARSITEKTUR KODE

Bab ini menjelaskan struktur kode program C# dan arsitektur teknis dari proyek game Life on Land.

### 3.1 Struktur File Proyek Unity

```text
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── Player.cs               # Controller pergerakan, stamina, inventory, & hotbar
│   │   ├── EnvironmentManager.cs   # Matriks kelembapan, kalkulasi O2, & disaster Heatwave
│   │   └── WorldObject.cs          # Base class objek grid di dunia game
│   ├── Entities/
│   │   ├── Tree.cs                 # FSM pertumbuhan tanaman (Seed->Sprout->Mature->Withered)
│   │   ├── WaterSourceNode.cs      # Pengambilan air kolam
│   │   └── BuildingBlueprint.cs   # Logika bangunan Soil Purifier & Irrigation Pipes
│   ├── UI/
│   │   ├── UIManager.cs            # HUD Stamina, O2 Buffer, Hotbar highlight, & Pause Menu
│   │   ├── DialogueManager.cs     # Sistem Visual Novel dialog box & portrait NPC
│   │   └── AchievementsUI.cs       # Panel pencapaian PlayFab & Leaderboard
│   └── Backend/
│       ├── SaveSystem.cs           # Serialisasi JSON lokal (SaveData.json)
│       └── PlayFabAuthManager.cs   # Service wrapper API PlayFab Cloud Save & Auth
├── Prefabs/                        # Prefab Tile, Tree, NPC, & Buildings
├── Sprites/                        # Sprite sheet pixel art 32 PPU (Karakter & Tilemaps)
├── Audio/                          # File sound effect (.wav) & background music (.mp3)
└── Scenes/                         # MainMenu, Stage1_Red, Stage2_Orange, Stage3_Pink
```

### 3.2 Penjelasan Logika Coding Utama

#### 1. Controller Utama Karakter (Player.cs)
File Player.cs mengontrol input pergerakan WASD, pengurangan stamina saat menggali/menyiram, serta eksekusi penggunaan item pada hotbar slot 0 hingga 5:

```csharp
public class Player : MonoBehaviour {
    [SerializeField] private float baseMovementSpeed = 5f;
    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private int activeHotbarSlot = 0;

    public void ProcessMovementInput(float horizontal, float vertical) {
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        transform.Translate(movement * baseMovementSpeed * Time.deltaTime);
    }

    public void SelectHotbarSlot(int slotIndex) {
        if (slotIndex >= 0 && slotIndex < 6) {
            activeHotbarSlot = slotIndex;
            UIManager.Instance.UpdateHotbarSelection(activeHotbarSlot);
        }
    }

    public void UseActiveTool(Vector2 targetGridCoordinates) {
        if (currentStamina < 5f) return;
        
        switch (activeHotbarSlot) {
            case 0: // Shovel
                EnvironmentManager.Instance.DigTile(targetGridCoordinates);
                currentStamina -= 5f;
                break;
            case 1: // Watering Can
                EnvironmentManager.Instance.WaterTile(targetGridCoordinates);
                currentStamina -= 2f;
                break;
            // Slot 2-4: Plant Seeds, Slot 5: Infrastructure
        }
    }
}
```

#### 2. Matriks Ekosistem Dinamis (EnvironmentManager.cs)
File EnvironmentManager.cs memproses status ubin tanah, penguapan kelembapan (moisture decay), dan kalkulasi persentase O2 atmosfer secara global:

```csharp
public class EnvironmentManager : MonoBehaviour {
    public static EnvironmentManager Instance { get; private set; }
    [SerializeField] private float globalO2Percentage = 15.0f;
    private float[,] soilMoistureGrid = new float[50, 50];

    public void ExecuteStateTick() {
        float decayFactor = isHeatwaveActive ? 0.10f : 0.05f;
        for (int x = 0; x < 50; x++) {
            for (int y = 0; y < 50; y++) {
                if (soilMoistureGrid[x, y] > 0) {
                    soilMoistureGrid[x, y] -= decayFactor * Time.deltaTime;
                    soilMoistureGrid[x, y] = Mathf.Max(0, soilMoistureGrid[x, y]);
                }
            }
        }
        RecalculateAtmosphericComposition();
    }
}
```
