## 🚀 CI/CD mit GitHub Actions

Dieses Projekt verwendet **GitHub Actions** für automatisiertes Testen und Deployment.

### 🔹 Pipeline Übersicht
- **API Tests (.NET 9)**
  - `dotnet build` und `dotnet test` werden für die SmartTaskAPI ausgeführt
- **ML-Service Tests (Python)**
  - `pytest` wird im `ml-microservice` Verzeichnis ausgeführt
- **Docker Build & Push**
  - Läuft nur auf `main`
  - Baut Images für `smarttaskapi` und `ml-microservice`
  - Push in GitHub Container Registry (`ghcr.io`)

### 🔹 Workflows
- Bei jedem **Push** oder **Pull Request** auf `dev/*` oder `main`
  - Tests für API & ML laufen automatisch
- Bei Merge auf `main`
  - Docker Images werden automatisch gebaut und in die Registry gepusht

### 🔹 Docker Images ziehen
```bash
docker pull ghcr.io/<dein-user>/<repo-name>/smarttaskapi:latest
docker pull ghcr.io/<dein-user>/<repo-name>/ml-microservice:latest
