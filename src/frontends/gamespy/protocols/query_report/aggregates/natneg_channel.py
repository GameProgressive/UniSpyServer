from typing import TYPE_CHECKING
from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.library.network.brockers import WebSocketBrocker
from frontends.gamespy.protocols.query_report.v2.applications.handlers import ClientMessageHandler
from frontends.gamespy.protocols.query_report.v2.contracts.requests import ClientMessageRequest
from types import MappingProxyType

if TYPE_CHECKING:
    from frontends.gamespy.protocols.query_report.applications.client import Client


class NatNegChannel:
    """
    todo send data to the ip endpoint when receive data, not find client from pool to save memory
    """
    broker: BrockerBase
    pool: MappingProxyType[str, "Client"]

    def __init__(self, broker_cls: type[BrockerBase] = WebSocketBrocker) -> None:
        self.broker = broker_cls(
            "natneg", f"{CONFIG.backend.url}/QueryReport/Channel", self.recieve_message)
        self.pool = MappingProxyType(Client.pool)

    def recieve_message(self, request: bytes):
        message = ClientMessageRequest(request)
        message.parse()
        client = None
        if message.target_ip_endpoint in self.pool:
            client = self.pool[message.target_ip_endpoint]

        if client is None:
            GLOBAL_LOGGER.warn(f"Client:{message.target_ip_address}:{
                               message.target_port} not found, we ignore natneg message from SB: {message.server_browser_sender_id}")
            return

        GLOBAL_LOGGER.info(f"Get client message from server browser: {
                           message.server_browser_sender_id} [{message.natneg_message}]")
        handler = ClientMessageHandler(client, message)
        handler.handle()
