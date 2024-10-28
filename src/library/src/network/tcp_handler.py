import socket
import socketserver
from typing import Optional
from library.src.abstractions.client import ClientBase
from library.src.abstractions.connections import ConnectionBase, NetworkServerBase

from library.src.network import DATA_SIZE
from library.src.configs import CONFIG


class TcpConnection(ConnectionBase):
    def send(self, data) -> None:
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
    conn: Optional[TcpConnection] = None

    def handle(self) -> None:
        if self.conn is None:
            self.conn = TcpConnection(self, *self.server.unispy_params)  # type: ignore
        self.conn.on_connected()
        while True:
            try:
                data = self.request.recv(DATA_SIZE)
                # ignore disconnect data
                if not data:
                    break
                self.conn.on_received(data)
            except ConnectionResetError:
                self.conn.on_disconnected()
            except Exception as e:
                print(e)
        pass


class TcpServer(NetworkServerBase):
    def start(self) -> None:
        self._server = socketserver.ThreadingTCPServer(
            (self._config.public_address, self._config.listening_port),
            TcpHandler,
        )
        self._server.allow_reuse_address = True
        self._server.unispy_params = (self._config, self._client_cls, self._logger)  # type: ignore
        self._server.serve_forever()


class TestClient(ClientBase):
    def create_switcher(self, buffer) -> None:
        # return super().create_switcher(buffer)
        print(buffer)
        pass

    def on_connected(self) -> None:
        # return super().on_connected()
        print("connected!")
        pass


if __name__ == "__main__":
    from library.tests.mock_objects import LogMock

    s = TcpServer(list(CONFIG.servers.values())[0], TestClient, LogMock())
    s.start()
    pass
