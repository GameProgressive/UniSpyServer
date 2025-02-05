from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.network.http_handler import HttpServer
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.web_services.applications.client import Client


class ServerLauncher(ServerLauncherBase):
    server: "HttpServer"

    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["WebServices"]

    def _launch_server(self):
        assert self.config is not None
        assert self.logger is not None
        self.server = HttpServer(self.config, Client, self.logger)
        super()._launch_server()


if __name__ == "__main__":

    s = ServerLauncher()
    s.start()
