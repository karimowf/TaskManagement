# Task Management App — ASP.NET Core MVC

A simple yet well-structured Task Management application built with **ASP.NET Core 8 MVC**, **EF Core**, and **PostgreSQL**.

---

## Features

- **Full CRUD**: Create, Read, Update, Delete tasks
- **Runtime-computed Status** — never stored in the database:
  - `Done` → IsCompleted = true
  - `Overdue` → Deadline has passed
  - `Urgent` → Deadline is within the next 24 hours
  - `Active` → everything else
- **Sorting** on Task List by Priority (Low → Medium → High) and Deadline
- **Manual Validation** (no DataAnnotations):
  - Title cannot be empty
  - Deadline cannot be a past date
- **Partial Views** — `_TaskRow.cshtml` renders each task row, `_TaskForm.cshtml` is shared between Create and Edit

---

## Architecture

```
TaskManagement/
├── Controllers/
│   └── TaskController.cs          # HTTP layer — routes requests to Service
├── Services/
│   ├── Interfaces/ITaskService.cs
│   └── TaskService.cs             # Business logic: status computation, sorting, mapping
├── Repositories/
│   ├── Interfaces/ITaskRepository.cs
│   └── TaskRepository.cs          # Data access via EF Core
├── Models/
│   └── TaskItem.cs                # DB entity (no Status field)
├── ViewModels/
│   ├── TaskViewModel.cs           # Carries runtime Status + manual validation
│   └── TaskListViewModel.cs       # Task list + sorting state
├── Enums/
│   ├── Priority.cs
│   └── TaskStatus.cs              # Runtime only — never persisted
├── Data/
│   └── AppDbContext.cs
├── Migrations/
├── Views/
│   ├── Task/
│   │   ├── Index.cshtml           # Task list with sortable columns
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Details.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _TaskRow.cshtml        # Partial View for one task row
│       └── _TaskForm.cshtml       # Shared form partial (Create + Edit)
├── Program.cs
├── appsettings.json
└── TaskManagement.csproj
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) (running locally or via Docker)

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/TaskManagement.git
cd TaskManagement
```

### 2. Configure the database

Edit `appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanagement;Username=postgres;Password=YOUR_PASSWORD"
}
```

### 3. Run migrations & start the app

```bash
dotnet restore
dotnet run
```

> Migrations are applied automatically on startup via `db.Database.Migrate()` in `Program.cs`.

### 4. Manual migration (optional)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Key Design Decisions

| Decision | Reason |
|----------|--------|
| `Status` not stored in DB | It's derived data — keeping it computed prevents stale state and reduces DB complexity |
| Manual validation in `TaskViewModel.Validate()` | Satisfies the requirement; avoids DataAnnotations |
| Repository pattern | Decouples EF Core from business logic; makes unit testing easier |
| Service layer | Centralises all business rules (status, sorting, mapping) away from the controller |
| Partial Views (`_TaskRow`, `_TaskForm`) | Promotes reuse and keeps Views lean |
| Priority enum values (1/2/3) | Allows natural `OrderBy((int)Priority)` without a custom comparer |

---

## Tech Stack

- ASP.NET Core 8 MVC
- Entity Framework Core 8
- Npgsql (PostgreSQL EF provider)
- Bootstrap 5 + Bootstrap Icons
