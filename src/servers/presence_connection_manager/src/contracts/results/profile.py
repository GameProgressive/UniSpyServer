from typing import Optional

from pydantic import BaseModel
from servers.presence_connection_manager.src.abstractions.contracts import ResultBase


class GetProfileDataModel(BaseModel):
    nick: Optional[str] = None
    profile_id: Optional[int] = None
    unique_nick: Optional[str] = None
    email: Optional[str] = None
    firstname: Optional[str] = None
    lastname: Optional[str] = None
    icquin: Optional[int] = None
    homepage: Optional[str] = None
    zipcode: Optional[str] = None
    countrycode: Optional[str] = None
    longitude: Optional[float] = None
    latitude: Optional[float] = None
    location: Optional[str] = None
    birthday: Optional[int] = None
    birthmonth: Optional[int] = None
    birthyear: Optional[int] = None
    sex: Optional[int] = None
    publicmask: Optional[int] = None
    aim: Optional[str] = None
    picture: Optional[int] = None
    occupationid: Optional[int] = None
    industryid: Optional[int] = None
    incomeid: Optional[int] = None
    marriedid: Optional[int] = None
    childcount: Optional[int] = None
    interests1: Optional[int] = None
    ownership1: Optional[int] = None
    connectiontype: Optional[int] = None


class GetProfileResult(ResultBase):
    user_profile: Optional[GetProfileDataModel] = None


class NewProfileResult(ResultBase):
    profile_id: int
