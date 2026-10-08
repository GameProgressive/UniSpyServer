from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter

CHALLENGE = b"\\basic\\\\secure\\000000"


class ClientInfo(ClientInfoBase):
    game_secret_key: str
    client_challenge: str
    game_name: str


class Client(ClientBase):
    is_log_raw: bool
    info: ClientInfo
    # crypto: EnctypeX | None

    def __init__(self, connection: ConnectionBase, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True
        self.info = ClientInfo()

    def on_connected(self) -> None:
        self.connection.send(CHALLENGE)
        self.log_network_sending(CHALLENGE)
        super().on_connected()

    def _create_switcher(self, buffer: bytes) -> "SwitcherBase":
        raise NotImplementedError()
        # from frontends.gamespy.protocols.server_browser.v2.applications.switcher import Switcher
        # return Switcher(self, buffer)
