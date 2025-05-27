from typing import Any, Optional, Union

from pydantic import ValidationError

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
    LoginType,
    SdkRevisionType,
)

from backends.library.abstractions.contracts import RequestBase


class ErrorOnParse(RequestBase):
    raw_request: str


# region buddy


class AddBlockRequest(RequestBase):
    taget_id: int
    profile_id: int
    session_key: str


class BuddyListRequest(RequestBase):
    profile_id: int
    namespace_id: int
    raw_request: Optional[str] = None


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
    status_state: str
    buddy_ip: str
    host_ip: str
    host_private_ip: str
    query_report_port: int
    host_port: int
    session_flags: str
    rich_status: str
    game_type: str
    game_variant: str
    game_map_name: str
    quiet_mode_flags: str


class StatusRequest(RequestBase):
    session_key: str
    status_string: str
    location_string: str
    current_status: GPStatusCode


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
    user_data: str
    unique_nick: Optional[str] = None
    namespace_id: Optional[int] = None
    auth_token: Optional[str] = None
    nick: Optional[str] = None
    email: Optional[str] = None
    product_id: int
    type: Union[LoginType, int]
    sdk_revision_type: list[SdkRevisionType]
    game_port: int
    partner_id: int
    game_name: Optional[str] = None
    quiet_mode_flags: int
    firewall: bool

    def model_post_init(self, __context: Any) -> None:
        super().model_post_init(__context)
        if self.type == LoginType.AUTH_TOKEN:
            if self.auth_token is None:
                raise ValidationError("authtoken is missing.")
        elif self.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            if self.unique_nick is None:
                raise ValidationError("unique nick is missing.")
            if self.namespace_id is None:
                raise ValidationError("namespace is missing.")
        elif self.type == LoginType.NICK_EMAIL:
            if self.nick is None:
                raise ValidationError("nick name is missing.")
            if self.email is None:
                raise ValidationError("email is missing.")
        else:
            raise ValidationError(f"request type {self.type} not found.")


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
