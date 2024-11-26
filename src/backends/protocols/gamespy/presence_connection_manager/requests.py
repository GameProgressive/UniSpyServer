from typing import Optional, Union

from pydantic import UUID4, BaseModel
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from servers.presence_connection_manager.src.aggregates.user_status_info import (
    UserStatusInfo,
)
from servers.presence_connection_manager.src.aggregates.enums import (
    LoginType,
    PublicMasks,
    SdkRevisionType,
)

import backends.library.abstractions.contracts as lib


class RequestBase(BaseModel):
    server_id: UUID4


class ErrorOnParse(RequestBase):
    raw_request: str


# region buddy
class AddBuddyRequest(RequestBase):
    friend_profile_id: int
    reason: str


class DelBuddyRequest(RequestBase):
    friend_profile_id: int


class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    session_key: str
    """the invite target profile id"""


class StatusInfoRequest(RequestBase):
    is_get_status_info: bool
    profile_id: int
    namespace_id: int
    status_info: UserStatusInfo


class StatusRequest(RequestBase):
    status: UserStatus
    is_get_status: bool


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


class AddBlockRequest(RequestBase):
    taget_id: int


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
    has_public_mask_flag: Optional[bool] = None
    public_mask: Optional[PublicMasks] = None
    session_key: Optional[str] = None
    partner_id: Optional[int] = None
    nick: Optional[str] = None
    uniquenick: Optional[str] = None
    has_first_name_flag: Optional[bool] = None
    first_name: Optional[str] = None
    has_last_name_flag: Optional[bool] = None
    last_name: Optional[str] = None
    has_icq_flag: Optional[bool] = None
    icq_uin: Optional[int] = None
    has_home_page_flag: Optional[bool] = None
    home_page: Optional[str] = None
    has_birthday_flag: Optional[bool] = False
    birth_day: Optional[int] = None
    birth_month: Optional[int] = None
    birth_year: Optional[int] = None
    has_sex_flag: Optional[bool] = False
    sex: Optional[bool] = None
    has_zip_code: Optional[bool] = False
    zip_code: Optional[str] = None
    has_country_code: Optional[bool] = False
    country_code: Optional[str] = None


class UpdateUIRequest(RequestBase):
    extra_info: dict


if __name__ == "__main__":
    pass
