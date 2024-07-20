import abc
import library.src.abstractions.contracts
from library.src.extentions.string_extentions import IPEndPoint
from servers.natneg.src.enums.general import (
    NatClientIndex,
    NatPortType,
    RequestType,
    ResponseType,
)

MAGIC_DATA = bytes([0xFD, 0xFC, 0x1E, 0x66, 0x6A, 0xB2])


class RequestBase(library.src.abstractions.contracts.RequestBase):
    version: int
    cookie: bytes
    port_type: NatPortType
    command_name: bytes
    raw_request: bytes

    def __init__(self, raw_request: bytes = None):
        assert isinstance(raw_request, bytes)
        self.raw_request = raw_request

    def parse(self) -> None:
        if len(self.raw_request) < 12:
            return

        self.version = self.raw_request[6]
        self.command_name = RequestType(self.raw_request[7])
        self.cookie = self.raw_request[8:12]
        self.port_type = NatPortType(self.raw_request[12])


class ResultBase(library.src.abstractions.contracts.ResultBase):
    packet_type: ResponseType
    pass


class ResponseBase(library.src.abstractions.contracts.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: bytes

    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        super().__init__(request, result)
        assert issubclass(request, RequestBase)
        assert issubclass(result, ResultBase)

    def build(self) -> None:
        data = bytes()
        data += MAGIC_DATA
        data += self._request.version.to_bytes(1, "little")
        data += int(self._result.packet_type).to_bytes(1, "little")
        data += self._request.cookie
        self.sending_buffer = data


class CommonRequestBase(RequestBase):
    client_index: NatClientIndex
    use_game_port: bool

    def parse(self):
        super().parse()
        self.client_index = NatClientIndex(self.raw_request[13])
        self.use_game_port = bool(self.raw_request[14])


class CommonResultBase(ResultBase, abc.ABC):
    ip_endpoint: IPEndPoint


class CommonResponseBase(ResponseBase):
    _result: CommonResultBase
    _request: CommonRequestBase

    def build(self) -> None:
        super().build()
        data = bytes()
        data += self.sending_buffer
        data += int(self._request.port_type).to_bytes(1, "little")
        data += int(self._request.client_index).to_bytes(1, "little")
        data += bytes(self._request.use_game_port)
        data += self._result.ip_endpoint.get_ip_bytes()
        data += self._result.ip_endpoint.get_port_bytes()
        self.sending_buffer = data
