from library.extentions.encoding import get_bytes
from servers.presence_connection_manager.contracts.requests.general import (
    KeepAliveRequest,
)
from servers.query_report.v2.abstractions.response_base import ResponseBase
from servers.query_report.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageRequest,
    HeartBeatRequest,
)
from servers.query_report.v2.contracts.results import ChallengeResult, HeartBeatResult
from servers.query_report.v2.enums.general import ServerAvailability


RESPONSE_PREFIX = bytes([0xFE, 0xFD, 0x09, 0x00, 0x00, 0x00])


class AvaliableResponse(ResponseBase):
    def __init__(self, request: AvaliableRequest) -> None:
        assert isinstance(request, AvaliableRequest)
        super().__init__(request, None)

    def build(self):
        data = bytearray()
        data.extend(RESPONSE_PREFIX)
        data.append(ServerAvailability.AVAILABLE)
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
        data.extend(self._result.remote_ip_endpoint.get_ip_bytes())
        data.extend(SPLITER)
        data.extend(self._result.remote_ip_endpoint.get_port_bytes())
        self.sending_buffer = bytes(data)


class KeepAliveResponse(ResponseBase):
    def __init__(self, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(request, None)
