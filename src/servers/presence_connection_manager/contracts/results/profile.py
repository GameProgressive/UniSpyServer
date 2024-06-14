from servers.presence_connection_manager.abstractions.contracts import ResultBase


class GetProfileDataModel:
    nick: str = None
    profile_id: int = None
    unique_nick: str = None
    email: str = None
    firstname: str = None
    lastname: str = None
    icquin: int = None
    homepage: str = None
    zipcode: str = None
    countrycode: str = None
    longitude: float = None
    latitude: float = None
    location: str = None
    birthday: int = None
    birthmonth: int = None
    birthyear: int = None
    sex: int = None
    publicmask: int = None
    aim: str = None
    picture: int = None
    occupationid: int = None
    industryid: int = None
    incomeid: int = None
    marriedid: int = None
    childcount: int = None
    interests1: int = None
    ownership1: int = None
    connectiontype: int = None


class GetProfileResult(ResultBase):
    user_profile: GetProfileDataModel = None


class NewProfileResult(ResultBase):
    profile_id: int
