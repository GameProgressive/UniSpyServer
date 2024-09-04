from servers.web_services.src.abstractions.contracts import RequestBase
from servers.web_services.src.abstractions.handler import CmdHandlerBase
from servers.web_services.src.applications.client import Client
from servers.web_services.src.modules.direct2game.contracts.requests import (
    GetPurchaseHistoryRequest,
    GetStoreAvailabilityRequest,
)
from servers.web_services.src.modules.direct2game.contracts.responses import (
    GetPurchaseHistoryResponse,
    GetStoreAvailabilityResponse,
)
from servers.web_services.src.modules.direct2game.contracts.results import (
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
        self._response = GetPurchaseHistoryResponse(self._request, self._result)


class GetStoreAvailabilityHandler(CmdHandlerBase):
    _request: GetStoreAvailabilityRequest
    _result: GetStoreAvailabilityResult

    def __init__(self, client: Client, request: GetStoreAvailabilityRequest) -> None:
        assert isinstance(request, GetStoreAvailabilityRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetStoreAvailabilityResponse(self._request, self._result)
