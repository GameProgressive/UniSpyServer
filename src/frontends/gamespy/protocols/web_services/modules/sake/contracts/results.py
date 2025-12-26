from typing import OrderedDict

from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResultBase


class CreateRecordResult(ResultBase):
    table_id: str
    record_id: int
    # fields: dict


class UpdateRecordResult(ResultBase):
    table_id: str
    record_id: int


class GetMyRecordsResult(ResultBase):
    values: list[dict]


class SearchForRecordsResult(ResultBase):
    values: list[list[dict]]


class GetSpecificRecordsResult(ResultBase):
    records: dict


class GetRandomRecordsResult(ResultBase):
    records: dict


class GetRecordLimitResult(ResultBase):
    pass


class RateRecordResult(ResultBase):
    pass


class DeleteRecordResult(ResultBase):
    pass
