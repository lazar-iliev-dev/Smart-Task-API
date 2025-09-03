from services.indexer import TaskIndexer

def test_index_and_search():
    tasks = [
        {"id": "1", "title": "Buy milk", "description": "From the supermarket"},
        {"id": "2", "title": "Write report", "description": "Annual sales report"},
    ]
    indexer = TaskIndexer()
    indexer.build_index(tasks)

    results = indexer.search("milk")
    assert results
    assert results[0][0]["id"] == "1"
