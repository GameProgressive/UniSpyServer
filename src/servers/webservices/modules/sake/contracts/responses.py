from servers.webservices.modules.sake.abstractions.contracts import ResponseBase
from servers.webservices.modules.sake.contracts.requests import CreateRecordRequest
from servers.webservices.modules.sake.contracts.results import CreateRecordResult


class CreateRecordResponse(ResponseBase):
    _result: CreateRecordResult
    _request: CreateRecordRequest

    def build(self) -> None:
        self._content.add("CreateRecordResult")
        self._content.add("tableid", self._result.table_id)
        self._content.add("recordid", self._result.record_id)

        for field in self._result.fields:
            self._content.add("fields", field)

        super().build()


class GetMyRecordResponse(ResponseBase):
    pass


class SearchForRecordsResponse(ResponseBase):
    pass