import library.abstractions.contracts
from servers.query_report.exceptions.exceptions import QRException
from servers.query_report.v2.enums.general import RequestType

MAGIC_DATA = [0xFE, 0xFD]


class RequestBase(library.abstractions.contracts.RequestBase):
    instant_key: int
    command_name: RequestType
    raw_request: bytes

    def __init__(self, raw_request: bytes) -> None:
        assert isinstance(raw_request, bytes)
        super().__init__(raw_request)

    def parse(self):
        if len(self.raw_request) < 3:
            raise QRException
        self.command_name = RequestType(self.raw_request[0])
        self.instant_key = int(self.raw_request[1:5])
