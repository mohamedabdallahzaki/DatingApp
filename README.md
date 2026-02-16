# 💘 DatingApp

A modern dating application built with **.NET 10 Web API** backend and an **Angular** frontend. The API provides user authentication, member profile management, and photo upload capabilities powered by **Cloudinary**.

---

## 🚀 Tech Stack

| Layer              | Technology                              |
| ------------------ | --------------------------------------- |
| **Runtime**        | .NET 10                                 |
| **Language**       | C#                                      |
| **Database**       | SQL Server                              |
| **ORM**            | Entity Framework Core 10.0              |
| **Authentication** | JWT Bearer Tokens (HMAC-SHA512)         |
| **Image Storage**  | Cloudinary                              |
| **API Docs**       | OpenAPI / Swagger                       |
| **Frontend**       | Angular (served on `localhost:4200`)     |

---

## ✨ Features

- **User Registration & Login** — Secure authentication with HMAC-SHA512 password hashing and JWT tokens
- **Member Profiles** — Full CRUD for member profiles with details like bio, city, country, gender, and date of birth
- **Photo Management** — Upload, set main photo, and delete photos via Cloudinary integration
- **Database Seeding** — Pre-populated seed data for development and testing
- **Global Exception Handling** — Custom middleware with environment-aware error responses
- **CORS Support** — Configurable allowed origins for cross-origin requests

---

## 🏗️ Architecture

The project follows a **clean layered architecture** with key design patterns:

```
┌──────────────────────────────────────────────┐
│                Controllers                    │
│     AccountController · MembersController     │
├──────────────────────────────────────────────┤
│             Services / Interfaces             │
│    IServiceToken · IPhotoService              │
│    IMemberRepository                          │
├──────────────────────────────────────────────┤
│              Data / Repository                │
│    DatingContext · MemberRepository            │
│    SeedData                                   │
├──────────────────────────────────────────────┤
│                 Entities                      │
│      AppUser · Member · Photo · DTOs          │
├──────────────────────────────────────────────┤
│               SQL Server                      │
└──────────────────────────────────────────────┘
```

**Patterns Used:**
- **Repository Pattern** — Data access abstracted via `IMemberRepository`
- **Service Layer** — Business logic in `ServiceToken` and `PhotoService`
- **DTO Pattern** — Separate data transfer objects for API contracts
- **Options Pattern** — Strongly-typed configuration (e.g., `CloudinarySettings`)
- **Middleware Pipeline** — Custom exception handling via `ExpectionsMiddleware`

---

## 📁 Project Structure

```
DatingApp/
├── DatingApp.sln
└── API/
    ├── Controllers/
    │   ├── BaseApiController.cs       # Base controller with [ApiController] and routing
    │   ├── AccountController.cs       # Register & Login endpoints
    │   └── MembersController.cs       # Member CRUD & photo management
    ├── Data/
    │   ├── DatingContext.cs           # EF Core DbContext
    │   ├── Configruation/             # EF Core entity configurations
    │   ├── Repository/
    │   │   └── MemberRepository.cs    # Data access implementation
    │   ├── SeedData.cs                # Database seeding logic
    │   └── UserSeedData.json          # Seed data JSON file
    ├── Entities/
    │   ├── AppUser.cs                 # User account entity
    │   ├── Member.cs                  # Member profile entity
    │   ├── Photo.cs                   # Photo entity
    │   └── DTO/
    │       ├── LoginDto.cs
    │       ├── RegisterDto.cs
    │       ├── UserDto.cs
    │       ├── MemberDto.cs
    │       ├── UpdateMebmerDto.cs
    │       └── UserSeedDto.cs
    ├── Exceptions/
    │   └── ApiException.cs            # Structured API error response
    ├── Extensions/
    │   └── UserDtoExtensions.cs       # Extension methods for mapping
    ├── Interface/
    │   ├── IMemberRepository.cs
    │   ├── IPhotoService.cs
    │   └── IServiceToken.cs
    ├── Middleware/
    │   └── ExpectionsMiddleware.cs     # Global exception handler
    ├── Services/
    │   ├── PhotoService.cs            # Cloudinary upload/delete
    │   └── ServiceToken.cs            # JWT token generation
    ├── Settings/
    │   └── CloudinarySettings.cs      # Cloudinary config model
    ├── Program.cs                     # App entry point & DI setup
    ├── appsettings.json               # App configuration
    └── appsettings.Development.json   # Dev-specific config
```

---

## 📡 API Endpoints

### Account (`/api/Account`)

| Method | Endpoint              | Description           | Auth |
| ------ | --------------------- | --------------------- | ---- |
| POST   | `/api/Account/Register` | Register a new user   | ❌    |
| POST   | `/api/Account/Login`    | Login & receive JWT   | ❌    |

### Members (`/api/Members`)

| Method | Endpoint                                | Description                     | Auth |
| ------ | --------------------------------------- | ------------------------------- | ---- |
| GET    | `/api/Members`                          | Get all members                 | ✅    |
| GET    | `/api/Members/{id}`                     | Get member by ID                | ✅    |
| GET    | `/api/Members/{id}/Photos`              | Get all photos for a member     | ✅    |
| PUT    | `/api/Members`                          | Update current member profile   | ✅    |
| POST   | `/api/Members/add-photo`                | Upload a photo                  | ✅    |
| POST   | `/api/Members/set-main-photo/{photoId}` | Set a photo as main profile pic | ✅    |
| DELETE | `/api/Members/delete-photo/{photoId}`   | Delete a photo                  | ✅    |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Node.js & Angular CLI](https://angular.io/cli) (for frontend)

### 1. Clone the Repository

```bash
git clone https://github.com/mohamedabdallahzaki/DatingApp.git
cd DatingApp
```

### 2. Configure User Secrets

Set your JWT token key using .NET User Secrets:

```bash
cd API
dotnet user-secrets set "TokenKey" "your-super-secret-key-at-least-64-characters-long-for-security"
```

### 3. Update Connection String

Edit `appsettings.json` if your SQL Server instance differs from the default:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DatingDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Run Database Migrations

The app automatically applies migrations and seeds data on startup. Alternatively, run manually:

```bash
dotnet ef database update
```

### 5. Run the API

```bash
dotnet run
```

The API will start on `https://localhost:5001` (or the configured port). Swagger UI is available in development mode at `/openapi/v1.json`.

### 6. Run the Frontend

```bash
cd client
npm install
ng serve
```

Navigate to `http://localhost:4200`.

---

## 🔐 Authentication Flow

```
┌─────────┐     POST /api/Account/Register     ┌─────────┐
│  Client  │ ──────────────────────────────────► │   API   │
│          │ ◄────────────────────────────────── │         │
│          │     { token, displayName, image }   │         │
│          │                                     │         │
│          │     POST /api/Account/Login         │         │
│          │ ──────────────────────────────────► │         │
│          │ ◄────────────────────────────────── │         │
│          │     { token, displayName, image }   │         │
│          │                                     │         │
│          │   GET /api/Members (Bearer token)   │         │
│          │ ──────────────────────────────────► │         │
│          │ ◄────────────────────────────────── │         │
│          │     [ member data ]                 │         │
└─────────┘                                     └─────────┘
```

1. User registers or logs in → receives a **JWT token**
2. Token is sent as `Authorization: Bearer <token>` header on subsequent requests
3. Protected endpoints validate the token via middleware

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
