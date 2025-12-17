from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import ServerStatus
from frontends.gamespy.protocols.query_report.v1.contracts.results import HeartbeatPreResult


class HeartbeatPreResponse(ResponseBase):
    _result: HeartbeatPreResult

    def build(self) -> None:
        self.sending_buffer = "\\status\\"
