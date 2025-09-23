from fastapi import APIRouter, Request

from backends.protocols.gamespy.game_traffic_relay.handlers import (
    GtrHeartBeatHandler,
)
from backends.protocols.gamespy.game_traffic_relay.requests import (
    GtrHeartBeatRequest,
)
from backends.urls import GAME_TRAFFIC_RELAY
from frontends.gamespy.library.exceptions.general import UniSpyException

router = APIRouter()


@router.get(f"{GAME_TRAFFIC_RELAY}/get_my_ip")
def get_my_ip(request: Request):
    assert request.client
    return {"ip": request.client.host}


@router.post(f"{GAME_TRAFFIC_RELAY}/heartbeat")
def heartbeat(heartbeat: GtrHeartBeatRequest):

    handler = GtrHeartBeatHandler(heartbeat)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
