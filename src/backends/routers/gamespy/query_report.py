from fastapi import APIRouter

from backends.protocols.gamespy.query_report.requests import ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest
from backends.urls import QUERY_REPORT
from servers.presence_connection_manager.src.contracts.requests.general import KeepAliveRequest
from servers.query_report.src.v2.contracts.requests import AvaliableRequest

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
    uvicorn.run(router, host="0.0.0.0", port=8000)
