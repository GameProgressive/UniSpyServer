from library.src.extentions.encoding import get_bytes
from servers.query_report.src.v2.abstractions.contracts import ResponseBase
from servers.query_report.src.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageRequest,
    HeartBeatRequest,
    KeepAliveRequest
)
from servers.query_report.src.v2.contracts.results import (
    ChallengeResult,
    HeartBeatResult,
)
from servers.query_report.src.v2.aggregates.enums import ServerAvailability
from library.src.extentions.bytes_extentions import ip_to_4_bytes, port_to_2_bytes

RESPONSE_PREFIX = bytes([0xFE, 0xFD, 0x09, 0x00, 0x00, 0x00])


class AvaliableResponse(ResponseBase):
    def __init__(self, request: AvaliableRequest) -> None:
        assert isinstance(request, AvaliableRequest)
        super().__init__(request, None)

    def build(self):
        data = bytearray()
        data.extend(RESPONSE_PREFIX)
        data.append(ServerAvailability.AVAILABLE.value)
        self.sending_buffer = bytes(data)


MESSAGE = "UniSpy echo!"


class ChallengeResponse(ResponseBase):
    def __init__(self, request: ChallengeRequest, result: ChallengeResult) -> None:
        assert isinstance(request, ChallengeRequest)
        assert isinstance(result, ChallengeResult)
        super().__init__(request, result)

    def build(self) -> None:
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(get_bytes(MESSAGE))
        self.sending_buffer = bytes(data)


class ClientMessageResponse(ResponseBase):
    _request: ClientMessageRequest

    def __init__(self, request: ClientMessageRequest) -> None:
        assert isinstance(request, ClientMessageRequest)
        super().__init__(request, None)

    def build(self):
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(self._request.message_key.to_bytes(4, byteorder="little"))
        data.extend(self._request.natneg_message)
        self.sending_buffer = bytes(data)


CHALLENGE = bytes([0x54, 0x54, 0x54, 0x00, 0x00])
SPLITER = bytes([0x00, 0x00, 0x00, 0x00])


class HeartBeatResponse(ResponseBase):
    _request: HeartBeatRequest
    _result: HeartBeatResult

    def __init__(self, request: HeartBeatRequest, result: HeartBeatResult) -> None:
        assert isinstance(request, HeartBeatRequest)
        assert isinstance(result, HeartBeatResult)
        super().__init__(request, result)

    def build(self) -> None:
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(CHALLENGE)
        data.extend(ip_to_4_bytes(self._result.remote_ip_address))
        data.extend(SPLITER)
        data.extend(port_to_2_bytes(self._result.remote_port))
        self.sending_buffer = bytes(data)


class KeepAliveResponse(ResponseBase):
    def __init__(self, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(request, None)
