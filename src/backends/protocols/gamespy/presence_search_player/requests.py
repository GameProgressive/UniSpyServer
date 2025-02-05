from typing import Optional
from backends.library.abstractions.contracts import RequestBase as RB
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import SearchType


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
    nick: str
    email: str
    password: str
    uniquenick: str
    namespace_id: int
    product_id: int
    game_port: Optional[int] = None
    cd_key: Optional[str] = None
    partner_id: Optional[int] = None
    game_name: Optional[str] = None


class NicksRequest(RequestBase):
    password: str
    email: str
    namespace_id: int
    is_require_uniquenicks: bool


class OthersListRequest(RequestBase):
    profile_ids: list[int]
    namespace_id: int


class OthersRequest(RequestBase):
    profile_id: int
    game_name: str
    namespace_id: int


class SearchRequest(RequestBase):
    skip_num: int
    request_type: SearchType
    game_name: str
    profile_id: int | None = None
    partner_id: int
    email: str | None = None
    nick: str | None = None
    uniquenick: str | None = None
    session_key: str | None = None
    firstname: str | None = None
    lastname: str | None = None
    icquin: str | None = None
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
