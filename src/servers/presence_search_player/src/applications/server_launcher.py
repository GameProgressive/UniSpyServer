from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.tcp_handler import TcpServer
from library.src.configs import CONFIG
from servers.presence_search_player.src.applications.client import Client


class ServerLauncher(ServerLauncherBase):

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["PresenceSearchPlayer"]

    def _launch_server(self):
        TcpServer(self.config, Client, self.logger).start()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
