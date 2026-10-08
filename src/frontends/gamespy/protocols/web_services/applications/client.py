from http.server import BaseHTTPRequestHandler
from typing import cast

from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.http_handler import HttpConnection, HttpData


class ClientInfo(ClientInfoBase):
    pass


class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}

    def __init__(
        self, connection: HttpConnection, server_config: ServerConfig, logger: LogWriter
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()
        self.is_log_raw = False

    def on_received(self, buffer: bytes) -> None:
        """
        http server do not have custom encryption
        """
        if not isinstance(buffer, bytes):
            raise UniSpyException("buffer type is invalid")
        http_h = cast(BaseHTTPRequestHandler, self.connection.handler)
        data = HttpData(
            path=http_h.path, headers=dict(http_h.headers), body=buffer.decode()
        )
        http_raw = data.model_dump_json()
        self.log_network_receving(http_raw)
        switcher = self._create_switcher(data)
        if switcher is not None:
            switcher.handle()

    def _create_switcher(self, buffer: HttpData) -> SwitcherBase | None:
        """
        this function overide is different than super class \n
        http request need check route, if route url is not in our process list \n
        we log error and return None
        """
        import frontends.gamespy.protocols.web_services.modules.altas.applications.switcher as altas
        import frontends.gamespy.protocols.web_services.modules.auth.applications.switcher as auth
        import frontends.gamespy.protocols.web_services.modules.sake.applications.switcher as sake

        if buffer.path is None:
            raise Exception("invalid http request")
        if (
            "sakefileserver/uploadstream.aspx" in buffer.path
            or "SakeFileServer/download.aspx" in buffer.path
            or "SakeStorageServer/StorageServer.asmx" in buffer.path
        ):
            return sake.Switcher(self, buffer)
        elif "SakeStorageServer/Public/StorageServer.asmx" in buffer.path:
            return sake.Switcher(self, buffer)
        elif "AuthService/AuthService.asmx" in buffer.path:
            return auth.Switcher(self, buffer)
        elif (
            "/CompetitionService/CompetitionService.asmx" in buffer.path
            or "/AtlasDataServices/GameConfig.asmx" in buffer.path
        ):
            return altas.Switcher(self, buffer)
        else:
            self.log_error(f"unsupported url:{buffer.path}")
            return None
