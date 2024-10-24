from typing import Optional
from servers.presence_connection_manager.src.aggregates.user_status_info import (
    UserStatusInfo,
)
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from pydantic import BaseModel
from servers.presence_connection_manager.src.abstractions.contracts import ResultBase

# region General


class LoginDataModel(BaseModel):
    user_id: int
    profile_id: int
    nick: str
    email: str
    unique_nick: str
    password_hash: str
    email_verified_flag: bool
    banned_flag: bool
    namespace_id: int
    sub_profile_id: int


class LoginResult(ResultBase):
    response_proof: str
    data: LoginDataModel


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

class GetProfileDataModel(BaseModel):
    nick: str
    profile_id: int
    unique_nick: str
    email: str
    firstname: str
    lastname: str
    icquin: int
    homepage: str
    zipcode: str
    countrycode: str
    longitude: float
    latitude: float
    location: str
    birthday: int
    birthmonth: int
    birthyear: int
    sex: int
    publicmask: int
    aim: str
    picture: int
    occupationid: int
    industryid: int
    incomeid: int
    marriedid: int
    childcount: int
    interests1: int
    ownership1: int
    connectiontype: int


class GetProfileResult(ResultBase):
    user_profile: GetProfileDataModel


class NewProfileResult(ResultBase):
    profile_id: int
