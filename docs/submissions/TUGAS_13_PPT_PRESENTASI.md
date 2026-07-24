# OUTLINE PRESENTASI SLIDE (TUGAS 13)

**Judul Project:** Life on Land — Top-Down Tactical Eco-Restoration Simulator  
**Mata Kuliah:** Game Development (Sesi 13)  
**Total Slide:** 20 Slide (Format Presentasi Ujian Akhir Project)

### SLIDE 1: TITLE SLIDE
- **Judul:** LIFE ON LAND — Top-Down Tactical Eco-Restoration Simulator
- **Sub-Judul:** Laporan Akhir Project Game Development
- **Tim Pengembang:** Kelompok Game Development (Galih Adhi Kusuma, Berkat Perdana Saragih, Oscar Adi Dharma, Firschanya Alula R.)
- **Dosen Pengampu:** Dosen Pengampu Mata Kuliah Game Development
- **Visual:** Background animasi logo game Life on Land dan sprite Umbra.

### SLIDE 2: EXECUTIVE SUMMARY DAN HIGHLIGHTS
- Progress Project: 100% Final Build Selesai.
- Engine & Tech Stack: Unity 6 C#, PlayFab Cloud Backend, 2D Orthographic Pixel Art.
- Hasil Testing: SUS Alpha Score 63.45 (Grade D / OK), UAT Beta Score 82.4% (Sangat Layak).

### SLIDE 3: LATAR BELAKANG DAN PERMASALAHAN
- Krisis Degradasi Ekosistem: Bumi masa depan mengalami penyusutan vegetasi hingga kadar O2 jatuh ke 15.0%.
- Masalah Media Edukasi: Kurangnya game simulasi taktikal yang menggambarkan hubungan sebab-akibat antara retensi air tanah, penanaman pohon, dan kualitas udara.

### SLIDE 4: SOLUSI DAN DESAIN UTAMA GAME
- Solusi: Game Life on Land mengombinasikan cozy farming mechanics dengan ketegangan quest kejaran antagonis.
- 3 Bioma Utama: Red Region (Arid Oasis), Orange Region (Scorched Grove), Pink Bloom (Boss Stage).

### SLIDE 5: METODOLOGI PENGEMBANGAN (GDLC)
- Enam Tahapan GDLC: Concept -> Pre-Production -> Production -> Testing -> Release -> Post-Production.

### SLIDE 6: KARAKTER DAN ALUR CERITA
- Umbra (Restorer / Sloth Monk): Protagonis penyabar dan disiplin.
- Guardians: Maliz (Bear Wrath Barbarian), Oryel (Fox Pride Rogue), Pyper (Moth Lust Bard).
- Antagonis: The Villain (Pembakar vegetasi bioma).

### SLIDE 7: MEKANIK UTAMA GAME (HOTBAR DAN PURIFIKASI)
- Hotbar 6 Slot: 1: Sekop, 2: Gembor Air, 3: Semak Gurun, 4: Pohon Pinus, 5: Paku Ngengat, 6: Blueprint.
- Two-Step Purification: Burnt Tile -> Sekop (Dug Soil) -> Penyiraman Air -> Normal Clean Soil.

### SLIDE 8: DYNAMIC SOIL MOISTURE DAN PLANT FSM
- Matriks Kelembapan Tanah: Moisture decay over time, dipercepat oleh Heatwave.
- State Machine Tanaman: Seed -> Sprout -> Mature -> Withered -> Revive via watering.

### SLIDE 9: SISTEM INFRASTRUKTUR DAN OTOMASI
- Soil Purifier: Pemurnian tanah otomatis skala area.
- Irrigation Pipes: Penyiraman air otomatis untuk menangani gelombang panas Stage 3.
- Biosphere Dome: Struktur penutup utama untuk mengurung Villain dan mengembalikan O2 ke 21.0%.

### SLIDE 10: ARSITEKTUR TEKNIS (UNITY DAN PLAYFAB)
- Presentation Layer: Unity UI & Camera Orthographic 2D.
- Logic Layer: C# Controllers (Player.cs, EnvironmentManager.cs, Tree.cs).
- Persistence Layer: Local SaveData.json & PlayFab Cloud Data API.

### SLIDE 11: DEMO GAMEPLAY STAGE 1 (RED REGION)
- Goal: Bantu Maliz the Bear, ambil 10 unit air, tanam 5 Desert Shrubs, capai O2 50.0%.
- Visual Placeholder: `[PLACEHOLDER SCREENSHOT: Stage 1 Gameplay]`

### SLIDE 12: DEMO GAMEPLAY STAGE 2 (ORANGE REGION)
- Goal: Buktikan skill ke Oryel the Fox, bangun 1 Soil Purifier, tanam 8 Pine Trees, capai O2 21.0%.
- Visual Placeholder: `[PLACEHOLDER SCREENSHOT: Stage 2 Gameplay]`

### SLIDE 13: DEMO GAMEPLAY STAGE 3 (PINK BLOOM DAN BOSS)
- Goal: Tahan godaan Pyper, atasi Heatwave dengan Pipa Irigasi, bangun Biosphere Dome, dan tangkap Villain.
- Visual Placeholder: `[PLACEHOLDER SCREENSHOT: Stage 3 Gameplay]`

### SLIDE 14: METODE DAN DEMOGRAFI PENGUJIAN
- Responden Testing: 21 Orang (52.4% Laki-laki, 47.6% Perempuan, 66.7% Mahasiswa).
- Instrumen: System Usability Scale (SUS 1-4) & User Acceptance Testing (UAT 1-5).

### SLIDE 15: HASIL PENGUJIAN ALPHA (SUS TESTING)
- Rata-rata Skor SUS: **63.45**.
- Evaluasi Benchmarking: Acceptability Range: Marginal High, Grade Scale: Grade D, Adjective Rating: OK.

### SLIDE 16: HASIL PENGUJIAN BETA (UAT TESTING)
- Skor Keberhasilan UAT: **4.12 / 5.00 (82.4%)**.
- Kategori: **Sangat Layak / Sangat Berhasil**.
- Rincian Aspek: UI (84.8%), Navigation (83.8%), Functionality (81.0%), Speed (80.0%), Suitability (82.8%).

### SLIDE 17: UAT TESTIMONIAL DAN WAWANCARA NARASUMBER
- Cuplikan masukan 5 narasumber (Ananda, Dominggus, Fahri, Fahmi, Rizky).
- Visual Placeholder: `[PLACEHOLDER FOTO DOKUMENTASI WAWANCARA]`

### SLIDE 18: KONTRIBUSI TIM DAN LOG AKTIVITAS
- Pembagian tugas 4 anggota kelompok (Programming, Systems, Art & UI, Testing & Documentation).

### SLIDE 19: KESIMPULAN
- Game Life on Land berhasil dibangun 100% sesuai spesifikasi, memberikan pengalaman simulasi restorasi lingkungan yang interaktif dan mendidik.

### SLIDE 20: SARAN DAN CLOSING
- Plans for Future: Versi Mobile Touchscreen, variasi bioma laut & tanaman endemik baru.
- Q&A & Terima Kasih!
