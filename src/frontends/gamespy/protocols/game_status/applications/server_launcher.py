from frontends.gamespy.library.abstractions.server_launcher import (
    ServiceBase,
    ServicesFactory,
)
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.game_status.applications.client import Client


class Service(ServiceBase):
    server: "TcpServer"

    def __init__(self) -> None:
        super().__init__(
            config_name="GameStatus",
            client_cls=Client,
            network_server_cls=TcpServer,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    gs = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([gs]))
    helper.start()
