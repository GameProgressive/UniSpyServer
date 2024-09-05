from fastapi import FastAPI

from backends.protocols.gamespy.presence_search_player.requests import CheckRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest
from backends.urls import PRESENCE_SEARCH_PLAYER
from servers.presence_search_player.src.contracts.requests import NewUserRequest

app = FastAPI()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/CheckHandler")
def check(request: CheckRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/NewUserHandler")
def new_user(request: NewUserRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/NicksHandler")
def nicks(request: NicksRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/OthersHandler")
def others(request: OthersRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/OthersListHandler")
def others_list(request: OthersListRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/PMatchHandler")
def player_match(request: object):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/SearchHandler")
def search(request: SearchRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/SearchUniqueHandler")
def search_unique(request: SearchUniqueRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/UniqueSearchHandler")
def unique_search(request: UniqueSearchRequest):
    raise NotImplementedError()


@app.post(f"/{PRESENCE_SEARCH_PLAYER}/ValidHandler")
def valid(request: ValidRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
