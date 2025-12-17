from fastapi import APIRouter, Request

from backends.library.abstractions.contracts import RESPONSES_DEF, Response
from backends.library.utils.misc import check_public_ip
from backends.protocols.gamespy.game_traffic_relay.handlers import (
    GtrHeartBeatHandler,
)
from backends.protocols.gamespy.game_traffic_relay.requests import (
    GtrHeartBeatRequest,
)
from backends.urls import GAME_TRAFFIC_RELAY
from frontends.gamespy.library.exceptions.general import UniSpyException

router = APIRouter()


@router.post(f"{GAME_TRAFFIC_RELAY}/Heartbeat", responses=RESPONSES_DEF)
def heartbeat(request: Request, heartbeat: GtrHeartBeatRequest) -> Response:
    assert request.client is not None
    check_public_ip(request.client.host, heartbeat.public_ip_address)

    handler = GtrHeartBeatHandler(heartbeat)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
