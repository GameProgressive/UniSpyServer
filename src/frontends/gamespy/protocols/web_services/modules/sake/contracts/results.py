from typing import OrderedDict

from pydantic import BaseModel
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResultBase


class CreateRecordResult(ResultBase):
    table_id: str
    record_id: str
    fields: list


class GetMyRecordsResult(ResultBase):
    class GetMyRecordsInfo(BaseModel):
        field_name: str
        field_type: str
        field_value: str
    records: list[GetMyRecordsInfo]


class SearchForRecordsResult(ResultBase):
    user_data: OrderedDict[str, str]
