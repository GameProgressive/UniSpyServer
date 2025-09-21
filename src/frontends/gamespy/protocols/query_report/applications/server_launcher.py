from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.query_report.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    natneg_channel: object

    def __init__(self) -> None:
        super().__init__(
            config_name="QueryReport", 
            client_cls=Client, 
            server_cls=UdpServer
        )


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
