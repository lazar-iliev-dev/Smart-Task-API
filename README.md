<!DOCTYPE html>
<html lang="de">

<h3> SmartTaskApi — .NET 9 Web API for task management (CRUD, filters, JWT auth).  </h3>

![Build](https://img.shields.io/badge/build-passing-brightgreen)
![Docker](https://img.shields.io/badge/docker-ready-blue)
![License](https://img.shields.io/badge/license-MIT-yellow)

---
# ⚠️ PROJECT MIGRATED

> **This repository has been consolidated into the [Enterprise Operations Platform](https://github.com/lazar-iliev-dev/Enterprise-Operations-Platform) – a modern microservices architecture using .NET Aspire.**
> 
> **What changed:**
> - Upgraded to .NET 9 with Aspire orchestration
> - Integrated with Knowledge Management Service
> - Added OpenTelemetry observability
> - Migrated to cloud-native deployment (Docker + PostgreSQL)
>
> 👉 **For the latest version, visit the [new repository](https://github.com/lazar-iliev-dev/Enterprise-Operations-Platform)**

---

## 📜 Legacy Documentation (Archived)

*The content below represents the standalone version of this project before migration.*

---

<h3>SmartTaskApi — .NET 9 Web API for task management (CRUD, filters, JWT auth)</h3>

![Build](https://img.shields.io/badge/build-archived-inactive)
![Docker](https://img.shields.io/badge/docker-ready-blue)
![License](https://img.shields.io/badge/license-MIT-yellow)

## 🌍 Kurzbeschreibung / Short description

🇩🇪  
SmartTaskApi ist eine moderne .NET 9 WebAPI für ein Aufgabenmanagement-System.  
Die API bietet Endpunkte zum Erstellen, Bearbeiten, Löschen und Filtern von Aufgaben nach Status und Fälligkeitsdatum.  
Authentifizierung erfolgt via JWT. Deployment erfolgt containerbasiert (Render / Docker).

🇬🇧  
SmartTaskApi is a modern .NET 9 Web API for a task management system.  
The API provides endpoints to create, edit, delete and filter tasks by status and due date.  
Authentication uses JWT. Deployment is container-based (Render / Docker).

## 🚀 Features / Funktionen

- ✅ CRUD: Create, Read, Update, Delete tasks  
- ✅ JWT authentication (login / register)  
- ✅ Swagger / OpenAPI documentation  
- ✅ Docker support  
- 🚧 Filter tasks by status and due date (work in progress)  
- 🚧 Deployment scripts / CI (work in progress)


## 📦 Tech Stack

- .NET 9 WebAPI  
- Entity Framework Core (EF Core)  
- PostgreSQL (Supabase / Docker)  
- Npgsql (Postgres EF provider)  
- Docker / docker-compose  
- Swagger / Swashbuckle  
- Serilog  


## 🛠️ Voraussetzungen / Requirements

- .NET 9 SDK → [Download](https://dotnet.microsoft.com/)  
- Docker (für containerized development)  
- Optional: `psql` / PgAdmin / Supabase dashboard  


## 🐳 Quick start — Lokales Setup (Docker)

1. Repository klonen:
 ```bash
 git clone https://github.com/lazar-iliev-dev/Smart-Task-Api.git
 d SmartTaskAPI

```bash
cp .env.example .env
```

2. Postgres starten:

```bash
docker compose up -d postgres
```

Migration anwenden (lokale DB erstellen & aktualisieren):

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=smarttaskdb;Username=postgres;Password=<your pw>;SslMode=Disable"
dotnet ef database update -p src/Infrastructure/Infrastructure.csproj -s src/Api/Api.csproj
```
Alle Services starten (API + ML + DB):

```bash
docker compose up -d --build
```

Test der APIs:

Swagger (API): 👉 http://localhost:5284/swagger

ML Service: 👉 http://localhost:5001/api/search?q=test



---

## 📖 API Reference

### Authentication

| Endpoint             | Method | Auth | Description     |
| -------------------- | ------ | ---- | --------------- |
| `/api/auth/register` | POST   | ❌    | Create new user |
| `/api/auth/login`    | POST   | ❌    | Login, get JWT  |

#### Example: Register

```bash
curl -X POST http://localhost:5284/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "StrongPass123",
    "role": "User"
  }'
```

#### Example: Login

```bash
curl -X POST http://localhost:5284/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "StrongPass123"
  }'
```

👉 Returns: `{ "token": "eyJhbGciOi..." }`

### Tasks

| Endpoint          | Method | Auth | Description     |
| ----------------- | ------ | ---- | --------------- |
| `/api/tasks`      | GET    | ✅    | Get all tasks   |
| `/api/tasks/{id}` | GET    | ✅    | Get task by ID  |
| `/api/tasks`      | POST   | ✅    | Create new task |
| `/api/tasks/{id}` | PUT    | ✅    | Update task     |
| `/api/tasks/{id}` | DELETE | ✅    | Delete task     |

#### Example: Create Task

```bash
curl -X POST http://localhost:5284/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "title": "Finish README",
    "description": "Write API reference docs",
    "dueDate": "2025-09-01T12:00:00Z",
    "status": 0,
    "priority": 1
  }'
```

#### Example: Get Tasks

```bash
curl -X GET http://localhost:5284/api/tasks \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

👉 Returns:

```json
[
  {
    "id": "f3e2a9d0-6c1b-4f6d-9b7b-81c9b4a4a7b1",
    "title": "Finish README",
    "description": "Write API reference docs",
    "dueDate": "2025-09-01T12:00:00Z",
    "status": 0,
    "priority": 1
  }
]
```

## 🧪 Testing

* Unit / Integration tests (work in progress) 
* Empfehlung: `xUnit` + `WebApplicationFactory`


## 🚢 Deployment

* Render (Docker) 🚧
* GitHub Actions CI/CD 🚧


## 🤝 Contributing

Contributions welcome.
Workflow: **Fork → Branch → Pull Request**
Bitte kurze Beschreibung im PR hinzufügen.


## 📜 License & Author

<footer>
  <p>Lizenz: MIT | Author: <strong>Lazar Iliev</strong> — GitHub: <a href="https://github.com/lazar-iliev-dev" target="_blank">@lazar-iliev-dev</a></p>
</footer>
</html>
