from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.tcp_handler import TcpServer
from library.src.configs import CONFIG
from servers.server_browser.src.v2.applications.client import Client


class ServerLauncher(ServerLauncherBase):

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["ServerBrowserV2"]

    def _launch_server(self):
        assert self.config is not None
        assert self.logger is not None
        self.server = TcpServer(self.config, Client, self.logger)
        super()._launch_server()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
