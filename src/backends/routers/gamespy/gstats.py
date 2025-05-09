from fastapi import APIRouter
from backends.protocols.gamespy.game_status.handlers import *
from backends.urls import GAMESTATUS

router = APIRouter()


@router.post(f"{GAMESTATUS}/AuthGameHandler")
def auth_game(request: AuthGameRequest):
    handler = AuthGameHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/AuthPlayerHandler")
def auth_player(request: AuthPlayerRequest):
    handler = AuthPlayerHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/NewGameHandler")
def new_game(request: NewGameRequest):
    handler = NewGameHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/GetPlayerDataHandler")
def get_player_data(request: GetPlayerDataRequest):
    handler = GetPlayerDataHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/SetPlayerDataHandler")
def set_player_data(request: SetPlayerDataRequest):
    handler = SetPlayerDataHandler(request)
    handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/UpdateGameHandler")
def updaet_game(request: UpdateGameRequest):
    handler = UpdateGameHandler(request)
    handler.handle()
    return handler._response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
