from typing import Optional
from library.src.abstractions.client import ClientBase, ClientInfoBase
from library.src.abstractions.switcher import SwitcherBase
from library.src.log.log_manager import LogWriter
from library.src.network.tcp_handler import TcpConnection
from library.src.configs import ServerConfig
from servers.game_status.src.aggregations.gscrypt import GSCrypt


CHALLENGE_RESPONSE = "\\challenge\\00000000000000000000\\final\\"


class ClientInfo(ClientInfoBase):
    session_key: Optional[str]
    game_name: Optional[str]
    is_user_authenticated: bool
    is_player_authenticated: bool
    is_game_authenticated: bool
    profile_id: Optional[int]
    game_session_key: Optional[str]

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

    def __init__(self, connection: TcpConnection, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.info = ClientInfo()

    def on_connected(self) -> None:
        self.crypto = GSCrypt()
        self.log_network_sending(CHALLENGE_RESPONSE)
        self.connection.send(CHALLENGE_RESPONSE.encode("ascii"))

    def decrypt_message(self, buffer: bytes) -> bytes:
        if self.crypto is None:
            return buffer

        temp = buffer.decode("ascii").split("\\final\\")[0]

        if len(temp) > 1:
            message = ""
            for t in temp:
                complete_buffer = (t+"\\final\\").encode()
                message += self.crypto.decrypt(complete_buffer).decode()
            return message.encode()

        return self.crypto.decrypt(buffer)

    def create_switcher(self, buffer: bytes) -> SwitcherBase:
        from servers.game_status.src.applications.switcher import Switcher
        return Switcher(self, buffer.decode())
