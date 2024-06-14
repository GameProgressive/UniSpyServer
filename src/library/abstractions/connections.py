import abc
import socketserver
from library.abstractions.client import ClientBase
from library.extentions.string_extentions import IPEndPoint
from library.log.log_manager import LogWriter
from library.unispy_server_config import ServerConfig


class ConnectionBase(abc.ABC):
    remote_ip: str
    remote_port: int
    _is_started: bool = False
    config: ServerConfig
    t_client: type[ClientBase]
    logger: LogWriter
    handler: socketserver.BaseRequestHandler
    _client: ClientBase

    def __init__(
        self,
        handler: socketserver.BaseRequestHandler,
        config: ServerConfig,
        t_client: ClientBase,
        logger: LogWriter,
    ) -> None:
        super().__init__()
        assert isinstance(config, ServerConfig)
        assert issubclass(t_client, ClientBase)
        # assert isinstance(logger, LogWriter)
        assert issubclass(type(handler), socketserver.BaseRequestHandler)
        self.remote_ip = handler.client_address[0]
        self.remote_port = int(handler.client_address[1])
        self.config = config
        self.t_client = t_client
        self.logger = logger
        self._client = self.t_client(self, self.config, self.logger)

    def on_received(self, data: bytes) -> None:
        self._client.on_received(data)

    @abc.abstractmethod
    def send(self, data: bytes) -> None:
        if not self._is_started:
            raise Exception("Server is not running.")
        assert isinstance(data, bytes)


class UcpConnectionBase(ConnectionBase, abc.ABC):
    pass


class TcpConnectionBase(ConnectionBase, abc.ABC):
    @abc.abstractmethod
    def on_connected(self):
        pass

    @abc.abstractmethod
    def on_disconnected(self):
        pass

    @abc.abstractmethod
    def disconnect(self):
        pass


class HttpConnectionBase(TcpConnectionBase, abc.ABC):
    pass


class ServerBase(abc.ABC):
    _config: ServerConfig
    _t_client: type[ClientBase]
    _logger: LogWriter
    _server: socketserver.BaseServer

    def __init__(
        self, config: ServerConfig, t_client: type[ClientBase], logger: LogWriter
    ) -> None:
        assert isinstance(config, ServerConfig)
        assert issubclass(t_client, ClientBase)
        # assert isinstance(logger, LogWriter)
        self._config = config
        self._t_client = t_client
        self._logger = logger

    @abc.abstractclassmethod
    def start(self):
        pass

    def __del__(self):
        self._server.shutdown()
