from fastapi import APIRouter, Request

from backends.library.abstractions.contracts import RESPONSES_DEF, Response
from backends.protocols.gamespy.game_traffic_relay.handlers import (
    GtrHeartBeatHandler,
)
from backends.protocols.gamespy.game_traffic_relay.requests import (
    GtrHeartBeatRequest,
)
from backends.protocols.gamespy.game_traffic_relay.responses import GetMyIPResponse
from backends.urls import GAME_TRAFFIC_RELAY

router = APIRouter()


@router.post(f"{GAME_TRAFFIC_RELAY}/get_my_ip", responses=RESPONSES_DEF)
def get_my_ip(request: Request) -> GetMyIPResponse:
    assert request.client
    return GetMyIPResponse(ip=request.client.host)


@router.post(f"{GAME_TRAFFIC_RELAY}/heartbeat", responses=RESPONSES_DEF)
def heartbeat(heartbeat: GtrHeartBeatRequest) -> Response:
    handler = GtrHeartBeatHandler(heartbeat)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
