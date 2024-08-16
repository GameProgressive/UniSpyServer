import socketserver

from library.src.network.udp_handler import UdpHandler


class ConnectionListener:
    cookie: bytes
    ip_addr: str
    port: int

    def __init__(self, ip_addr: str, port: int) -> None:
        assert isinstance(ip_addr, str)
        assert isinstance(port, int)
        self.ip_addr = ip_addr
        self.port = port

    def start(self):
        with socketserver.ThreadingUDPServer(
            (self.ip_addr, self.port), UdpHandler
        ) as s:
            s.serve_forever()
