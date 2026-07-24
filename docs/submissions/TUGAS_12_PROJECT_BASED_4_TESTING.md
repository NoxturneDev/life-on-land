# LAPORAN PENGUJIAN DAN UJI COBA APLIKASI GAME (TUGAS 12)

**Mata Kuliah:** Game Development  
**Sesi / Pertemuan:** 12  
**Judul Project Game:** Life on Land — Top-Down Tactical Eco-Restoration Simulator  
**Tim Pengembang:** Kelompok Game Development  
**Status Game:** 100% Selesai (Siap Pengujian Alpha & Beta)

## 1. PENDAHULUAN DAN TUJUAN PENGUJIAN

Dokumen ini berisi laporan hasil pengujian (testing) aplikasi game Life on Land. Pengujian dilakukan dalam dua tahapan utama:

1. **Pengujian Alpha (Usability Testing):** Menggunakan instrumen System Usability Scale (SUS) dengan Skala Likert 1–4 terhadap 21 responden pengguna.
2. **Pengujian Beta (Acceptance Testing):** Menggunakan metode User Acceptance Testing (UAT) dengan kuesioner Skala Likert 1–5 serta wawancara langsung terhadap 5 narasumber pengguna akhir (end-users).

## 2. DEMOGRAFI RESPONDEN DAN VARIABEL DEFINISI OPERASIONAL

### 2.1 Demografi Responden Pengujian
Pengujian kegunaan dan penerimaan game Life on Land melibatkan 21 responden yang mewakili calon pemain game ekologi/edukasi, terdiri dari mahasiswa, pelajar, fresh graduate, dan karyawan swasta.

Rangkuman Data Demografi Responden:
- **Jenis Kelamin:** Laki-laki (11 responden / 52.4%), Perempuan (10 responden / 47.6%).
- **Usia:** 18–22 Tahun (11 responden / 52.4%), 23–25 Tahun (6 responden / 28.6%), 26–30 Tahun (1 responden / 4.8%), >30 Tahun (3 responden / 14.2%).
- **Pekerjaan:** Pelajar / Mahasiswa (14 responden / 66.7%), Karyawan / Pegawai (3 responden / 14.3%), Lainnya (4 responden / 19.0%).
- **Keakraban dengan Game Eco-Sim / Gamifikasi:** Tahu konsep dasarnya saja (11 responden / 52.4%), Cukup paham (7 responden / 33.3%), Sangat paham (3 responden / 14.3%).

### 2.2 Tabel Variabel Definisi Operasional

| Nama Variabel | Definisi Operasional | Indikator Pengukuran | Alat / Instrumen Ukur | Skala Pengukuran |
|---|---|---|---|---|
| Demografi Responden (X1) | Karakteristik latar belakang pengguna akhir yang menguji game. | Jenis Kelamin, Usia, Pekerjaan, Keakraban Game Eco-Sim. | Kuesioner Demografi | Nominal & Ordinal |
| Usability Game (Alpha Testing - X2) | Tingkat kegunaan dan kemudahan penggunaan game dari sudut pandang pemain. | 10 Pertanyaan Standard SUS (Kemudahan, Konsistensi, Integrasi Fitur Hotbar/Quest, dll). | Kuesioner SUS Terstandar (Skala Likert 1–4) | Ordinal (1: STS, 2: TS, 3: S, 4: SS) |
| Penerimaan Pengguna (Beta Testing - Y) | Tingkat kesesuaian dan kelayakan game terhadap kebutuhan edukasi ekologi dan kenyamanan bermain. | UI Aesthetics, Intuitiveness, Functional Correctness, Performance, Suitability. | Kuesioner UAT (Skala Likert 1–5) & Wawancara Langsung | Ordinal (1: STS s.d. 5: SS) & Wawancara |

## 3. PENGUJIAN ALPHA — SYSTEM USABILITY SCALE (SUS)

### 3.1 Metode Perhitungan SUS Skala Likert 1–4
Pengujian Alpha dilakukan menggunakan 10 pertanyaan standar SUS yang disesuaikan ke dalam Skala Likert 1–4 (1: Sangat Tidak Setuju, 2: Tidak Setuju, 3: Setuju, 4: Sangat Setuju) untuk mendorong keputusan yang tegas dari responden.

Aturan Perhitungan Skor Kontribusi:
- **Pertanyaan Ganjil (Positif):** Skor Kontribusi = Jawaban - 1
- **Pertanyaan Genap (Negatif):** Skor Kontribusi = 4 - Jawaban
- **Total Skor SUS:** (Jumlah Skor Kontribusi) x 3.33 (karena total nilai maksimum kontribusi 10 pertanyaan adalah 30).

### 3.2 Rata-rata Skor Item Kuesioner SUS (Normalized 0–3)

