from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.http_handler import HttpServer
from frontends.gamespy.protocols.web_services.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    server: "HttpServer"

    def __init__(self) -> None:
        super().__init__(
            config_name="WebServices", client_cls=Client, server_cls=HttpServer
        )

if __name__ == "__main__":

    s = ServerLauncher()
    s.start()
