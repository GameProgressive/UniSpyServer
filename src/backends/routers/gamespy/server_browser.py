from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse
from backends.protocols.gamespy.server_browser.handlers import P2PGroupRoomListHandler, ServerFullInfoListHandler, ServerInfoHandler, ServerMainListHandler
from backends.protocols.gamespy.server_browser.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest
from backends.protocols.gamespy.server_browser.responses import P2PGroupRoomListResponse, ServerFullInfoListResponse, ServerInfoResponse, ServerMainListResponse
from backends.urls import SERVER_BROWSER_V2

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


@router.post(f"{SERVER_BROWSER_V2}/SendMessageHandler", responses=RESPONSES_DEF)
def send_message(request: SendMessageRequest) -> OKResponse:
    raise NotImplementedError()


@router.post(f"{SERVER_BROWSER_V2}/P2PGroupRoomListHandler", responses=RESPONSES_DEF)
def p2p_group_room_list(request: ServerListRequest) -> P2PGroupRoomListResponse:
    handler = P2PGroupRoomListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerInfoHandler", responses=RESPONSES_DEF)
def server_info(request: ServerInfoRequest) -> ServerInfoResponse:
    handler = ServerInfoHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerMainListHandler", responses=RESPONSES_DEF)
def server_list(request: ServerListRequest) -> ServerMainListResponse:
    """ we send all server data to client to make it have HAS_FULL_RULES_FLAG 
        and will not send ServerBrowserAuxUpdateServer(sb, server, async, fullUpdate) to 
    """
    handler = ServerMainListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerFullInfoListHandler", responses=RESPONSES_DEF)
def full_info_list(request: ServerListRequest) -> ServerFullInfoListResponse:
    handler = ServerFullInfoListHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
