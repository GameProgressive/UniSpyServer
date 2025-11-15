from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory, ServiceBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client


class Service(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="ServerBrowserV2", client_cls=Client, network_server_cls=TcpServer
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    sb2 = Service()
    # todo: add v1 server here
    helper = DebugHelper("./frontends/", ServicesFactory([sb2]))
    helper.start()
