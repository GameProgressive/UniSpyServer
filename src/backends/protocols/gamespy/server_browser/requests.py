from typing import List, Optional
import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import RequestType, ServerListUpdateOption


class RequestBase(lib.RequestBase):
    request_length: int
    raw_request: bytes
    command_name: RequestType


class ServerListUpdateOptionRequestBase(RequestBase):
    source_ip: str
    request_version: int
    protocol_version: int
    encoding_version: int
    game_version: int
    query_options: int
    dev_game_name: str
    game_name: str
    client_challenge: str
    update_option: ServerListUpdateOption
    keys: list[str]
    filter: list[str]
    max_servers: int


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
