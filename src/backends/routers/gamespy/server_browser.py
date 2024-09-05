from fastapi import APIRouter, WebSocket
from backends.protocols.gamespy.server_browser.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest
from backends.urls import SERVER_BROWSER_V1, SERVER_BROWSER_V2

router = APIRouter()
# todo maybe implement this in websocket way


@router.websocket(f"/{SERVER_BROWSER_V2}/AdHocHandler")
async def check(websocket: WebSocket):
    """
    notify every server browser to send message to its client
    """
    await websocket.accept()
    while True:
        data = await websocket.receive_text()
        await websocket.send_text(f"Message text was: {data}")
    raise NotImplementedError()


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
    uvicorn.run(router, host="0.0.0.0", port=8000)
