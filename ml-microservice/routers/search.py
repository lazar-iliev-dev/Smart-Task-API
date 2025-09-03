from fastapi import APIRouter, Query, BackgroundTasks
from services.indexer import TaskIndexer
from services.fetcher import fetch_tasks

router = APIRouter()
indexer = TaskIndexer()


@router.get("/search")
def smart_search(q: str = Query(..., description="Suchanfrage")):
    results = indexer.search(q)
    return [{"task": t, "score": round(score, 3)} for t, score in results]


@router.get("/similar/{task_id}")
def similar_tasks(task_id: str, top_k: int = 3):
    results = indexer.similar(task_id, top_k)
    return [{"task": t, "score": round(score, 3)} for t, score in results]


@router.post("/reindex")
def reindex(background_tasks: BackgroundTasks):
    def job():
        tasks = fetch_tasks()
        indexer.build_index(tasks)

    background_tasks.add_task(job)
    return {"status": "accepted", "message": "Reindexing gestartet"}
