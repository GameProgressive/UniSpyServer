
from frontends.gamespy.library.abstractions.client import ClientBase, ClientInfoBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.tcp_handler import TcpConnection
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.protocols.game_status.aggregations.gscrypt import GSCrypt


CHALLENGE_RESPONSE = "\\challenge\\00000000000000000000\\final\\"


class ClientInfo(ClientInfoBase):
    session_key: str | None
    game_name: str | None
    is_user_authenticated: bool
    is_player_authenticated: bool
    is_game_authenticated: bool
    profile_id: int | None
    game_session_key: str | None

    def __init__(self) -> None:
        super().__init__()
        self.session_key = None
        self.game_name = None
        self.profile_id = None
        self.game_session_key = None
        self.is_user_authenticated = False
        self.is_player_authenticated = False
        self.is_game_authenticated = False


class Client(ClientBase):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}

    def __init__(
        self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter
    ):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def on_connected(self) -> None:
        self.crypto = GSCrypt()
        self.log_network_sending(CHALLENGE_RESPONSE)
        enc_buffer = self.crypto.encrypt(CHALLENGE_RESPONSE.encode("ascii"))
        self.connection.send(enc_buffer)

    def decrypt_message(self, buffer: bytes) -> bytes:
        if self.crypto is None:
            return buffer

        temp = buffer.decode("ascii").split("\\final\\")[0]

        if len(temp) > 1:
            message = ""
            for t in temp:
                complete_buffer = (t + "\\final\\").encode()
                message += self.crypto.decrypt(complete_buffer).decode("ascii")
            return message.encode()

        return self.crypto.decrypt(buffer)

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.game_status.applications.switcher import (
            Switcher,
        )
        # ! sometime the request will decode with exception, something happend in sdk,
        # ! we do not process that here, let client login again will resolve the problem
        try:
            data_str = buffer.decode("ascii")
        except UnicodeDecodeError:
            self.log_warn(
                f"can not decode {buffer} with ascii, change decode method to latin1")
            data_str = buffer.decode("latin1")
        return Switcher(self, data_str)
