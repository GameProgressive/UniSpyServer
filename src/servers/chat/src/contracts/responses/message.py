from library.src.abstractions.contracts import RequestBase, ResultBase
from servers.chat.src.abstractions.contract import ResponseBase
from servers.chat.src.aggregates.response_name import *
from servers.chat.src.contracts.requests.message import (
    ATMRequest,
    NoticeRequest,
    PrivateRequest,
    UTMRequest,
)
from servers.chat.src.contracts.results.message import (
    ATMResult,
    NoticeResult,
    PrivateResult,
    UTMResult,
)


class ATMResponse(ResponseBase):
    _request: ATMRequest
    _result: ATMResult

    def __init__(self, request: ATMRequest, result: ATMResult) -> None:
        assert isinstance(request, ATMRequest)
        assert isinstance(result, ATMResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {ABOVE_THE_TABLE_MSG} {self._result.target_name} :{self._request.message}\r\n"


class NoticeResponse(ResponseBase):
    _request: NoticeRequest
    _result: NoticeResult

    def __init__(self, request: NoticeRequest, result: NoticeResult) -> None:
        assert isinstance(result, NoticeResult)
        assert isinstance(request, NoticeRequest)

        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {NOTICE} {self._result.target_name} {self._request.message}\r\n"


class PrivateResponse(ResponseBase):
    _request: PrivateRequest
    _result: PrivateResult

    def __init__(self, request: PrivateRequest, result: PrivateResult) -> None:
        assert isinstance(result, PrivateRequest)
        assert isinstance(request, PrivateResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {PRIVATE_MSG} {self._result.target_name} :{self._request.message}\r\n"


class UTMResponse(ResponseBase):
    _request: UTMRequest
    _result: UTMResult

    def __init__(self, request: UTMRequest, result: UTMResult) -> None:
        assert isinstance(request, UTMRequest)
        assert isinstance(result, UTMResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {UNDER_THE_TABLE_MSG} {self._result.target_name} :{self._request.message}"
