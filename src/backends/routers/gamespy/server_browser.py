from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.protocols.gamespy.server_browser.handlers import ServerInfoHandler, ServerMainListHandler
from backends.protocols.gamespy.server_browser.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest
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
            websocket.send_text(f"Message text was: {data}")
        except WebSocketDisconnect:
            del client_pool[websocket.client.host]


@router.post(f"{SERVER_BROWSER_V2}/SendMessageHandler")
def send_message(request: SendMessageRequest):
    raise NotImplementedError()


@router.post(f"{SERVER_BROWSER_V2}/ServerInfoHandler")
def server_info(request: ServerInfoRequest):
    handler = ServerInfoHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerMainListHandler")
def server_list(request: ServerListRequest):
    handler = ServerMainListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{SERVER_BROWSER_V2}/ServerFullInfoListHandler")
def full_info_list(request: ServerListRequest):
    handler = ServerMainListHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
