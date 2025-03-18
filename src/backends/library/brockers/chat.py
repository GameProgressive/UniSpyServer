

from library.network.brockers import RedisBrocker
from fastapi import WebSocket
# init in fast api routers
REDIS_BROCKER = None

FRONTENDS: dict[str, WebSocket] = {}
"""
{"client ip and port" : WebSocket}
store the channel name and server websockets
"""
CHANNELS: dict[str, list[WebSocket]] = {}
"""
{"channel_name" : "list of WebSocket"}
store the frontends server websockets
"""


async def channel_publish(channel_name: str, message: dict):
    if channel_name in CHANNELS:
        for client in CHANNELS[channel_name]:
            await client.send(message)
