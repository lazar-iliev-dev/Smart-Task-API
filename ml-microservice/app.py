from fastapi import FastAPI, Query 
from typing import List
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import uvicorn

app = FastAPI(titel="Smart Search API", version="1.0 ")

import requests

API_URL = "http://smarttaskapi:80/api/task"  
# Wichtig: Container-Name von deinem .NET-Service im Docker-Compose benutzen
# Falls lokal getestet: "http://localhost:5284/api/task"

def fetch_tasks():
    try:
        response = requests.get(API_URL, timeout=5)
        response.raise_for_status()
        return response.json()  # Erwartet: Liste von {id, title, description}
    except Exception as e:
        print(f"⚠️ Fehler beim Laden der Tasks: {e}")
        return []


#search
@app.get("/search")
def smart_search(q: str = Query(..., description="Search query")):
    tasks = fetch_tasks()
    if not tasks:
        return {"error": "Keine Tasks gefunden"}

    documents = [t["title"] + " " + (t.get("description") or "") for t in tasks]
    vectorizer = TfidfVectorizer(stop_words="english")
    tfidf_matrix = vectorizer.fit_transform(documents + [q])

    cosine_similarities = cosine_similarity(tfidf_matrix[-1], tfidf_matrix[:-1]).flatten()
    scored_tasks = sorted(
        zip(tasks, cosine_similarities),
        key=lambda x: x[1],
        reverse=True
    )

    return [{"task": t, "score": round(float(score), 3)} for t, score in scored_tasks]


#similar/{task_id}
@app.get("/similar/{task_id}")
def similar_tasks(task_id: str, top_k: int = 3):
    tasks = fetch_tasks()
    if not tasks:
        return {"error": "Keine Tasks gefunden"}

    documents = [t["title"] + " " + (t.get("description") or "") for t in tasks]
    vectorizer = TfidfVectorizer(stop_words="english")
    tfidf_matrix = vectorizer.fit_transform(documents)

    idx = next((i for i, t in enumerate(tasks) if str(t["id"]) == str(task_id)), None)
    if idx is None:
        return {"error": "Task not found"}

    cosine_similarities = cosine_similarity(tfidf_matrix[idx], tfidf_matrix).flatten()
    similar_indices = cosine_similarities.argsort()[::-1][1:top_k+1]

    return [{"task": tasks[i], "score": round(float(cosine_similarities[i]), 3)} for i in similar_indices]



if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
