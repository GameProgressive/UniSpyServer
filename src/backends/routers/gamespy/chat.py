from backends.library.abstractions.contracts import OKResponse
from backends.protocols.gamespy.chat.brocker_manager import MANAGER
from backends.protocols.gamespy.chat.handlers import (
    CdKeyHandler,
    CryptHandler,
    GetKeyHandler,
    GetUdpRelayHandler,
    InviteHandler,
    JoinHandler,
    ModeHandler,
    NamesHandler,
    NickHandler,
    PrivateHandler,
    UserHandler,
)
from backends.protocols.gamespy.chat.requests import (
    AtmRequest,
    CdkeyRequest,
    CryptRequest,
    GetCKeyRequest,
    GetChannelKeyRequest,
    GetKeyRequest,
    GetUdpRelayRequest,
    InviteRequest,
    JoinRequest,
    KickRequest,
    ListRequest,
    LoginRequest,
    ModeRequest,
    NamesRequest,
    NickRequest,
    NoticeRequest,
    PartRequest,
    PrivateRequest,
    QuitRequest,
    SetChannelKeyRequest,
    SetGroupRequest,
    SetKeyRequest,
    TopicRequest,
    UtmRequest,
    UserIPRequest,
    UserRequest,
    WhoIsRequest,
    WhoRequest,
)
from backends.urls import CHAT
from fastapi import APIRouter, FastAPI, WebSocket, WebSocketDisconnect
from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage

router = APIRouter()


def check_request(request: dict) -> BrockerMessage:
    msg = BrockerMessage(**request)
    return msg


@router.websocket(f"{CHAT}/ws")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket) and ws.client is not None:
        MANAGER.connect(ws)
    try:
        while True:
            _ = await ws.receive_json()
    except WebSocketDisconnect:
        if ws.client is not None:
            MANAGER.disconnect(ws)
        print("Client disconnected")


# region General


@router.post(f"{CHAT}/CdKeyHandler")
def cdkey(request: CdkeyRequest):
    handler = CdKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetKeyHandler")
def getkey(request: GetKeyRequest):
    handler = GetKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetUdpRelayHandler")
def get_udp_relay(request: GetUdpRelayRequest):
    handler = GetUdpRelayHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/InviteHandler")
def invite(request: InviteRequest):
    handler = InviteHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/ListHandler")
def list_data(request: ListRequest):
    # handler = ListHandler
    raise NotImplementedError()


@router.post(f"{CHAT}/LoginHandler")
def login(request: LoginRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/NickHandler")
def nick(request: NickRequest):
    handler = NickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/QuitHandler")
def quit(request: QuitRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/SetKeyHandler")
def set_key(request: SetKeyRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/UserHandler")
def user(request: UserRequest):
    handler = UserHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/UserIPHandler")
def user_ip(request: UserIPRequest):
    print(request)
    return OKResponse()


@router.post(f"{CHAT}/WhoHandler")
def who(request: WhoRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/WhoIsHandler")
def whois(request: WhoIsRequest):
    raise NotImplementedError()


# region channel
@router.post(f"{CHAT}/GetChannelKeyHandler")
def get_channel_key(request: GetChannelKeyRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/GetCKeyHandler")
def get_ckey(request: GetCKeyRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/JoinHandler")
def join(request: JoinRequest):
    handler = JoinHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/KickHandler")
def kick(request: KickRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/ModeHandler")
def mode(request: ModeRequest):
    handler = ModeHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/NamesHandler")
def names(request: NamesRequest):
    handler = NamesHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/PartHandler")
def part(request: PartRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/SetChannelKeyHandler")
def set_channel_key(request: SetChannelKeyRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/SetGroupHandler")
def set_group(request: SetGroupRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/TopicHandler")
def topic(request: TopicRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/CryptHandler")
def crypt(request: CryptRequest):
    handler = CryptHandler(request)
    handler.handle()
    return handler.response


# region Message


@router.post(f"{CHAT}/ATMHandler")
def atm(request: AtmRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/NoticeHandler")
def notice(request: NoticeRequest):
    raise NotImplementedError()


@router.post(f"{CHAT}/PrivateHandler")
def private(request: PrivateRequest):
    handler = PrivateHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/UTMHandler")
def utm(request: UtmRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
