# traqtiv 🏃
### TRACK. ACTIV. FITNESS.
 
> A fitness & lifestyle management system for independent athletes - Final Project | Object-Oriented Programming Workshop 20586 | The Open University of Israel
 
---
 
## 📖 Overview
 
**traqtiv** is a modern, multi-platform fitness and lifestyle management system designed for independent athletes who want to track, manage, and analyze their physical activity in one place.
 
The system combines a cross-platform mobile app, a web analytics dashboard, and a central backend server - all communicating through a secure RESTful API.
 
---
 
## ✨ Features
 
- **User Management** - Registration, login, and JWT-based authentication
- **Workout Tracking** - Add, edit, delete, and browse workout history
- **Daily Activity Monitoring** - Steps, calories, active minutes, and distance (with automatic sync from HealthKit / Health Connect)
- **Body Metrics** - Track weight, resting heart rate, and BMI over time
- **Smart Recommendations & Alerts** - Rule-based engine detecting overload, inactivity, and more
- **Weather & Air Quality Integration** - Outdoor activity warnings based on real-time environmental conditions
- **Web Dashboard** - Detailed charts and statistics for long-term progress analysis
---
 
## 🏗️ System Architecture
 
```
┌─────────────────┐     ┌─────────────────┐
│  Mobile App     │     │   Web Dashboard │
│  (.NET MAUI)    │     │ (React + TS)    │
└────────┬────────┘     └────────┬────────┘
         │                       │
         └───────────┬───────────┘
                     │ RESTful API (HTTPS + JWT)
              ┌──────▼──────┐
              │   Backend   │
              │ ASP.NET Core│
              └──────┬──────┘
               ┌─────┴──────┐
       ┌───────▼───┐  ┌─────▼──────────┐
       │PostgreSQL │  │Weather/Air API │
       │    DB     │  │  (External)    │
       └───────────┘  └────────────────┘
```
 
---
 
## 🛠️ Tech Stack
 
| Layer | Technology |
|-------|-----------|
| Mobile Frontend | .NET MAUI (iOS / Android), MVVM Pattern |
| Web Frontend | React + TypeScript, React Query, Recharts, Axios |
| Backend | ASP.NET Core Web API |
| Authentication | JWT (JSON Web Tokens) |
| ORM | Entity Framework Core (Code First) |
| Database | PostgreSQL (`SmartFitnessDb`) |
| Code Generation | NSwag (auto-generates typed client from Swagger) |
 
---
 
## 📁 Project Structure
 
```
traqtiv/
├── Traqtiv.Backend/               # ASP.NET Core Web API
│   ├── Controllers/               # AuthController, UserController, WorkoutController...
│   ├── Services/                  # Business logic (IAuthService, IWorkoutService...)
│   ├── Dal/                       # ISmartFitnessDal, SmartFitnessDal, SmartFitnessDb
│   ├── Models/                    # Entities: User, Workout, DailyActivity, Alert...
│   ├── DTOs/                      # Request/Response DTOs
│   ├── Helpers/                   # JwtHelper, PasswordHelper, RecommendationEngine...
│   └── Enums/                     # WorkoutType, AlertSeverity, RecommendationType...
│
├── Traqtiv.Mobile/                # .NET MAUI App
│   ├── Pages/                     # UI Screens (LoginPage, HomePage, WorkoutsPage...)
│   ├── ViewModels/                # MVVM ViewModels (LoginVm, HomeVm, WorkoutsVm...)
│   ├── Services/                  # ApiClient, AuthService, WorkoutService, HealthService...
│   ├── Helpers/                   # SecureStorageHelper, AlertHelper, ConnectivityHelper
│   └── Models/                    # Auto-generated via NSwag (SmartFitnessClient)
│
└── Traqtiv.Web/                   # React + TypeScript Dashboard
    └── src/
        ├── pages/                 # LoginPage, DashboardPage, WorkoutsPage...
        ├── components/            # Navbar, Sidebar, StatCard, Charts, AlertCard...
        ├── services/              # apiClient (Axios), authService, workoutService...
        ├── hooks/                 # useAuth, useWorkouts, useDailyActivity...
        ├── context/               # AuthContext
        ├── types/                 # TypeScript Interfaces (WorkoutDto, AlertDto...)
        └── utils/                 # dateUtils, chartUtils, validationUtils
```
 
---
 
## 🗄️ Data Model
 
The system manages the following tables in PostgreSQL:
 
| Table | Description |
|-------|-------------|
| `User` | Registered user details |
| `Workout` | Workouts (type, duration, status, calories) |
| `BodyMetrics` | Body measurements (weight, heart rate, BMI) |
| `DailyActivity` | Daily activity data (steps, calories, distance) |
| `Recommendation` | User-tailored recommendations |
| `Alert` | Alerts with severity levels (Low / Medium / High) |
 
