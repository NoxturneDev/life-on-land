# PANDUAN PENGEMBANGAN (DEVELOPER GUIDELINE)
## GAME "LIFE ON LAND" (ECO-RESTORATION SIMULATOR)

**Sistem Arsitektur, Kontrak Kelas C#, Standar Koding, dan Desain Fitur Stage**  
**Universitas Esa Unggul — 2026**

---

## BAB I: GAMBARAN UMUM DAN PILAR DESAIN GIM

### 1.1 Visi Gim dan Konsep Utama
* **Judul Gim:** Life on Land
* **Genre:** Top-Down Cozy Stage-Based Forest Ecosystem Simulator
* **Platform Target:** PC / Windows / macOS / Linux & WebGL
* **Perspektif:** Fixed 2D Orthographic Top-Down
* **Arsitektur:** Monolithic Client dengan Local Serialization (`SaveData.json` / SQLite) & PlayFab Cloud

### 1.2 Alur Utama Permainan (Core Game Loop)
```
    +------------------------------------------+
    |         Eksplorasi & Pengumpulan         | <----------------+
    | (Buka Peti Pasokan, Ambil Air Kolam)     |                  |
    |                                          |                  |
    |                    v                     |                  |
    |     Perolehan Quest & Benih Pohon        |                  |
    |   (Bicara NPC, Selesaikan Mini-Quest)    |                  |
    |                                          |                  |
    |                    v                     |                  |
    |       Penanaman Pohon Strategis          |                  | Iterasi Loop
    | (Bersihkan Ubin Hangus, Jaga Kelembapan) |                  |
    |                                          |                  |
    |                    v                     |                  |
    |    Pembaruan Lingkungan Dinamis (O2)     |                  |
    |   (Capai Target O2 & Jumlah Pohon Hidup) |                  |
    |                                          |                  |
    |                    v                     |                  |
    |        Gerbang Kemajuan Tahap (Stage)    |                  |
    | (Antagonis Kabur, Jalur Baru Terbuka)    |                  |
    +------------------------------------------+                  |
                         |                                        |
                         +----------------------------------------+
```

### 1.3 Pilar Desain (Design Pillars)
1. **Cozy with Stakes:** Mekanik bercocok tanam yang menenangkan dipadukan dengan dorongan batas O2 dan ancaman pembakaran lahan oleh Blaze.
2. **Tanah sebagai Karakter Utama:** Variabel kelembapan (*moisture*), kontaminasi (*corruption*), dan kadar oksigen menjadikan tanah sebagai objek dinamis yang merespons tindakan pemain.
3. **Pemaaf Terhadap Kegagalan:** Pohon yang layu (*Withered*) dapat dipulihkan kembali hanya dengan menyiramnya air, sehingga mendorong eksperimentasi pemain.
4. **Restorasi Visual yang Nyata:** Perubahan visual dunia dari warna cokelat sepia gersang menjadi lingkungan hijau yang asri memberikan kepuasan instan.

---

## BAB II: SPESIFIKASI STAGE DAN KARAKTER GUARDIAN

Game terbagi menjadi 3 wilayah (*stage*) berurutan yang masing-masing dikuasai oleh karakter *Guardian* unik:

### 2.1 Stage 1: Red Region (The Arid Oasis)
* **Aset Visual:** Tanah liat merah, pasir kering, tunggul pohon merah layu.
* **Karakter NPC:** **Maliz** (Beruang Wrath Barbarian; berpenampilan sangat tangguh tetapi sebenarnya sedih dan merasa tidak berdaya karena dunianya terbakar).
* **Mini-Quest NPC:** Mengambil **10 unit air** dari kolam dalam menggunakan gembor air untuk diberikan kepada Maliz.
* **Imbalan (Reward):** **Benih Semak Gurun (Desert Shrub Seeds - Type B)** (Kebutuhan air rendah, meningkatkan retensi kelembapan tanah tetangga).
* **Quest Utama Stage:** Memurnikan 5 ubin terbakar, menanam 5 Semak Gurun hingga dewasa, menaikkan O2 lokal hingga **50.0%**.
* **Pintu Keluar Stage:** Antagonis Blaze muncul di gerbang, mengejek kemenangan kecil pemain, lalu melarikan diri ke Orange Region. Maliz membuka blokade jalan.

### 2.2 Stage 2: Orange Region (The Scorched Grove)
* **Aset Visual:** Tanah berdaun jingga gugur, serasah kering, kanopi oranye mati.
* **Karakter NPC:** **Oryel** (Rubah Pride Rogue; sangat mandiri, keras kepala, dan menolak bantuan dari luar karena yakin bisa mengatasi kebakaran sendiri).
* **Mini-Quest NPC:** Menyiangi dan menyiram **5 ubin terbakar** untuk membuktikan kemampuan pemurnian pemain kepada Oryel.
* **Imbalan (Reward):** **Benih Pohon Pinus (Pine Tree Seeds - Type A)** (Emisi O2 tinggi, kebutuhan air tinggi) dan cetak biru **Pemurni Tanah (Soil Purifier)**.
* **Quest Utama Stage:** Membangun 1 Pemurni Tanah, menanam 8 Pohon Pinus dewasa, menaikkan O2 lokal hingga **21.0%**.
* **Pintu Keluar Stage:** Jalur menuju sarang akhir Blaze di Pink Bloom terbuka.

