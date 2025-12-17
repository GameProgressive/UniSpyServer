from contextlib import asynccontextmanager
from fastapi import APIRouter, WebSocket
from backends.library.database.pg_orm import ENGINE
from backends.library.networks.redis_brocker import RedisBrocker
from frontends.gamespy.library.configs import CONFIG

import backends.protocols.gamespy.chat.data as data
from sqlalchemy.orm import Session
from backends.library.networks.ws_manager import WebsocketManager as WsManager
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER


class WebsocketManager(WsManager):
    """
    current: single server mode
    client1 -> frontend1 -> backend1 (rest api)
    client2 <-  |        <-
    client3 <-  |
    """

    """
    future: distributed mode
    client1 -> frontend1 -> backend1 (rest api)
    client2 <- |         <-
    client3 <- |          
                            -> (websocket) -> redis
    client2 -> frontend2 <- backend2    <-   |
    client3 -> frontend3 <- backend3    <-   |
    client4 -> frontend4 <- backend4    <-   |
    """

    def _get_wss_in_channel(self, channel_name: str) -> list[WebSocket]:
        with Session(ENGINE) as session:
            ws_addrss = data.get_websocket_addr_by_channel_name(
                channel_name, session)
            wss = []
            for addr in ws_addrss:
                if addr in self.client_pool:
                    wss.append(self.client_pool[addr])
            return wss

    def broadcast_channel_message(self, channel_name: str, message: str, ws_client: WebSocket):
        """
            create redis pubsub to share message cross all backends
            currently we simply implement without redis pubsub
        """
        exclude_addr = self.get_address_str(ws_client)
        wss = self._get_wss_in_channel(channel_name)
        self.broadcast_except(message, wss, [exclude_addr])


def handle_client_message(message: str):
    from backends.protocols.gamespy.chat.requests import PublishMessageRequest
    from backends.protocols.gamespy.chat.handlers import PublishMessageHandler
    try:
        request = PublishMessageRequest.model_validate(message)
        PublishMessageHandler.broad_cast_loacl(request)
    except Exception as e:
        GLOBAL_LOGGER.error(str(e))


MANAGER = WebsocketManager()
BROCKER = RedisBrocker("chat", CONFIG.redis.url, handle_client_message)


@asynccontextmanager
async def launch_brocker(_: APIRouter):
    BROCKER.subscribe()
    yield
