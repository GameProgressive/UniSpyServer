import abc
import socketserver
import threading
from frontends.gamespy.library.abstractions.client import ClientBase

from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.configs import ServerConfig


class ConnectionBase:
    remote_ip: str
    remote_port: int
    _is_started: bool
    config: ServerConfig
    t_client: type[ClientBase]
    logger: LogWriter
    handler: socketserver.BaseRequestHandler
    _client: ClientBase

    def __init__(
        self,
        handler: socketserver.BaseRequestHandler,
        config: ServerConfig,
        t_client: type[ClientBase],
        logger: LogWriter,
    ) -> None:
        super().__init__()
        assert isinstance(config, ServerConfig)
        self.handler = handler
        self.remote_ip = handler.client_address[0]
        self.remote_port = int(handler.client_address[1])
        self.ip_endpoint = f"{self.remote_ip}:{self.remote_port}"
        self.config = config
        self.t_client = t_client
        self.logger = logger
        self._client = self.t_client(self, self.config, self.logger)
        self._is_started = False

    def on_received(self, data: bytes) -> None:
        self._client.on_received(data)

    @abc.abstractmethod
    def send(self, data: bytes) -> None:
        assert isinstance(data, bytes)
        if not self._is_started:
            raise Exception("Server is not running.")
        assert isinstance(data, bytes)


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
    _config: ServerConfig
    _client_cls: type[ClientBase]
    _logger: LogWriter
    _server: socketserver.BaseServer

    def __init__(
        self, config: ServerConfig, t_client: type[ClientBase], logger: LogWriter
    ) -> None:
        assert isinstance(config, ServerConfig)
        assert issubclass(t_client, ClientBase)
        # assert isinstance(logger, LogWriter)
        self._config = config
        self._client_cls = t_client
        self._logger = logger

    def start(self):
        thread = threading.Thread(target=self._server.serve_forever)
        thread.start()

    def __del__(self):
        self._server.shutdown()
