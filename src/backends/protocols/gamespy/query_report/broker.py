from contextlib import asynccontextmanager

from fastapi import APIRouter
from backends.protocols.gamespy.query_report.handlers import ClientMessageHandler
from backends.protocols.gamespy.query_report.requests import ClientMessageRequest
from backends.library.networks.redis_brocker import RedisBrocker
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER

from backends.library.networks.ws_manager import WebsocketManager as WsManager


class WebsocketManager(WsManager):
    pass


def handle_client_message(message: str):
    try:
        request = ClientMessageRequest.model_validate(message)
        handler = ClientMessageHandler(request)
        handler.handle()
    except Exception as e:
        GLOBAL_LOGGER.error(str(e))


MANAGER = WebsocketManager()
BROCKER = RedisBrocker("master", CONFIG.redis.url, handle_client_message)


@asynccontextmanager
async def launch_brocker(_: APIRouter):
    BROCKER.subscribe()
    yield
