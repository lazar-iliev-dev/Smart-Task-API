from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity

class TaskIndexer:
    def __init__(self):
        self.vectorizer = TfidfVectorizer(stop_words="english")
        self.tasks = []
        self.tfidf_matrix = None

    def build_index(self, tasks):
        self.tasks = tasks
        docs = [f"{t['title']} {t.get('description','')}" for t in tasks]
        self.tfidf_matrix = self.vectorizer.fit_transform(docs)

    def search(self, query, top_k=5):
        if self.tfidf_matrix is None:
            return []
        query_vec = self.vectorizer.transform([query])
        sims = cosine_similarity(query_vec, self.tfidf_matrix).flatten()
        ranked = sorted(zip(self.tasks, sims), key=lambda x: x[1], reverse=True)
        return ranked[:top_k]

    def similar(self, task_id, top_k=3):
        if self.tfidf_matrix is None:
            return []
        idx = next((i for i, t in enumerate(self.tasks) if t["id"] == task_id), None)
        if idx is None:
            return []
        sims = cosine_similarity(self.tfidf_matrix[idx], self.tfidf_matrix).flatten()
        ranked = sorted(
            [(self.tasks[i], sims[i]) for i in range(len(self.tasks)) if i != idx],
            key=lambda x: x[1],
            reverse=True
        )
        return ranked[:top_k]
