from frontends.gamespy.protocols.natneg.abstractions.contracts import CommonResultBase, ResultBase
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    ConnectPacketStatus,
    PreInitState,
    ResponseType,
)


class AddressCheckResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.ADDRESS_REPLY


class ConnectResult(ResultBase):
    is_both_client_ready: bool
    got_your_data: bytes = bytes([1])
    status: ConnectPacketStatus | None
    ip: str | None
    port: int | None
    packet_type: ResponseType = ResponseType.CONNECT


class InitResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.INIT_ACK


class ErtAckResult(InitResult):
    packet_type: ResponseType = ResponseType.ERT_ACK


class NatifyResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.ERT_TEST


class PreInitResult(ResultBase):
    client_index: int
    state: PreInitState
    client_id: int
    packet_type: ResponseType = ResponseType.PRE_INIT_ACK
    state = PreInitState.READY


class ReportResult(ResultBase):
    packet_type: ResponseType = ResponseType.REPORT_ACK
