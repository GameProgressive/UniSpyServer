from fastapi import APIRouter, WebSocket
from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse
from backends.protocols.gamespy.query_report.broker import MANAGER, launch_brocker
from backends.protocols.gamespy.query_report.handlers import (
    AvaliableHandler, HeartbeatHandler, KeepAliveHandler)
from backends.protocols.gamespy.query_report.requests import (
    AvaliableRequest, ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest, KeepAliveRequest)
from backends.urls import QUERY_REPORT


router = APIRouter(lifespan=launch_brocker)


@router.websocket(f"{QUERY_REPORT}/ws")
async def websocket_endpoint(ws: WebSocket):
    await MANAGER.process_websocket(ws)


@router.post(f"{QUERY_REPORT}/HeartbeatHandler", responses=RESPONSES_DEF)
def heartbeat(request: HeartBeatRequest)->OKResponse:
    handler = HeartbeatHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{QUERY_REPORT}/ChallengeHanler", responses=RESPONSES_DEF)
def challenge(request: ChallengeRequest)->OKResponse:
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/AvailableHandler", responses=RESPONSES_DEF)
def available(request: AvaliableRequest)->OKResponse:
    handler = AvaliableHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{QUERY_REPORT}/ClientMessageAckHandler", responses=RESPONSES_DEF)
def client_message(request: ClientMessageRequest)->OKResponse:
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/EchoHandler", responses=RESPONSES_DEF)
def echo(request: EchoRequest)->OKResponse:
    raise NotImplementedError()


@router.post(f"{QUERY_REPORT}/KeepAliveHandler", responses=RESPONSES_DEF)
def keep_alive(request: KeepAliveRequest)->OKResponse:
    handler = KeepAliveHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
