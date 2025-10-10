import asyncio
from logging import Logger
import logging
from fastapi import WebSocket
from backends.library.database.pg_orm import ENGINE
from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage

import backends.protocols.gamespy.chat.data as data
from sqlalchemy.orm import Session
from backends.library.networks.ws_manager import WebsocketManager as WsManager


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

    def process_message(self, message: dict) -> BrockerMessage:
        self.logger.info(f"[cast] [recv] {message}")
        msg = BrockerMessage.model_validate(message)
        return msg

    def _get_wss_in_channel(self, channel_name: str) -> list[WebSocket]:
        with Session(ENGINE) as session:
            ws_addrss = data.get_websocket_addr_by_channel_name(
                channel_name, session)
            wss = []
            for addr in ws_addrss:
                if addr in self.client_pool:
                    wss.append(self.client_pool[addr])
            return wss

    def broadcast_channel_message(self, message: BrockerMessage, ws_client: WebSocket):
        """
            create redis pubsub to share message cross all backends
            currently we simply implement without redis pubsub
        """
        exclude_addr = self.get_address_str(ws_client)
        wss = self._get_wss_in_channel(message.channel_name)
        message_str = message.model_dump_json()
        self.broadcast_except(message_str, wss, [exclude_addr])


MANAGER = WebsocketManager()
