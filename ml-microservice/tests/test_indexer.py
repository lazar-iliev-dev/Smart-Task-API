import pytest
from services.indexer import TaskIndexer

@pytest.fixture
def sample_tasks():
    return [
        {"id": "1", "title": "Buy milk", "description": "From the supermarket"},
        {"id": "2", "title": "Write report", "description": "Annual sales report"},
        {"id": "3", "title": "Go jogging", "description": "Morning run in the park"},
    ]

def test_index_and_search(sample_tasks):
    indexer = TaskIndexer()
    indexer.build_index(sample_tasks)

    results = indexer.search("milk")
    assert results
    assert results[0][0]["id"] == "1"

def test_similar_tasks(sample_tasks):
    indexer = TaskIndexer()
    indexer.build_index(sample_tasks)

    results = indexer.similar("2", top_k=2)
    assert len(results) == 2
    assert all("task" not in r for r in results)  # returns tuples (task, score)
