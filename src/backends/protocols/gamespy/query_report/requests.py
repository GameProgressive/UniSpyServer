
from pydantic import UUID4
import backends.library.abstractions.contracts as lib

from frontends.gamespy.protocols.query_report.v2.aggregates.enums import GameServerStatus, RequestType


class RequestBase(lib.RequestBase):
    instant_key: str
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
    target_query_report_id: UUID4
    natneg_message: str
    target_ip_address: str
    target_port: int
    command_name: None = None


class HeartBeatRequest(RequestBase):
    server_data: dict[str, object] | None
    player_data: list[dict[str, object]] | None
    team_data: list[dict[str, object]] | None
    server_status: GameServerStatus
    group_id: int | None
    game_name: str


class EchoRequest(RequestBase):
    pass


class KeepAliveRequest(RequestBase):
    pass
