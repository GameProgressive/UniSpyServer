from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import SakeCode
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import CreateRecordResult, DeleteRecordResult, GetMyRecordsResult, SearchForRecordsResult, UpdateRecordResult


class CreateRecordResponse(ResponseBase):
    _result: CreateRecordResult

    def build(self) -> None:
        self._content = SoapEnvelop("CreateRecordResponse")
        self._content.add("CreateRecordResult", SakeCode.SUCCESS.value)
        self._content.add("recordid", self._result.record_id)
        super().build()


class UpdateRecordResponse(ResponseBase):
    _result: UpdateRecordResult

    def build(self) -> None:
        self._content = SoapEnvelop("UpdateRecordResponse")
        self._content.add("UpdateRecordResult", SakeCode.SUCCESS.value)
        self._content.add("recordid", self._result.record_id)
        super().build()


class DeleteRecordResponse(ResponseBase):
    _result: DeleteRecordResult

    def build(self) -> None:
        self._content = SoapEnvelop("DeleteRecordResponse")
        self._content.add("DeleteRecordResult", SakeCode.SUCCESS.value)
        super().build()


class GetMyRecordResponse(ResponseBase):
    _result: GetMyRecordsResult

    def build(self) -> None:
        self._content = SoapEnvelop("GetMyRecordResponse")
        self._content.add("GetMyRecordResult", SakeCode.SUCCESS.value)
        self._content.add("values")

        self._content.add("ArrayOfRecordValue", {
            "RecordValue": self._result.values})
        super().build()


class SearchForRecordsResponse(ResponseBase):
    _result: SearchForRecordsResult

    def build(self) -> None:
        """
        <?xml version="1.0" encoding="utf-8"?>
            <SearchForRecordsResponse>
                <SearchForRecordsResult>Success</SearchForRecordsResult>
                <values>
                    <ArrayOfRecordValue>
                        <RecordValue>
                            <byteValue>
                                <value>123</value>
                            </byteValue>
                        </RecordValue>
                        <RecordValue>
                            <intValue>
                                <value>123456789</value>
                            </intValue>
                        </RecordValue>
                    </ArrayOfRecordValue>
                </values>
            </SearchForRecordsResponse>
        """
        self._content = SoapEnvelop("SearchForRecordsResponse")
        self._content.add("SearchForRecordsResult", "Success")
        self._content.add("values")
        for record in self._result.values:
            record_value = []
            for values in record:
                record_value.append(values['value'])
            self._content.add("ArrayOfRecordValue", {
                              "RecordValue": record_value})

        super().build()
