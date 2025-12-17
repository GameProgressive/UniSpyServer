from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory, ServiceBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.presence_search_player.applications.client import (
    Client,
)


class Service(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="PresenceSearchPlayer",
            client_cls=Client,
            network_server_cls=TcpServer
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    psp = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([psp]))
    helper.start()
