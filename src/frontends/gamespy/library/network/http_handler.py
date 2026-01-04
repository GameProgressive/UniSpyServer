from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import (
    ConnectionBase,
    NetworkServerBase,
)
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.log.log_manager import LogWriter
import json


class HttpData:
    path: str | None
    headers: dict | None
    body: str | None

    def __init__(
        self,
        path: str | None = None,
        headers: dict | None = None,
        body: str | None = None,
    ) -> None:
        self.path = path
        self.headers = headers
        self.body = body

    @staticmethod
    def from_str(raw: str) -> "HttpData":
        """
        create http data object from a json str
        """
        try:
            data: dict = json.loads(raw)
            h_data = HttpData.from_dict(data)
            return h_data
        except Exception as e:
            raise UniSpyException(str(e))

    @staticmethod
    def from_dict(raw: dict) -> "HttpData":
        assert isinstance(raw, dict)
        body = raw.get("body")
        path = raw.get("path")
        headers = raw.get("headers")
        h_data = HttpData(body=body, path=path, headers=headers)
        return h_data

    @staticmethod
    def from_bytes(raw: bytes) -> "HttpData":
        """
        convert bytes to HttpData
        """
        return HttpData.from_str(raw.decode())

    def to_bytes(self):
        """
        convert HttpData to bytes
        """
        http_str = str(self)
        return http_str.encode()

    def __str__(self) -> str:
        """convert HttpData to string representation"""
        http_str = json.dumps(self.__dict__)
        return http_str


class HttpConnection(ConnectionBase):
    handler: BaseHTTPRequestHandler

    def send(self, data: bytes) -> None:
        assert isinstance(data, bytes)
        http_data = HttpData.from_str(data.decode())
        self.handler.send_response(200)
        self.handler.send_header("Content-type", "application/xml")
        if http_data.headers is not None:
            for key, value in http_data.headers.items():
                self.handler.send_header(key, value)
        self.handler.end_headers()
        if http_data.body is None:
            raise UniSpyException("soap body should not be null")
        body_bytes = http_data.body.encode()
        self.handler.wfile.write(body_bytes)


class HttpHandler(BaseHTTPRequestHandler):
    conn: HttpConnection

    def do_POST(self) -> None:
        content_length = int(self.headers["Content-Length"])
        data = self.rfile.read(content_length).decode()
        self.conn = HttpConnection(self, *self.server.unispy_params)  # type: ignore
        self.conn.on_received(data.encode())

    def log_message(self, format, *args):
        """
        override BaseHTTPRequestHandler to not print any log
        """
        pass


class HttpServer(NetworkServerBase):
    def __init__(
        self, config: ServerConfig, t_client: type[ClientBase], logger: LogWriter
    ) -> None:
        super().__init__(config, t_client, logger)
        self._server = ThreadingHTTPServer(
            (self._config.listening_address, self._config.listening_port), HttpHandler
        )
        self._server.unispy_params = (  # type: ignore
            self._config,
            self._client_cls,
            self._logger,
        )


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
    # HttpData(body="hello", headers={"session": "1"}, path="data")
    # HttpData.from_bytes(b"\n\n\nhello")
    data = HttpData.from_str(
        '{"path": "/AuthService/AuthService.asmx", "headers": {"Content-Type": "text/xml", "SOAPAction": "http://gamespy.net/AuthService/CreateUserAccount", "GameID": "5989", "Host": "svz2and.auth.pubsvs.gamespy.com", "Content-Length": "1030"}, "body": "<?xml version=\\"1.0\\" encoding=\\"UTF-8\\"?>\\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=\\"http://schemas.xmlsoap.org/soap/envelope/\\" xmlns:SOAP-ENC=\\"http://schemas.xmlsoap.org/soap/encoding/\\" xmlns:xsi=\\"http://www.w3.org/2001/XMLSchema-instance\\" xmlns:xsd=\\"http://www.w3.org/2001/XMLSchema\\" xmlns:ns1=\\"http://gamespy.net/AuthService/\\">\\n  <SOAP-ENV:Body>\\n    <ns1:CreateUserAccount>\\n      <ns1:version>1</ns1:version>\\n      <ns1:partnercode>99</ns1:partnercode>\\n      <ns1:namespaceid>102</ns1:namespaceid>\\n      <ns1:email>4C84AA7017F21B0A5B83B56D6D06C86D22E93662@example.com</ns1:email>\\n      <ns1:profilenick>4C84AA7017F21B0A5B83</ns1:profilenick>\\n      <ns1:uniquenick>4C84AA7017F21B0A5B83B56D6D06C86D22E93662</ns1:uniquenick>\\n      <ns1:password>\\n        <ns1:Value>0AA62006A6228749A325F0CD8E4A86CFCA749DB0E8F0933E175C86B967118CF4D627267522156FD3F384D49895D7A527C8545B95C7BF09181A46EFF3EBB1FFC372BCDCB47286B91108FFC649041A7F4594AA6182D97890040F81FBE95B892AAB7135579EED692D71336F22C64DBC4F33CF2FCA3A57EB4E3F81169D4F6694F84B</ns1:Value>\\n      </ns1:password>\\n    </ns1:CreateUserAccount>\\n  </SOAP-ENV:Body>\\n</SOAP-ENV:Envelope>"}'
    )

    http_raw = str(data)
    HttpData.from_str(http_raw)
    # create_http_server(list(CONFIG.servers.values())[0], ClientBase)
    # from frontends.tests.gamespy.library.mock_objects import LogMock

    # s = HttpServer(list(CONFIG.servers.values())[0], TestClient, LogMock())
