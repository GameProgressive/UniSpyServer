
from pydantic import BaseModel
import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import ResponseName


class SakeRequestBase(lib.RequestBase):
    game_id: int
    secret_key: str
    login_ticket: str
    table_id: int


class CreateRecordData(BaseModel):
    name: str
    type: str
    value: str


class CreateRecordRequest(SakeRequestBase):
    values: list[CreateRecordData]


class DeleteRecordRequest(SakeRequestBase):
    record_id: int


class GetMyRecordsData(BaseModel):
    name: str
    value: str


class GetMyRecordsRequest(SakeRequestBase):
    fields: list[GetMyRecordsData]


class GetRandomRecordsData(BaseModel):
    name: str
    value: str


class GetRandomRecordsRequest(SakeRequestBase):
    max: int
    fields: list[GetRandomRecordsData]


class GetRecordLimitRequest(SakeRequestBase):
    pass


class GetSpecificRecordsData(BaseModel):
    name: str
    type: str


class GetSpecificRecordsRequest(SakeRequestBase):

    record_ids: list[GetSpecificRecordsData]
    fields: list[GetSpecificRecordsData]


class RateRecordRequest(SakeRequestBase):
    record_id: str
    rating: str


class SearchForRecordsData(BaseModel):
    name: str
    type: str


class SearchForRecordsRequest(SakeRequestBase):
    filter: str
    sort: str
    offset: str
    max: str
    surrounding: str
    owner_ids: str
    cache_flag: str
    fields: list[SearchForRecordsData]


class UpdateRecordData(BaseModel):
    name: str
    type: str
    value: str


class UpdateRecordRequest(SakeRequestBase):
    record_id: str
    values: list[UpdateRecordData]


# Auth

class AuthRequestBase(lib.RequestBase):
    version: int
    partner_code: int
    namespace_id: int
    game_id: int | None = None
    response_name: ResponseName


class LoginProfileRequest(AuthRequestBase):
    email: str
    uniquenick: str
    cdkey: str
    password: str


class LoginPS3CertRequest(AuthRequestBase):
    ps3_cert: str
    npticket: str


class LoginRemoteAuthRequest(AuthRequestBase):
    auth_token: str
    challenge: str


class LoginUniqueNickRequest(AuthRequestBase):
    uniquenick: str
    password: str


class CreateUserAccountRequest(AuthRequestBase):
    email: str
    nick: str
    uniquenick: str
    password: str

# D2G


class Direct2GameRequestBase(lib.RequestBase):
    pass


class GetPurchaseHistoryRequest(Direct2GameRequestBase):
    game_id: int
    access_token: str
    proof: str
    certificate: str


class GetStoreAvailabilityRequest(Direct2GameRequestBase):
    game_id: int
    version: int
    region: str
    access_token: str
