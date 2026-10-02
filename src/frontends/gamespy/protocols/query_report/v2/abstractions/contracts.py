import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import (
    PacketType,
    CommndName,
)

MAGIC_DATA = [0xFE, 0xFD]


class RequestBase(lib.RequestBase):
    instant_key: str
    command_name: CommndName
    raw_request: bytes

    @staticmethod
    def gs_parse(raw_request: bytes) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["command_name"] = raw_request[0]
        data_dict["instant_key"] = str(int.from_bytes(raw_request[1:5]))
        data = RequestBase(**data_dict)
        return data


class ResultBase(lib.ResultBase):
    packet_type: PacketType
    command_name: CommndName
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
