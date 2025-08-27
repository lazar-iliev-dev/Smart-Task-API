
# Smart-Task-Api

*SmartTaskApi — .NET 9 Web API for task management (CRUD, filters, JWT auth).*


![Build](https://img.shields.io/badge/build-passing-brightgreen)
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

---

## 🚀 Features / Funktionen

- ✅ CRUD: Create, Read, Update, Delete tasks  
- ✅ JWT authentication (login / register)  
- ✅ Swagger / OpenAPI documentation
- ✅ Docker support  
- 🚧 Filter tasks by status and due date (work in progress)  
- 🚧 Deployment scripts / CI (work in progress)

---

## 📦 Tech Stack

- .NET 9 WebAPI  
- Entity Framework Core (EF Core)  
- PostgreSQL (Supabase / Docker)  
- Npgsql (Postgres EF provider)  
- Docker / docker-compose  
- Swagger / Swashbuckle  
- Serilog  

---

## 📂 Ordnerstruktur / Project structure

```
```bash
SmartTaskApi/
├── Controllers/
│   ├── TaskController.cs
│   └── AuthController.cs
├── Models/
│   ├── TaskItem.cs
│   └── User.cs
├── DTO/
│   ├── CreateTaskDto.cs
│   ├── UserDto.cs
│   ├── RegisterRequest.cs
│   └── LoginRequest.cs
├── Data/
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── Services/
│   └── JwtService.cs
├── Middlewares/
│   └── ExceptionMiddleware.cs
├── Migrations/
├── docker-compose.yml
├── Dockerfile
├── Program.cs
├── appsettings.json
└── README.md

````

---

## 🛠️ Voraussetzungen / Requirements

- .NET 9 SDK → [Download](https://dotnet.microsoft.com/)  
- Docker (für containerized development)  
- Optional: `psql` / PgAdmin / Supabase dashboard  

---

## 🐳 Quick start — lokal mit Docker

1. Repository klonen:
   ```bash
   git clone https://github.com/lazar-iliev-dev/Smart-Task-Api.git
   cd SmartTaskAPI
````

2. Environment konfigurieren:
   Passe `appsettings.Development.json` oder Environment-Variablen an:

   * `ConnectionStrings:DefaultConnection`
   * `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`

3. Docker starten:

   ```bash
   docker-compose up -d
   ```

4. Migration anwenden:

   ```bash
   dotnet ef database update --project SmartTaskAPI.csproj
   ```

5. App öffnen:
   👉 [http://localhost:5284/swagger](http://localhost:5284/swagger)

---

## 💻 Quick start — ohne Docker (lokal)

### Konfiguration

Erstelle/ändere `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=smarttaskdb;Username=postgres;Password=D4Hbfg9dqB"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_MIN_16_CHARS",
    "Issuer": "smarttask",
    "Audience": "smarttask_users"
  }
}
```

### Start

```bash
dotnet ef database update --project SmartTaskApi.csproj
dotnet run --project SmartTaskApi.csproj
```

👉 Swagger: [http://localhost:5284/swagger](http://localhost:5284/swagger)

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

---

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

---

## 📷 Swagger UI

![Swagger Screenshot](docs/swagger.png)

---

## 🧪 Testing

* 🚧 Unit / Integration tests (work in progress)
* Empfehlung: `xUnit` + `WebApplicationFactory`

---

## 🚢 Deployment

* Render (Docker)
* GitHub Actions CI/CD 🚧

---

## 🤝 Contributing

Contributions welcome.
Workflow: **Fork → Branch → Pull Request**
Bitte kurze Beschreibung im PR hinzufügen.

---

## 📜 License & Author

* Lizenz: MIT
* Author: **Lazar Iliev** — GitHub: [@lazar-iliev-dev](https://github.com/lazar-iliev-dev)
```
