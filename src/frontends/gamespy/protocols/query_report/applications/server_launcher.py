from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.query_report.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    natneg_channel: object

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["QueryReport"]

    def _launch_server(self):
        assert self.config is not None
        assert self.logger is not None
        self.server = UdpServer(self.config, Client, self.logger)
        super()._launch_server()


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
