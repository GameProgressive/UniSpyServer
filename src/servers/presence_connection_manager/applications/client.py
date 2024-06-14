from library.abstractions.client import ClientBase, ClientInfoBase

from library.abstractions.connections import TcpConnectionBase
from library.abstractions.switcher import SwitcherBase
from library.log.log_manager import LogWriter
from library.unispy_server_config import ServerConfig
from servers.presence_connection_manager.aggregates.login_challenge import (
    SERVER_CHALLENGE,
)
from servers.presence_connection_manager.handlers.switcher import Switcher
from servers.presence_connection_manager.enums.general import LoginStatus

from typing import TYPE_CHECKING

from servers.presence_connection_manager.aggregates.sdk_revision import SdkRevision
from servers.presence_connection_manager.enums.general import LoginStatus

LOGIN_TICKET = "0000000000000000000000__"
SESSION_KEY = 1111


class ClientInfo(ClientInfoBase):
    user_id: int
    profile_id: int
    sub_profile_id: int
    login_status: LoginStatus = LoginStatus.CONNECTED
    namespace_id: int
    sdk_revision: SdkRevision


class Client(ClientBase):
    info: ClientInfo
    connection: TcpConnectionBase

    def __init__(
        self,
        connection: TcpConnectionBase,
        server_config: ServerConfig,
        logger: LogWriter,
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def on_connected(self) -> None:
        super().on_connected()
        if self.info.login_status != LoginStatus.CONNECTED:
            self.connection.disconnect()
            self.log_warn(
                "The server challenge has already been sent. Cannot send another login challenge."
            )
        self.info.login_status = LoginStatus.PROCESSING
        buffer = f"\\lc\\1\\challenge\\{SERVER_CHALLENGE}\\id\\1\\final\\"
        self.send(buffer)
        self.log_network_sending(buffer)

    def create_switcher(self, buffer) -> "SwitcherBase":
        return Switcher(self, buffer)
