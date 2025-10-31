from typing import TYPE_CHECKING, Optional, cast
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
import xml.etree.ElementTree as ET

from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import LoginProfileRequest, LoginProfileWithGameIdRequest, LoginRemoteAuthRequest, LoginRemoteAuthWithGameIdRequest, LoginUniqueNickRequest, LoginUniqueNickWithGameIdRequest
from frontends.gamespy.protocols.web_services.modules.auth.applications.handlers import LoginProfileHandler, LoginProfileWithGameIdHandler, LoginRemoteAuthHandler, LoginRemoteAuthWithGameIdHandler, LoginUniqueNickHandler, LoginUniqueNickWithGameIdHandler
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.requests import GetPurchaseHistoryRequest, GetStoreAvailabilityRequest
from frontends.gamespy.protocols.web_services.modules.direct2game.applications.handlers import GetPurchaseHistoryHandler, GetStoreAvailabilityHandler
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, SearchForRecordsRequest
from frontends.gamespy.protocols.web_services.modules.sake.applications.handlers import CreateRecordHandler, GetMyRecordsHandler, SearchForRecordsHandler


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: ClientBase, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        # assert isinstance(client,Client)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        name_node = ET.fromstring(self._raw_request)[0][0]
        if name_node is None:
            raise WebException("name node is missing from soap request")
        if name_node.tag is None:
            raise WebException(
                "name node text field is missing from soap request")
        name = name_node.tag.split("}")[1]

        if len(name) < 4:
            raise WebException("request name invalid")
        self._requests.append((name, self._raw_request))

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)

        match name:
            # Altas services
            case "CreateMatchlessSession":
                raise NotImplementedError()
            case "CreateSession":
                raise NotImplementedError()
            case "SetReportIntention":
                raise NotImplementedError()
            case "SubmitReport":
                raise NotImplementedError()

            # Auth services
            case "LoginProfile":
                return LoginProfileHandler(
                    self._client, LoginProfileRequest(raw_request))
            case "LoginProfileWithGameId":
                return LoginProfileWithGameIdHandler(
                    self._client, LoginProfileWithGameIdRequest(raw_request))
            case "LoginRemoteAuth":
                return LoginRemoteAuthHandler(self._client, LoginRemoteAuthRequest(raw_request))

            case "LoginRemoteAuthWithGameId":
                return LoginRemoteAuthWithGameIdHandler(self._client, LoginRemoteAuthWithGameIdRequest(raw_request))

            case "LoginUniqueNick":
                return LoginUniqueNickHandler(self._client, LoginUniqueNickRequest(raw_request))
            case "LoginUniqueNickWithGameId":
                return LoginUniqueNickWithGameIdHandler(self._client, LoginUniqueNickWithGameIdRequest(raw_request))

            # Direct2Game services

            case "GetStoreAvailability":
                return GetStoreAvailabilityHandler(self._client, GetStoreAvailabilityRequest(raw_request))

            case "GetPurchaseHistory":
                return GetPurchaseHistoryHandler(self._client, GetPurchaseHistoryRequest(raw_request))

            case "GetTargettedAd":
                raise NotImplementedError()

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

            # SAKE
            case "CreateRecord":
                return CreateRecordHandler(self._client, CreateRecordRequest(raw_request))
            case "DeleteRecord":
                raise NotImplementedError()
            case "GetMyRecords":
                return GetMyRecordsHandler(
                    self._client, GetMyRecordsRequest(raw_request))
            case "GetRandomRecords":
                raise NotImplementedError()
            case "GetRecordLimit":
                raise NotImplementedError()
            case "RateRecord":
                raise NotImplementedError()
            case "SearchForRecords":
                return SearchForRecordsHandler(self._client, SearchForRecordsRequest(raw_request))
            case "UpdateRecord":
                raise NotImplementedError()
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
