from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
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
    s = ServerLauncher()
    s.start()
