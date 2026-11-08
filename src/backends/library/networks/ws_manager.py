import asyncio
import logging
from fastapi import WebSocket, WebSocketDisconnect

from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.library.network.websocket_brocker import WebsocketMessage
import secrets


class WebsocketManager:
    """
    ! Websocket is only for send message to client not receive any message from client
    client1 -> frontend1 -> backend1 (rest api)
    client2 <- |         <-
    client3 <- |          
                            -> (websocket) -> redis
    client2 -> frontend2 <- backend2    <-   |
    client3 -> frontend3 <- backend3    <-   |
    client4 -> frontend4 <- backend4    <-   |
    """
    client_pool: dict[str, WebSocket]
    logger: logging.Logger
    token_pool: dict[int, str]
    loop: asyncio.AbstractEventLoop

    def __init__(self) -> None:
        self.client_pool = {}
        self.logger = logging.getLogger("backend")
        self.token_pool = {}
        self.loop = asyncio.get_running_loop()

    def connect(self, ws: WebSocket):
        assert ws.client is not None
        token = secrets.token_urlsafe(32)
        self.token_pool[id(ws)] = token
        self.client_pool[token] = ws
        return token

    def disconnect(self, ws: WebSocket):
        assert ws.client is not None
        ws_id = id(ws)
        token = self.token_pool.get(ws_id)
        if token in self.client_pool:
            del self.client_pool[token]
            del self.token_pool[ws_id]
        return token

    def get_websocket(self, ip_address: str) -> WebSocket | None:
        if ip_address in self.client_pool:
            return self.client_pool[ip_address]

    def broadcast_all(self, message: WebsocketMessage):
        self._broadcast(message, list(self.client_pool.values()))

    def _broadcast(self, message: WebsocketMessage, wss: list[WebSocket]):
        self.logger.info(f"[cast] [send] {message}")
        for ws in wss:
            asyncio.run_coroutine_threadsafe(
                ws.send_json(message.model_dump(mode="json")), self.loop)
            # loop.create_task(ws.send_json(message.model_dump(mode="json")))

    def broadcast_selected(self, message: WebsocketMessage, wss: list[WebSocket]):
        self._broadcast(message, wss)

    async def accept_websocket(self, ws: WebSocket):
        """
        process websocket connection here
        """
        await ws.accept()
        if isinstance(ws, WebSocket) and ws.client is not None:
            self.connect(ws)
        try:
            while True:
                _ = await ws.receive_json()
        except WebSocketDisconnect:
            if ws.client is not None:
                # remove chat info by websocket
                self.disconnect(ws)
                GLOBAL_LOGGER.info(
                    f"websocket client: [{ws.client.host}:{ws.client.port} is disconnected")
