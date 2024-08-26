# from backends.urls import CHAT
# from fastapi import FastAPI, WebSocket

# app = FastAPI()


# @app.websocket(f"/{CHAT}/join")
# def join(request: "JoinRequest"):
#     # directly send the irc chat raw message to the channel
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/setckey")
# def setckey(request: "SetCKeyRequest"):
#     sender = session.get("nickname")
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/setchankey")
# def set_channel_key(request: "SetChannelKeyRequest"):
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/atm")
# def atm(request: "ATMRequest"):
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/utm")
# def utm(request: "UTMRequest"):
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/notice")
# def notice(request: "UTMRequest"):
#     raise NotImplementedError()


# @app.websocket(f"/{CHAT}/private")
# def notice(request: "PrivateRequest"):
#     raise NotImplementedError()
