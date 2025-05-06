from backends.library.brockers.chat import MANAGER
from backends.protocols.gamespy.chat.handlers import (
    CdKeyHandler,
    GetKeyHandler,
    GetUdpRelayHandler,
    InviteHandler,
)
from backends.protocols.gamespy.chat.requests import (
    AtmRequest,
    CdkeyRequest,
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


@router.websocket(f"{CHAT}/Broker")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket) and ws.client is not None:
        MANAGER.connect(ws)
    try:
        while True:
            request = await ws.receive_json()
            r = check_request(request)
            if r.message is None:
                return
            # currently we broadcast all message
            MANAGER.broadcast(r.model_dump())

    except WebSocketDisconnect:
        if ws.client is not None:
            MANAGER.disconnect(ws)
        print("Client disconnected")


# region General


@router.post(f"{CHAT}/CdKeyHandler")
async def cdkey(request: CdkeyRequest):
    handler = CdKeyHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetKeyHandler")
async def getkey(request: GetKeyRequest):
    handler = GetKeyHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetUdpRelayHandler")
async def get_udp_relay(request: GetUdpRelayRequest):
    handler = GetUdpRelayHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{CHAT}/InviteHandler")
async def invite(request: InviteRequest):
    handler = InviteHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{CHAT}/ListHandler")
async def list_data(request: ListRequest):
    # handler = ListHandler
    raise NotImplementedError()


@router.post(f"{CHAT}/LoginHandler")
async def login(request: LoginRequest):
    pass


@router.post(f"{CHAT}/NickHandler")
async def nick(request: NickRequest):
    pass


@router.post(f"{CHAT}/QuitHandler")
async def quit(request: QuitRequest):
    pass


@router.post(f"{CHAT}/SetKeyHandler")
async def set_key(request: SetKeyRequest):
    pass


@router.post(f"{CHAT}/UserHandler")
async def user(request: UserRequest):
    pass


@router.post(f"{CHAT}/UserIPHandler")
async def user_ip(request: UserIPRequest):
    pass


@router.post(f"{CHAT}/WhoHandler")
async def who(request: WhoRequest):
    pass


@router.post(f"{CHAT}/WhoIsHandler")
async def whois(request: WhoIsRequest):
    pass


# region channel
@router.post(f"{CHAT}/GetChannelKeyHandler")
async def get_channel_key(request: GetChannelKeyRequest):
    pass


@router.post(f"{CHAT}/GetCKeyHandler")
async def get_ckey(request: GetCKeyRequest):
    pass


@router.post(f"{CHAT}/JoinHandler")
async def join(request: JoinRequest):
    pass


@router.post(f"{CHAT}/KickHandler")
async def kick(request: KickRequest):
    pass


@router.post(f"{CHAT}/ModeHandler")
async def mode(request: ModeRequest):
    pass


@router.post(f"{CHAT}/NamesHandler")
async def names(request: NamesRequest):
    pass


@router.post(f"{CHAT}/PartHandler")
async def part(request: PartRequest):
    pass


@router.post(f"{CHAT}/SetChannelKeyHandler")
async def set_channel_key(request: SetChannelKeyRequest):
    pass


@router.post(f"{CHAT}/SetGroupHandler")
async def set_group(request: SetGroupRequest):
    pass


@router.post(f"{CHAT}/TopicHandler")
async def topic(request: TopicRequest):
    pass


# region Message


@router.post(f"{CHAT}/ATMHandler")
async def atm(request: AtmRequest):
    pass


@router.post(f"{CHAT}/NoticeHandler")
def notice(request: NoticeRequest):
    pass


@router.post(f"{CHAT}/PrivateHandler")
async def private(request: PrivateRequest):
    pass


@router.post(f"{CHAT}/UTMHandler")
async def utm(request: UtmRequest):
    pass


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
