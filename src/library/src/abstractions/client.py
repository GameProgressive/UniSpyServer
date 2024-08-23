import abc
from library.src.encryption.encoding import Encoding
from library.src.exceptions.error import UniSpyException
from library.src.log.log_manager import LogWriter
from library.src.log.log_manager import LogWriter
from library.src.unispy_server_config import ServerConfig

from typing import TYPE_CHECKING, Optional

if TYPE_CHECKING:
    from library.src.abstractions.handler import CmdHandlerBase
    from library.src.abstractions.connections import ConnectionBase
    from library.src.abstractions.switcher import SwitcherBase
    from library.src.abstractions.enctypt_base import EncryptBase
    from library.src.abstractions.contracts import ResponseBase
    from library.src.abstractions.client import ClientInfoBase
    from library.src.network.http_handler import HttpRequest


class ClientBase:
    server_config: ServerConfig
    connection: "ConnectionBase"
    logger: LogWriter
    crypto: Optional["EncryptBase"] = None
    info: "ClientInfoBase"
    is_log_raw: bool = False

    def __init__(
        self, connection: "ConnectionBase", server_config: ServerConfig, logger: LogWriter
    ):
        assert isinstance(server_config, ServerConfig)
        # assert isinstance(logger, LogWriter)
        self.server_config = server_config

        self.connection = connection
        self.logger = logger
        self._log_prefix = f"[{self.connection.remote_ip}:{
            self.connection.remote_port}]"

    def on_connected(self) -> None:
        pass

    def on_disconnected(self) -> None:
        pass

    @abc.abstractmethod
    def create_switcher(self, buffer) -> "SwitcherBase":
        pass

    def on_received(self, buffer: "Optional[bytes | HttpRequest]") -> None:
        if isinstance(buffer, bytes):
            if self.crypto is not None:
                buffer = self.crypto.decrypt(buffer)

        switcher: "SwitcherBase" = self.create_switcher(buffer)
        switcher.handle()

    def decrypt_message(self, buffer) -> bytes:
        if self.crypto is not None:
            return self.crypto.decrypt(buffer)
        else:
            return buffer

    def send(self, response: "ResponseBase") -> None:
        from library.src.abstractions.contracts import ResponseBase

        assert issubclass(type(response), ResponseBase)
        response.build()
        sending_buffer = response.sending_buffer
        if isinstance(sending_buffer, str):
            buffer: bytes = Encoding.get_bytes(sending_buffer)
        if isinstance(sending_buffer, bytes):
            buffer = sending_buffer
        else:
            raise UniSpyException("not supported buffer type")
        self.log_network_sending(buffer)

        if self.crypto is not None:
            buffer = self.crypto.encrypt(buffer)

        self.connection.send(buffer)

    def log_debug(self, message: str) -> None:
        self.logger.debug(f"{self._log_prefix}: {message}")

    def log_info(self, message: str) -> None:
        self.logger.info(f"{self._log_prefix}: {message}")

    def log_warn(self, message: str) -> None:
        self.logger.warn(f"{self._log_prefix}: {message}")

    def log_error(self, message: str) -> None:
        self.logger.error(f"{self._log_prefix}: {message}")

    def log_network_sending(self, data: object) -> None:
        self.logger.info(f"{self._log_prefix} [send]: {data}")

    def log_network_receving(self, data: object) -> None:
        self.logger.info(f"{self._log_prefix} [recv]: {data}")

    def log_current_class(self, object: "CmdHandlerBase") -> None:
        self.logger.debug(f"{self._log_prefix} [=>] <{
                          object.__class__.__name__}>")


class EasyTimer:
    def __init__(self, interval, refresh_interval) -> None:
        self.interval = interval
        self.refresh_interval = refresh_interval
        self.is_expired = False

    def elapsed(self, s, e) -> None:
        pass

    def start(self) -> None:
        pass

    def refresh_last_active_time(self) -> None:
        pass

    def dispose(self) -> None:
        pass


class ClientInfoBase:
    pass
