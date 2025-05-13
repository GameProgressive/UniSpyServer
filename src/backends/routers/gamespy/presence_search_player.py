from fastapi import APIRouter

from backends.protocols.gamespy.presence_search_player.handlers import CheckHandler, NewUserHandler, NicksHandler, OthersHandler, OthersListHandler, SearchHandler, SearchUniqueHandler, UniqueSearchHandler, ValidHandler
from backends.protocols.gamespy.presence_search_player.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest
from backends.urls import PRESENCE_SEARCH_PLAYER

router = APIRouter()


@router.post(f"{PRESENCE_SEARCH_PLAYER}/CheckHandler")
def check(request: CheckRequest):
    handler = CheckHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/NewUserHandler")
def new_user(request: NewUserRequest):
    handler = NewUserHandler(request)
    handler.handle()
    handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/NicksHandler")
def nicks(request: NicksRequest):
    handler = NicksHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/OthersHandler")
def others(request: OthersRequest):
    handler = OthersHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/OthersListHandler")
def others_list(request: OthersListRequest):
    handler = OthersListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/PMatchHandler")
def player_match(request: dict):
    raise NotImplementedError()


@router.post(f"{PRESENCE_SEARCH_PLAYER}/SearchHandler")
def search(request: SearchRequest):
    handler = SearchHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/SearchUniqueHandler")
def search_unique(request: SearchUniqueRequest):
    handler = SearchUniqueHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/UniqueSearchHandler")
def unique_search(request: UniqueSearchRequest):
    handler = UniqueSearchHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/ValidHandler")
def valid(request: ValidRequest):
    handler = ValidHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
