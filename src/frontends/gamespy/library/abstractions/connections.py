import abc
import socketserver
import threading

from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter


class ServerParams:
    config: ServerConfig
    logger: LogWriter
    client_cls: type[ClientBase]

    def __init__(
        self,
        config: ServerConfig,
        client_cls: type[ClientBase],
        logger: LogWriter,
    ) -> None:
        assert isinstance(config, ServerConfig)
        assert issubclass(client_cls, ClientBase)
        self.config = config
        self.logger = logger
        self.client_cls = client_cls


class ConnectionBase:
    remote_ip: str
    remote_port: int
    _is_started: bool
    handler: socketserver.BaseRequestHandler
    _client: ClientBase
    ip_endpoint: str
    """
    <ip>:<port> string
    """
    unispy_param: ServerParams

    def __init__(
        self, handler: socketserver.BaseRequestHandler, param: ServerParams
    ) -> None:
        super().__init__()
        self.unispy_param = param
        self.handler = handler
        self.remote_ip = handler.client_address[0]
        self.remote_port = int(handler.client_address[1])
        self.ip_endpoint = f"{self.remote_ip}:{self.remote_port}"
        self._client = self.unispy_param.client_cls(
            self, self.unispy_param.config, self.unispy_param.logger
        )
        self._is_started = False

    def on_received(self, data: bytes) -> None:
        self._client.on_received(data)

    @abc.abstractmethod
    def send(self, data: object) -> None:
        assert data is not None
        if not self._is_started:
            raise Exception("Server is not running.")
        assert isinstance(data, object)


class UcpConnectionBase(ConnectionBase):
    pass


class TcpConnectionBase(ConnectionBase):
    @abc.abstractmethod
    def on_connected(self):
        pass

    @abc.abstractmethod
    def on_disconnected(self):
        pass

    @abc.abstractmethod
    def disconnect(self):
        pass


class HttpConnectionBase(TcpConnectionBase):
    pass


class NetworkServerBase:
    _server: socketserver.BaseServer
    unispy_param: ServerParams

    def __init__(self, param: ServerParams) -> None:
        self.unispy_param = param

    def start(self):
        """
        server non-blocking start
        """
        thread = threading.Thread(target=self._server.serve_forever)
        thread.start()

    def __del__(self):
        self._server.shutdown()

    def stop(self):
        self.__del__()
