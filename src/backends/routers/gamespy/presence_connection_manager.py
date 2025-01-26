from fastapi import APIRouter

from backends.protocols.gamespy.chat.requests import RegisterNickRequest
from backends.protocols.gamespy.presence_connection_manager.handlers import AddBlockHandler, GetProfileHandler, KeepAliveHandler, LoginHandler, LogoutHandler, NewProfileHandler, NewUserHandler, RegisterCDKeyHandler, RegisterNickHandler, StatusHandler, StatusInfoHandler, UpdateProfileHandler
from backends.protocols.gamespy.presence_connection_manager.requests import GetProfileRequest, LoginRequest, LogoutRequest, NewProfileRequest, RegisterCDKeyRequest, StatusInfoRequest, StatusRequest, UpdateProfileRequest, KeepAliveRequest, NewUserRequest, AddBlockRequest
from backends.urls import PRESENCE_CONNECTION_MANAGER


router = APIRouter()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LoginHandler")
async def login(request: LoginRequest):
    handler = LoginHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LogoutHandler")
async def logout(request: LogoutRequest):
    handler = LogoutHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/KeepAliveHandler")
async def keep_alive(request: KeepAliveRequest):
    handler = KeepAliveHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewUserHandler")
async def new_user(request: NewUserRequest):
    handler = NewUserHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AddBlockHandler")
async def add_block(request: AddBlockRequest):
    handler = AddBlockHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/GetProfileHandler")
async def get_profile(request: GetProfileRequest):
    handler = GetProfileHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewProfileHandler")
async def new_proflie(request: NewProfileRequest):
    handler = NewProfileHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterCDKeyHandler")
async def register_cdkey(request: RegisterCDKeyRequest):
    handler = RegisterCDKeyHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterNickHandler")
async def register_nick(request: RegisterNickRequest):
    handler = RegisterNickHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/UpdateProfileHandler")
async def update_profile(request: UpdateProfileRequest):
    handler = UpdateProfileHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusHandler")
async def status(request: StatusRequest):
    handler = StatusHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusInfoHandler")
async def status_info(request: StatusInfoRequest):
    handler = StatusInfoHandler(request)
    await handler.handle()
    return handler.response

if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
