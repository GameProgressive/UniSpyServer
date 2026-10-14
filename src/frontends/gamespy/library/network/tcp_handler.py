import socket
import socketserver

from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import (
    ConnectionBase,
    NetworkServerBase,
    ServerParams,
)
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.network import DATA_SIZE


class TcpConnection(ConnectionBase):
    def send(self, data: bytes) -> None:
        assert isinstance(data, bytes)
        sock: socket.socket = self.handler.request
        sock.sendall(data)

    def on_connected(self) -> None:
        self._client.on_connected()

    def on_disconnected(self) -> None:
        self._client.on_disconnected()

    def disconnect(self) -> None:
        sock: socket.socket = self.handler.request
        sock.close()


class TcpHandler(socketserver.BaseRequestHandler):
    request: socket.socket
    conn: TcpConnection

    def handle(self) -> None:
        self.conn = TcpConnection(self, *self.server.unispy_params)  # type: ignore
        self.conn.on_connected()
        try:
            while True:
                data = self.request.recv(DATA_SIZE)
                # ignore disconnect data
                if not data:
                    self.conn.on_disconnected()
                    break
                self.conn.on_received(data)
        except ConnectionResetError:
            pass
        except Exception as e:
            if str(e):
                self.server.unispy_params[2].error(e)  # type: ignore
            else:
                self.server.unispy_params[2].error(type(e).__name__)  # type: ignore
        finally:
            self.conn.on_disconnected()


class TcpServer(NetworkServerBase):

    def __init__(self, param: ServerParams) -> None:
        super().__init__(param)
        self._server = socketserver.ThreadingTCPServer(
            (self._param.config.listening_address, self._param.config.listening_port),
            TcpHandler,
            bind_and_activate=False,
        )
        self._server.allow_reuse_address = True
        self._server.server_bind()
        self._server.server_activate()


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
    from frontends.tests.gamespy.library.mock_objects import LogMock

    s_param = ServerParams(list(CONFIG.servers.values())[0], TestClient, LogMock())
    s = TcpServer(s_param)
    s.start()
    pass
