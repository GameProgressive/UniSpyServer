
from pydantic import UUID4
import backends.library.abstractions.contracts as lib

from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import RequestType as V2RequestType

from frontends.gamespy.protocols.query_report.v1.aggregates.enums import RequestType as V1RequestType


class RequestBase(lib.RequestBase):
    instant_key: str
    command_name: V2RequestType
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
    data: dict[str, str]
    status: GameServerStatus
    group_id: int | None
    game_name: str




class LegacyHeartbeatRequest(lib.RequestBase):
    command_name: V1RequestType
    raw_request: str
    query_id: str
    game_name: str
    data: dict[str, str]

class EchoRequest(RequestBase):
    pass


class KeepAliveRequest(RequestBase):
    pass
