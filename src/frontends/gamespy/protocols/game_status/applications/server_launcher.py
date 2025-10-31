from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.library.abstractions.server_launcher import ServerFactory, ServerLauncherBase
from frontends.gamespy.library.network.tcp_handler import TcpServer


class ServerLauncher(ServerLauncherBase):
    server: "TcpServer"

    def __init__(self) -> None:
        super().__init__(
            config_name="GameStatus",
            client_cls=Client,
            server_cls=TcpServer,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    gs = ServerLauncher()
    helper = DebugHelper("./frontends/", ServerFactory([gs]))
    helper.start()
