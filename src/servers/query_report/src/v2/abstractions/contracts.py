from servers.query_report.src.v2.aggregates.enums import PacketType
import library.src.abstractions.contracts
from servers.query_report.src.aggregates.exceptions import QRException
from servers.query_report.src.v2.aggregates.enums import RequestType

MAGIC_DATA = [0xFE, 0xFD]


class RequestBase(library.src.abstractions.contracts.RequestBase):
    instant_key: int
    command_name: RequestType
    raw_request: bytes

    def __init__(self, raw_request: bytes) -> None:
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)

    def parse(self):
        if len(self.raw_request) < 3:
            raise QRException("request length not valid")
        self.command_name = RequestType(self.raw_request[0])
        self.instant_key = int(self.raw_request[1:5])


class ResultBase(library.src.abstractions.contracts.ResultBase):
    packet_type: PacketType


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    _result: ResultBase
    _request: RequestBase
    sending_buffer: bytes
