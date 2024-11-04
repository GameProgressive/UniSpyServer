from library.src.abstractions.client import ClientBase

from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from library.src.network.tcp_handler import TcpConnection
from library.src.configs import ServerConfig

from typing import TYPE_CHECKING, Optional
if TYPE_CHECKING:
    from servers.chat.src.aggregates.channel import Channel


class ClientInfo:
    previously_joined_channel: Optional[str]
    joined_channels: dict[str, "Channel"]
    nick_name: Optional[str]
    gamename: Optional[str]
    user_name: Optional[str]

    def __init__(self) -> None:
        self.joined_channels = {}
        self.nick_name = None
        self.gamename = None
        self.user_name = None
        self.previously_joined_channel = None
    

class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"]

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from servers.chat.src.applications.switcher import Switcher
        return Switcher(self, buffer.decode())
