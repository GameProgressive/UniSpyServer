from backends.urls import CHAT
from fastapi import APIRouter, FastAPI, WebSocket, WebSocketDisconnect

router = APIRouter()
clients: list[WebSocket] = []


@router.websocket("/chat")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    if isinstance(websocket,WebSocket):
        clients.append(websocket)
    try:
        while True:
            message = await websocket.receive_text()
            print(message)
            for client in clients:
                await client.send_text(message)
    except WebSocketDisconnect:
        clients.remove(websocket)
        print("Client disconnected")


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
