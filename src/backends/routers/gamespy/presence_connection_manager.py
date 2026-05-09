from fastapi import APIRouter

from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse
from backends.protocols.gamespy.chat.requests import RegisterNickRequest
from backends.protocols.gamespy.presence_connection_manager.handlers import AddBlockHandler, AddBuddyHandler, AuthAddBuddyHandler, BlockListRetriveHandler, BuddyListRetriveHandler, BuddyMessageFriendAddHandler, GetProfileHandler, KeepAliveHandler, LoginHandler, LogoutHandler, NewProfileHandler, NewUserHandler, RegisterCDKeyHandler, RegisterNickHandler, StatusHandler, StatusInfoHandler, UpdateProfileHandler
from backends.protocols.gamespy.presence_connection_manager.requests import AddBuddyRequest, AuthAddBuddyRequest, BlockListRetriveRequest, BuddyListRetriveRequest, BuddyMessageFriendAddRequest, GetProfileRequest, LoginRequest, LogoutRequest, NewProfileRequest, RegisterCDKeyRequest, StatusInfoRequest, StatusRequest, UpdateProfileRequest, KeepAliveRequest, NewUserRequest, AddBlockRequest
from backends.protocols.gamespy.presence_connection_manager.responses import BlockListRetriveResponse, BuddyListRetriveResponse, BuddyMessageFriendAddResponse, GetProfileResponse, LoginResponse
from backends.urls import PRESENCE_CONNECTION_MANAGER


router = APIRouter()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LoginHandler", responses=RESPONSES_DEF)
def login(request: LoginRequest) -> LoginResponse:
    handler = LoginHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/LogoutHandler", responses=RESPONSES_DEF)
def logout(request: LogoutRequest) -> OKResponse:
    handler = LogoutHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/KeepAliveHandler", responses=RESPONSES_DEF)
def keep_alive(request: KeepAliveRequest) -> OKResponse:
    handler = KeepAliveHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewUserHandler", responses=RESPONSES_DEF)
def new_user(request: NewUserRequest) -> OKResponse:
    handler = NewUserHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AddBlockHandler", responses=RESPONSES_DEF)
def add_block(request: AddBlockRequest) -> OKResponse:
    handler = AddBlockHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AddBuddyHandler", responses=RESPONSES_DEF)
def add_buddy(request: AddBuddyRequest) -> OKResponse:
    handler = AddBuddyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/BuddyMessageFriendAddHandler", responses=RESPONSES_DEF)
def buddy_message_friend_add(request: BuddyMessageFriendAddRequest) -> BuddyMessageFriendAddResponse:
    handler = BuddyMessageFriendAddHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/AuthAddBuddyHandler", responses=RESPONSES_DEF)
def auth_add_buddy_handler(request: AuthAddBuddyRequest) -> OKResponse:
    handler = AuthAddBuddyHandler(request)
    handler.handle()
    return OKResponse()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/GetProfileHandler", responses=RESPONSES_DEF)
def get_profile(request: GetProfileRequest) -> GetProfileResponse:
    handler = GetProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/NewProfileHandler", responses=RESPONSES_DEF)
def new_proflie(request: NewProfileRequest) -> OKResponse:
    handler = NewProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterCDKeyHandler", responses=RESPONSES_DEF)
def register_cdkey(request: RegisterCDKeyRequest) -> OKResponse:
    handler = RegisterCDKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/RegisterNickHandler", responses=RESPONSES_DEF)
def register_nick(request: RegisterNickRequest) -> OKResponse:
    handler = RegisterNickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/UpdateProfileHandler", responses=RESPONSES_DEF)
def update_profile(request: UpdateProfileRequest) -> OKResponse:
    handler = UpdateProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusHandler", responses=RESPONSES_DEF)
def status(request: StatusRequest) -> OKResponse:
    handler = StatusHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/StatusInfoHandler", responses=RESPONSES_DEF)
def status_info(request: StatusInfoRequest) -> OKResponse:
    handler = StatusInfoHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/BuddyListRetriveHandler", responses=RESPONSES_DEF)
def buddy_list(request: BuddyListRetriveRequest) -> BuddyListRetriveResponse:
    handler = BuddyListRetriveHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/BlockListRetriveHandler", responses=RESPONSES_DEF)
def block_list(request: BlockListRetriveRequest) -> BlockListRetriveResponse:
    handler = BlockListRetriveHandler(request)
    handler.handle()
    return handler.response

# region Buddy Message


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/BuddyMessageFriendAddHandler", responses=RESPONSES_DEF)
def buddy_add_request_message(request) -> None:
    raise NotImplementedError()


@router.post(f"{PRESENCE_CONNECTION_MANAGER}/BuddyMessageHandler", responses=RESPONSES_DEF)
def buddy_message(request):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
