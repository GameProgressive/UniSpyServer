from fastapi import FastAPI

from backends.urls import GAMESPY_PREFIX, WEB_SERVICES

URL = f"{GAMESPY_PREFIX}/{WEB_SERVICES}"
app = FastAPI()

# SAKE services


@app.post(f"/{URL}/CreateRecord")
def create_record(request):
    raise NotImplementedError()
