from logging import Logger
import logging
from fastapi import WebSocket
from backends.library.database.pg_orm import ENGINE
from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage

import backends.protocols.gamespy.chat.data as data
from sqlalchemy.orm import Session


class ClientManager:
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
    logger: Logger

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

    def process_message(self, message: dict) -> BrockerMessage:
        self.logger.info(f"[cast] [recv] {message}")
        msg = BrockerMessage.model_validate(message)
        return msg

    # create redis pubsub to share message cross all backends
    # currently we simply implement without redis pubsub
    async def broadcast(self, message: BrockerMessage, ws_client: WebSocket):
        exclude_addr = self.get_address_str(ws_client)
        with Session(ENGINE) as session:
            wss = data.get_websocket_addr_by_channel_name(message.channel_name, session)
        if exclude_addr in wss:
            wss.remove(exclude_addr)
        for ws_addr in wss:
            if ws_addr in self.client_pool:
                ws = self.client_pool[ws_addr]
                await ws.send_json(message.model_dump_json())
                self.logger.info(f"[cast] [send] {message.model_dump_json()}")


MANAGER = ClientManager()
