from fastapi import FastAPI

from backends.gamespy.protocols.presence_search_player.requests import LoginRequest
from backends.urls import *

app = FastAPI()


@app.route(f"/{PRESENCE_CONNECTION_MANAGER}/login", methods=["POST"])
def pcm_login(request: LoginRequest):

    pass
