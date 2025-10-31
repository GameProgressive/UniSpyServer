from frontends.gamespy.library.abstractions.server_launcher import ServerFactory, ServerLauncherBase
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
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    qr = ServerLauncher()
    helper = DebugHelper("./frontends/", ServerFactory([qr]))
    helper.start()
