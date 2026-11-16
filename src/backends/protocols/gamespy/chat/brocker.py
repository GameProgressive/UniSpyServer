from contextlib import asynccontextmanager
from fastapi import APIRouter
from backends.library.networks.redis_brocker import RedisBrocker
from frontends.gamespy.library.configs import CONFIG
from backends.library.networks.websocket_manager import WebsocketManager
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER


def handle_client_message(message: str):
    from backends.protocols.gamespy.chat.handlers import MessageBroadcastHandler
    MessageBroadcastHandler.handler_global_message(message)


_manager: WebsocketManager | None = None
_brocker: RedisBrocker | None = None


def get_manager() -> WebsocketManager:
    assert _manager
    return _manager


def get_brocker() -> RedisBrocker:
    assert _brocker
    return _brocker


@asynccontextmanager
async def launch_brocker(_: APIRouter):
    global _manager
    _manager = WebsocketManager()
    global _brocker
    _brocker = RedisBrocker("chat", CONFIG.redis.url,
                            GLOBAL_LOGGER, handle_client_message)
    _brocker.subscribe()
    yield
