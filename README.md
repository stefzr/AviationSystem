# Enterprise Workflow API

A lightweight RESTful backend built with C# and .NET 8, simulating an aircraft component maintenance tracking system. It models operational workflow states (Pending, In Progress, Completed) and includes structured logging and automated CI builds.

## Tech Stack

* **Runtime / Framework:** .NET 8 (ASP.NET Core Web API)
* **Data Access:** Entity Framework Core (configured with an In-Memory provider for local development)
* **Logging:** Serilog (Console sink with structured message templates)
* **CI/CD:** GitHub Actions (automated build & test workflow)
* **Containerization:** Docker (multi-stage build)

## Project Structure

* `Aviation.Api/Controllers/` – Handles HTTP routing and status code mappings (200, 201, 404).
* `Aviation.Api/Models/` – Domain entity definitions (`MaintenanceTask`).
* `Aviation.Api/Data/` – EF Core `DbContext` configuration and initial mock data seeding.

## API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/MaintenanceTasks` | Returns a list of all maintenance tasks |
| `GET` | `/api/MaintenanceTasks/{id}` | Returns a single task by ID (returns 404 if not found) |
| `POST` | `/api/MaintenanceTasks` | Creates a new task and returns a 201 Created status |

## Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run Locally
```bash
git clone [https://github.com/stefzr/AviationSystem.git](https://github.com/stefzr/AviationSystem.git)
cd AviationSystem/Aviation.Api
dotnet run