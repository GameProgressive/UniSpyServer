
from pydantic import UUID4, Field, constr
import backends.library.abstractions.contracts as lib

from servers.query_report.src.v2.aggregates.enums import GameServerStatus, RequestType


class RequestBase(lib.RequestBase):
    instant_key: str = Field(..., max_length=10)
    command_name: RequestType
    raw_request: str


class AvaliableRequest(RequestBase):
    pass


class ChallengeRequest(RequestBase):
    pass


class ClientMessageAckRequest(RequestBase):
    pass


class ClientMessageRequest(RequestBase):
    server_browser_sender_id: UUID4
    natneg_message: bytes
    target_ip_address: str
    target_port: str
    message_key: int
    cookie: int


class HeartBeatRequest(RequestBase):
    server_data: dict[str, object]
    player_data: list[dict[str, object]]
    team_data: list[dict[str, object]]
    server_status: GameServerStatus
    group_id: int | None
    game_name: str


class EchoRequest(RequestBase):
    pass


class KeepAliveRequest(RequestBase):
    pass
