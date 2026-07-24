# PANDUAN PENGGUNA (USER GUIDELINE)
## GAME "LIFE ON LAND" (ECO-RESTORATION SIMULATOR)

**Panduan Pengoperasian, Kontrol Permaianan, dan Strategi Restorasi Lahan**  
**Universitas Esa Unggul — 2026**

---

## BAB I: PENDAHULUAN DAN CERITA PENGGUNA

### 1.1 Peran Anda Sebagai Restorer
Selamat datang di **Life on Land**! Bumi masa depan mengalami krisis ekosistem masif di mana kadar oksigen ($O_2$) atmosferik tersisa hanya $15.0\%$. Anda berperan sebagai **Umbra**, sang *Restorer* terakhir yang terlatih untuk memulihkan tanah gersang dan menghidupkan kembali vegetasi bumi tile-demi-tile.

### 1.2 Misi Utama Permainan
1. **Memurnikan Lahan Terkontaminasi:** Membersihkan ubin tanah terbakar (*Burnt Tiles*) sisa pembakaran antagonis Blaze.
2. **Menjaga Kelembapan Tanah:** Menyiram tanah dan mengatur jarak tanaman agar tidak layu akibat penguapan air.
3. **Menaikkan Kadar Oksigen ($O_2$):** Menanam vegetasi pendukung hingga emisi $O_2$ mencapai batas aman ($\ge 21.0\%$, atau $50.0\%$ pada Stage 1 Demo).
4. **Mengejar Antagonis Blaze:** Membantu para *Guardian* wilayah (Maliz, Oryel, Pyper) untuk membuka gerbang antar-stage.

---

## BAB II: KONTROL DASAR DAN PERKAKAS HOTBAR (TOMBOL 1–6)

### 2.1 Navigasi dan Interaksi Karakter
* **Berjalan / Pergerakan:** Tekan tombol **W, A, S, D** atau **Tombol Panah** pada keyboard.
* **Menggunakan Alat / Menanam:** Arahkan kursor/posisi pemain ke petak ubin yang di-highlight, lalu tekan **Spasi** atau **Klik Kiri Mouse**.
* **Membuka Menu Pause:** Tekan tombol **ESC** untuk membuka Menu Pause dan melihat *Achievements*.

### 2.2 Fungsi 6 Slot Hotbar Perkakas (Tombol 1–6)

| Slot | Nama Perkakas / Benih | Tombol Keyboard | Fungsi Utama | Consumables / Cost |
|:---:|---|:---:|---|---|
| **1** | **Sekop / Hoe** | `1` | Menggali ubin terbakar (*Burnt Tile*) menjadi ubin tergali (*Dug Burnt Soil*). | Memakai 5 Stamina |
| **2** | **Gembor Air (Watering Can)** | `2` | Menyiram ubin tergali menjadi tanah bersih (*Normal Soil*), menyiram tanaman, & memulihkan pohon layu (*Withered*). | Memakai 1 Unit Air |
| **3** | **Benih Semak Gurun (Desert Shrub)** | `3` | Menanam benih Type B (kebutuhan air rendah, membantu menjaga kelembapan ubin sekitar). | 1 Benih Semak |
| **4** | **Benih Pohon Pinus (Pine Tree)** | `4` | Menanam benih Type A (emisi $O_2$ sangat tinggi, kebutuhan air tinggi). | 1 Benih Pinus |
| **5** | **Benih Pakis Ngengat (Silkmoth Fern)** | `5` | Menanam benih Type C (tahan gelombang panas / *heatwaves*). | 1 Benih Pakis |
| **6** | **Blueprint Infrastruktur** | `6` | Membuka sub-menu konstruksi bangunan (*Soil Purifier*, *Irrigation Pipes*, *Biosphere Dome*). | Bahan Bangunan |

---

## BAB III: PANDUAN LANGKAH PERMAINAN (STAGE 1: RED REGION DEMO)

Berikut adalah panduan langkah demi langkah untuk memenangkan **Stage 1 (Red Region)**:

### Langkah 1: Memulai Permainan dari Main Menu
1. Jalankan game `LifeOnLand.exe` atau play di Unity Editor.
2. Pada tampilan **Main Menu**, masukkan nama Restorer Anda lalu klik **Start Game** / **Login**.

### Langkah 2: Mengambil Quest dari Maliz the Bear
1. Setelah cutscene pembakaran lahan selesai, temui NPC **Maliz** (Beruang merah bata) di area Oasis.
2. Dekati Maliz untuk memicu dialog. Maliz akan meminta bantuan Anda membawa **10 unit air** dari kolam dalam (*Deep Pond*).

