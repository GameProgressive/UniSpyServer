from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from typing import TYPE_CHECKING
from pydantic import BaseModel
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import (
    ConnectionBase,
    NetworkServerBase,
)
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.http_data import HttpData



class HttpConnection(ConnectionBase):
    handler: BaseHTTPRequestHandler

    def send(self, data: HttpData) -> None:
        assert isinstance(data, HttpData)
        self.handler.send_response(200)
        if data.headers is not None:
            for key, value in data.headers.items():
                self.handler.send_header(key, str(value))
        self.handler.end_headers()
        body_bytes = data.body.encode()
        self.handler.wfile.write(body_bytes)
        self.handler.wfile.flush()
        self.handler.close_connection = True


class HttpHandler(BaseHTTPRequestHandler):
    conn: HttpConnection
    protocol_version = "HTTP/1.0"

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
    data = HttpData.model_validate_strings(
        '{"path": "/AuthService/AuthService.asmx", "headers": {"Content-Type": "text/xml", "SOAPAction": "http://gamespy.net/AuthService/CreateUserAccount", "GameID": "5989", "Host": "svz2and.auth.pubsvs.gamespy.com", "Content-Length": "1030"}, "body": "<?xml version=\\"1.0\\" encoding=\\"UTF-8\\"?>\\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=\\"http://schemas.xmlsoap.org/soap/envelope/\\" xmlns:SOAP-ENC=\\"http://schemas.xmlsoap.org/soap/encoding/\\" xmlns:xsi=\\"http://www.w3.org/2001/XMLSchema-instance\\" xmlns:xsd=\\"http://www.w3.org/2001/XMLSchema\\" xmlns:ns1=\\"http://gamespy.net/AuthService/\\">\\n  <SOAP-ENV:Body>\\n    <ns1:CreateUserAccount>\\n      <ns1:version>1</ns1:version>\\n      <ns1:partnercode>99</ns1:partnercode>\\n      <ns1:namespaceid>102</ns1:namespaceid>\\n      <ns1:email>4C84AA7017F21B0A5B83B56D6D06C86D22E93662@example.com</ns1:email>\\n      <ns1:profilenick>4C84AA7017F21B0A5B83</ns1:profilenick>\\n      <ns1:uniquenick>4C84AA7017F21B0A5B83B56D6D06C86D22E93662</ns1:uniquenick>\\n      <ns1:password>\\n        <ns1:Value>0AA62006A6228749A325F0CD8E4A86CFCA749DB0E8F0933E175C86B967118CF4D627267522156FD3F384D49895D7A527C8545B95C7BF09181A46EFF3EBB1FFC372BCDCB47286B91108FFC649041A7F4594AA6182D97890040F81FBE95B892AAB7135579EED692D71336F22C64DBC4F33CF2FCA3A57EB4E3F81169D4F6694F84B</ns1:Value>\\n      </ns1:password>\\n    </ns1:CreateUserAccount>\\n  </SOAP-ENV:Body>\\n</SOAP-ENV:Envelope>"}'
    )

    HttpData.model_validate_strings(data)
    # create_http_server(list(CONFIG.servers.values())[0], ClientBase)
    # from frontends.tests.gamespy.library.mock_objects import LogMock

    # s = HttpServer(list(CONFIG.servers.values())[0], TestClient, LogMock())
