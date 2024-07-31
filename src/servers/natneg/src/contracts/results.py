from typing import Any
from servers.natneg.src.abstractions.contracts import CommonResultBase, ResultBase
from servers.natneg.src.enums.general import (
    ConnectPacketStatus,
    PreInitState,
    ResponseType,
)


class AddressCheckResult(CommonResultBase):
    packet_type: ResponseType = ResponseType.ADDRESS_REPLY


class ConnectResult(ResultBase):
    got_your_data: bytes = bytes([1])
    finished: ConnectPacketStatus = ConnectPacketStatus.NO_ERROR
    ip: str
    port: int
    version: bytes
    cookie: bytes
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
