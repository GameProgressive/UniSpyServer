from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.udp_handler import UdpServer
from library.src.configs import CONFIG
from servers.presence_connection_manager.src.applications.client import Client


class ServerLauncher(ServerLauncherBase):

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["PresenceConnectionManager"]

    def _launch_server(self):
        UdpServer(self.config, Client, self.logger).start()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
