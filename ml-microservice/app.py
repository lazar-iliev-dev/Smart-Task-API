from fastapi import FastAPI
from routers import search

app = FastAPI(
    title="Smart Search API",
    version="1.0",
    description="ML-based smart search microservice for SmartTaskAPI"
)

# Hook up the router
app.include_router(search.router, prefix="/api", tags=["smart-search"])


@app.get("/health")
def health():
    return {"status": "ok", "service": "ml-microservice"}
