from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.udp_handler import UdpConnection
from frontends.gamespy.library.configs import ServerConfig


class Client(ClientBase):
    client_pool: dict[str, "Client"] = {}

    def __init__(self, connection: UdpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True

    def _create_switcher(self, buffer: bytes):
        assert isinstance(buffer, bytes)
        from frontends.gamespy.protocols.natneg.applications.switcher import Switcher

        return Switcher(self, buffer)
