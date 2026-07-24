# PETUNJUK PENYELESAIAN DAN PENGUMPULAN TUGAS (TUGAS 10 s.d. TUGAS 13)

**Mata Kuliah:** Game Development (Tahun 2026)  
**Judul Project Game:** Life on Land — Top-Down Tactical Eco-Restoration Simulator  
**Lokasi Berkas:** `c:\Users\galih\Documents\Projects\Game\My project\docs\submissions\`

## 1. RANGKUMAN BERKAS TUGAS YANG TELAH DIBUAT

Seluruh naskah draft tugas dari Tugas 10 hingga Tugas 13 telah berhasil dibuat secara lengkap dan bersih dalam dua format: **Markdown (.md)** dan **Microsoft Word (.docx)**.

| Pertemuan / Tugas | Nama Berkas Markdown (.md) | Nama Berkas Microsoft Word (.docx) | Ringkasan Isi Berkas |
|---|---|---|---|
| **Tugas 10** (Project Based 2) | TUGAS_10_PROJECT_BASED_2.md | TUGAS_10_PROJECT_BASED_2.docx | Laporan Progress 75%, Konsep Gamifikasi Level & Quest, Skema Hotbar & Purifikasi, Integrasi 50% PlayFab API, Placeholder Screenshots. |
| **Tugas 11** (Project Based 3) | TUGAS_11_PROJECT_BASED_3.md | TUGAS_11_PROJECT_BASED_3.docx | Laporan Final 100% Selesai, Integrasi 100% PlayFab Backend & Cloud Save, Skema JSON SaveData, Asset/Animation Kustom, Full Gameplay Flow Screenshots. |
| **Tugas 12** (Project Based 4) | TUGAS_12_PROJECT_BASED_4_TESTING.md | TUGAS_12_PROJECT_BASED_4_TESTING.docx | Demografi 21 Responden, Variabel Operasional, Alpha Testing SUS Likert 1-4 (Skor 63.45 / Grade D - OK), Beta Testing UAT Likert 1-5 (Skor 82.4% / Sangat Layak) & Transkrip 5 Wawancara. |
| **Tugas 13 - Laporan Akhir** | TUGAS_13_LAPORAN_AKHIR_5_BAB.md | TUGAS_13_LAPORAN_AKHIR_5_BAB.docx | Laporan Akhir 5 BAB Lengkap sesuai template resmi (Pendahuluan, Landasan Teori, Asset/Prototype, Hasil & Pembahasan PIECES/UML/PlayFab, Kesimpulan & Saran, Daftar Pustaka IEEE). |
| **Tugas 13 - PPT Presentasi** | TUGAS_13_PPT_PRESENTASI.md | TUGAS_13_PPT_PRESENTASI.docx | Struktur 20 Slide Presentasi Sidang Akhir Project beserta poin penjelasan voiceover per slide. |
| **Tugas 13 - Poster** | TUGAS_13_POSTER_CONTENT.md | TUGAS_13_POSTER_CONTENT.docx | Konten Layout Poster A4 Horizontal (Landscape) mencakup Abstrak, System Architecture, UI Screenshots, dan Hasil Testing SUS & UAT. |
| **Tugas 13 - Video Script** | TUGAS_13_VIDEO_SCRIPT_REELS.md | (Hanya .md) | Naskah Narasi Voiceover Video Reels Instagram 90 Detik, Draft Caption Instagram, Hashtags, dan Checklist Tag Dosen. |
| **Tugas 13 - Technical Guide** | TUGAS_13_DOKUMENTASI_TUTORIAL_DAN_TEKNIKAL_GUIDE.md | TUGAS_13_DOKUMENTASI_TUTORIAL_DAN_TEKNIKAL_GUIDE.docx | Panduan Instalasi & Compile Unity PC/Web (BAB I), User Manual Controls & Quest Walkthrough (BAB II), dan Dokumentasi Kode C# Player.cs / EnvironmentManager.cs (BAB III). |

## 2. PLACEHOLDER YANG PERLU DISESUAIKAN (CHECKLIST SINGKAT)

Sebelum mengunggah file PDF/DOCX ke LMS kuliah atau Google Drive, disarankan untuk melengkapi beberapa placeholder informasi personal berikut:

1. **Identitas Anggota Kelompok dan NIM:** Cari teks `[ISI_NIM_GALIH]`, `[ISI_NIM_BERKAT]`, `[ISI_NIM_OSCAR]`, dan `[ISI_NIM_FIRSCHANYA]` di file Tugas 13 Poster/Laporan Akhir, lalu isi dengan NIM asli masing-masing anggota.
2. **Nama dan Foto Dosen Pengampu:** Cari teks `[ISI_NAMA_DOSEN_PENGAMPU]` dan `@[ISI_USERNAME_INSTAGRAM_DOSEN]` di file Poster, Slide, dan Video Script, lalu ganti dengan nama serta username Instagram asli Dosen.
3. **Gambar Screenshot dan Foto Asli (Opsional):** Di dalam dokumen telah disediakan penanda `[PLACEHOLDER SCREENSHOT: ...]`. Jika Anda ingin menempelkan screenshot tangkapan layar asli dari Unity Editor atau foto sesi playtest, cukup buka file `.docx` terkait di Microsoft Word dan insert picture di lokasi placeholder tersebut.
4. **Video Reels Instagram (Tugas 13):** Rekam layar game Unity selama 1-2 menit (rasio 9:16 vertikal). Bacakan naskah voice-over dari `TUGAS_13_VIDEO_SCRIPT_REELS.md`. Upload ke Instagram, tag akun Dosen, dan salin link postingan ke file pengumpulan.

## 3. CARA MENGUBAH FILE `.docx` MENJADI `.pdf` SIAP UPLOAD

Ketentuan tugas kuliah mewajibkan pengumpulan dalam format PDF. Anda dapat mengubah seluruh file `.docx` menjadi `.pdf` dengan langkah mudah:

### Opsi A: Membuka di Microsoft Word / WPS Office (Sangat Disarankan)
1. Buka file `.docx` yang diinginkan di Microsoft Word.
2. Klik **File -> Save As** (atau **Export**).
3. Pilih format **PDF (*.pdf)**, lalu simpan.

### Opsi B: Mengkonversi Otomatis Lewat Command Prompt / Terminal
Jika pada laptop Anda terpasang Microsoft Word atau LibreOffice, Anda dapat menjalankan perintah Python berikut untuk mengubah seluruh file `.docx` di folder submissions menjadi PDF sekaligus:

```bash
python -c "import docx2pdf; docx2pdf.convert(r'c:\Users\galih\Documents\Projects\Game\My project\docs\submissions')"
```
