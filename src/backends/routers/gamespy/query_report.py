from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.protocols.gamespy.query_report.broker import MANAGER, launch_brocker
from backends.protocols.gamespy.query_report.handlers import (
    AvaliableHandler, HeartbeatHandler, KeepAliveHandler)
from backends.protocols.gamespy.query_report.requests import (
    AvaliableRequest, ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest, KeepAliveRequest)
from backends.urls import QUERY_REPORT


router = APIRouter(lifespan=launch_brocker)


@router.websocket(f"{QUERY_REPORT}/ws")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket) and ws.client is not None:
        MANAGER.connect(ws)
    try:
        while True:
            _ = await ws.receive_json()
            # query report do not process ws client message
    except WebSocketDisconnect:
        if ws.client is not None:
            MANAGER.disconnect(ws)
        print("Client disconnected")


@router.post(f"{QUERY_REPORT}/HeartbeatHandler")
def heartbeat(request: HeartBeatRequest):
    handler = HeartbeatHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{QUERY_REPORT}/ChallengeHanler")
def challenge(request: ChallengeRequest):
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/AvailableHandler")
def available(request: AvaliableRequest):
    handler = AvaliableHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{QUERY_REPORT}/ClientMessageAckHandler")
def client_message(request: ClientMessageRequest):
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/EchoHandler")
def echo(request: EchoRequest):
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/KeepAliveHandler")
def keep_alive(request: KeepAliveRequest):
    handler = KeepAliveHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
