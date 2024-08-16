import socketserver

import responses
from library.src.abstractions.client import ClientBase
from library.src.abstractions.connections import ConnectionBase
from library.src.abstractions.handler import CmdHandlerBase
from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from library.src.unispy_server_config import CONFIG
from servers.natneg.src.handlers.switcher import CmdSwitcher


class ConnectionMock(ConnectionBase):
    def send(self, data: bytes) -> None:
        pass


class RequestHandlerMock(socketserver.BaseRequestHandler):
    def __init__(self) -> None:
        pass

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


def create_mock_url(client: ClientBase, handler: type[CmdHandlerBase], data: dict) -> None:
    url = f"{
        CONFIG.backend.url}/{client.server_config.server_name}/{handler.__name__}/"
    responses.add(responses.POST, url, json=data, status=200)
