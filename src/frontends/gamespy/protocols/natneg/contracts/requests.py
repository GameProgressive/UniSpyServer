import socket
import struct
from socket import inet_ntoa

# from frontends.gamespy.library.extentions.string_extentions import IPEndPoint
from frontends.gamespy.protocols.natneg.abstractions.contracts import (
    MAGIC_DATA,
    CommonRequestBase,
    RequestBase,
)
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
    PreInitState,
    CommandName,
)


class AddressCheckRequest(CommonRequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "AddressCheckRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data = AddressCheckRequest(**data.model_dump())
        return data


class PingRequest(RequestBase):
    ip: str
    port: int
    got_your_data: bool
    is_finished: bool

    @staticmethod
    def gs_parse(raw_request: bytes) -> "PingRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["ip"] = socket.inet_ntoa(raw_request[12:16])
        data_dict["port"] = int.from_bytes(raw_request[16:18])
        data_dict["got_your_data"] = bool(raw_request[18])
        data_dict["is_finished"] = bool(raw_request[19])

        data = PingRequest(**data_dict)

        return data


class ConnectAckRequest(RequestBase):
    client_index: NatClientIndex

    @staticmethod
    def gs_parse(raw_request: bytes) -> "ConnectAckRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["client_index"] = raw_request[13]
        data = ConnectAckRequest(**data_dict)
        return data


class ConnectRequest(CommonRequestBase):
    """
    Server will send this request to client to let them connect to each other
    """

    @staticmethod
    def gs_parse(raw_request: bytes) -> "ConnectRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data = ConnectRequest(**data.model_dump())
        return data


class ErtAckRequest(CommonRequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "ErtAckRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data = ErtAckRequest(**data.model_dump())
        return data


class InitRequest(CommonRequestBase):
    game_name: str | None
    private_ip: str
    private_port: int

    @staticmethod
    def gs_parse(raw_request: bytes) -> "InitRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        ip_bytes = raw_request[15:19]
        port_bytes = raw_request[19:21][::-1]
        data_dict["private_port"] = struct.unpack("H", port_bytes)[0]
        data_dict["private_ip"] = inet_ntoa(ip_bytes)
        data_dict.update(game_name=None)
        if len(raw_request) > 21 and raw_request[-1] == 0:
            game_name_bytes = raw_request[21:-1]
            decoded_game_name = game_name_bytes.decode("ascii").replace("\x00", "")

            if len(decoded_game_name) != 0:
                data_dict["game_name"] = decoded_game_name

        data = InitRequest(**data_dict)

        return data


class NatifyRequest(CommonRequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "NatifyRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data = NatifyRequest(**data.model_dump())
        return data


class PreInitRequest(RequestBase):
    state: int
    """
    todo check the type of state
    """
    target_cookie: int
    client_index: NatClientIndex

    @staticmethod
    def gs_parse(raw_request: bytes) -> "PreInitRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["client_index"] = raw_request[12]
        data_dict["state"] = int(raw_request[13])
        data_dict["target_cookie"] = int.from_bytes(raw_request[14:18])

        data = PreInitRequest(**data_dict)

        return data


class ReportRequest(CommonRequestBase):
    is_nat_success: bool
    game_name: str
    nat_type: NatType
    mapping_scheme: NatPortMappingScheme

    @staticmethod
    def gs_parse(raw_request: bytes) -> "ReportRequest":
        data = CommonRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["is_nat_success"] = False if raw_request[14] == 0 else True
        data_dict["nat_type"] = raw_request[15]
        data_dict["mapping_scheme"] = raw_request[17]

        end_index = raw_request[23:].index(0)
        data_dict["game_name"] = raw_request[23 : 23 + end_index].decode("ascii")

        data = ReportRequest(**data_dict)

        return data
