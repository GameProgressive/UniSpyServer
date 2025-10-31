from frontends.gamespy.library.abstractions.server_launcher import ServerFactory, ServerLauncherBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.presence_search_player.applications.client import (
    Client,
)


class ServerLauncher(ServerLauncherBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="PresenceSearchPlayer",
            client_cls=Client,
            server_cls=TcpServer
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    psp = ServerLauncher()
    helper = DebugHelper("./frontends/", ServerFactory([psp]))
    helper.start()
