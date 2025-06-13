from fastapi import APIRouter

from backends.protocols.gamespy.game_traffic_relay.handlers import (
    UpdateGTRServiceHandler,
)
from backends.protocols.gamespy.game_traffic_relay.requests import (
    UpdateGTRServiceRequest,
)
from backends.urls import GAME_TRAFFIC_RELAY

router = APIRouter()


@router.post(f"{GAME_TRAFFIC_RELAY}/heartbeat")
def heartbeat(request: UpdateGTRServiceRequest):
    handler = UpdateGTRServiceHandler(request)
    handler.handle()
    return


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
