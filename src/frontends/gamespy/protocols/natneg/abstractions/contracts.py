import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.bytes_extentions import ip_to_4_bytes
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortType,
    CommandName,
    ResponseType,
)

MAGIC_DATA = b"\xfd\xfc\x1e\x66\x6a\xb2"


class RequestBase(lib.RequestBase):
    version: int
    cookie: int
    """
    byteorder: big
    """
    command_name: CommandName
    raw_request: bytes

    @staticmethod
    def gs_parse(raw_request: bytes) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data.command_name = raw_request[7]
        data_dict = data.model_dump()
        data_dict["version"] = int(raw_request[6])
        data_dict["cookie"] = int.from_bytes(raw_request[8:12])
        data = RequestBase(**data_dict)
        return data


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
    port_type: NatPortType

    @staticmethod
    def gs_parse(raw_request: bytes) -> "CommonRequestBase":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["port_type"] = int(raw_request[12])
        data_dict["client_index"] = int(raw_request[13])
        data_dict["use_game_port"] = bool(raw_request[14])
        data = CommonRequestBase(**data_dict)
        return data


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
