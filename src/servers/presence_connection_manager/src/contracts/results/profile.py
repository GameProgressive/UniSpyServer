from typing import Optional

from pydantic import BaseModel
from servers.presence_connection_manager.src.abstractions.contracts import ResultBase


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