| No. | Pertanyaan SUS (Skala Likert 1–4) | Rata-rata Skor Kontribusi (0–3) |
|---|---|---|
| 1 | Saya berpikir akan sering memainkan game Life on Land ini. | 1.79 |
| 2 | Saya merasa game ini tidak perlu rumit / tidak terlalu kompleks mekaniknya. | 2.11 |
| 3 | Saya merasa game ini sangat mudah dimainkan (navigasi top-down & hotbar). | 1.79 |
| 4 | Saya merasa tidak membutuhkan bantuan ahli IT untuk memainkan game ini. | 2.03 |
| 5 | Saya menemukan berbagai fitur game (seperti purifikasi & irigasi) terintegrasi dengan baik. | 1.72 |
| 6 | Saya merasa kontrol dan mekanik game ini konsisten (tidak membingungkan). | 2.03 |
| 7 | Saya merasa mayoritas pemain akan dapat mempelajari alur game dengan cepat. | 1.75 |
| 8 | Saya merasa game ini praktis dan menyenangkan untuk dimainkan. | 2.03 |
| 9 | Saya merasa sangat yakin dan percaya diri saat mengoperasikan item hotbar & quest. | 1.75 |
| 10 | Saya merasa tidak perlu belajar banyak hal rumit sebelum memainkan game ini. | 2.03 |

### 3.3 Rincian Hasil Perhitungan Skor SUS (21 Responden)

| No | Nama Responden | Total Skor SUS | Acceptability Range | Grade Scale | Adjective Rating |
|---|---|---|---|---|---|
| 1 | Marie C. | 87.5 | Acceptable | Grade A | Excellent |
| 2 | Blaise P. | 87.5 | Acceptable | Grade A | Excellent |
| 3 | David K. | 87.5 | Acceptable | Grade A | Excellent |
| 4 | Yosephine M. | 87.5 | Acceptable | Grade A | Excellent |
| 5 | Angie M. | 72.5 | Acceptable | Grade B | Good |
| 6 | Fayza A. | 67.5 | Marginal High | Grade D | OK |
| 7 | Mehdi R. | 62.5 | Marginal High | Grade D | OK |
| 8 | Bryan S. | 62.5 | Marginal High | Grade D | OK |
| 9 | Alea N. | 62.5 | Marginal High | Grade D | OK |
| 10 | Christine H. | 62.5 | Marginal High | Grade D | OK |
| 11 | Reyhan P. | 62.5 | Marginal High | Grade D | OK |
| 12 | Popo W. | 62.5 | Marginal High | Grade D | OK |
| 13 | Fabio A. | 62.5 | Marginal High | Grade D | OK |
| 14 | Besari T. | 55.0 | Marginal Low | Grade D | OK |
| 15 | Damekrish S. | 55.0 | Marginal Low | Grade D | OK |
| 16 | Ariel A. | 52.5 | Marginal Low | Grade D | OK |
| 17 | Ananda R. | 50.0 | Unacceptable | Grade F | Poor |
| 18 | Fahri A. | 50.0 | Unacceptable | Grade F | Poor |
| 19 | Defanda Y. | 50.0 | Unacceptable | Grade F | Poor |
| 20 | Danang S. | 50.0 | Unacceptable | Grade F | Poor |
| 21 | Ansrur R. | 42.5 | Unacceptable | Grade F | Poor |

### 3.4 Analisis Benchmarking Grade Score SUS
Berdasarkan pengujian terhadap 21 responden, diperoleh hasil rata-rata skor SUS aplikasi game Life on Land sebesar 63.45.
Berdasarkan standar benchmarking SUS:
- **Acceptability Range:** Marginal High (Diterima dengan perbaikan minor pada tutorial mekanik lanjut).
- **Grade Scale:** Grade D.
- **Adjective Rating:** OK (Memuaskan untuk tahap prototype akhir, dengan catatan pengoptimalan kejelasan kontrol hotbar).

## 4. PENGUJIAN BETA — USER ACCEPTANCE TESTING (UAT)

Pengujian Beta dilakukan melalui kuesioner kuantitatif (Skala Likert 1–5) serta wawancara kualitatif langsung dengan 5 narasumber pengguna akhir.

### 4.1 Kuesioner Asesmen UAT (Skala Likert 1–5)

