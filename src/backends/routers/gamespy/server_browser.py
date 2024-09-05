from fastapi import FastAPI
from backends.protocols.gamespy.server_browser.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest
from backends.urls import SERVER_BROWSER_V1, SERVER_BROWSER_V2
app = FastAPI

# todo maybe implement this in websocket way
# @app.post(f"/{SERVER_BROWSER_V2}/AdHocHandler")
# def check(request: ADHocRequest):
#     raise NotImplementedError()


@app.post(f"/{SERVER_BROWSER_V2}/SendMessageHandler")
def send_message(request: SendMessageRequest):
    raise NotImplementedError()


@app.post(f"/{SERVER_BROWSER_V2}/ServerInfoHandler")
def server_info(request: ServerInfoRequest):
    raise NotImplementedError()


@app.post(f"/{SERVER_BROWSER_V2}/ServerListHandler")
def server_list(request: ServerListRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
