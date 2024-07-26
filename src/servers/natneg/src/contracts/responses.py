from servers.natneg.src.abstractions.contracts import (
    CommonResponseBase
)
from servers.natneg.src.contracts.requests import InitRequest
from servers.natneg.src.contracts.results import InitResult


class InitResponse(CommonResponseBase):
    _request: InitRequest
    _result: InitResult

    def __init__(self, request: "InitRequest", result: "InitResult") -> None:
        super().__init__(request, result)
        assert isinstance(request, InitRequest)
        assert isinstance(result, InitResult)
