from fastapi import APIRouter, WebSocket, WebSocketDisconnect

from backends.protocols.gamespy.query_report.broker import MANAGER
from backends.protocols.gamespy.query_report.handlers import AvaliableHandler, Heartbeathandler, KeepAliveHandler
from backends.protocols.gamespy.query_report.requests import AvaliableRequest, ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest, KeepAliveRequest
from backends.urls import QUERY_REPORT

router = APIRouter()

@router.websocket(f"{QUERY_REPORT}/ws")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket) and ws.client is not None:
        MANAGER.connect(ws)
    try:
        while True:
            data = await ws.receive_json()
            msg = MANAGER.process_message(data)
            await MANAGER.broadcast(msg, ws)
    except WebSocketDisconnect:
        if ws.client is not None:
            MANAGER.disconnect(ws)
        # todo remove chat info by websocket
        print("Client disconnected")
@router.post(f"{QUERY_REPORT}/HeartBeatHandler")
def heartbeat(request: HeartBeatRequest):
    handler = Heartbeathandler(request)
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
