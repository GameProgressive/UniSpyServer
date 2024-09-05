from fastapi import FastAPI

from backends.protocols.gamespy.chat.requests import RegisterNickRequest
from backends.protocols.gamespy.presence_connection_manager.requests import GetProfileRequest, LoginRequest, LogoutRequest, NewProfileRequest, RegisterCDKeyRequest, StatusInfoRequest, StatusRequest, UpdateProfileRequest
from backends.urls import *

from servers.presence_connection_manager.src.contracts.requests.general import KeepAliveRequest
from servers.presence_connection_manager.src.contracts.requests.profile import AddBlockRequest
from servers.presence_search_player.src.contracts.requests import NewUserRequest


app = FastAPI()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/LoginHandler")
def login(request: LoginRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/LogoutHandler")
def logout(request: LogoutRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/KeepAliveHandler")
def keep_alive(request: KeepAliveRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/NewUserHandler")
def new_user(request: NewUserRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/AddBlockHandler")
def add_block(request: AddBlockRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/GetProfileHandler")
def get_profile(request: GetProfileRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/NewProfileHandler")
def new_proflie(request: NewProfileRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/RegisterCDKeyHandler")
def register_cdkey(request: RegisterCDKeyRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/RegisterNickHandler")
def register_nick(request: RegisterNickRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/UpdateProfileHandler")
def update_profile(request: UpdateProfileRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/StatusHandler")
def status(request: StatusRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_CONNECTION_MANAGER}/StatusInfoHandler")
def status_info(request: StatusInfoRequest):
    raise NotImplementedError()




if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)