from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResultBase


class CreateRecordResult(ResultBase):
    table_id: str
    record_id: int
    # fields: dict


class UpdateRecordResult(ResultBase):
    table_id: str
    record_id: int


class GetMyRecordsResult(ResultBase):
    records: dict
    fields: list[str]


class SearchForRecordsResult(ResultBase):
    records_list: list[dict]
    fields: list[str]


class GetSpecificRecordsResult(ResultBase):
    records: dict


class GetRandomRecordsResult(ResultBase):
    records: dict


class GetRecordLimitResult(ResultBase):
    pass


class GetRecordCountResult(ResultBase):
    count: int


class RateRecordResult(ResultBase):
    pass


class DeleteRecordResult(ResultBase):
    pass
