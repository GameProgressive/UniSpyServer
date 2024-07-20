from library.src.extentions.string_extentions import IPEndPoint
from servers.query_report.src.v2.abstractions.result_base import ResultBase
from servers.query_report.src.v2.enums.general import PacketType


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
    remote_ip_endpoint: IPEndPoint
    packet_type: PacketType = PacketType.HEARTBEAT
