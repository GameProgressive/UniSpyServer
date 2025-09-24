from frontends.gamespy.protocols.query_report.v2.aggregates.enums import PacketType
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import RequestType
import frontends.gamespy.library.abstractions.contracts as lib

MAGIC_DATA = [0xFE, 0xFD]


class RequestBase(lib.RequestBase):
    instant_key: str
    command_name: RequestType
    raw_request: bytes

    def __init__(self, raw_request: bytes) -> None:
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)

    def parse(self):
        if len(self.raw_request) < 3:
            raise QRException("request length not valid")
        self.command_name = RequestType(self.raw_request[0])
        self.instant_key = str(int.from_bytes(self.raw_request[1:5]))


class ResultBase(lib.ResultBase):
    packet_type: PacketType
    command_name: RequestType
    instant_key: str

class ResponseBase(lib.ResponseBase):
    _result: ResultBase
    sending_buffer: bytes

    def build(self) -> None:
        data = bytearray()
        data.extend(MAGIC_DATA)
        data.append(self._result.command_name.value)
        data.extend(int(self._result.instant_key).to_bytes(4))
        self.sending_buffer = bytes(data)
