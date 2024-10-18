from dataclasses import dataclass
import json
from typing import Optional
import uuid
from backends.urls import CHAT
from fastapi import APIRouter, FastAPI, WebSocket, WebSocketDisconnect

from servers.chat.src.aggregates.channel import BrockerMessage

router = APIRouter()
channels: dict[str, list[WebSocket]] = {"test": []}
"""
{"channel_name" : "list of WebSocket"}
"""
clients: dict[str, WebSocket] = {}
"""
{"client ip and port" : WebSocket}
"""



@router.post(f"{CHAT}/add_channel")
def add_channel(channel_name: str, server_id: uuid.UUID, server_ip: str):
    # first validate the server_id server_ip etc. info
    # if server is valid we initialize the channel

    # we initialize the channel
    if channel_name not in channels:
        channels[channel_name] = []


def check_request(request: str) -> Optional[BrockerMessage]:
    ch_msg = None
    try:
        request_dict = json.loads(request)
        ch_msg = BrockerMessage(**request_dict)
    except Exception as e:
        print(e)
        return None
    return ch_msg


async def multicast_message(ws: WebSocket):
    pass


@router.websocket(f"{CHAT}/Channel")
async def websocket_endpoint(ws: WebSocket):
    await ws.accept()
    if isinstance(ws, WebSocket):
        client_key = f"{ws.client.host}:{ws.client.port}"
        clients[client_key] = ws
    try:
        while True:
            request = await ws.receive_text()
            msg = check_request(request)
            if msg is None:
                return
            channels[msg.channel_name].append(ws)
            channel_clients: list[WebSocket] = channels[msg.channel_name]

            for client in channel_clients:
                # we do not send data to the publisher
                if client == ws:
                    continue
                await client.send_text(request)

    except WebSocketDisconnect:
        client_key = f"{ws.client.host}:{ws.client.port}"
        del clients[client_key]
        channels[msg.channel_name].remove(ws)
        print("Client disconnected")


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
