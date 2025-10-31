from fastapi import APIRouter

from backends.library.abstractions.contracts import RESPONSES_DEF

from backends.protocols.gamespy.chat.response import NicksResponse
from backends.protocols.gamespy.presence_search_player.handlers import CheckHandler, NewUserHandler, NicksHandler, OthersHandler, OthersListHandler, SearchHandler, SearchUniqueHandler, UniqueSearchHandler, ValidHandler

from backends.protocols.gamespy.presence_search_player.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest

from backends.protocols.gamespy.presence_search_player.responses import CheckResponse, NewUserResponse, OthersListResponse, OthersResponse, SearchResponse, SearchUniqueResponse, UniqueSearchResponse, ValidResponse

from backends.urls import PRESENCE_SEARCH_PLAYER

router = APIRouter()


@router.post(f"{PRESENCE_SEARCH_PLAYER}/CheckHandler", responses=RESPONSES_DEF)
def check(request: CheckRequest) -> CheckResponse:
    handler = CheckHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/NewUserHandler", responses=RESPONSES_DEF)
def new_user(request: NewUserRequest) -> NewUserResponse:
    handler = NewUserHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/NicksHandler", responses=RESPONSES_DEF)
def nicks(request: NicksRequest) -> NicksResponse:
    handler = NicksHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/OthersHandler", responses=RESPONSES_DEF)
def others(request: OthersRequest) -> OthersResponse:
    handler = OthersHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/OthersListHandler", responses=RESPONSES_DEF)
def others_list(request: OthersListRequest) -> OthersListResponse:
    handler = OthersListHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/PMatchHandler", responses=RESPONSES_DEF)
def player_match(request: dict):
    raise NotImplementedError()


@router.post(f"{PRESENCE_SEARCH_PLAYER}/SearchHandler", responses=RESPONSES_DEF)
def search(request: SearchRequest) -> SearchResponse:
    handler = SearchHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/SearchUniqueHandler", responses=RESPONSES_DEF)
def search_unique(request: SearchUniqueRequest) -> SearchUniqueResponse:
    handler = SearchUniqueHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/UniqueSearchHandler", responses=RESPONSES_DEF)
def unique_search(request: UniqueSearchRequest) -> UniqueSearchResponse:
    handler = UniqueSearchHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{PRESENCE_SEARCH_PLAYER}/ValidHandler", responses=RESPONSES_DEF)
def valid(request: ValidRequest) -> ValidResponse:
    handler = ValidHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
