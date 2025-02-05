import socketserver

import responses
from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.configs import CONFIG, ServerConfig


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


class BrokerMock(BrockerBase):
    def __init__(self) -> None:
        pass

    def subscribe(self):
        pass

    def publish_message(self, message):
        pass

    def unsubscribe(self):
        pass


def create_mock_url(config: ServerConfig, handler: type[CmdHandlerBase], data: dict) -> None:
    # fmt: off
    url = f"{CONFIG.backend.url}/GameSpy/{config.server_name}/{handler.__name__}"
    responses.add(responses.POST, url, json=data, status=200)
    # fmt: on
