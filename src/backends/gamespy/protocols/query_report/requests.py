
from pydantic import UUID4
import backends.gamespy.library.abstractions.contracts as lib

from servers.query_report.src.v2.enums.general import GameServerStatus, RequestType


class RequestBase(lib.RequestBase):
    instant_key: int
    command_name: RequestType
    raw_request: bytes


class AvaliableRequest(RequestBase):
    pass


class ChallengeRequest(RequestBase):
    pass


class ClientMessageAckRequest(RequestBase):
    pass


class ClientMessageRequest(RequestBase):
    server_browser_sender_id: UUID4
    natneg_message: list[int]
    target_ip_address: str
    target_port: str
    message_key: int
    cookie: int


class HeartBeatRequest(RequestBase):
    server_data: dict[str, str]
    player_data: list[dict[str, str]]
    team_data: list[dict[str, str]]
    server_status: GameServerStatus
    group_id: int


class EchoRequest(RequestBase):
    pass


class KeepAliveRequest(RequestBase):
    pass
