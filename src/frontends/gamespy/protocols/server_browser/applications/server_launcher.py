from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="ServerBrowserV2", client_cls=Client, server_cls=TcpServer
        )

if __name__ == "__main__":
    v2 = ServerLauncher()
    v2.start()
    # todo: add v1 server here

