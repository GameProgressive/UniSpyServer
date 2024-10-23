from fastapi import APIRouter
from backends.protocols.gamespy.game_status.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from backends.urls import GAMESTATUS
router = APIRouter()


@router.post(f"{GAMESTATUS}/AuthGameHandler")
async def auth_game(request: AuthGameRequest):
    raise NotImplementedError()


@router.post(f"{GAMESTATUS}/AuthPlayerHandler")
async def auth_player(request: AuthPlayerRequest):
    raise NotImplementedError()


@router.post(f"{GAMESTATUS}/NewGameHandler")
async def new_game(request: NewGameRequest):
    raise NotImplementedError()


@router.post(f"{GAMESTATUS}/GetPlayerDataHandler")
async def get_player_data(request: GetPlayerDataRequest):
    raise NotImplementedError()


@router.post(f"{GAMESTATUS}/SetPlayerDataHandler")
async def set_player_data(request: SetPlayerDataRequest):
    raise NotImplementedError()


@router.post(f"{GAMESTATUS}/UpdateGameHandler")
async def updaet_game(request: UpdateGameRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
