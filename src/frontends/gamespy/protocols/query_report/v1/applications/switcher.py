
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import RequestType
from frontends.gamespy.protocols.query_report.v1.applications.handlers import HeartbeatPreHandler, LegacyHeartbeatHandler
from frontends.gamespy.protocols.query_report.v1.contracts.requests import HeartbeatPreRequest, LegacyHeartbeatRequest


class Switcher(SwitcherBase):
    _raw_request: str
    _client: Client

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise QRException("Invalid request length")
        if self._raw_request[0] != "\\":
            raise QRException("Invalid queryreport v1 request")
        name = self._raw_request.strip("\\").split("\\")[0]
        if name not in RequestType:
            self._client.log_debug(
                f"Request: {name} is not a valid request.")
        self._requests.append((RequestType(name), self._raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: str) -> CmdHandlerBase | None:
        match(name):
            case RequestType.HEARTBEAT:
                return HeartbeatPreHandler(self._client, HeartbeatPreRequest(raw_request))
            case RequestType.HEARTBEAT_ACK:
                return LegacyHeartbeatHandler(self._client, LegacyHeartbeatRequest(raw_request))
