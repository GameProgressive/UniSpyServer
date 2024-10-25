from fastapi import APIRouter

from backends.protocols.gamespy.presence_connection_manager.requests import KeepAliveRequest
from backends.protocols.gamespy.query_report.requests import AvaliableRequest, ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest
from backends.urls import QUERY_REPORT

router = APIRouter()


@router.post(f"/{QUERY_REPORT}/HeartBeatHandler")
async def heartbeat(request: HeartBeatRequest):
    raise NotImplementedError()


@router.post(f"/{QUERY_REPORT}/ChallengeHanler")
async def challenge(request: ChallengeRequest):
    raise NotImplementedError()


@router.post(f"/{QUERY_REPORT}/AvailableHandler")
async def available(request: AvaliableRequest):
    raise NotImplementedError()


@router.post(f"/{QUERY_REPORT}/ClientMessageAckHandler")
async def client_message(request: ClientMessageRequest):
    raise NotImplementedError()


@router.post(f"/{QUERY_REPORT}/EchoHandler")
async def echo(request: EchoRequest):
    raise NotImplementedError()


@router.post(f"/{QUERY_REPORT}/KeepAliveHandler")
async def keep_alive(request: KeepAliveRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)