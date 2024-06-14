from flask import Flask, request
from backends.gamespy.servers.presence_search_player.requests import (
    LoginRequest,
)
from backends.urls import *

app = Flask(__name__)


@app.route(f"/{PRESENCE_CONNECTION_MANAGER}/login", methods=["POST"])
def pcm_login():
    req = LoginRequest(request.json)
    pass




