import library.src.abstractions.contracts
from servers.query_report.src.exceptions.exceptions import QRException
from servers.query_report.src.v2.enums.general import RequestType

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


import abc
import library.src.abstractions.contracts
from servers.query_report.src.v2.enums.general import PacketType


class ResultBase(library.src.abstractions.contracts.ResultBase, abc.ABC):
    packet_type: PacketType


import abc
import library.src.abstractions.contracts


class ResponseBase(library.src.abstractions.contracts.ResponseBase, abc.ABC):
    _result: ResultBase
    _request: RequestBase
    sending_buffer: bytes
