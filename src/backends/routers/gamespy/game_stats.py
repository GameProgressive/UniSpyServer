from fastapi import APIRouter
from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse, Response
from backends.protocols.gamespy.game_status.handlers import (
    AuthGameHandler,
    AuthPlayerHandler,
    GetPlayerDataHandler,
    NewGameHandler,
    SetPlayerDataHandler,
    UpdateGameHandler,
)
from backends.protocols.gamespy.game_status.requests import (
    AuthGameRequest,
    AuthPlayerRequest,
    GetPlayerDataRequest,
    NewGameRequest,
    SetPlayerDataRequest,
    UpdateGameRequest,
)
from backends.protocols.gamespy.game_status.response import AuthGameResponse, AuthPlayerResponse, GetPlayerDataResponse
from backends.urls import GAMESTATUS

router = APIRouter()


@router.post(f"{GAMESTATUS}/AuthGameHandler", responses=RESPONSES_DEF)
def auth_game(request: AuthGameRequest) -> AuthGameResponse:
    handler = AuthGameHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{GAMESTATUS}/AuthPlayerHandler", responses=RESPONSES_DEF)
def auth_player(request: AuthPlayerRequest) -> AuthPlayerResponse:
    handler = AuthPlayerHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{GAMESTATUS}/NewGameHandler", responses=RESPONSES_DEF)
def new_game(request: NewGameRequest) -> OKResponse:
    handler = NewGameHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{GAMESTATUS}/GetPlayerDataHandler", responses=RESPONSES_DEF)
def get_player_data(request: GetPlayerDataRequest) -> GetPlayerDataResponse:
    handler = GetPlayerDataHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{GAMESTATUS}/SetPlayerDataHandler", responses=RESPONSES_DEF)
def set_player_data(request: SetPlayerDataRequest) -> Response:
    handler = SetPlayerDataHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{GAMESTATUS}/UpdateGameHandler", responses=RESPONSES_DEF)
def updaet_game(request: UpdateGameRequest) -> OKResponse:
    handler = UpdateGameHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
