from frontends.gamespy.library.abstractions.server_launcher import (
    ServiceBase,
    ServicesFactory,
)
from frontends.gamespy.library.network.udp_handler import UdpService
from frontends.gamespy.protocols.query_report.applications.client import Client


class Service(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="QueryReport",
            client_cls=Client,
            network_server_cls=UdpService
        )

    def start(self):
        super().start()
        self._start_websocket()


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    qr = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([qr]))
    helper.start()
