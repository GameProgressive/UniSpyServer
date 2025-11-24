from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.query_report.v1.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.query_report.v1.contracts.requests import LegacyHeartbeatRequest, HeartbeatPreRequest
from frontends.gamespy.protocols.query_report.v1.contracts.responses import HeartbeatPreResponse
from frontends.gamespy.protocols.query_report.v1.contracts.results import HeartbeatPreResult


class HeartbeatPreHandler(CmdHandlerBase):
    _request: HeartbeatPreRequest
    _result: HeartbeatPreResult
    _response: HeartbeatPreResponse

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._is_fetching = False
        self._is_uploading = False

    def _response_construct(self) -> None:
        self._result = HeartbeatPreResult(
            status=self._request.status,
            game_name=self._request.game_name)
        self._response = HeartbeatPreResponse(self._result)


class LegacyHeartbeatHandler(CmdHandlerBase):
    _request: LegacyHeartbeatRequest

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._is_fetching = False
