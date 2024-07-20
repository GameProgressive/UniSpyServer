import socketserver
from library.src.extentions.string_extentions import IPEndPoint


class ConnectionListener:
    cookie: bytes
    ip_end_point: IPEndPoint

    def __init__(self, ip_end_point: IPEndPoint) -> None:
        assert isinstance(ip_end_point, IPEndPoint)
        self.ip_end_point = ip_end_point

    def start(self):
        with socketserver.ThreadingUDPServer(
            self.ip_end_point.ip, self.ip_end_point.port
        ) as s:
            s.serve_forever()
