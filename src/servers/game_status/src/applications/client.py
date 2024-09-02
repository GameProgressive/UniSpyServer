from library.src.abstractions.client import ClientBase, ClientInfoBase
from library.src.abstractions.switcher import SwitcherBase
from servers.game_status.src.aggregations.gscrypt import GSCrypt


CHALLENGE_RESPONSE = "\\challenge\\00000000000000000000\\final\\"


class ClientInfo(ClientInfoBase):
    session_key: str = None
    game_name: str = None
    is_user_authenticated: bool = False
    is_player_authenticated: bool = False
    is_game_authenticated: bool = False
    profile_id: int = None
    game_session_key: str = None


class Client(ClientBase):
    info: ClientInfo

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
        from servers.game_status.src.handlers.switcher import Switcher
        return Switcher(self, buffer.decode())
