from library.abstractions.client import ClientBase
from servers.chat.abstractions.message import MessageHandlerBase, MessageRequestBase
from servers.chat.contracts.requests.message import (
    ATMRequest,
    NoticeRequest,
    PrivateRequest,
    UTMRequest,
)
from servers.chat.contracts.responses.message import (
    ATMResponse,
    NoticeResponse,
    PrivateResponse,
    UTMResponse,
)
from servers.chat.contracts.results.message import (
    ATMResult,
    NoticeResult,
    PrivateResult,
    UTMResult,
)


class ATMHandler(MessageHandlerBase):
    _request: ATMRequest
    _result: ATMResult

    def __init__(self, client: ClientBase, request: ATMRequest):
        assert isinstance(request, ATMRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = ATMResponse(self._request, self._result)


class UTMHandler(MessageHandlerBase):
    _request: UTMRequest
    _result: UTMResult

    def __init__(self, client: ClientBase, request: UTMRequest):
        assert isinstance(request, UTMRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = UTMResponse(self._request, self._result)


class NoticeHandler(MessageHandlerBase):
    _request: NoticeRequest
    _result: NoticeResult

    def __init__(self, client: ClientBase, request: NoticeRequest):
        assert isinstance(request, NoticeRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = NoticeResponse(self._request, self._result)


class PrivateHandler(MessageHandlerBase):
    _request: PrivateRequest
    _result: PrivateResult

    def __init__(self, client: ClientBase, request: PrivateRequest):
        assert isinstance(request, PrivateRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = PrivateResponse(self._request, self._result)
