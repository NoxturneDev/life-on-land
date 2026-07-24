# Talent Achieve — Installation & Execution Guide

## Prerequisites

| Software | Version | Purpose |
|----------|---------|---------|
| Python | 3.10+ | Backend ML server |
| Flutter SDK | ^3.11.5 | Mobile app development |
| Android Studio / VS Code | Latest | IDE |
| Git | Latest | Version control |
| Firebase Account | Free tier | Backend services |
| Node.js | 18+ (optional) | For some Flutter tools |

---

## Part 1: Clone the Repository

```bash
git clone <repository-url>
cd project-mobile
```

---

## Part 2: Backend Setup (Python/Flask + ML)

### 2.1 Install Python Dependencies

```bash
cd backend
pip install -r requirements.txt
```

The `requirements.txt` includes:
- Flask >= 3.0.0
- flask-cors
- TensorFlow >= 2.16.1
- scikit-learn
- pandas
- numpy
- joblib
- matplotlib
- seaborn

### 2.2 Generate Dataset

```bash
python dataset/generate_dataset.py
```

This generates `dataset/employee_kpi_data.csv` with 5,000 synthetic employee records (24 KPI features + 1 target).

### 2.3 Run the ML Pipeline

Start the Flask server first:

```bash
python -m app
```

The server starts at `http://0.0.0.0:5000`.

Then, in a **separate terminal**, run each pipeline step:

**Step 1 — Preprocessing:**
```bash
curl -X POST http://localhost:5000/api/preprocessing/run
```

Response:
```json
{
  "status": "success",
  "train_size": 3500,
  "val_size": 750,
  "test_size": 750,
  "num_features": 24
}
```

**Step 2 — Training:**
```bash
curl -X POST http://localhost:5000/api/model/train
```

This trains the NCF model for up to 100 epochs with early stopping. Takes ~1-2 minutes.

Response:
```json
{
  "status": "success",
  "epochs_run": 67,
  "final_val_accuracy": 1.0
}
```

**Step 3 — Evaluation:**
```bash
curl http://localhost:5000/api/evaluate
```

Returns classification metrics (accuracy, precision, recall, F1) and regression metrics (MSE, RMSE, MAE).

**Step 4 — Test Prediction:**
```bash
curl -X POST http://localhost:5000/api/predict \
  -H "Content-Type: application/json" \
  -d '{
    "tasks_completed": 45,
    "average_task_quality": 8,
    "projects_led": 3,
    "client_satisfaction_score": 88,
    "hours_worked": 42,
    "deadline_met_score": 8,
    "innovation_score": 7,
    "efficiency_score": 9,
    "meetings_attended": 15,
    "collaboration_score": 8,
    "punctuality_score": 9,
    "training_hours_completed": 15,
    "work_engagement_score": 8,
    "peer_interaction_score": 9,
    "initiative_score": 8,
    "task_followup_score": 9,
    "speech_sentiment_score": 0.75,
    "speech_energy_level": 8,
    "speech_clarity_score": 8,
    "tone_consistency_score": 7,
    "speaking_speed": 130,
    "pause_frequency": 2,
    "pitch_variation": 8,
    "volume_stability_score": 8
  }'
```

Response:
```json
{
  "prediction": {
    "performance_rating": "High",
    "overall_score": 87.5,
    "probabilities": {"High": 0.95, "Low": 0.01, "Medium": 0.04}
  }
}
```

### 2.4 Alternative: Lightweight Evaluation (No TensorFlow)

If TensorFlow cannot be installed, use the lightweight evaluation script:

```bash
pip install numpy pandas scikit-learn h5py joblib
python evaluate_lightweight.py
```

This reads model weights directly from `saved_models/best_ncf_model.h5` using h5py and runs inference with pure numpy — no TensorFlow required.

---

## Part 3: Frontend Setup (Flutter)

### 3.1 Install Flutter

Follow the official guide: https://docs.flutter.dev/get-started/install

Verify installation:
```bash
flutter doctor
```

Ensure all checks pass (especially Android toolchain and connected device).

### 3.2 Install Dart Dependencies

```bash
cd ..
flutter pub get
```

### 3.3 Firebase Configuration

**Step 1 — Create Firebase Project:**
1. Go to https://console.firebase.google.com
2. Create a new project named `kpi-project-kelompok-2`
3. Enable the following services:
   - **Authentication** → Sign-in method → Email/Password → Enable
   - **Cloud Firestore** → Create database → Start in test mode

**Step 2 — Register Android App:**
1. In Firebase Console, click "Add app" → Android
2. Package name: `com.talentintel.talentintel_ai`
3. Download `google-services.json`
4. Place it in `android/app/google-services.json`

**Step 3 — Register iOS App (optional):**
1. In Firebase Console, click "Add app" → iOS
2. Bundle ID: `com.talentintel.talentintelAI`
3. Download `GoogleService-Info.plist`
4. Place it in `ios/Runner/GoogleService-Info.plist`

