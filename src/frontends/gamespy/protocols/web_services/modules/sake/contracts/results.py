from typing import OrderedDict

from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResultBase


class CreateRecordResult(ResultBase):
    table_id: str
    record_id: int
    fields: dict


class GetMyRecordsResult(ResultBase):
    records: dict


class SearchForRecordsResult(ResultBase):
    user_data: OrderedDict[str, str]


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