from backends.library.abstractions.contracts import RequestBase as RB
from servers.presence_search_player.src.aggregates.enums import SearchType


class RequestBase(RB):
    operation_id: int

# general

# we just need to recreate the requests and just put the property inside it. The result we can use the results inside servers.


class CheckRequest(RequestBase):
    nick: str
    password: str
    email: str
    partner_id: int


class NewUserRequest(RequestBase):
    product_id: int
    game_port: int
    cd_key: str
    has_game_name: bool
    has_product_id: bool
    has_cdkey: bool
    has_partner_id: bool
    has_game_port: bool
    nick: str
    email: str
    password: str
    partner_id: int
    game_name: str
    uniquenick: str
    namespace_id: int


class NicksRequest(RequestBase):
    password: str
    email: str
    is_require_uniquenicks: bool
    namespace_id: int


class OthersListRequest(RequestBase):
    profile_ids: list[int] = []
    namespace_id: int = 0


class OthersRequest(RequestBase):
    profile_id: int
    game_name: str
    namespace_id: int


class SearchRequest(RequestBase):
    skip_num: int
    request_type: SearchType
    game_name: str
    profile_id: int
    partner_id: int
    email: str
    nick: str
    uniquenick: str
    session_key: str
    firstname: str
    lastname: str
    icquin: str
    namespace_id: int


class SearchUniqueRequest(RequestBase):
    uniquenick: str
    namespace_ids: list[int]


class UniqueSearchRequest(RequestBase):
    preferred_nick: str
    game_name: str
    namespace_id: int


class ValidRequest(RequestBase):
    email: str
