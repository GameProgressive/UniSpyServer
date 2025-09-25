
import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    ServerListUpdateOption,
)


class RequestBase(lib.RequestBase):
    raw_request: bytes


class ServerListUpdateOptionRequestBase(RequestBase):
    request_version: int
    protocol_version: int
    encoding_version: int
    game_version: int
    dev_game_name: str
    game_name: str
    client_challenge: str
    update_option: ServerListUpdateOption
    keys: list[str]
    filter: str | None = None
    max_servers: int | None = None
    source_ip: str | None = None
    query_options: int | None = None


class ServerListRequest(ServerListUpdateOptionRequestBase):
    pass


class AdHocRequestBase(RequestBase):
    game_server_public_ip: str
    game_server_public_port: int


class SendMessageRequest(AdHocRequestBase):
    prefix_message: str
    client_message: str


class ServerInfoRequest(AdHocRequestBase):
    pass
