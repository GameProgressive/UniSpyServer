
from pydantic import BaseModel
import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums as auth
import frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums as sake


class HttpData(BaseModel):
    path: str
    headers: dict
    body: str


class WebRequestBase(lib.RequestBase):
    raw_request: str
    """
    we combine path, headers, body to a raw request and send to backend
    """
    command_name: str

# region Sake


class SakeRequestBase(WebRequestBase):
    game_id: int
    secret_key: str | None
    login_ticket: str
    """
    in c sdk called login_ticket \n
    in c# sdk called session_token
    """
    table_id: str
    command_name: sake.CommandName


class CreateRecordRequest(SakeRequestBase):
    records: list


class DeleteRecordRequest(SakeRequestBase):
    record_id: int


class GetMyRecordsRequest(SakeRequestBase):
    fields: list[str]


class GetRandomRecordsRequest(SakeRequestBase):
    max: int
    fields: list[str]


class GetRecordLimitRequest(SakeRequestBase):
    pass


class GetSpecificRecordsRequest(SakeRequestBase):

    record_ids: list
    fields: dict


class RateRecordRequest(SakeRequestBase):
    record_id: str
    rating: str


class SearchForRecordsRequest(SakeRequestBase):
    filter: str
    sort: str
    offset: str
    max: int
    surrounding: str
    owner_ids: str | None
    cache_flag: str
    fields: list[str]


class UpdateRecordRequest(SakeRequestBase):
    record_id: str
    records: list


# region Auth

class AuthRequestBase(WebRequestBase):
    version: int
    partner_code: int
    namespace_id: int
    game_id: int
    command_name: auth.CommandName


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
    operation_id: int = 0
    product_id: int = 1
    game_id: int | None = None


# region D2G


class Direct2GameRequestBase(WebRequestBase):
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