### 2.3 Stage 3: Pink Bloom (Boss Stage)
* **Aset Visual:** Kelopak bunga merah muda, kabut merah muda berkilau, flora lentera ngengat.
* **Karakter NPC:** **Pyper** (Ngengat Lust Bard; sangat memesona dan memikat, bernegosiasi rahasia dengan Blaze dan mencoba merayu pemain untuk bergabung).
* **Tantangan Gelombang Panas:** Blaze hadir secara aktif memicu *Heatwaves* yang menggandakan laju penguapan air tanah.
* **Alat Pendukung:** **Benih Pakis Ngengat (Silkmoth Fern Seeds - Type C)** (Tahan panas, emisi O2 tinggi) dan cetak biru **Pipa Irigasi (Irrigation Pipes)**.
* **Quest Utama Stage:** Menjaga kelembapan vegetasi dengan Pipa Irigasi selama gelombang panas, membangun **Biosphere Dome** untuk memulihkan bumi dan menjebak Blaze, menaikkan O2 global hingga **21.0%**.

---

## BAB III: MATRIKS STRATEGI SPESIES TANAMAN

| Tipe Tanaman | Nama Spesies | Karakteristik Utama | Peran Strategis |
|---|---|---|---|
| **Type A** | Pohon Pinus (*Pine Tree*) | Kebutuhan air tinggi, emisi $O_2$ sangat tinggi | Mesin utama penghasil oksigen masif |
| **Type B** | Semak Gurun (*Desert Shrub*) | Kebutuhan air rendah, meningkatkan retensi air ubin sekitar | Penstabil kelembapan tanah penyokong Type A |
| **Type C** | Pakis Ngengat (*Silkmoth Fern*) | Tahan gelombang panas (*heatwave-resistant*), emisi $O_2$ tinggi | Spesies wajib untuk Stage 3 Pink Bloom |

---

## BAB IV: ATURAN HOTBAR DAN CARA KERJA MATRIKS DUNIA

### 4.1 Tata Letak Hotbar Perkakas (Tombol 1–6)
* **Slot 1 (Sekop / Hoe):** Menggali ubin terbakar (*Corrupted Burnt Tile*) menjadi *Dug Burnt Soil* (memakai 5 Stamina).
* **Slot 2 (Gembor Air / Watering Can):** Menyiram *Dug Burnt Soil* menjadi tanah bersih (*Normal Soil*), serta menyiram tanaman layu.
* **Slot 3 (Benih Semak Gurun):** Menanam benih Type B.
* **Slot 4 (Benih Pohon Pinus):** Menanam benih Type A.
* **Slot 5 (Benih Pakis Ngengat):** Menanam benih Type C.
* **Slot 6 (Cetak Biru Infrastruktur):** Menempatkan bangunan (*Soil Purifier*, *Irrigation Pipes*, *Biosphere Dome*).

---

## BAB V: STANDAR PENGKODEAN DAN IMPOR PIXEL ART

### 5.1 Standar Pengkodean C# (C# Coding Conventions)
1. **Aturan Penamaan:**
   * Nama Class, Method, dan Public Property menggunakan **PascalCase** (contoh: `EnvironmentManager`, `ProcessMovementInput`).
   * Field Private dan Variabel Lokal menggunakan **camelCase** (contoh: `baseMovementSpeed`, `currentStamina`).
2. **Pengelolaan Komponen Unity:**
   * Selalu *cache* komponen `Rigidbody2D`, `Animator`, dan `SpriteRenderer` di method `Awake()` atau `Start()`. Dilarang memanggil `GetComponent()` secara berulang di dalam `Update()` atau `FixedUpdate()`.
   * Gunakan atribut `[SerializeField]` untuk mengekspos variabel private ke Unity Inspector.
3. **Kalkulasi Fisika 2D:**
   * Pergerakan karakter utama harus memperbarui atribut `rb.linearVelocity` (atau `rb.velocity`) di dalam method `FixedUpdate()`.

### 5.2 Standar Impor Piksel Art (Pixel Art Import Rules)
* **Pixels Per Unit (PPU):** Diset tepat pada nilai **32**.
* **Filter Mode:** Wajib diset ke **Point (no filter)**.
* **Compression:** Wajib diset ke **Uncompressed**.
* **Pivot Point:** Prefab interaktif wajib memiliki pivot di **BottomCenter** untuk memastikan Y-sorting rendering depth bekerja akurat.

---

## BAB VI: NASKAH DIALOG DEMO VISUAL NOVEL

### Stage 1 — Red Region (Maliz)
**Opening (Blaze membakar oasis terakhir)**
> **Blaze:** Titik hijau terakhir di dunia yang mati ini. Betapa menyedihkan... Biarkan semuanya menjadi abu!  
> **Maliz:** Pohon-pohonku! Hentikan, Blaze! Tolong, siapa saja... oasis ini adalah satu-satunya rumah kami!  
> **Umbra:** Tenanglah, prajurit. Tanah ini masih menyimpan kehidupan. Aku akan membantumu memulihkannya.

**Assignment (Quest Air)**
> **Maliz:** Hutan ini membutuhkan air dari kolam dalam. Bawakan aku 10 unit air, dan aku akan memberimu benih Semak Gurun terbaikku!

**Completion (Stage 1 Selesai)**
> **Umbra:** Udara mulai segar kembali. Oksigen telah mencapai 50%.  
> **Maliz:** Kamu benar-benar berhasil, Restorer! Gerbang menuju Orange Region sekarang terbuka. Kejar Blaze sebelum dia menghancurkan wilayah Oryel!
