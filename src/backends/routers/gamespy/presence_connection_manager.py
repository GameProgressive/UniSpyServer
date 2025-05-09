from fastapi import APIRouter

from backends.protocols.gamespy.chat.requests import RegisterNickRequest
from backends.protocols.gamespy.presence_connection_manager.handlers import AddBlockHandler, GetProfileHandler, KeepAliveHandler, LoginHandler, LogoutHandler, NewProfileHandler, NewUserHandler, RegisterCDKeyHandler, RegisterNickHandler, StatusHandler, StatusInfoHandler, UpdateProfileHandler
from backends.protocols.gamespy.presence_connection_manager.requests import GetProfileRequest, LoginRequest, LogoutRequest, NewProfileRequest, RegisterCDKeyRequest, StatusInfoRequest, StatusRequest, UpdateProfileRequest, KeepAliveRequest, NewUserRequest, AddBlockRequest
from backends.urls import PRESENCE_CONNECTION_MANAGER


router = APIRouter()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LoginHandler")
def login(request: LoginRequest):
    handler = LoginHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LogoutHandler")
def logout(request: LogoutRequest):
    handler = LogoutHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/KeepAliveHandler")
def keep_alive(request: KeepAliveRequest):
    handler = KeepAliveHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewUserHandler")
def new_user(request: NewUserRequest):
    handler = NewUserHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AddBlockHandler")
def add_block(request: AddBlockRequest):
    handler = AddBlockHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/GetProfileHandler")
def get_profile(request: GetProfileRequest):
    handler = GetProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewProfileHandler")
def new_proflie(request: NewProfileRequest):
    handler = NewProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterCDKeyHandler")
def register_cdkey(request: RegisterCDKeyRequest):
    handler = RegisterCDKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterNickHandler")
def register_nick(request: RegisterNickRequest):
    handler = RegisterNickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/UpdateProfileHandler")
def update_profile(request: UpdateProfileRequest):
    handler = UpdateProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusHandler")
def status(request: StatusRequest):
    handler = StatusHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusInfoHandler")
def status_info(request: StatusInfoRequest):
    handler = StatusInfoHandler(request)
    handler.handle()
    return handler.response

if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
