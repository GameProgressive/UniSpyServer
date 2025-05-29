import socket
from frontends.gamespy.protocols.natneg.abstractions.contracts import (
    CommonResponseBase,
    ResponseBase,
)
from frontends.gamespy.protocols.natneg.contracts.requests import (
    AddressCheckRequest,
    ConnectRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
)
from frontends.gamespy.protocols.natneg.contracts.results import (
    AddressCheckResult,
    ConnectResult,
    ErtAckResult,
    InitResult,
    NatifyResult,
)


class InitResponse(CommonResponseBase):
    _request: InitRequest
    _result: InitResult

    def __init__(self, request: InitRequest, result: InitResult) -> None:
        super().__init__(request, result)
        assert isinstance(request, InitRequest)
        assert isinstance(result, InitResult)


class ErcAckResponse(InitResponse):
    _request: ErtAckRequest
    _result: ErtAckResult

    def __init__(self, request: ErtAckRequest, result: ErtAckResult) -> None:
        assert isinstance(request, ErtAckRequest)
        assert isinstance(result, ErtAckResult)
        self._request = request
        self._result = result


class NatifyResponse(CommonResponseBase):
    _request: NatifyRequest
    _result: NatifyResult

    def __init__(self, request: NatifyRequest, result: NatifyResult) -> None:
        assert isinstance(request, NatifyRequest)
        assert isinstance(result, NatifyResult)
        super().__init__(request, result)


class AddressCheckResponse(CommonResponseBase):
    _request: AddressCheckRequest
    _result: AddressCheckResult

    def __init__(
        self, request: AddressCheckRequest, result: AddressCheckResult
    ) -> None:
        assert isinstance(request, AddressCheckRequest)
        assert isinstance(result, AddressCheckResult)
        super().__init__(request, result)


class ConnectResponse(ResponseBase):
    _result: ConnectResult
    _request: ConnectRequest

    def build(self) -> None:
        super().build()
        data = bytes()
        data += self.sending_buffer
        data += socket.inet_aton(self._result.ip)
        data += self._result.port.to_bytes(2)[::-1]
        data += self._result.got_your_data
        data += self._result.finished.value.to_bytes(1)
        self.sending_buffer = data
