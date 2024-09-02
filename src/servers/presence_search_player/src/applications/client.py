from library.src.abstractions.client import ClientBase

from library.src.abstractions.switcher import SwitcherBase


class Client(ClientBase):

    def _create_switcher(self, buffer) -> SwitcherBase:
        from servers.presence_search_player.src.handlers.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer)

    def create_switcher(self, buffer: bytes) -> SwitcherBase:
        from servers.presence_search_player.src.handlers.switcher import CmdSwitcher
        return CmdSwitcher(self, buffer.decode())
