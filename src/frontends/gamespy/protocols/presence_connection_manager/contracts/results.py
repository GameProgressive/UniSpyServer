from pydantic import BaseModel
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import ResultBase
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import GPStatusCode, LoginType

# region General


class LoginData(BaseModel):
    user_id: int
    profile_id: int
    sub_profile_id: int
    nick: str
    email: str
    unique_nick: str
    password_hash: str
    email_verified_flag: bool
    banned_flag: bool
    namespace_id: int


class LoginResult(ResultBase):
    data: LoginData
    user_data: str
    type: LoginType
    partner_id: int
    user_challenge: str

class NewUserResult(ResultBase):
    user_id: int
    profile_id: int


# region Buddy


class AddBuddyResult(ResultBase):
    pass


class BlockListResult(ResultBase):
    profile_ids: list[int]
    operation_id: int


class BuddyListResult(ResultBase):
    profile_ids: list[int]


class StatusInfoResult(ResultBase):
    profile_id: int
    product_id: int
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


class StatusResult(ResultBase):
    status_string: str
    location_string: str
    current_status: GPStatusCode

# class NewUserResult()


# region Profile

class GetProfileData(BaseModel):
    nick: str
    profile_id: int
    unique_nick: str
    email: str
    extra_infos: dict


class GetProfileResult(ResultBase):
    user_profile: GetProfileData


class NewProfileResult(ResultBase):
    profile_id: int


class RegisterNickResult(ResultBase):
    pass
