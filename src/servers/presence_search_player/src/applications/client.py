from library.src.abstractions.client import ClientBase
from servers.presence_search_player.src.handlers.switcher import CmdSwitcher


class Client(ClientBase):

    def _create_switcher(self, buffer) -> CmdSwitcher:
        return CmdSwitcher(self, buffer)
