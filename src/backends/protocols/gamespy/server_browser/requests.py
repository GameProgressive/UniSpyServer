from typing import List, Optional
import backends.library.abstractions.contracts as lib
from servers.server_browser.src.v2.aggregations.enums import RequestType, ServerListUpdateOption


class RequestBase(lib.RequestBase):
    request_length: int
    raw_request: bytes
    command_name: RequestType


class ServerListUpdateOptionRequestBase(RequestBase):
    source_ip: str
    request_version: Optional[int] = None
    protocol_version: Optional[int] = None
    encoding_version: Optional[int] = None
    game_version: Optional[int] = None
    query_options: Optional[int] = None
    dev_game_name: Optional[str] = None
    game_name: Optional[str] = None
    client_challenge: Optional[str] = None
    update_option: Optional[ServerListUpdateOption] = None
    keys: Optional[List[str]] = None
    filter: Optional[str] = None
    max_servers: Optional[int] = None


class ServerListRequest(ServerListUpdateOptionRequestBase):
    pass


class AdHocRequestBase(RequestBase):
    game_server_public_ip: list[int]
    game_server_public_port: list[int]


class SendMessageRequest(AdHocRequestBase):
    prefix_message: list[int]
    client_message: list[int]


class ServerInfoRequest(AdHocRequestBase):
    pass
