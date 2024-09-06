from fastapi import APIRouter

from backends.protocols.gamespy.presence_search_player.requests import CheckRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest
from backends.urls import PRESENCE_SEARCH_PLAYER
from servers.presence_search_player.src.contracts.requests import NewUserRequest

router = APIRouter()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/CheckHandler")
async def check(request: CheckRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/NewUserHandler")
async def new_user(request: NewUserRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/NicksHandler")
async def nicks(request: NicksRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/OthersHandler")
async def others(request: OthersRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/OthersListHandler")
async def others_list(request: OthersListRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/PMatchHandler")
async def player_match(request: object):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/SearchHandler")
async def search(request: SearchRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/SearchUniqueHandler")
async def search_unique(request: SearchUniqueRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/UniqueSearchHandler")
async def unique_search(request: UniqueSearchRequest):
    raise NotImplementedError()


@router.post(f"/{PRESENCE_SEARCH_PLAYER}/ValidHandler")
async def valid(request: ValidRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)