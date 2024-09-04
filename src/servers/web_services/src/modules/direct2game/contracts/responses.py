from servers.web_services.src.modules.direct2game.abstractions.contracts import ResponseBase
from servers.web_services.src.modules.direct2game.contracts.results import (
    GetPurchaseHistoryResult,
    GetStoreAvailabilityResult,
)


class GetPurchaseHistoryResponse(ResponseBase):
    _result: GetPurchaseHistoryResult

    def build(self) -> None:
        self._content.add("GetPurchaseHistoryResponse")
        self._content.add("GetPurchaseHistoryResult")
        self._content.add("status")
        self._content.add("code", self._result.code)
        self._content.change_to_element("GetPurchaseHistoryResult")
        self._content.add("orderpurchases")
        self._content.add("count", 0)
        super().build()


class GetStoreAvailabilityResponse(ResponseBase):
    _result: GetStoreAvailabilityResult

    def build(self) -> None:
        self._content.add("GetStoreAvailabilityResponse")
        self._content.add("GetStoreAvailabilityResult")
        self._content.add("status")
        self._content.add("code", self._result.code)
        self._content.change_to_element("GetStoreAvailabilityResult")
        self._content.add("storestatusid", int(self._result.store_status_id))
        super().build()
