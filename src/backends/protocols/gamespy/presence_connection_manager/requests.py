from typing import Optional, Union

from pydantic import UUID4, BaseModel

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginType,
    PublicMasks,
    SdkRevisionType,
)

from backends.library.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.presence_connection_manager.aggregates.user_status import UserStatus, UserStatusInfo


class ErrorOnParse(RequestBase):
    raw_request: str


class AddBlockRequest(RequestBase):
    taget_id: int
    profile_id: int
    session_key: str

# region buddy


class BuddyListRequest(RequestBase):
    profile_id: int
    namespace_id: int


class BlockListRequest(RequestBase):
    profile_id: int
    namespace_id: int


class AddBuddyRequest(RequestBase):
    profile_id: int
    target_id: int
    namespace_id: int
    reason: str


class DelBuddyRequest(RequestBase):
    profile_id: int
    target_id: int
    namespace_id: int


class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    session_key: str
    """the invite target profile id"""


class StatusInfoRequest(RequestBase):
    is_get: bool
    profile_id: int
    namespace_id: int
    status_info: UserStatusInfo


class StatusRequest(RequestBase):
    session_key: str
    status: UserStatus
    is_get: bool


# region general


class KeepAliveRequest(RequestBase):
    client_ip: str
    client_port: int
    pass


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
    type: Union[LoginType, int]
    sdk_revision_type: Union[SdkRevisionType, int]
    game_port: int
    user_id: int
    profile_id: int
    partner_id: int
    game_name: int
    quiet_mode_flags: int
    firewall: bool


class LogoutRequest(RequestBase):
    pass


# region profile


class GetProfileRequest(RequestBase):
    profile_id: int
    session_key: str


class NewProfileRequest(RequestBase):
    is_replace_nick_name: bool
    session_key: str
    new_nick: str
    old_nick: str


class RegisterCDKeyRequest(RequestBase):
    session_key: str
    cdkey_enc: str


class RegisterNickRequest(RequestBase):
    unique_nick: str
    session_key: str
    partner_id: int


class UpdateProfileRequest(RequestBase):
    session_key: str
    extra_infos: dict


class UpdateUserInfoRequest(RequestBase):
    session_key: str
    extra_infos: dict


if __name__ == "__main__":
    pass
