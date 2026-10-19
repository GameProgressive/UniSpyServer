import socket
import socketserver

from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import (
    ConnectionBase,
    NetworkServerBase,
    ServerParams,
)
from frontends.gamespy.library.configs import CONFIG, ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter


class UdpConnection(ConnectionBase):
    def send(self, data: bytes) -> None:
        assert isinstance(data, bytes)
        conn: socket.socket = self.handler.request[1]
        conn.sendto(data, self.handler.client_address)


class UdpHandler(socketserver.BaseRequestHandler):
    request: tuple[bytes, socket.socket]
    conn: UdpConnection

    def handle(self) -> None:
        data = self.request[0]
        conn = UdpConnection(self, self.server.unispy_param)  # type: ignore
        conn.on_received(data)

    def send(self, data: bytes) -> None:
        conn: socket.socket = self.request[1]
        conn.sendto(data, self.client_address)





class UdpServer(NetworkServerBase):
    def __init__(self, param: ServerParams) -> None:
        super().__init__(param)
        self._server = socketserver.ThreadingUDPServer(
            (self.unispy_param.config.listening_address,
             self.unispy_param.config.listening_port),
            UdpHandler,
        )
        self._server.unispy_param = self.unispy_param  # type:ignore
        self._server.allow_reuse_address = True

    def __exit__(self, *args):
        self._server.__exit__(*args)


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
    # create_udp_server(list(CONFIG.servers.values())[0], ClientBase)
    from frontends.tests.gamespy.library.mock_objects import LogMock

    s_param = ServerParams(list(CONFIG.servers.values())
                           [0], TestClient, LogMock())
    s = UdpServer(s_param)
    s.start()
    pass
