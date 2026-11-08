import logging
from contextlib import asynccontextmanager

from fastapi import APIRouter

from backends.library.networks.redis_brocker import RedisBrocker
from backends.library.networks.ws_manager import WebsocketManager as WsManager
from backends.protocols.gamespy.query_report.handlers import ClientMessageHandler
from backends.protocols.gamespy.query_report.requests import ClientMessageRequest
from frontends.gamespy.library.configs import CONFIG

logger = logging.getLogger("backend")


class WebsocketManager(WsManager):
    pass


def handle_natneg_message(message: str):
    try:
        request = ClientMessageRequest.model_validate(message)
        handler = ClientMessageHandler(request)
        handler.handle()
    except Exception as e:
        logger.error(str(e))


_manager: WebsocketManager | None = None
_broker: RedisBrocker | None = None


def get_manager() -> WebsocketManager:
    assert _manager
    return _manager


def get_broker() -> RedisBrocker:
    assert _broker
    return _broker


@asynccontextmanager
async def launch_brocker(_: APIRouter):
    global _manager, _broker
    _manager = WebsocketManager()
    _broker = RedisBrocker(
        "master", CONFIG.redis.url, handle_natneg_message)
    _broker.subscribe()
    yield
