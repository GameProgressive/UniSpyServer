from socket import inet_ntoa
import socket
import struct


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
    RequestType,
)


class AddressCheckRequest(CommonRequestBase):
    pass


class PingRequest(RequestBase):
    def parse(self) -> None:
        self.version = int(self.raw_request[6])
        self.command_name = RequestType(self.raw_request[7])
        self.cookie = int.from_bytes(
            self.raw_request[8:12], byteorder="little")
        self.ip = socket.inet_ntoa(self.raw_request[12:16])
        self.port = int.from_bytes(self.raw_request[16:18])
        # port here is not in little endian
        self.got_your_data = bool(self.raw_request[18])
        self.is_finished = bool(self.raw_request[19])


class ConnectAckRequest(RequestBase):
    client_index: NatClientIndex

    def parse(self) -> None:
        if len(self.raw_request) < 12:
            return

        self.version = int(self.raw_request[6])
        self.command_name = RequestType(self.raw_request[7])
        self.cookie = int.from_bytes(
            self.raw_request[8:12])
        self.client_index = NatClientIndex(self.raw_request[13])


class ConnectRequest(CommonRequestBase):
    """
    Server will send this request to client to let them connect to each other
    """

    client_index: NatClientIndex

    @staticmethod
    def build(version: int, command_name: RequestType, cookie: int, port_type: NatPortType, client_index: NatClientIndex, use_game_port: bool) -> bytes:
        data = bytes()
        data += MAGIC_DATA
        data += version.to_bytes(1)
        data += command_name.value.to_bytes(1)
        data += cookie.to_bytes(4)
        data += port_type.value.to_bytes(1)
        data += client_index.value.to_bytes(1)
        data += use_game_port.to_bytes(1)
        return data


class ErtAckRequest(CommonRequestBase):
    pass


class InitRequest(CommonRequestBase):
    game_name: str | None
    private_ip: str
    private_port: int

    def __init__(self, raw_request: bytes | None = None):
        super().__init__(raw_request)
        self.game_name = "unknown"

    def parse(self) -> None:
        super().parse()
        ip_bytes = self.raw_request[15:19]
        port_bytes = self.raw_request[19:21][::-1]
        port = struct.unpack("H", port_bytes)[0]
        ip_address_str = inet_ntoa(ip_bytes)
        self.private_ip = ip_address_str
        self.private_port = port

        if len(self.raw_request) > 21 and self.raw_request[-1] == 0:
            game_name_bytes = self.raw_request[21:-1]
            game_name = game_name_bytes.decode("ascii").replace("\x00", "")
            if len(game_name) != 0:
                self.game_name = game_name


class NatifyRequest(CommonRequestBase):
    pass


class PreInitRequest(RequestBase):
    state: PreInitState
    target_cookie: int

    def parse(self) -> None:
        super().parse()
        self.state = PreInitState(self.raw_request[12])
        self.target_cookie = int.from_bytes(self.raw_request[13:17])


class ReportRequest(CommonRequestBase):
    is_nat_success: bool
    game_name: str
    nat_type: NatType
    mapping_scheme: NatPortMappingScheme

    def parse(self):
        super().parse()
        if len(self.raw_request) < 12:
            return
        self.version = self.raw_request[6]
        self.command_name = RequestType(self.raw_request[7])
        self.cookie = int.from_bytes(self.raw_request[8:12], byteorder="big")
        self.port_type = NatPortType(self.raw_request[12])
        self.client_index = NatClientIndex(self.raw_request[13])
        self.is_nat_success = False if self.raw_request[14] == 0 else True
        self.nat_type = NatType(self.raw_request[15])
        self.mapping_scheme = NatPortMappingScheme(self.raw_request[17])

        end_index = self.raw_request[23:].index(0)
        self.game_name = self.raw_request[23: 23 + end_index].decode("ascii")
