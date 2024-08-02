import socket
import socketserver

from library.src.abstractions.client import ClientBase
from library.src.abstractions.connections import ConnectionBase, ServerBase
from library.src.extentions.string_extentions import IPEndPoint
from library.src.log.log_manager import LogWriter
from library.src.unispy_server_config import CONFIG, ServerConfig


class UdpConnection(ConnectionBase):
    def send(self, data) -> None:
        conn: socket.socket = self.handler.request[1]
        conn.sendto(data, self.handler.client_address)


class UdpHandler(socketserver.BaseRequestHandler):
    request: socket.socket
    conn: UdpConnection

    def handle(self) -> None:
        data = self.request[0]
        conn = UdpConnection(self, *self.server.handler_params)
        conn.on_received(data)

    def send(self, data: bytes) -> None:
        conn: socket.socket = self.request[1]
        conn.sendto(data, self.client_address)


class UdpServer(ServerBase):
    def start(self) -> None:
        self._server = socketserver.ThreadingUDPServer(
            (self._config.public_address, self._config.listening_port),
            UdpHandler,
        )
        self._server.handler_params = (self._config, self._t_client, self._logger)
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
    # create_udp_server(list(CONFIG.servers.values())[0], ClientBase)
    s = UdpServer(list(CONFIG.servers.values())[0], TestClient, None)
    s.start()
    pass
