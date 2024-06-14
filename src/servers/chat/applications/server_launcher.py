from library.abstractions.server_launcher_base import ServerLauncherBase
from library.network.tcp.tcp_handler import create_tcp_server
from library.unispy_server_config import CONFIG
from servers.chat.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["Chat"]

    def _launch_server(self):
        create_tcp_server(self.config, Client)


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
