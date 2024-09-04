from pydantic import BaseModel
import backends.gamespy.library.abstractions.contracts as lib


class RequestBase(lib.RequestBase):
    game_id: int
    secret_key: str
    login_ticket: str
    table_id: str


class CreateRecordData(BaseModel):
    name: str
    type: str
    value: str


class CreateRecordRequest(RequestBase):
    values: list[CreateRecordData]


class DeleteRecordRequest(RequestBase):
    record_id: int


class GetMyRecordsData(BaseModel):
    name: str
    value: str


class GetMyRecordsRequest(RequestBase):
    fields: list[GetMyRecordsData]


class GetRandomRecordsData(BaseModel):
    name: str
    value: str


class GetRandomRecordsRequest(RequestBase):
    max: int
    fields: list[GetRandomRecordsData]


class GetRecordLimitRequest(RequestBase):
    pass


class GetSpecificRecordsData(BaseModel):
    name: str
    type: str


class GetSpecificRecordsRequest(RequestBase):

    record_ids: list[GetSpecificRecordsData]
    fields: list[GetSpecificRecordsData]



class RateRecordRequest(RequestBase):
    record_id: str
    rating: str


class SearchForRecordsData(BaseModel):
    name: str
    type: str


class SearchForRecordsRequest(RequestBase):
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


class UpdateRecordRequest(RequestBase):
    record_id: str
    values: list[UpdateRecordData]
