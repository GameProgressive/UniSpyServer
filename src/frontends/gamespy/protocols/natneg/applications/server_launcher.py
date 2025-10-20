from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.natneg.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    server: UdpServer

    def __init__(self) -> None:
        super().__init__(
            config_name="NatNegotiation",
            client_cls=Client,
            server_cls=UdpServer,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    helper = DebugHelper(
        "./frontends/gamespy/protocols/natneg", ServerLauncher)
    helper.start()
