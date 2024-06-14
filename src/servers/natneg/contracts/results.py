from servers.natneg.abstractions.contracts import CommonResultBase, ResultBase
from servers.natneg.enums.general import ConnectPacketStatus, PreInitState, ResponseType


class AddressCheckResult(CommonResultBase):
    packet_type: ResponseType = bytes(ResponseType.ADDRESS_REPLY)


class ConnectResult(ResultBase):
    got_your_data: bytes = bytes([1])
    finished: ConnectPacketStatus = ConnectPacketStatus.NO_ERROR
    ip: str
    port: int
    version: bytes
    cookie: bytes

    def __init__(self) -> None:
        super().__init__()
        self.packet_type = ResponseType.CONNECT


class ErtAckResult(CommonResultBase):
    def __init__(self) -> None:
        super().__init__()
        self.packet_type = ResponseType.ERT_ACK


class InitResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.INIT_ACK


class NatifyResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.ERT_TEST


class PreInitResult(ResultBase):
    client_index: int
    state: PreInitState
    client_id: int

    def __init__(self) -> None:
        super().__init__()
        self.packet_type = ResponseType.PRE_INIT_ACK
        self.state = PreInitState.READY


class ReportResult(ResultBase):
    packet_type = ResponseType.REPORT_ACK
