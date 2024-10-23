from library.src.abstractions.client import ClientBase
from typing import TYPE_CHECKING

from library.src.log.log_manager import LogWriter
from library.src.network.tcp_handler import TcpConnection
from library.src.configs import ServerConfig

if TYPE_CHECKING:
    from servers.chat.src.aggregates.channel import Channel


class ClientInfo:
    previously_joined_channel: str
    joined_channels: dict[str, "Channel"]
    nick_name: str
    gamename: str
    user_name: str


class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()
