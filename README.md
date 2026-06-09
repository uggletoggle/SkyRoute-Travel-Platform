# ✈️ SkyRouteTravel

A full-stack flight search and booking platform built as a **monorepo**:

| Layer | Technology | Location |
|---|---|---|
| **API** | ASP.NET Core 9 · Minimal APIs · SQLite | `SkyRouteTravel.Api/` |
| **Application** | Clean Architecture · FluentValidation · AutoMapper | `SkyRouteTravel.Application/` |
| **Infrastructure** | Entity Framework Core · Repository pattern | `SkyRouteTravel.Infrastructure/` |
| **Frontend** | Angular 21 · Tailwind CSS 4 · TypeScript | `SkyRouteTravelUI/` *(submodule)* |
| **Tests** | xUnit · Moq | `SkyRouteTravel.Tests/` |

---

## 📦 Repository Structure

```
SkyRouteTravel/                    ← Monorepo root
├── SkyRouteTravel.slnx            ← .NET solution file
├── SkyRouteTravel.Api/            ← ASP.NET Core Minimal API
├── SkyRouteTravel.Application/    ← Domain logic & use cases
├── SkyRouteTravel.Infrastructure/ ← EF Core, repositories, DB
├── SkyRouteTravel.Tests/          ← Unit tests (xUnit + Moq)
├── SkyRouteTravelUI/              ← Angular frontend (git submodule)
└── .gitmodules                    ← Submodule configuration
```

---

## 🔧 Prerequisites

| Tool | Minimum Version | Install |
|---|---|---|
| .NET SDK | 9.0 | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| Node.js | 20 LTS | [nodejs.org](https://nodejs.org) |
| npm | 11+ | Bundled with Node.js |
| Angular CLI | 21 | `npm install -g @angular/cli` |
| Git | any | [git-scm.com](https://git-scm.com) |

---

## 🚀 Getting Started

### 1. Clone the monorepo (with submodules)

```bash
git clone --recurse-submodules <repository-url>
cd SkyRouteTravel
```

> If you already cloned without `--recurse-submodules`, initialize the submodule manually:
> ```bash
> git submodule update --init --recursive
> ```

---

## 🖥️ Backend (ASP.NET Core API)

### Run in Development

```bash
# From the monorepo root
cd SkyRouteTravel.Api
dotnet run
```

The API starts at:
- **HTTP**: `http://localhost:5218`
- **HTTPS**: `https://localhost:7228`

> On first run the SQLite database is **automatically created and seeded** with sample flights, airports, and providers.

### Swagger UI

Open your browser at:

```
http://localhost:5218/swagger
```

All endpoints are documented and testable from the Swagger UI.

### Available Endpoints

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/flights/search` | Search flights by origin, destination & date |
| `GET` | `/api/flights/{id}` | Get a flight by ID |
| `GET` | `/api/airports` | List all airports |
| `GET` | `/api/providers` | List all flight providers |
| `POST` | `/api/bookings` | Book a flight |
| `GET` | `/api/bookings/{id}` | Get a booking by ID |

### Run with Docker

```bash
# From the monorepo root
docker build -f SkyRouteTravel.Api/Dockerfile -t skyroutetravel-api .
docker run -p 8080:8080 -p 8081:8081 skyroutetravel-api
```

---

## 🌐 Frontend (Angular)

### Install Dependencies

```bash
cd SkyRouteTravelUI
npm install
```

### Run the Dev Server

```bash
npm start
```

The UI will be available at: **`http://localhost:4200`**

> The Angular app is pre-configured to call the API at `http://localhost:5218`. Make sure the backend is running before using the UI.

### Build for Production

```bash
npm run build
```

Output is placed in `SkyRouteTravelUI/dist/`.

---

## 🧪 Running Tests

### Backend Unit Tests

```bash
# From the monorepo root
dotnet test
```

Runs all xUnit tests in `SkyRouteTravel.Tests/` covering:
- Flight search service
- Booking service & validators
- Provider strategy pattern (BudgetWings, GlobalAir)

### Frontend Tests

```bash
cd SkyRouteTravelUI
npm test
```

Runs unit tests with Vitest.

---

## ⚙️ Configuration

### Backend (`SkyRouteTravel.Api/appsettings.json`)

| Key | Default | Description |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `Data Source=SkyRouteTravel.db` | SQLite connection string |
| `AllowedHosts` | `*` | CORS-allowed hosts |

The API uses **SQLite** by default — no database server required. To switch to SQL Server or PostgreSQL, update the `DbContext` registration in `SkyRouteTravel.Infrastructure/DependencyInjection.cs`.

---

## 🔀 Working with the Submodule (SkyRouteTravelUI)

The Angular frontend lives in a **separate git repository** linked here as a submodule.

### Pull latest submodule changes

```bash
git submodule update --remote --merge
```

### Update the submodule remote URL

If the Angular repo is hosted separately (e.g. on GitHub), update `.gitmodules`:

```bash
git submodule set-url SkyRouteTravelUI https://github.com/<your-org>/SkyRouteTravelUI.git
git submodule sync
```

Then commit the updated `.gitmodules` file.

---

## 🏗️ Architecture Overview

```
┌──────────────────────────────────┐
│         Angular Frontend          │  http://localhost:4200
│         (SkyRouteTravelUI)        │
└───────────────┬──────────────────┘
                │ HTTP / REST
┌───────────────▼──────────────────┐
│     ASP.NET Core Minimal API      │  http://localhost:5218
│       (SkyRouteTravel.Api)        │
└───────────────┬──────────────────┘
                │
┌───────────────▼──────────────────┐
│     Application Layer             │
│  Services · Validators · DTOs    │
└───────────────┬──────────────────┘
                │
┌───────────────▼──────────────────┐
│     Infrastructure Layer          │
│  EF Core · SQLite · Repositories │
└──────────────────────────────────┘
```

---

## 📄 License

This project is for educational / portfolio purposes.
