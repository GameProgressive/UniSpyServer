from fastapi import APIRouter
from backends.protocols.gamespy.game_status.handlers import *
from backends.urls import GAMESTATUS

router = APIRouter()


@router.post(f"{GAMESTATUS}/AuthGameHandler")
async def auth_game(request: AuthGameRequest):
    handler = AuthGameHandler(request)
    await handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/AuthPlayerHandler")
async def auth_player(request: AuthPlayerRequest):
    handler = AuthPlayerHandler(request)
    await handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/NewGameHandler")
async def new_game(request: NewGameRequest):
    handler = NewGameHandler(request)
    await handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/GetPlayerDataHandler")
async def get_player_data(request: GetPlayerDataRequest):
    handler = GetPlayerDataHandler(request)
    await handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/SetPlayerDataHandler")
async def set_player_data(request: SetPlayerDataRequest):
    handler = SetPlayerDataHandler(request)
    await handler.handle()
    return handler._response


@router.post(f"{GAMESTATUS}/UpdateGameHandler")
async def updaet_game(request: UpdateGameRequest):
    handler = UpdateGameHandler(request)
    await handler.handle()
    return handler._response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
