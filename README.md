# ✈️ Aviation Workflow API

![.NET Version](https://img.shields.io/badge/.NET-10.0-blueviolet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-blue)
![Architecture](https://img.shields.io/badge/Architecture-Minimal_APIs-success)

A modern, lightweight RESTful backend built with C# and .NET 10, simulating an aircraft component maintenance tracking system. 

The project has been heavily refactored to follow modern .NET conventions, replacing legacy Controllers with high-performance **Minimal APIs** and utilizing a robust **BaseEntity** architecture for automated audit trailing.

## 🛠 Tech Stack

* **Runtime:** .NET 10 (ASP.NET Core Minimal APIs)
* **Data Access:** Entity Framework Core 10 (In-Memory provider for local development)
* **Architecture:** Domain-Driven Design concepts (Entities, Entity Configurations, Lookup Tables)
* **Logging:** Serilog (Console sink with structured message templates)
* **CI/CD:** GitHub Actions (Automated build & test workflow)
* **Containerization:** Docker (Multi-stage build)

## 📂 Project Structure

The solution utilizes the new lightweight `.slnx` format and is structured for scalability:

* `Aviation.Api/Endpoints/` – Contains all Minimal API endpoint definitions (`MaintenanceTaskEndpoints`, `StatusEndpoints`, `PriorityEndpoints`), replacing traditional Controllers.
* `Aviation.Api/Entities/` – Domain models inheriting from a central `BaseEntity` (which provides `Id`, `CreatedOn`, `CreatedBy`, `LastUpdatedOn`, `LastUpdatedBy`).
* `Aviation.Api/Data/Configurations/` – EF Core `IEntityTypeConfiguration` classes isolating database constraints from domain logic.
* `Aviation.Api/Data/` – EF Core `DbContext` and initial mock data seeding.
* `.claude/` & `CLAUDE.md` – AI assistant guidelines ensuring consistent code generation (AI-Ready workspace).

## 🚀 API Endpoints

### Maintenance Tasks
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/MaintenanceTasks` | Returns a list of all maintenance tasks |
| `GET` | `/api/MaintenanceTasks/{id}` | Returns a single task by ID (`long`) |
| `POST` | `/api/MaintenanceTasks` | Creates a new task and returns a 201 Created status |

### System Lookups (Statuses & Priorities)
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Statuses` | Returns all available operational workflow states |
| `GET` | `/api/Statuses/{id}` | Returns a single status by ID |
| `GET` | `/api/Priorities` | Returns all available priority levels |
| `GET` | `/api/Priorities/{id}` | Returns a single priority by ID |

## 🏁 Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run Locally

Clone the repository and run the project from the root directory using the modern `.slnx` solution file:

```bash
git clone [https://github.com/stefzr/AviationSystem.git](https://github.com/stefzr/AviationSystem.git)
cd AviationSystem
dotnet run --project Aviation.Api