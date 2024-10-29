from servers.query_report.src.v2.abstractions.contracts import ResultBase
from servers.query_report.src.v2.aggregates.enums import PacketType


class ChallengeResult(ResultBase):
    packet_type: PacketType = PacketType.CHALLENGE


class ClientMessageResult(ResultBase):
    natneg_message: bytes
    message_key: int
    packet_type: PacketType = PacketType.CLIENT_MESSAGE


class EchoResult(ResultBase):
    info: dict
    packet_type: PacketType = PacketType.ECHO


class HeartBeatResult(ResultBase):
    packet_type: PacketType = PacketType.HEARTBEAT
    remote_ip_address:str
    remote_port:int
