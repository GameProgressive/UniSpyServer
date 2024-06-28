from fastapi import FastAPI

from backends.gamespy.protocols.presence_search_player.requests import LoginRequest
from backends.urls import *

app = FastAPI()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/login")
def login(request: LoginRequest):

    pass
