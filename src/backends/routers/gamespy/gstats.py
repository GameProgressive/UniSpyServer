from fastapi import FastAPI
from backends.protocols.gamespy.game_status.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from backends.urls import GAMESTATUS
app = FastAPI()


@app.post(f"{GAMESTATUS}/AuthGameHandler/")
async def update_item(request: AuthGameRequest):
    raise NotImplementedError()


@app.post(f"{GAMESTATUS}/AuthPlayerHandler/")
async def update_item(request: AuthPlayerRequest):
    raise NotImplementedError()


@app.post(f"{GAMESTATUS}/NewGameHandler/")
async def update_item(request: NewGameRequest):
    raise NotImplementedError()


@app.post(f"{GAMESTATUS}/GetPlayerDataHandler/")
async def update_item(request: GetPlayerDataRequest):
    raise NotImplementedError()


@app.post(f"{GAMESTATUS}/SetPlayerDataHandler/")
async def update_item(request: SetPlayerDataRequest):
    raise NotImplementedError()


@app.post(f"{GAMESTATUS}/UpdateGameHandler/")
async def update_item(request: UpdateGameRequest):
    raise NotImplementedError()

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
