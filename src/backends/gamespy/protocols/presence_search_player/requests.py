import abc
from dataclasses import dataclass
from backends.gamespy.library.abstractions.request_base import RequestBase as RB
from servers.presence_connection_manager.enums.general import LoginType, SdkRevisionType


class RequestBase(RB, abc.ABC):
    operation_id: int = None


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


class NicksRequest(RequestBase):
    password: str
    email: str
    is_require_uniquenicks: bool


class LoginRequest(RequestBase):
    user_challenge: str
    response: str
    unique_nick: str
    user_data: str
    namespace_id: int
    auth_token: str
    nick: str
    email: str
    product_id: int
    type: LoginType
    sdk_revision_type: SdkRevisionType
    game_port: int
    user_id: int
    profile_id: int
    partner_id: int
    game_name: int
    quiet_mode_flags: int
    firewall: bool
