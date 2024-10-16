from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.protocols.gamespy.server_browser.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest
from backends.urls import SERVER_BROWSER_V1, SERVER_BROWSER_V2

router = APIRouter()
# todo maybe implement this in websocket way

client_pool: dict[str, WebSocket] = {}


@router.websocket(f"/{SERVER_BROWSER_V2}/AdHocHandler")
async def check(websocket: WebSocket):
    """
    notify every server browser to send message to its client
    """
    raise NotImplementedError()
    await websocket.accept()
    while True:
        try:
            data = await websocket.receive_text()
            client_pool[websocket.client.host] = websocket
            await websocket.send_text(f"Message text was: {data}")
        except WebSocketDisconnect:
            del client_pool[websocket.client.host]


@router.post(f"/{SERVER_BROWSER_V2}/SendMessageHandler")
async def send_message(request: SendMessageRequest):
    raise NotImplementedError()


@router.post(f"/{SERVER_BROWSER_V2}/ServerInfoHandler")
async def server_info(request: ServerInfoRequest):
    raise NotImplementedError()


@router.post(f"/{SERVER_BROWSER_V2}/ServerListHandler")
async def server_list(request: ServerListRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
