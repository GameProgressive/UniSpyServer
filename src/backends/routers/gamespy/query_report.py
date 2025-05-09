from fastapi import APIRouter

from backends.protocols.gamespy.presence_connection_manager.requests import KeepAliveRequest
from backends.protocols.gamespy.query_report.handlers import AvaliableHandler, Heartbeathandler
from backends.protocols.gamespy.query_report.requests import AvaliableRequest, ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest
from backends.urls import QUERY_REPORT

router = APIRouter()


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
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