---
 
## 🎨 Design Patterns
 
| Pattern | Where It's Applied |
|---------|-------------------|
| **Singleton** | All Services in backend and mobile; `SmartFitnessDb` |
| **Strategy** | `ISmartFitnessDal`, `IService`, all Service interfaces |
| **Facade** | `SmartFitnessDal`, `BaseController`, `ApiClient` (mobile & web) |
| **Template Method** | `BaseEntity`, `BaseController`, `BaseService`, `BaseViewModel` |
| **Observer** | `RecommendationService` listens to `WorkoutService`, `DailyActivityService`, `WeatherService` |
| **Proxy** | `SmartFitnessClient` (auto-generated by NSwag) |
 
---
 
## 🚀 Getting Started
 
### Prerequisites
 
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 15+](https://www.postgresql.org/)
### Backend
 
```bash
cd Traqtiv.Backend
 
# Update the connection string in appsettings.json
# "ConnectionStrings": { "DefaultConnection": "Host=localhost;Database=SmartFitnessDb;..." }
 
dotnet restore
dotnet ef database update       # Creates the database and applies migrations
dotnet run
```
 
> The server runs on `https://localhost:5001`. Swagger UI is available at `/swagger`.
 
### Web Dashboard
 
```bash
cd Traqtiv.Web
npm install
npm run dev
```
 
> Dashboard runs on `http://localhost:5173`
 
### Mobile App
 
```bash
cd Traqtiv.Mobile
 
# Update the server address in Helpers/AppConstants.cs
 
dotnet build -t:Run -f net8.0-android   # Android
dotnet build -t:Run -f net8.0-ios       # iOS
```
 
> **Note:** In the current MVP, installation is done via a dedicated APK/IPA file, not through an app store.
 
### Regenerating the NSwag Client (after API changes)
 
```bash
cd Traqtiv.Mobile
dotnet nswag run nswag.json
```
 
---
 
## ⚙️ Environment Variables
 
Create `appsettings.Development.json` in the Backend directory:
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SmartFitnessDb;Username=YOUR_USER;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Secret": "YOUR_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "traqtiv",
    "Audience": "traqtiv-users"
  },
  "WeatherApi": {
    "BaseUrl": "https://YOUR_WEATHER_API_URL",
    "AirQualityUrl": "https://YOUR_AIR_QUALITY_API_URL"
  }
}
```
 
---
 
## 📱 Mobile Screens
 
| Screen | Description |
|--------|-------------|
| **Splash Screen** | Initial loading screen |
| **Login / Register** | Authentication and new user registration |
| **Home** | Main dashboard — daily summary, recent workouts, active alerts |
| **Workouts** | Workout list with add / edit / delete |
| **Daily Activity** | Steps, calories, active minutes tracking |
| **Body Metrics** | Body measurement history |
| **Recommendations** | Personalized recommendations and alerts by severity |
| **Profile** | Personal details and logout |
 
---
 
## ⚠️ Current MVP Limitations
 
- No offline support — requires a continuous internet connection
- Recommendations engine uses static rules only (no AI/ML)
- No admin panel
- No admin or trainer user roles — end user is the independent athlete only
- The system does not replace professional or medical advice
---
 
## 🔭 Planned Future Enhancements
 
- [ ] AI/ML-based recommendations engine
- [ ] Personal trainer data sharing
- [ ] Smart fitness equipment integration (Bluetooth)
- [ ] Gamification — achievements, points, and leaderboards
- [ ] Offline mode with automatic sync
- [ ] Push notifications for important alerts
- [ ] Pre-built workout plans
- [ ] Dark mode
- [ ] Multi-language support
---
 
## 📋 Coding Conventions
 
| Type | Convention | Example |
|------|-----------|---------|
| Classes, public methods & properties | PascalCase | `GetUserById` |
| Local variables & parameters | camelCase | `workoutData` |
| Private fields | camelCase with `_` prefix | `_userService` |
| Interfaces | Prefixed with `I` | `IWorkoutService` |
| Constants | UPPER_SNAKE_CASE | `API_BASE_URL` |
| Mobile pages | Suffixed with `Page` | `WorkoutsPage` |
| ViewModels | Suffixed with `Vm` | `WorkoutsVm` |
 
---
 
## 👤 Author
 
**Gil Moshe**  
Object-Oriented Programming Workshop - 20586  
The Open University of Israel
 
---
 
> *"traqtiv - because tracking your progress is the first step toward reaching your goal."*
