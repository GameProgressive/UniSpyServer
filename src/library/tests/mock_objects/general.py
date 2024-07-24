import socketserver
from library.src.abstractions.client import ClientBase
from library.src.abstractions.connections import ConnectionBase
from library.src.log.log_manager import LogWriter


class ClientMock(ClientBase):
    pass


class ConnectionMock(ConnectionBase):
    def send(self, data: bytes) -> None:
        return print(data)


class RequestHandlerMock(socketserver.BaseRequestHandler):
    pass


class LogMock(LogWriter):
    def __init__(self) -> None:
        super().__init__(None)

    def info(self, message):
        print(message)

    def error(self, message):
        print(message)

    def warn(self, message):
        print(message)
