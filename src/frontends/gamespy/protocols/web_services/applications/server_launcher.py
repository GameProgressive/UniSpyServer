from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory, ServiceBase
from frontends.gamespy.library.network.http_handler import HttpServer
from frontends.gamespy.protocols.web_services.applications.client import Client


class Service(ServiceBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="WebServices",
            client_cls=Client,
            network_server_cls=HttpServer
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    web = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([web]))
    helper.start()
