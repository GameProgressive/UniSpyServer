from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import SakeCode
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import CreateRecordResult, SearchForRecordsResult


class CreateRecordResponse(ResponseBase):
    _result: CreateRecordResult

    def build(self) -> None:
        self._content = SoapEnvelop("CreateRecordResponse")
        self._content.add("CreateRecordResult", SakeCode.SUCCESS.value)
        self._content.add("recordid",self._result.record_id)
        super().build()


class GetMyRecordResponse(ResponseBase):
    def build(self) -> None:
        self._content
        super().build()


class SearchForRecordsResponse(ResponseBase):
    _result: SearchForRecordsResult

    def build(self) -> None:
        self._content = SoapEnvelop("SearchForRecords")
        self._content.add("SearchForRecordsResult", "Success")
        if self._result.user_data is not None:
            for key, value in self._result.user_data.items():
                self._content.add("values", value)
        else:
            self._content.add("values")

        super().build()
