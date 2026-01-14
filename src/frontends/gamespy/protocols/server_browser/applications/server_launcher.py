from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory, ServiceBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client as ClientV2
from frontends.gamespy.protocols.server_browser.v1.applications.client import Client as ClientV1


class ServiceV2(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="ServerBrowserV2", client_cls=ClientV2, network_server_cls=TcpServer
        )


class ServiceV1(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="ServerBrowserV1", client_cls=ClientV1, network_server_cls=TcpServer
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    sb2 = ServiceV2()
    sb1 = ServiceV1()
    # todo: add v1 server here
    helper = DebugHelper("./frontends/", ServicesFactory([sb1, sb2]))
    helper.start()
