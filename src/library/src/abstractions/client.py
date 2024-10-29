from library.src.encryption.encoding import Encoding
from library.src.exceptions.general import UniSpyException
from library.src.log.log_manager import LogWriter
from library.src.log.log_manager import LogWriter
from library.src.configs import ServerConfig

from typing import TYPE_CHECKING, Optional

if TYPE_CHECKING:
    from library.src.abstractions.connections import ConnectionBase
    from library.src.abstractions.handler import CmdHandlerBase
    from library.src.abstractions.switcher import SwitcherBase
    from library.src.abstractions.enctypt_base import EncryptBase
    from library.src.abstractions.contracts import ResponseBase
    from library.src.abstractions.client import ClientInfoBase
    from library.src.network.http_handler import HttpRequest


class ClientInfoBase:
    pass


class ClientBase:
    server_config: ServerConfig
    connection: "ConnectionBase"
    logger: LogWriter
    crypto: Optional["EncryptBase"]
    info: "ClientInfoBase"
    is_log_raw: bool
    pool: dict[str, "ClientBase"]
    """
    Note: initialize in child class as class static member 
    """

    def __init__(
        self, connection: "ConnectionBase", server_config: ServerConfig, logger: LogWriter
    ):
        assert isinstance(server_config, ServerConfig)
        assert isinstance(logger, LogWriter)
        from library.src.abstractions.connections import ConnectionBase
        assert issubclass(type(connection), ConnectionBase)
        self.server_config = server_config
        self.connection = connection
        self.logger = logger
        self.crypto = None
        self.is_log_raw = False
        # fmt: off
        self._log_prefix = f"[{self.connection.ip_endpoint}]"
        # fmt: on

    def on_connected(self) -> None:
        # this operation will append the child client instance to the static member of childclass, if child class is overide the static member ClientBase.pool
        self.pool[self.connection.ip_endpoint] = self
        pass

    def on_disconnected(self) -> None:
        del self.pool[self.connection.ip_endpoint]
        pass

    def _create_switcher(self, buffer: bytes | str) -> "SwitcherBase":  # type: ignore
        """
        virtual method helps verify buffer type
        """
        assert isinstance(buffer, bytes) or isinstance(buffer, str)

    def on_received(self, buffer: bytes | str) -> None:
        if isinstance(buffer, bytes):
            if self.crypto is not None:
                buffer = self.crypto.decrypt(buffer)
        elif isinstance(buffer, str):
            pass
        else:
            raise UniSpyException("buffer type is invalid")
        self.log_network_receving(buffer)
        switcher: "SwitcherBase" = self._create_switcher(buffer)
        switcher.handle()

    def decrypt_message(self, buffer: bytes) -> bytes:
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
        elif isinstance(sending_buffer, bytes):
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
        print()
        pass

    def start(self) -> None:
        pass

    def refresh_last_active_time(self) -> None:
        pass

    def dispose(self) -> None:
        pass
