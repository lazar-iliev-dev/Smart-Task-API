import pytest
from fastapi.testclient import TestClient
from app import app

client = TestClient(app)

def test_health_endpoint():
    response = client.get("/health")
    assert response.status_code == 200
    data = response.json()
    assert data["status"] == "ok"
    assert data["service"] == "ml-microservice"

def test_search_empty_index():
    response = client.get("/api/search?q=test")
    assert response.status_code == 200
    assert response.json() == []  # index not yet built

def test_reindex_and_search(monkeypatch):
    # Fake fetcher
    def fake_fetch_tasks():
        return [
            {"id": "1", "title": "Test Task", "description": "Some description"},
            {"id": "2", "title": "Another Task", "description": "Details"},
        ]

    monkeypatch.setattr("routers.search.fetch_tasks", fake_fetch_tasks)

    # Trigger reindex
    r = client.post("/api/reindex")
    assert r.status_code == 200

    # Search for "Test"
    s = client.get("/api/search?q=Test")
    assert s.status_code == 200
    results = s.json()
    assert len(results) > 0
    assert results[0]["task"]["id"] == "1"
