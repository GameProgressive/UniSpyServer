from typing import TYPE_CHECKING
from library.src.abstractions.client import ClientBase, ClientInfoBase
from library.src.abstractions.enctypt_base import EncryptBase
from servers.server_browser.src.v2.aggregations.enums import ServerListUpdateOption
if TYPE_CHECKING:
    from library.src.abstractions.switcher import SwitcherBase


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
        from servers.server_browser.src.v2.applications.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer)
