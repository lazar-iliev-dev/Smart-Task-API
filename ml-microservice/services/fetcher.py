import requests
import os

API_URL = os.getenv("DOTNET_API_URL", "http://smarttaskapi:80")

def fetch_tasks():
    r = requests.get(f"{API_URL}/api/tasks")
    r.raise_for_status()
    return r.json()
