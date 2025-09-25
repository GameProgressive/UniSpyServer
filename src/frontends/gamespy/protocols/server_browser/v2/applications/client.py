from typing import TYPE_CHECKING
from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.abstractions.enctypt_base import EncryptBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import ServerListUpdateOption
if TYPE_CHECKING:
    from frontends.gamespy.library.abstractions.switcher import SwitcherBase


class ClientInfo(ClientInfoBase):
    game_secret_key: str
    client_challenge: str
    search_type: ServerListUpdateOption
    game_name: str


class Client(ClientBase):
    is_log_raw: bool
    info: ClientInfo
    crypto: EncryptBase

    def __init__(self, connection: ConnectionBase, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True
        self.info = ClientInfo()

    def _create_switcher(self, buffer: bytes) -> "SwitcherBase":
        from frontends.gamespy.protocols.server_browser.v2.applications.switcher import Switcher
        return Switcher(self, buffer)
