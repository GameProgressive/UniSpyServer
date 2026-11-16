from frontends.gamespy.library.abstractions.server_launcher import (
    ServiceBase,
    ServicesFactory,
)
from frontends.gamespy.library.network.tcp_handler import TcpService
from frontends.gamespy.protocols.game_status.applications.client import Client


class Service(ServiceBase):
    server: "TcpService"

    def __init__(self) -> None:
        super().__init__(
            config_name="GameStatus",
            client_cls=Client,
            network_server_cls=TcpService,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    gs = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([gs]))
    helper.start()
