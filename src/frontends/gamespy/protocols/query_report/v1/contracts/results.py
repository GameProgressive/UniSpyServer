from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import ResultBase
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import ServerStatus


class HeartbeatPreResult(ResultBase):
    status: ServerStatus
    game_name: str
