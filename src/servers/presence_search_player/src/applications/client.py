from library.src.abstractions.client import ClientBase

from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from library.src.network.tcp_handler import TcpConnection
from library.src.configs import ServerConfig


class Client(ClientBase):
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)

    def _create_switcher(self, buffer) -> SwitcherBase:
        from servers.presence_search_player.src.applications.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer)