**Step 4 — Verify Firebase Config:**
The file `lib/firebase_options.dart` should contain your Firebase project configuration. If not generated, run:
```bash
dart pub global activate flutterfire_cli
flutterfire configure
```

### 3.4 Run the App

**On Android Emulator/Device:**
```bash
flutter run
```

**On Chrome (Web):**
```bash
flutter run -d chrome
```

**Build APK:**
```bash
flutter build apk --release
```

The APK will be at `build/app/outputs/flutter-apk/app-release.apk`.

---

## Part 4: First-Time Usage

### 4.1 Create HRD Account

1. Open the app
2. On the Login screen, the app requires existing accounts in Firebase
3. Go to Firebase Console → Authentication → Add user
4. Create: `hrd@talentachieve.com` / password: `password123`
5. Go to Firestore → `users` collection → Add document:
   ```
   UID: (copy from Auth user)
   email: hrd@talentachieve.com
   name: HRD Admin
   role: hrd
   department: Human Resources
   position: HR Manager
   ```

### 4.2 Create Employee Accounts

Repeat for employees:
1. Firebase Auth → Add user → `employee1@talentachieve.com`
2. Firestore `users` → Add document with `role: employee`

Or use the app's **HRD Dashboard → Create Employee** feature after logging in as HRD.

### 4.3 Run the Full Pipeline

1. Login as HRD
2. Go to **Dataset Management** tab
3. Upload `employee_kpi_data.csv` (or click the pipeline button)
4. Wait for the 4-step pipeline to complete
5. Go to **Leaderboard** to see predicted scores
6. Click any employee to see the **Employee Analysis** with radar chart

---

## Part 5: API Reference

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/health` | GET | Health check |
| `/api/dataset/info` | GET | Dataset info (records, columns, class distribution) |
| `/api/preprocessing/run` | POST | Run preprocessing pipeline |
| `/api/model/train` | POST | Train NCF model |
| `/api/evaluate` | GET | Evaluate model on test set |
| `/api/predict` | POST | Predict for a single employee |
| `/api/dataset/upload` | POST | Upload CSV and run predictions |

---

## Part 6: Troubleshooting

### TensorFlow Installation Issues

If `pip install tensorflow` fails or gives import errors:

**Option A — Use tensorflow-cpu (smaller):**
```bash
pip install tensorflow-cpu
```

**Option B — Use the lightweight evaluation:**
```bash
pip install numpy pandas scikit-learn h5py joblib
cd backend
python evaluate_lightweight.py
```

### Flutter Build Issues

```bash
flutter clean
flutter pub get
flutter run
```

### Firebase Connection Issues

- Ensure `google-services.json` is in `android/app/`
- Ensure Firebase Auth is enabled in Console
- Ensure Firestore rules allow read/write (test mode)

### Port 5000 Already in Use

```bash
# Find and kill the process
lsof -i :5000
kill <PID>

# Or use a different port
python -m app  # edit app.py to change port
```

---

## Part 7: File Structure Reference

```
project-mobile/
├── lib/                          # Flutter source code
│   ├── main.dart                 # Entry point
│   ├── injection.dart            # Dependency injection
│   ├── core/                     # Shared layer
│   │   ├── constants/            # Colors, strings
│   │   ├── theme/                # Light/dark themes
│   │   ├── widgets/              # Reusable widgets
│   │   └── router/               # GoRouter config
│   └── features/                 # Feature modules
│       ├── auth/                 # Authentication
│       ├── hrd_dashboard/        # HRD portal
│       ├── employee_dashboard/   # Employee portal
│       ├── employee_analysis/    # Analysis (HRD view)
│       ├── ncf_insights/         # AI insights
│       ├── dataset_management/   # Dataset & pipeline
│       └── profile/              # Profile management
├── backend/                      # Python ML backend
│   ├── app.py                    # Flask API server
│   ├── requirements.txt          # Python dependencies
│   ├── model/                    # NCF model
│   │   ├── ncf_model.py          # Architecture
│   │   └── trainer.py            # Training logic
│   ├── preprocessing/            # Data preprocessing
│   │   └── preprocessor.py
│   ├── evaluation/               # Model evaluation
│   │   └── evaluator.py
│   ├── dataset/                  # Dataset generation
│   │   ├── generate_dataset.py
│   │   └── employee_kpi_data.csv
│   ├── saved_models/             # Trained model files
│   │   ├── best_ncf_model.h5
│   │   ├── scaler.pkl
│   │   └── label_encoder.pkl
│   └── evaluate_lightweight.py   # Lightweight eval script
├── android/                      # Android platform
├── ios/                          # iOS platform
├── web/                          # Web platform
└── pubspec.yaml                  # Dart dependencies
```
