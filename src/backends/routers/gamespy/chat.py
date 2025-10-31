from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse, Response
from backends.protocols.gamespy.chat.brocker import MANAGER, launch_brocker
from backends.protocols.gamespy.chat.handlers import (
    CdKeyHandler,
    CryptHandler,
    GetCKeyHandler,
    GetChannelKeyHandler,
    GetKeyHandler,
    GetUdpRelayHandler,
    InviteHandler,
    JoinHandler,
    ModeHandler,
    NamesHandler,
    NickHandler,
    PartHandler,
    PrivateHandler,
    PublishMessageHandler,
    QuitHandler,
    SetKeyHandler,
    TopicHandler,
    UserHandler,
    WhoIsHandler,
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
    PublishMessageRequest,
    QuitRequest,
    SetCKeyRequest,
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
from backends.protocols.gamespy.chat.response import AtmResponse, CryptResponse, GetCkeyResponse, GetKeyResponse, JoinResponse, ListResponse, ModeResponse, NamesResponse, NicksResponse, NoticeResponse, PartResponse, PrivateResponse, SetCKeyResponse, SetChannelKeyResponse, TopicResponse, UtmResponse, WhoIsResponse, WhoResponse
from backends.urls import CHAT
from fastapi import APIRouter, FastAPI, WebSocket


router = APIRouter(lifespan=launch_brocker)
client_pool = {}


@router.post(f"{CHAT}/PublishMessageHandler", responses=RESPONSES_DEF)
def publish_message(request: PublishMessageRequest) -> OKResponse:
    handler = PublishMessageHandler(request)
    handler.handle()
    return handler.response


@router.websocket(f"{CHAT}/ws")
async def websocket_endpoint(ws: WebSocket):
    await MANAGER.process_websocket(ws)


# region General


@router.post(f"{CHAT}/CdKeyHandler", responses=RESPONSES_DEF)
def cdkey(request: CdkeyRequest) -> Response:
    handler = CdKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetKeyHandler", responses=RESPONSES_DEF)
def getkey(request: GetKeyRequest) -> GetKeyResponse:
    handler = GetKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetUdpRelayHandler", responses=RESPONSES_DEF)
def get_udp_relay(request: GetUdpRelayRequest) -> Response:
    handler = GetUdpRelayHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/InviteHandler", responses=RESPONSES_DEF)
def invite(request: InviteRequest) -> Response:
    handler = InviteHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/ListHandler", responses=RESPONSES_DEF)
def list_data(request: ListRequest) -> ListResponse:
    # handler = ListHandler
    raise NotImplementedError()


@router.post(f"{CHAT}/LoginHandler", responses=RESPONSES_DEF)
def login(request: LoginRequest) -> Response:
    raise NotImplementedError()


@router.post(f"{CHAT}/NickHandler", responses=RESPONSES_DEF)
def nick(request: NickRequest) -> NicksResponse:
    handler = NickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/QuitHandler", responses=RESPONSES_DEF)
def quit(request: QuitRequest) -> Response:
    handler = QuitHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/SetKeyHandler", responses=RESPONSES_DEF)
def set_key(request: SetKeyRequest) -> OKResponse:
    handler = SetKeyHandler(request)
    handler.handle
    return handler.response


@router.post(f"{CHAT}/UserHandler", responses=RESPONSES_DEF)
def user(request: UserRequest) -> Response:
    handler = UserHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/UserIPHandler", responses=RESPONSES_DEF)
def user_ip(request: UserIPRequest) -> OKResponse:
    return OKResponse()


@router.post(f"{CHAT}/WhoHandler", responses=RESPONSES_DEF)
def who(request: WhoRequest) -> WhoResponse:
    raise NotImplementedError()


@router.post(f"{CHAT}/WhoIsHandler", responses=RESPONSES_DEF)
def whois(request: WhoIsRequest) -> WhoIsResponse:
    handler = WhoIsHandler(request)
    handler.handle()
    return handler.response


# region channel
@router.post(f"{CHAT}/GetChannelKeyHandler", responses=RESPONSES_DEF)
def get_channel_key(request: GetChannelKeyRequest) -> Response:
    handler = GetChannelKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/GetCKeyHandler", responses=RESPONSES_DEF)
def get_ckey(request: GetCKeyRequest) -> GetCkeyResponse:
    handler = GetCKeyHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/JoinHandler", responses=RESPONSES_DEF)
def join(request: JoinRequest) -> JoinResponse:
    handler = JoinHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/KickHandler", responses=RESPONSES_DEF)
def kick(request: KickRequest) -> Response:
    raise NotImplementedError()


@router.post(f"{CHAT}/ModeHandler", responses=RESPONSES_DEF)
def mode(request: ModeRequest) -> ModeResponse | OKResponse:
    handler = ModeHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/NamesHandler", responses=RESPONSES_DEF)
def names(request: NamesRequest) -> NamesResponse:
    handler = NamesHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/PartHandler", responses=RESPONSES_DEF)
def part(request: PartRequest) -> PartResponse:
    handler = PartHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/SetChannelKeyHandler", responses=RESPONSES_DEF)
def set_channel_key(request: SetChannelKeyRequest) -> SetChannelKeyResponse:
    raise NotImplementedError()


@router.post(f"{CHAT}/SetCKeyHandler", responses=RESPONSES_DEF)
def set_c_key(request: SetCKeyRequest) -> SetCKeyResponse:
    raise NotImplementedError()


@router.post(f"{CHAT}/SetGroupHandler", responses=RESPONSES_DEF)
def set_group(request: SetGroupRequest) -> Response:
    raise NotImplementedError()


@router.post(f"{CHAT}/TopicHandler", responses=RESPONSES_DEF)
def topic(request: TopicRequest) -> TopicResponse:
    handler = TopicHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/CryptHandler", responses=RESPONSES_DEF)
def crypt(request: CryptRequest) -> CryptResponse:
    handler = CryptHandler(request)
    handler.handle()
    return handler.response


# region Message


@router.post(f"{CHAT}/ATMHandler", responses=RESPONSES_DEF)
def atm(request: AtmRequest) -> AtmResponse:
    raise NotImplementedError()


@router.post(f"{CHAT}/NoticeHandler", responses=RESPONSES_DEF)
def notice(request: NoticeRequest) -> NoticeResponse:
    raise NotImplementedError()


@router.post(f"{CHAT}/PrivateHandler", responses=RESPONSES_DEF)
def private(request: PrivateRequest) -> PrivateResponse:
    handler = PrivateHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{CHAT}/UTMHandler", responses=RESPONSES_DEF)
def utm(request: UtmRequest) -> UtmResponse:
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
