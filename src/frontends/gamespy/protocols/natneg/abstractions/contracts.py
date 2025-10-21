
import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortType,
    RequestType,
    ResponseType,
)
from frontends.gamespy.library.extentions.bytes_extentions import ip_to_4_bytes

MAGIC_DATA = b"\xfd\xfc\x1e\x66\x6a\xb2"


class RequestBase(lib.RequestBase):
    version: int
    #! check bytes order
    cookie: int
    """
    byteorder:
        big
    """
    port_type: NatPortType
    command_name: RequestType
    raw_request: bytes

    def __init__(self, raw_request: bytes | None = None):
        assert isinstance(raw_request, bytes)
        self.raw_request = raw_request

    def parse(self) -> None:
        if len(self.raw_request) < 12:
            return

        self.version = int(self.raw_request[6])
        self.command_name = RequestType(self.raw_request[7])
        self.cookie = int.from_bytes(
            self.raw_request[8:12])
        self.port_type = NatPortType(self.raw_request[12])


class ResultBase(lib.ResultBase):
    packet_type: ResponseType
    version: int
    cookie: int
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase
    sending_buffer: bytes

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)

    def build(self) -> None:
        data = bytearray()
        data.extend(MAGIC_DATA)
        data.append(self._result.version)
        data.append(self._result.packet_type.value)
        data.extend(self._result.cookie.to_bytes(4))
        self.sending_buffer = bytes(data)


class CommonRequestBase(RequestBase):
    client_index: NatClientIndex
    use_game_port: bool

    def parse(self):
        super().parse()
        self.client_index = NatClientIndex(self.raw_request[13])
        self.use_game_port = bool(self.raw_request[14])


class CommonResultBase(ResultBase):
    public_ip_addr: str
    public_port: int
    port_type: NatPortType
    client_index: NatClientIndex
    use_game_port: bool


class CommonResponseBase(ResponseBase):
    _result: CommonResultBase

    def build(self) -> None:
        super().build()
        data = bytearray()
        data.extend(self.sending_buffer)
        data.append(self._result.port_type.value)
        data.append(self._result.client_index.value)
        data.append(int(self._result.use_game_port))
        data.extend(ip_to_4_bytes(self._result.public_ip_addr))
        data.extend(self._result.public_port.to_bytes(2))
        self.sending_buffer = bytes(data)