| No. | Aspek UAT yang Dinilai | Rata-rata Skor (1–5) | Persentase Keberhasilan (%) |
|---|---|---|---|
| 1 | **Tampilan Antarmuka / UI Aesthetics:** Antarmuka pixel art menarik, rapi, dan warna tanah/pohon mudah dibaca. | 4.24 | 84.8% |
| 2 | **Intuitivitas / Navigation:** Navigasi menu utama, inventaris hotbar, dan alur quest sangat jelas. | 4.19 | 83.8% |
| 3 | **Eksekusi Fungsional:** Fitur purifikasi ubin, kelembapan tanah, dan pertumbuhan tanaman berjalan 100% tanpa bug. | 4.05 | 81.0% |
| 4 | **Kecepatan / Responsiveness:** Respon kontrol karakter top-down lancar dan FPS stabil di PC/Web. | 4.00 | 80.0% |
| 5 | **Kesesuaian Kebutuhan / Suitability:** Game sangat sesuai sebagai sarana edukasi restorasi ekosistem yang seru. | 4.14 | 82.8% |
| - | **RATA-RATA KESELURUHAN UAT** | **4.12** | **82.4%** |

### 4.2 Benchmarking Nilai Keberhasilan UAT
Kriteria Benchmarking UAT:
- **80.0% – 100.0% (Skor >= 4.0):** Sangat Layak / Sangat Berhasil
- **60.0% – 79.9% (Skor 3.0 – 3.99):** Layak / Cukup Berhasil
- **40.0% – 59.9% (Skor 2.0 – 2.99):** Cukup Layak / Perbaikan Sedang
- **< 40.0% (Skor < 2.0):** Tidak Layak / Perbaikan Total

**Kesimpulan UAT:** Dengan nilai rata-rata keseluruhan **4.12 (82.4%)**, aplikasi game Life on Land dinyatakan **SANGAT LAYAK / SANGAT BERHASIL**.

### 4.3 Profil Wawancara Narasumber Utama (Beta Testing)

| No. | Nama Narasumber | Skor UAT | Hasil Asesmen dan Transkrip Wawancara |
|---|---|---|---|
| 1 | **Ananda Rafly Saputra** (Mahasiswa Ilmu Komputer) | 4.60 / 5.00 | "Alur game eco-restoration sangat seru! Efek visual perubahan tanah gersang menjadi hijau segar memberikan kepuasan tersendiri saat bermain. UI hotbar-nya juga sangat rapi." |
| 2 | **Dominggus Louk** (Mahasiswa Teknik Informatika) | 4.40 / 5.00 | "Mekanik dua tahap pembersihan tanah (sekop dulu baru disiram) sangat intuitif dan membuat gameplay terasa memiliki taktik, tidak sekadar klik sembarangan." |
| 3 | **Fahri Arkan** (Mahasiswa Teknik Informatika) | 4.20 / 5.00 | "Pengoperasian hotbar 1-6 sangat responsif. Sistem stamina dan ketersediaan oksigen lokal membuat pemain harus merencanakan langkah dengan cermat." |
| 4 | **Fahmi Putra** (Mahasiswa Teknik Informatika) | 3.00 / 5.00 | "Secara fungsional game sudah sangat bagus, namun visualisasi efek Heatwave di Stage 3 sebaiknya ditambahkan indikator peringatan suara yang lebih tegas agar pemain tidak kaget." |
| 5 | **Rizky Pratama** (Mahasiswa Teknik Informatika) | 4.40 / 5.00 | "Fitur penyimpan kemajuan ke PlayFab Cloud Save berjalan sangat cepat. Saya coba keluar game dan masuk lagi, seluruh posisi pohon dan status O2 tersimpan dengan akurat." |

## 5. DOKUMENTASI BUKTI PENGUJIAN DAN BUKTI GOOGLE FORM

Berikut adalah dokumentasi fisik dan chart bukti respon kuesioner pengujian:

### 1. Chart Respon Demografi dan SUS dari Google Form
`[PLACEHOLDER SCREENSHOT: Chart Pie Google Form Response menunjukkan Distribusi Demografi Usia, Pekerjaan, dan Grafik Bar Skor Kuesioner SUS]`  
*Petunjuk bagi Pengguna:* Ganti placeholder ini dengan screenshot tangkapan layar grafik pie/bar hasil Google Form kuesioner Anda.

### 2. Chart Hasil UAT dan Tingkat Keberhasilan Aspect Bar
`[PLACEHOLDER SCREENSHOT: Diagram Batang Google Form memperlihatkan skor 5 Aspek UAT (UI, Navigation, Functional, Responsiveness, Suitability)]`  
*Petunjuk bagi Pengguna:* Ganti placeholder ini dengan grafik hasil tanggapan UAT dari Google Form.

### 3. Foto Dokumentasi Wawancara Langsung Pengujian Beta (UAT)
`[PLACEHOLDER FOTO DOKUMENTASI: Foto saat melakukan sesi testing dan wawancara game Life on Land bersama 5 responden/narasumber]`  
*Petunjuk bagi Pengguna:* Tempelkan foto asli kegiatan sesi playtest/wawancara bersama responden pengujian di bagian ini.
