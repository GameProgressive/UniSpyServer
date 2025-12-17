from typing import final
from frontends.gamespy.protocols.query_report.v2.abstractions.contracts import ResultBase
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import PacketType


@final
class AvailableResult(ResultBase):
    packet_type: PacketType = PacketType.AVALIABLE_CHECK


@final
class ChallengeResult(ResultBase):
    packet_type: PacketType = PacketType.CHALLENGE


@final
class ClientMessageResult(ResultBase):
    natneg_message: bytes
    message_key: int
    packet_type: PacketType = PacketType.CLIENT_MESSAGE


@final
class EchoResult(ResultBase):
    info: dict
    packet_type: PacketType = PacketType.ECHO


@final
class HeartbeatResult(ResultBase):
    """
    this result is replied in unispy server
    """
    packet_type: PacketType = PacketType.HEARTBEAT
    remote_ip: str
    remote_port: int
