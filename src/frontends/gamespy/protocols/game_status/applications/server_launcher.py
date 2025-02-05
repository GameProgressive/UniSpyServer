from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.library.configs import CONFIG


class ServerLauncher(ServerLauncherBase):
    server: "TcpServer"

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["GameStatus"]

    def _launch_server(self):
        assert self.config is not None
        assert self.logger is not None
        self.server = TcpServer(self.config, Client, self.logger)
        super()._launch_server()



if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
