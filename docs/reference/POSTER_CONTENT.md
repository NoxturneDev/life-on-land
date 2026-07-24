TITLE
Rekomendasi Karyawan Teladan Berdasarkan KPI dengan Neural Collaborative Filtering Berbasis Mobile

AUTHORS
Berkat Perdana Saragih 1, Oscar Adi Dharma 2, Galih Adhi Kusuma 3, Firschanya Alula Rietmadha 4

AFFILIATIONS
Department of Informatics Engineering, Esa Unggul University 1,2,3,4

ABSTRAK
Penelitian ini mengembangkan aplikasi mobile berbasis AI untuk prediksi dan rekomendasi karyawan teladan menggunakan metode Neural Collaborative Filtering (NCF). Model NCF memprediksi rating performa karyawan (Low, Medium, High) dengan akurasi 99.87%, serta menghasilkan skor regresi 0-100 dengan RMSE 0.87. Evaluasi usability (SUS) menunjukkan skor 84 (kategori sangat baik/Grade A). Hasilnya menunjukkan sistem efektif dalam mendukung transparansi dan evaluasi KPI karyawan di lingkungan kerja.

ARSITEKTUR SISTEM
[PLACEHOLDER: Arsitektur Sistem Diagram]

Arsitektur aplikasi terdiri dari tiga layer utama. Presentation Layer menggunakan Flutter dengan BLoC state management untuk UI responsif. Business Logic Layer mengintegrasikan Repository pattern dengan Flask REST API yang menjalankan model NCF. Data Layer menggunakan Firebase Authentication untuk autentikasi dan Cloud Firestore untuk penyimpanan data. Model NCF menerima 24 fitur KPI sebagai input, diproses melalui shared dense layers (128-64-32), lalu menghasilkan dua output: klasifikasi performa (softmax, 3 kelas) dan skor regresi (linear, 0-100).

LATAR BELAKANG
Evaluasi kinerja karyawan masih dilakukan secara manual dan subjektif
Target KPI tidak terukur secara real-time
Proses HR lambat dan rentan terhadap bias penilaian
Karyawan tidak mengetahui capaian performa secara transparan

TUJUAN PENELITIAN
Membangun aplikasi mobile untuk tracking dan evaluasi KPI transparan
Mengintegrasikan model NCF untuk prediksi dan rekomendasi karyawan teladan
Menyediakan dashboard terpisah untuk HRD dan Employee
Membangun pipeline machine learning end-to-end

METODE PENELITIAN
Dataset : 5.000 record, 24 fitur KPI
Responden SUS : 30 orang
Metodologi : Waterfall
Framework : Flutter 3.11.5 + Python/Flask + TensorFlow

MODEL
NCF - Dual Output Head (Klasifikasi + Regresi)

EVALUASI
Confusion Matrix
Accuracy, Precision, Recall, F1-Score
MSE, RMSE, MAE
System Usability Scale (SUS)
User Acceptance Testing (UAT)

HASIL APLIKASI PER INDIVIDU (EMPLOYEE)
[PLACEHOLDER: Screenshot Employee Dashboard - Performance Hub dengan circular gauge]

[PLACEHOLDER: Screenshot NCF Insights - Strengths/Weaknesses bar chart]

Keterangan: Employee Dashboard menampilkan circular gauge probabilitas kinerja, tren performa 6 bulan, dan NCF Insights berupa analisis strengths/weaknesses beserta action steps.

HASIL APLIKASI PER GROUP (HRD)
[PLACEHOLDER: Screenshot HRD Dashboard - Executive Summary]

[PLACEHOLDER: Screenshot Leaderboard dengan status badges]

Keterangan: HRD Dashboard menampilkan executive summary dengan stat cards, department performance chart, dan leaderboard ranking karyawan berdasarkan skor prediksi NCF.

EVALUASI SYSTEM USABILITY SCALE
[PLACEHOLDER: Grafik/Skala SUS]

SUS rata-rata 84, yang menunjukkan tingkat kegunaan yang tinggi dan penerimaan pengguna yang kuat. Menurut standar interpretasi SUS, skor di atas 80 dikategorikan sebagai kegunaan "Sangat Baik" dan termasuk dalam Grade A.

HASIL APLIKASI DATASET MANAGEMENT
[PLACEHOLDER: Screenshot Dataset Management - Upload & Pipeline]

[PLACEHOLDER: Screenshot Employee Analysis - Radar Chart]

Keterangan: Modul Dataset Management memungkinkan HRD upload CSV dengan pipeline AI 4 tahap. Employee Analysis menampilkan radar chart 5 dimensi KPI dan dapat diekspor ke PDF.

EVALUASI MODEL NCF
[PLACEHOLDER: Grafik Training/Validation Loss]

[PLACEHOLDER: Confusion Matrix Heatmap]

Klasifikasi:
Accuracy: 99.87% (749/750 benar)
Macro Precision: 0.9978
Macro Recall: 0.9991
Macro F1: 0.9985

Regresi:
MSE: 0.7517
RMSE: 0.8670
MAE: 0.6212

KESIMPULAN
Model NCF berhasil diintegrasikan ke mobile app dengan akurasi 99.87% (1 kesalahan dari 750 sampel test).
Pipeline ML end-to-end berjalan optimal dari preprocessing hingga inferensi.
Aplikasi menyediakan evaluasi KPI transparan dan objektif untuk HRD dan karyawan.
Evaluasi SUS skor 84 (Grade A) menunjukkan usability sangat baik.

SARAN
Perluasan Dataset: Menggunakan data karyawan real dari sistem HRIS
Integrasi Data Real-Time: Menghubungkan dengan database HR existensi
Advanced Analytics: Cohort analysis, trend forecasting, predictive analytics
Model Optimization: Hyperparameter tuning dan experiment tracking
Offline Mode: Kemampuan akses data tanpa koneksi internet

KEYWORDS
Neural Collaborative Filtering | KPI | Employee Performance | Flutter | Firebase | Machine Learning | Mobile Application
