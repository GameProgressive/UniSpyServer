from frontends.gamespy.protocols.presence_connection_manager.aggregates.user_status import (
    UserStatusInfo,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.user_status import UserStatus
from pydantic import BaseModel
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import ResultBase

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


class NewUserResult(ResultBase):
    user_id: int
    profile_id: int


# region Buddy


class AddBuddyResult(ResultBase):
    pass


class BlockListResult(ResultBase):
    profile_ids: list[int]


class BuddyListResult(ResultBase):
    profile_ids: list[int]


class StatusInfoResult(ResultBase):
    profile_id: int
    product_id: int
    status_info: UserStatusInfo


class StatusResult(ResultBase):
    status: UserStatus


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
