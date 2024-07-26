import socketserver
from library.src.abstractions.client import ClientBase
from library.src.abstractions.connections import ConnectionBase
from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from servers.natneg.src.handlers.switcher import CmdSwitcher


class ConnectionMock(ConnectionBase):
    def send(self, data: bytes) -> None:
        return print(data)


class RequestHandlerMock():
    client_address: tuple = ("192.168.0.1", 0)
    pass


class LogMock(LogWriter):
    def __init__(self) -> None:
        super().__init__(None)

    def debug(self, message):
        print(message)

    def info(self, message):
        print(message)

    def error(self, message):
        print(message)

    def warn(self, message):
        print(message)
