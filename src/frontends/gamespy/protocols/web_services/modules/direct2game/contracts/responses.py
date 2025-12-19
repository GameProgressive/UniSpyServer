from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.direct2game.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.results import (
    GetPurchaseHistoryResult,
    GetStoreAvailabilityResult,
)


class GetPurchaseHistoryResponse(ResponseBase):
    _result: GetPurchaseHistoryResult

    def build(self) -> None:
        self._content = SoapEnvelop("GetPurchaseHistoryResult")
        self._content.add("status")
        self._content.add("code", self._result.code)
        self._content.go_to_content_element()
        self._content.add("orderpurchases")
        self._content.add("count", 0)
        super().build()


class GetStoreAvailabilityResponse(ResponseBase):
    _result: GetStoreAvailabilityResult

    def build(self) -> None:
        self._content = SoapEnvelop("GetStoreAvailabilityResult")
        self._content.add("status")
        self._content.add("code", self._result.code)
        self._content.go_to_content_element()
        self._content.add("storestatusid", int(self._result.store_status_id))
        super().build()
