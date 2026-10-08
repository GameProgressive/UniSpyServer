import xml.etree.ElementTree as ET

from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.aggregations.exceptions import (
    WebException,
)
from frontends.gamespy.protocols.web_services.applications.client import Client

class Switcher(SwitcherBase):
    """
    abstract class of web switcher
    """

    _raw_request: HttpData
    _client: Client

    def __init__(self, client: Client, raw_request: HttpData) -> None:
        assert isinstance(raw_request, HttpData)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        request = RequestBase.gs_parse(self._raw_request)
        name = request.command_name
        self._requests.append((name, self._raw_request))

    def _create_cmd_handlers(
        self, name: str, raw_request: HttpData
    ) -> CmdHandlerBase | None:
        match name:
            # InGameAd services
            case "ReportAdUsage":
                raise NotImplementedError()

            # PatchingAndTracking
            case "Motd":
                raise NotImplementedError()
            case "Vercheck":
                raise NotImplementedError()

            # Racing
            case "GetContestData":
                raise NotImplementedError()
            case "GetFriendRankings":
                raise NotImplementedError()
            case "GetRegionalData":
                raise NotImplementedError()
            case "GetTenAboveRankings":
                raise NotImplementedError()
            case "GetTopTenRankings":
                raise NotImplementedError()
            case "SubmitScores":
                raise NotImplementedError()
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
