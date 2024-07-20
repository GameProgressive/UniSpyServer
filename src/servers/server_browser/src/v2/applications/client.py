from library.src.abstractions.client import ClientBase, ClientInfoBase
from servers.server_browser.src.v2.enums.general import ServerListUpdateOption


class ClientInfo(ClientInfoBase):
    game_secret_key: str
    client_challenge: str
    search_type: ServerListUpdateOption
    game_name: str


class Client(ClientBase):
    is_log_raw: bool = True
    info: ClientInfo

    def create_switcher(self, buffer) -> None:
        pass
