from frontends.gamespy.library.abstractions.server_launcher import (
    ServiceBase,
    ServicesFactory,
)
from frontends.gamespy.library.network.udp_handler import UdpService
from frontends.gamespy.protocols.natneg.applications.client import Client


class Service(ServiceBase):
    server: UdpService

    def __init__(self) -> None:
        super().__init__(
            config_name="NatNegotiation",
            client_cls=Client,
            network_server_cls=UdpService,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    nn = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([nn]))
    helper.start()
