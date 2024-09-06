from library.src.abstractions.client import ClientBase

from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from library.src.network.tcp_handler import TcpConnection
from library.src.unispy_server_config import ServerConfig


class Client(ClientBase):
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)

    def _create_switcher(self, buffer) -> SwitcherBase:
        from servers.presence_search_player.src.handlers.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer)

    def create_switcher(self, buffer: bytes) -> SwitcherBase:
        from servers.presence_search_player.src.handlers.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer.decode())
