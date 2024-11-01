from fastapi import APIRouter

from backends.protocols.gamespy.chat.requests import RegisterNickRequest
from backends.protocols.gamespy.presence_connection_manager.requests import GetProfileRequest, LoginRequest, LogoutRequest, NewProfileRequest, RegisterCDKeyRequest, StatusInfoRequest, StatusRequest, UpdateProfileRequest, KeepAliveRequest, NewUserRequest, AddBlockRequest
from backends.urls import *


router = APIRouter()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LoginHandler")
async def login(request: LoginRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LogoutHandler")
async def logout(request: LogoutRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/KeepAliveHandler")
async def keep_alive(request: KeepAliveRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewUserHandler")
async def new_user(request: NewUserRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AddBlockHandler")
async def add_block(request: AddBlockRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/GetProfileHandler")
async def get_profile(request: GetProfileRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewProfileHandler")
async def new_proflie(request: NewProfileRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterCDKeyHandler")
async def register_cdkey(request: RegisterCDKeyRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterNickHandler")
async def register_nick(request: RegisterNickRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/UpdateProfileHandler")
async def update_profile(request: UpdateProfileRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusHandler")
async def status(request: StatusRequest):
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusInfoHandler")
async def status_info(request: StatusInfoRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
