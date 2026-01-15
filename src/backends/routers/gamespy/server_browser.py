from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse
import backends.protocols.gamespy.server_browser.v2.handlers as v2
import backends.protocols.gamespy.server_browser.v1.handlers as v1

from backends.urls import SERVER_BROWSER_V1, SERVER_BROWSER_V2
router = APIRouter()
# todo maybe implement this in websocket way

client_pool: dict[str, WebSocket] = {}


@router.websocket(f"{SERVER_BROWSER_V2}/AdHocHandler")
def check(websocket: WebSocket):
    """
    notify every server browser to send message to its client
    """
    raise NotImplementedError()
    websocket.accept()
    while True:
        try:
            data = websocket.receive_text()
            client_pool[websocket.client.host] = websocket
            websocket.send_text(
                f"Message text was: {data}", responses=RESPONSES_DEF)
        except WebSocketDisconnect:
            del client_pool[websocket.client.host]
# region V1


@router.post(f"{SERVER_BROWSER_V1}/ServerInfoHandler", responses=RESPONSES_DEF)
def server_info_v1(request: v1.ServerListRequest):
    handler = v1.ServerInfoHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V1}/ServerListCompressHandler", responses=RESPONSES_DEF)
def server_list_v1(request: v1.ServerListRequest):
    handler = v1.ServerListCompressHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V1}/GroupListHandler", responses=RESPONSES_DEF)
def group_list_v1(request: v1.ServerListRequest):
    handler = v1.GroupListHandler(request)
    handler.handle()
    return handler.response


# region V2


@router.post(f"{SERVER_BROWSER_V2}/SendMessageHandler", responses=RESPONSES_DEF)
def send_message_v2(request: v2.SendMessageRequest) -> OKResponse:
    raise NotImplementedError()


@router.post(f"{SERVER_BROWSER_V2}/P2PGroupRoomListHandler", responses=RESPONSES_DEF)
def p2p_group_room_list_v2(request: v2.ServerListRequest) -> v2.P2PGroupRoomListResponse:
    handler = v2.P2PGroupRoomListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerInfoHandler", responses=RESPONSES_DEF)
def server_info_v2(request: v2.ServerInfoRequest) -> v2.ServerInfoResponse:
    handler = v2.ServerInfoHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerMainListHandler", responses=RESPONSES_DEF)
def server_list_v2(request: v2.ServerListRequest) -> v2.ServerMainListResponse:
    """ we send all server data to client to make it have HAS_FULL_RULES_FLAG 
        and will not send ServerBrowserAuxUpdateServer(sb, server, async, fullUpdate) to 
    """
    handler = v2.ServerMainListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerFullInfoListHandler", responses=RESPONSES_DEF)
def full_info_list_v2(request: v2.ServerListRequest) -> v2.ServerFullInfoListResponse:
    handler = v2.ServerFullInfoListHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
