import asyncio
import logging
from fastapi import WebSocket, WebSocketDisconnect
from frontends.gamespy.library.network.websocket_brocker import WebsocketMessage


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
    loop: asyncio.AbstractEventLoop

    def __init__(self) -> None:
        self.client_pool = {}
        self.logger = logging.getLogger("backend")
        self.loop = asyncio.get_running_loop()

    def connect(self, ws: WebSocket, key: str):
        assert ws.client is not None
        self.client_pool[key] = ws

    def disconnect(self, ws: WebSocket, key: str):
        assert ws.client is not None
        if key in self.client_pool:
            del self.client_pool[key]

    def get_websocket(self, id: str) -> WebSocket | None:
        ws = self.client_pool.get(id)
        return ws

    def _broadcast(self, message: WebsocketMessage, wss: list[WebSocket]):
        self.logger.info(f"[cast] [send] {message}")
        for ws in wss:
            asyncio.run_coroutine_threadsafe(
                ws.send_json(message.model_dump(mode="json")), self.loop)

    def broadcast(self, message: WebsocketMessage, server_ids: list[str] | None = None):
        """
        broadcast message
        """
        wss: list[WebSocket] = []
        if server_ids is None:
            wss = list(self.client_pool.values())
        else:
            for id in server_ids:
                ws = self.client_pool.get(str(id))
                if ws is not None:
                    wss.append(ws)

        self._broadcast(message, wss)

    async def accept_websocket(self, ws: WebSocket, key: str):
        """
        process websocket connection here
        """
        await ws.accept()
        if isinstance(ws, WebSocket) and ws.client is not None:
            self.connect(ws, key)
        try:
            while True:
                _ = await ws.receive_json()
        except WebSocketDisconnect:
            if ws.client is not None:
                # remove chat info by websocket
                self.disconnect(ws, key)
                print(
                    f"websocket client: [{ws.client.host}:{ws.client.port} is disconnected")
