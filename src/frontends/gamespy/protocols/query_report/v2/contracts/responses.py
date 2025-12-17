from frontends.gamespy.library.extentions.encoding import get_bytes
from frontends.gamespy.protocols.query_report.v2.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageRequest,
    HeartbeatRequest,
    KeepAliveRequest
)
from frontends.gamespy.protocols.query_report.v2.contracts.results import (
    AvailableResult,
    ChallengeResult,
    ClientMessageResult,
    HeartbeatResult,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import ServerAvailability
from frontends.gamespy.library.extentions.bytes_extentions import ip_to_4_bytes, port_to_2_bytes

RESPONSE_PREFIX = bytes([0xFE, 0xFD, 0x09, 0x00, 0x00, 0x00])


class AvailableResponse(ResponseBase):
    def __init__(self, result: AvailableResult) -> None:
        assert isinstance(result, AvailableResult)
        super().__init__(result)

    def build(self):
        data = bytearray()
        data.extend(RESPONSE_PREFIX)
        data.append(ServerAvailability.AVAILABLE.value)
        self.sending_buffer = bytes(data)


MESSAGE = "UniSpy echo!"


class ChallengeResponse(ResponseBase):
    def __init__(self, result: ChallengeResult) -> None:
        assert isinstance(result, ChallengeResult)
        super().__init__( result)

    def build(self) -> None:
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(get_bytes(MESSAGE))
        self.sending_buffer = bytes(data)


class ClientMessageResponse(ResponseBase):
    _result: ClientMessageResult

    def __init__(self, result: ClientMessageResult) -> None:
        assert isinstance(result, ClientMessageResult)
        super().__init__(result)

    def build(self):
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(self._result.message_key.to_bytes(4, byteorder="little"))
        data.extend(self._result.natneg_message)
        self.sending_buffer = bytes(data)


CHALLENGE = bytes([0x54, 0x54, 0x54, 0x00, 0x00])
SPLITER = bytes([0x00, 0x00, 0x00, 0x00])


class HeartbeatResponse(ResponseBase):
    _result: HeartbeatResult

    def __init__(self, result: HeartbeatResult) -> None:
        assert isinstance(result, HeartbeatResult)
        super().__init__(result)

    def build(self) -> None:
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.extend(CHALLENGE)
        data.extend(ip_to_4_bytes(self._result.remote_ip))
        data.extend(SPLITER)
        data.extend(port_to_2_bytes(self._result.remote_port))
        self.sending_buffer = bytes(data)


# class KeepAliveResponse(ResponseBase):
#     def __init__(self, result:KeepAliveResponse) -> None:
#         assert isinstance(request, KeepAliveRequest)
#         super().__init__(request, None)
