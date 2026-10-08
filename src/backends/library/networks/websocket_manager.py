import asyncio
import logging
from fastapi import WebSocket, WebSocketDisconnect


class WebsocketManager:
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
    client_pool: dict[str, WebSocket]
    logger: logging.Logger

    def __init__(self) -> None:
        self.client_pool = {}
        self.logger = logging.getLogger("backend")

    def get_address_str(self, ws: WebSocket) -> str:
        assert ws.client is not None
        ws_address = f"{ws.client.host}:{ws.client.port}"
        return ws_address

    def connect(self, ws: WebSocket):
        assert ws.client is not None
        ws_address = self.get_address_str(ws)
        self.client_pool[ws_address] = ws

    def disconnect(self, ws: WebSocket):
        assert ws.client is not None
        ws_address = self.get_address_str(ws)
        if ws_address in self.client_pool:
            del self.client_pool[ws_address]

    def get_websocket(self, ws_address: str) -> WebSocket | None:
        if ws_address in self.client_pool:
            return self.client_pool[ws_address]

    def broadcast(self, message: str):
        self._broadcast(message, list(self.client_pool.values()))

    def _broadcast(self, message: str, wss: list[WebSocket]):
        loop = asyncio.get_event_loop()
        self.logger.info(f"[cast] [send] {message}")
        for ws in wss:
            loop.create_task(ws.send_json(message))

    def broadcast_except(self, message: str, wss: list[WebSocket], except_ip: list[str]):
        filtered_wss = []
        for ws in wss:
            assert ws.client is not None
            if ws.client.host not in except_ip:
                filtered_wss.append(ws)
        self._broadcast(message, filtered_wss)

    @staticmethod
    async def websocket_listener(ws:WebSocket, manager:"WebsocketManager"):
        """
        this function is only use to establish connection with websocket
        do not process message or send message here
        """
        assert issubclass(type(manager),WebsocketManager)
        await ws.accept()
        if isinstance(ws, WebSocket) and ws.client is not None:
            manager.connect(ws)
        try:
            while True:
                _ = await ws.receive_json()
        except WebSocketDisconnect:
            if ws.client is not None:
                manager.disconnect(ws)
            # todo remove chat info by websocket
            print("Client disconnected")
