from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.tcp_handler import TcpServer
from library.src.unispy_server_config import CONFIG
from servers.chat.src.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    server: "TcpServer"

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["Chat"]

    def _launch_server(self):
        TcpServer(self.config, Client).start()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
