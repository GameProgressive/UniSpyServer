from flask import session
from flask_socketio import SocketIO, emit

import socketio
from backends.urls import CHAT


# @socketio.on(f"/{CHAT}/join")
# def join(request: "JoinRequest"):
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/setckey")
# def setckey(request: "SetCKeyRequest"):
#     sender = session.get("nickname")
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/setchankey")
# def set_channel_key(request: "SetChannelKeyRequest"):
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/atm")
# def atm(request: "ATMRequest"):
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/utm")
# def utm(request: "UTMRequest"):
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/notice")
# def notice(request: "UTMRequest"):
#     raise NotImplementedError()


# @socketio.on(f"/{CHAT}/private")
# def notice(request: "PrivateRequest"):
#     raise NotImplementedError()
