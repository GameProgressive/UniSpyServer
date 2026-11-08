from contextlib import asynccontextmanager
from fastapi import APIRouter, WebSocket, WebSocketDisconnect
from backends.library.database.pg_orm import get_engine
from backends.library.networks.redis_brocker import RedisBrocker
from frontends.gamespy.library.configs import CONFIG

import backends.protocols.gamespy.chat.data as data
from backends.library.networks.ws_manager import WebsocketManager as WsManager
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.library.network.websocket_brocker import WebsocketMessage
import secrets


class WebsocketManager(WsManager):
    """
    ! only send message to client, do not receive any message from frontend client \n
    client1 -> frontend1 -> backend1 (rest api)     \n
    client2 <- |         <-                         \n
    client3 <- |                                    \n
                            -> (websocket) -> redis \n
    client2 -> frontend2 <- backend2    <-   |      \n
    client3 -> frontend3 <- backend3    <-   |      \n
    client4 -> frontend4 <- backend4    <-   |      \n
    """

    def broadcast_channel_message(self, channel_name: str, message: WebsocketMessage):
        """
            create redis pubsub to share message cross all backends
            currently we simply implement without redis pubsub
        """
        tokens = data.get_user_tokens_by_channel(channel_name)
        # wss = {
        #     token: self.client_pool[token]
        #     for token in tokens
        #     if token in self.client_pool
        # }
        wss = []
        for token in tokens:
            ws = self.client_pool.get(token)
            wss.append(ws)

        self.broadcast_selected(message, wss)

    async def accept_websocket(self, ws: WebSocket):
        """
        process websocket connection here
        """
        await ws.accept()
        if isinstance(ws, WebSocket) and ws.client is not None:
            token = self.connect(ws)
            await ws.send_text(token)
        try:
            while True:
                _ = await ws.receive_json()
        except WebSocketDisconnect:
            if ws.client is not None:
                # remove chat info by websocket
                self.disconnect(ws)
                GLOBAL_LOGGER.info(
                    f"websocket client: [{ws.client.host}:{ws.client.port} is disconnected")


def handle_client_message(message: str):
    from backends.protocols.gamespy.chat.requests import MessageBroadcastRequest
    from backends.protocols.gamespy.chat.handlers import MessageBroadcastHandler
    try:
        request = MessageBroadcastRequest.model_validate_json(message)
        handler = MessageBroadcastHandler(request)
        handler.broadcast_local()
    except Exception as e:
        GLOBAL_LOGGER.error(str(e))


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
    _brocker = RedisBrocker("chat", CONFIG.redis.url, handle_client_message)
    _brocker.subscribe()
    yield
