from servers.web_services.src.modules.sake.abstractions.generals import ResponseBase
from servers.web_services.src.modules.sake.contracts.requests import CreateRecordRequest, SearchForRecordsRequest
from servers.web_services.src.modules.sake.contracts.results import CreateRecordResult, SearchForRecordsResult


class CreateRecordResponse(ResponseBase):
    _result: "CreateRecordResult"
    _request: "CreateRecordRequest"

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
    _result: "SearchForRecordsResult"
    _request: "SearchForRecordsRequest"

    def build(self) -> None:
        self._content.add("SearchForRecordsResponse")
        self._content.add("SearchForRecordsResult", "Success")
        if self._result.user_data is not None:
            for key, value in self._result.user_data.items():
                self._content.add("values", value)
        else:
            self._content.add("values")

        super().build()
