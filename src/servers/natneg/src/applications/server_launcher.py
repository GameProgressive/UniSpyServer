from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.udp_handler import UdpServer
from library.src.unispy_server_config import CONFIG
from servers.natneg.src.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    server: UdpServer

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["NatNegotiation"]

    def _launch_server(self):
        UdpServer(self.config, Client).start()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
