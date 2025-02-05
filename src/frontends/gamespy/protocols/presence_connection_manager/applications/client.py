from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.tcp_handler import TcpConnection
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.protocols.presence_connection_manager.aggregates.login_challenge import (
    SERVER_CHALLENGE,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import LoginStatus
from frontends.gamespy.protocols.presence_connection_manager.aggregates.sdk_revision import SdkRevision
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import LoginStatus

LOGIN_TICKET = "0000000000000000000000__"
SESSION_KEY = 1111


class ClientInfo(ClientInfoBase):
    user_id: int
    profile_id: int
    sub_profile_id: int
    login_status: LoginStatus
    namespace_id: int
    sdk_revision: SdkRevision

    def __init__(self) -> None:
        super().__init__()
        self.login_status = LoginStatus.CONNECTED


class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}
    connection: TcpConnection

    def __init__(
        self,
        connection: TcpConnection,
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
        buffer = f"\\lc\\1\\challenge\\{SERVER_CHALLENGE}\\id\\1\\final\\".encode(
            "ascii"
        )
        self.log_network_sending(buffer)
        self.connection.send(buffer)

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.presence_connection_manager.applications.switcher import Switcher
        return Switcher(self, buffer.decode())
