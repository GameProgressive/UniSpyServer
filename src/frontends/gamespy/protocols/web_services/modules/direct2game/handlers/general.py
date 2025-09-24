from frontends.gamespy.protocols.web_services.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.requests import (
    GetPurchaseHistoryRequest,
    GetStoreAvailabilityRequest,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.responses import (
    GetPurchaseHistoryResponse,
    GetStoreAvailabilityResponse,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.results import (
    GetPurchaseHistoryResult,
    GetStoreAvailabilityResult,
)


class GetPurchaseHistoryHandler(CmdHandlerBase):
    _request: GetPurchaseHistoryRequest
    _result: GetPurchaseHistoryResult

    def __init__(self, client: Client, request: GetPurchaseHistoryRequest) -> None:
        assert isinstance(request, GetPurchaseHistoryRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetPurchaseHistoryResponse(self._result)


class GetStoreAvailabilityHandler(CmdHandlerBase):
    _request: GetStoreAvailabilityRequest
    _result: GetStoreAvailabilityResult

    def __init__(self, client: Client, request: GetStoreAvailabilityRequest) -> None:
        assert isinstance(request, GetStoreAvailabilityRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetStoreAvailabilityResponse(self._result)
