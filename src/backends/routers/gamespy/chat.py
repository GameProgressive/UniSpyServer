import json
from typing import Optional
from backends.protocols.gamespy.chat.requests import (ATMRequest, CdkeyRequest, GetCKeyRequest, GetChannelKeyRequest, GetKeyRequest, GetUdpRelayRequest, InviteRequest, JoinRequest, KickRequest,
                                                      ListRequest, LoginRequest, ModeRequest, NamesRequest, NickRequest, NoticeRequest, PartRequest, PrivateRequest, QuitRequest, SetChannelKeyRequest, SetGroupRequest, SetKeyRequest, TopicRequest, UTMRequest, UserIPRequest, UserRequest, WhoIsRequest, WhoRequest)
from backends.urls import CHAT
from fastapi import APIRouter, FastAPI, WebSocket, WebSocketDisconnect

from library.src.configs import ServerConfig
from servers.chat.src.aggregates.channel import BrockerMessage

router = APIRouter()
channels: dict[str, list[WebSocket]] = {"test": []}
"""
{"channel_name" : "list of WebSocket"}
"""
clients: dict[str, WebSocket] = {}
"""
{"client ip and port" : WebSocket}
"""


@router.post(f"{CHAT}/add_channel")
def add_channel(channel_name: str, config: ServerConfig):
    # first validate the server_id server_ip etc. info
    # if server is valid we initialize the channel

    # we initialize the channel
    if channel_name not in channels:
        channels[channel_name] = []


def check_request(request: str) -> Optional[BrockerMessage]:
    ch_msg = None
    try:
        request_dict = json.loads(request)
        ch_msg = BrockerMessage(**request_dict)
    except Exception as e:
        print(e)
        return None
    return ch_msg


async def multicast_message(ws: WebSocket):
    pass


@router.websocket(f"{CHAT}/Channel")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket) and ws.client is not None:
        client_key = f"{ws.client.host}:{ws.client.port}"
        clients[client_key] = ws
    try:
        while True:
            request = await ws.receive_text()
            msg = check_request(request)
            if msg is None:
                return
            channels[msg.channel_name].append(ws)
            channel_clients: list[WebSocket] = channels[msg.channel_name]

            for client in channel_clients:
                # we do not send data to the publisher
                if client == ws:
                    continue
                await client.send_text(request)

    except WebSocketDisconnect:
        if ws.client is not None:
            client_key = f"{ws.client.host}:{ws.client.port}"
            del clients[client_key]
            if msg is not None:
                channels[msg.channel_name].remove(ws)
        print("Client disconnected")


@router.post(f"{CHAT}/CdKeyHandler")
def cdkey(request: CdkeyRequest):
    pass


@router.post(f"{CHAT}/GetKeyHandler")
def getkey(request: GetKeyRequest):
    pass


@router.post(f"{CHAT}/GetUdpRelayHandler")
def get_udp_relay(request: GetUdpRelayRequest):
    pass

@router.post(f"{CHAT}/InviteHandler")
def invite(request: InviteRequest):
    pass


@router.post(f"{CHAT}/ListHandler")
def list_data(request: ListRequest):
    pass


@router.post(f"{CHAT}/LoginHandler")
def login(request: LoginRequest):
    pass


@router.post(f"{CHAT}/NickHandler")
def nick(request: NickRequest):
    pass


@router.post(f"{CHAT}/QuitHandler")
def quit(request: QuitRequest):
    pass


@router.post(f"{CHAT}/SetKeyHandler")
def set_key(request: SetKeyRequest):
    pass


@router.post(f"{CHAT}/UserHandler")
def user(request: UserRequest):
    pass


@router.post(f"{CHAT}/UserIPHandler")
def user_ip(request: UserIPRequest):
    pass


@router.post(f"{CHAT}/WhoHandler")
def who(request: WhoRequest):
    pass


@router.post(f"{CHAT}/WhoIsHandler")
def whois(request: WhoIsRequest):
    pass


# region channel
@router.post(f"{CHAT}/GetChannelKeyHandler")
def get_channel_key(request: GetChannelKeyRequest):
    pass


@router.post(f"{CHAT}/GetCKeyHandler")
def get_ckey(request: GetCKeyRequest):
    pass


@router.post(f"{CHAT}/JoinHandler")
def join(request: JoinRequest):
    pass


@router.post(f"{CHAT}/KickHandler")
def kick(request: KickRequest):
    pass


@router.post(f"{CHAT}/ModeHandler")
def mode(request: ModeRequest):
    pass


@router.post(f"{CHAT}/NamesHandler")
def names(request: NamesRequest):
    pass


@router.post(f"{CHAT}/PartHandler")
def part(request: PartRequest):
    pass


@router.post(f"{CHAT}/SetChannelKeyHandler")
def set_channel_key(request: SetChannelKeyRequest):
    pass


@router.post(f"{CHAT}/SetGroupHandler")
def set_group(request: SetGroupRequest):
    pass


@router.post(f"{CHAT}/TopicHandler")
def topic(request: TopicRequest):
    pass


@router.post(f"{CHAT}/ATMHandler")
def atm(request: ATMRequest):
    pass


@router.post(f"{CHAT}/NoticeHandler")
def notice(request: NoticeRequest):
    pass


@router.post(f"{CHAT}/PrivateHandler")
def private(request: PrivateRequest):
    pass


@router.post(f"{CHAT}/UTMHandler")
def utm(request: UTMRequest):
    pass


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8080)
