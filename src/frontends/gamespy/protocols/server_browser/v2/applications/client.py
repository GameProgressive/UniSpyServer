from typing import TYPE_CHECKING
from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.enctypt_base import EncryptBase
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import ServerListUpdateOption
if TYPE_CHECKING:
    from frontends.gamespy.library.abstractions.switcher import SwitcherBase


class ClientInfo(ClientInfoBase):
    game_secret_key: str
    client_challenge: str
    search_type: ServerListUpdateOption
    game_name: str


class Client(ClientBase):
    is_log_raw: bool = True
    info: ClientInfo
    crypto: EncryptBase

    def _create_switcher(self, buffer: bytes) -> "SwitcherBase":
        from frontends.gamespy.protocols.server_browser.v2.applications.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer)
