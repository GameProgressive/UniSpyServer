import socket
from frontends.gamespy.protocols.natneg.abstractions.contracts import (
    CommonResponseBase,
    ResponseBase,
)
from frontends.gamespy.protocols.natneg.contracts.results import (
    AddressCheckResult,
    ConnectResult,
    ErtAckResult,
    InitResult,
    NatifyResult,
)


class InitResponse(CommonResponseBase):
    _result: InitResult

    def __init__(self, result: InitResult) -> None:
        assert isinstance(result, InitResult)
        super().__init__(result)


class ErcAckResponse(InitResponse):
    _result: ErtAckResult

    def __init__(self, result: ErtAckResult) -> None:
        assert isinstance(result, ErtAckResult)
        self._result = result


class NatifyResponse(CommonResponseBase):
    _result: NatifyResult

    def __init__(self, result: NatifyResult) -> None:
        assert isinstance(result, NatifyResult)
        super().__init__(result)


class AddressCheckResponse(CommonResponseBase):
    _result: AddressCheckResult

    def __init__(
        self, result: AddressCheckResult
    ) -> None:
        assert isinstance(result, AddressCheckResult)
        super().__init__(result)


class ConnectResponse(ResponseBase):
    _result: ConnectResult

    def build(self) -> None:
        assert self._result.ip is not None
        assert self._result.port is not None
        assert self._result.status is not None
        super().build()
        data = bytes()
        data += self.sending_buffer
        data += socket.inet_aton(self._result.ip)
        data += self._result.port.to_bytes(2)
        data += self._result.got_your_data
        data += self._result.status.value.to_bytes(1)
        self.sending_buffer = data
