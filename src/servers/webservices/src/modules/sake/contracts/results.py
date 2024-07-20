from typing import OrderedDict
from servers.webservices.src.modules.sake.abstractions.general import ResultBase


class CreateRecordResult(ResultBase):
    table_id: str
    record_id: str
    fields: list


class GetMyRecordsResult(ResultBase):
    records: list[tuple]
    """
    [
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    ...
    (field_name,field_type,field_value)
    ]
    """


class SearchForRecordsResult(ResultBase):
    user_data: OrderedDict[str, str]
