from library.src.abstractions.client import ClientBase
from library.src.log.log_manager import LogWriter
from library.src.network.udp_handler import UdpConnection
from library.src.configs import ServerConfig


class Client(ClientBase):
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: UdpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True

    def create_switcher(self, buffer: bytes):
        assert isinstance(buffer, bytes)
        from servers.natneg.src.handlers.switcher import CmdSwitcher

        return CmdSwitcher(self, buffer)
