
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
                message += self.crypto.decrypt(complete_buffer).decode()
            return message.encode()

        return self.crypto.decrypt(buffer)

    def _create_switcher(self, buffer: bytes) -> SwitcherBase:
        from frontends.gamespy.protocols.game_status.applications.switcher import (
            Switcher,
        )

        return Switcher(self, buffer.decode())
