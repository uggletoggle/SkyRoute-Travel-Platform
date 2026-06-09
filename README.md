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

## 🏛️ Architecture Decisions, Trade-offs & Known Limitations

### 1. Clean Architecture (layered backend)

**Decision:** The backend is split into four projects — `Api`, `Application`, `Infrastructure`, and `Presentation` — following Clean Architecture principles. Dependency flow is strictly inward: only `Infrastructure` knows about EF Core; only `Application` knows about domain rules.

**Why:** Keeps business logic testable in isolation (no database required for unit tests), makes it straightforward to swap the data store, and prevents accidental coupling between the HTTP layer and persistence code.

**Trade-off:** More boilerplate than a single-project API — each new feature requires a service interface, a repository interface, a DTO, and an AutoMapper profile. For a project of this size, that overhead is real.

---

### 2. ASP.NET Core Minimal APIs (not MVC Controllers)

**Decision:** All HTTP endpoints are registered as Minimal API route handlers in `Program.cs` / endpoint extension methods, rather than deriving from `ControllerBase`.

**Why:** Less ceremony, faster startup, and first-class support for OpenAPI generation in .NET 9. Endpoint parameters and return types are explicit.

**Trade-off:** Larger endpoint groups can become verbose inside a single file. Complex filter pipelines (e.g., action filters, result filters) are less ergonomic than in controller-based MVC.

---

### 3. SQLite as the default database

**Decision:** EF Core is configured against a local SQLite file (`SkyRouteTravel.db`) that is created and seeded automatically on first run.

**Why:** Zero infrastructure dependency — anyone can clone and run without installing a database server. Ideal for a portfolio / demo project.

**Trade-off / Known limitation:** SQLite does not support every SQL Server or PostgreSQL feature (e.g., concurrent writes under high load, certain migration paths). To move to a production-grade DB, update the `AddDbContext` registration in `SkyRouteTravel.Infrastructure/DependencyInjection.cs` and re-run `dotnet ef migrations add`.

---

### 4. Strategy Pattern for flight providers

**Decision:** Each flight provider (GlobalAir, BudgetWings, Default) implements `IFlightProviderStrategy`, selected at runtime by `FlightProviderStrategyFactory` based on the provider name stored in the database.

**Why:** Adding a new provider requires only a new class that implements the interface — the factory and the rest of the application are untouched. This was the most explicit way to unit-test provider-specific pricing logic in isolation (see `SkyRouteTravel.Tests`).

**Trade-off:** Strategies are currently instantiated directly inside the factory (`new GlobalAirStrategy()`), bypassing the DI container. This means strategies cannot themselves have injected dependencies. If providers ever need to call external APIs, the factory will need to become DI-aware (accept an `IServiceProvider` or a keyed service map).

---

### 5. Angular signals for reactive state (no NgRx / RxJS BehaviorSubject)

**Decision:** The `ApiHealthService` exposes a `signal<ApiStatus>` rather than an Observable, and components consume it directly via the signal API (`status()`).

**Why:** Angular signals (stable since v17) offer fine-grained reactivity with less boilerplate than a full NgRx store or manual `BehaviorSubject` management. For a UI of this size, a global store would be overkill.

**Trade-off:** Signals and RxJS Observables have different mental models; interop (`toSignal`, `toObservable`) adds a layer of complexity if the project ever mixes both patterns extensively. The HTTP client (`HttpClient`) still returns Observables, so some conversion boundary always exists.

---

### 6. Frontend as a git submodule

**Decision:** `SkyRouteTravelUI/` is a separate git repository linked to the monorepo root via `.gitmodules`.

**Why:** Keeps frontend and backend histories independent so either can be versioned, branched, and deployed separately. CI/CD pipelines for each can run in isolation.

**Trade-off / Known limitation:** Submodules add friction for new contributors who forget `--recurse-submodules` on clone or `git submodule update --remote` to pull the latest frontend. The submodule currently points to a local path (`./SkyRouteTravelUI`) — update `.gitmodules` to an actual remote URL before collaborating with others or hosting on GitHub.

---

### 7. Open CORS policy in development

**Decision:** The API allows requests from any origin (`builder.Services.AddCors` with a wildcard origin policy) to remove friction during local development.

**Why:** Lets the Angular dev server at `localhost:4200` call the API at `localhost:5218` without per-developer configuration.

**Known limitation:** This policy must be tightened before any public deployment. Replace the wildcard with an explicit allowlist of trusted origins and restrict allowed methods/headers appropriately.

---

### 8. API health-check via a reused endpoint (no dedicated `/health` route)

**Decision:** `ApiHealthService` probes the API by calling `GET /api/providers` on startup rather than a dedicated `/health` or `/ping` endpoint.

**Why:** Avoids adding a new endpoint while still validating that the API and database are reachable end-to-end (the `/providers` route hits EF Core and returns data).

**Trade-off:** The probe response time includes a real DB query. A dedicated lightweight `/health` endpoint with no DB dependency would be faster and more conventional (e.g., for Kubernetes liveness probes). The health check also runs only once at Angular app startup; it does not poll or reconnect automatically if the API goes offline mid-session.

---

## 📄 License

This project is for educational / portfolio purposes.