### Langkah 3: Mengambil Air dari Kolam
1. Berjalanlah ke tepi kolam air (*Deep Pond*).
2. Tekan **Tombol 2** untuk memilih **Gembor Air**.
3. Tekan **Spasi** di dekat air hingga jumlah air di indikator HUD terisi **10/10 Units**.

### Langkah 4: Menyerahkan Air & Menerima Benih
1. Kembali ke NPC Maliz dan picu dialog penyerahan quest.
2. Maliz akan memberikan imbalan berupa **Benih Semak Gurun (Desert Shrub Seeds)**.

### Langkah 5: Pemurnian Lahan 2-Tahap (Two-Step Purification)
1. **Tahap 1 (Gali):** Tekan **Tombol 1** (Sekop), arahkan ke 5 ubin terbakar berwarna hitam, lalu tekan **Spasi**. Ubin berubah menjadi ubin tergali.
2. **Tahap 2 (Siram):** Tekan **Tombol 2** (Gembor Air), arahkan ke ubin tergali tersebut, lalu tekan **Spasi**. Ubin berubah menjadi tanah subur berwarna cokelat bersih.

### Langkah 6: Penanaman dan Perawatan Vegetasi
1. Tekan **Tombol 3** untuk memilih **Benih Semak Gurun**.
2. Tekan **Spasi** pada ubin subur bersih untuk menanam benih.
3. Siram benih secara teratur menggunakan **Tombol 2 (Gembor Air)**. Tanaman akan berkembang melalui siklus:  
   **Seed (Benih)** $\rightarrow$ **Sprout (Tunas)** $\rightarrow$ **Sapling** $\rightarrow$ **Young** $\rightarrow$ **Mature Tree (Pohon Dewasa)**.

### Langkah 7: Pemulihan Oksigen & Pembukaan Gerbang Stage
1. Setelah 5 Semak Gurun tumbuh dewasa, emisi $O_2$ akan terinjeksi ke atmosfer.
2. Perhatikan indikator $O_2$ di bagian atas layar hingga mencapai **50.0%**.
3. Gerbang batas wilayah akan terbuka, antagonis Blaze akan melarikan diri, dan Anda berhasil memenangkan Stage 1 Demo!

---

## BAB IV: STRATEGI SURVIVAL DAN PEMELIHARAAN LAHAN

### 4.1 Manajemen Stamina & Buffer Oksigen
* **Stamina:** Depleted setiap kali Anda berjalan jauh atau menggali ubin dengan Sekop. Pemulihan dapat dilakukan dengan memakan **Rations (Bubur Pasokan)** dari peti pasokan atau berdiam di dekat area ber- $O_2$ tinggi.
* **Buffer Oksigen Lokal:** Jika Anda berada di area terpolusi dengan $O_2 < 18.0\%$, buffer udara Anda akan menurun. Segera kembali ke area bersih untuk mengisi ulang buffer udara.

### 4.2 Penanganan Tanaman Layu (Withered Trees)
* Jika kelembapan tanah bernilai $0.0$, tanaman akan memasuki status **Withered (Layu)** dan berhenti memancarkan $O_2$.
* **Jangan Khawatir!** Tanaman layu **tidak mati permanen**. Cukup siramkan air pada ubin atau tanaman tersebut menggunakan **Tombol 2 (Gembor Air)**, maka tanaman akan pulih kembali ke status tumbuhnya semula!

---

## BAB V: TAMPILAN ANTARMUKA UI (HUD & MENUS)

### 5.1 Indikator Layar Utama (HUD Top Bar)
* **Bar Green (Stamina):** Menunjukkan sisa stamina fisik pemain ($0–100$).
* **Bar Blue (O2 Buffer):** Menunjukkan cadangan udara bersih saat berada di zona beracun ($0–10$).
* **Indikator Persentase Global O2:** Menunjukkan total oksigen atmosferik saat ini (Target Stage 1: $50.0\%$).
* **Counter Water Inventory:** Menunjukkan sisa pasokan air di gembor ($0–10$ units).

### 5.2 Menu Pause & Panel Achievements
* Tekan **ESC** saat bermain untuk menghentikan sementara permainan (*Pause*).
* Anda dapat memeriksa pencapaian (*Achievements*) seperti *First Steps*, *Water Bearer*, dan *Green Oasis*, atau kembali ke Main Menu.
