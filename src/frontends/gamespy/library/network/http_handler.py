from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import (
    ConnectionBase,
    NetworkServerBase,
)
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from frontends.gamespy.library.configs import CONFIG, ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter


class HttpConnection(ConnectionBase):
    handler: BaseHTTPRequestHandler

    def send(self, data: bytes) -> None:
        assert isinstance(data, bytes)
        self.handler.send_response(200)
        self.handler.send_header("Content-type", "text/xml")
        self.handler.end_headers()
        self.handler.wfile.write(data)


class HttpHandler(BaseHTTPRequestHandler):
    conn: HttpConnection

    def do_POST(self) -> None:
        # parsed_url = urlparse(self.path).geturl()
        content_length = int(self.headers["Content-Length"])
        data = self.rfile.read(content_length).decode()
        self.conn = HttpConnection(
            self, *self.server.unispy_params)  # type: ignore
        self.conn.on_received(data.encode())

    def log_message(self, format, *args):
        pass


class HttpServer(NetworkServerBase):
    def __init__(
        self, config: ServerConfig, t_client: type[ClientBase], logger: LogWriter
    ) -> None:
        super().__init__(config, t_client, logger)
        self._server = ThreadingHTTPServer(
            (self._config.listening_address,
             self._config.listening_port), HttpHandler
        )
        self._server.unispy_params = (  # type: ignore
            self._config,
            self._client_cls,
            self._logger)


class TestClient(ClientBase):
    def _create_switcher(self, buffer) -> None:
        # return super().create_switcher(buffer)
        print(buffer)
        pass

    def on_connected(self) -> None:
        # return super().on_connected()
        print("connected!")
        pass


if __name__ == "__main__":
    # create_http_server(list(CONFIG.servers.values())[0], ClientBase)
    from frontends.tests.gamespy.library.mock_objects import LogMock

    s = HttpServer(list(CONFIG.servers.values())[0], TestClient, LogMock())
