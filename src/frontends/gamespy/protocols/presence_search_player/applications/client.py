from frontends.gamespy.library.abstractions.client import ClientBase

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.tcp_handler import TcpConnection
from frontends.gamespy.library.configs import ServerConfig


class Client(ClientBase):
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
    
    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.presence_search_player.applications.switcher import CmdSwitcher
        temp_buffer = buffer.decode()
        return CmdSwitcher(self, temp_buffer)
