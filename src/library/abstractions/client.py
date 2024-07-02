import abc
from library.encryption.encoding import Encoding
from library.log.log_manager import LogWriter
from library.log.log_manager import LogWriter
from library.unispy_server_config import ServerConfig

from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from library.abstractions.connections import ConnectionBase
    from library.abstractions.switcher import SwitcherBase
    from library.abstractions.enctypt_base import EncryptBase
    from library.abstractions.contracts import ResponseBase
    from library.abstractions.client import ClientInfoBase


class ClientBase(abc.ABC):
    server_config: "ServerConfig"
    connection: object
    logger: "LogWriter"
    crypto: "EncryptBase" = None
    info: "ClientInfoBase"
    is_log_raw: "bool" = False

    def __init__(
        self, connection: "ConnectionBase", server_config: "ServerConfig", logger: "LogWriter"
    ):
        assert isinstance(server_config, ServerConfig)
        # assert isinstance(logger, LogWriter)
        self.server_config = server_config
        from library.abstractions.connections import ConnectionBase

        self.connection: ConnectionBase = connection
        self.logger = logger

    def on_connected(self) -> None:
        pass

    def on_disconnected(self) -> None:
        self.__del__()

    @abc.abstractmethod
    def create_switcher(self, buffer) -> "SwitcherBase":
        pass

    def on_received(self, buffer) -> None:
        switcher: "SwitcherBase" = self.create_switcher(buffer)
        switcher.handle()

    def decrypt_message(self, buffer) -> bytes:
        if self.crypto is not None:
            return self.crypto.decrypt(buffer)
        else:
            return buffer

    def send(self, response: "ResponseBase") -> None:
        response.build()
        sending_buffer = response.sending_buffer
        if isinstance(sending_buffer, str):
            buffer = Encoding.get_bytes(sending_buffer)
        else:
            buffer = sending_buffer

        self.log_network_sending(buffer)

        if self.crypto is not None:
            buffer = self.crypto.encrypt(buffer)

        self.connection.send(buffer)

    def test_received(self, buffer) -> None:
        if self.crypto is not None:
            self.crypto = None

        self.on_received(buffer)

    def log_verbose(self, message: str) -> None:
        pass

    def log_info(self, message: str) -> None:
        pass

    def log_warn(self, message: str) -> None:
        pass

    def log_error(self, message: str) -> None:
        pass

    def log_network_sending(self, data: object) -> None:
        pass

    def log_network_receving(self, data: object) -> None:
        pass

    def log_current_class(self, object) -> None:
        pass


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
