from library.src.abstractions.server_launcher import ServerLauncherBase
from library.src.network.http_handler import HttpServer
from library.src.configs import CONFIG
from servers.web_services.src.applications.client import Client


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
