from library.src.abstractions.client import ClientBase
from library.src.abstractions.handler import CmdHandlerBase
from library.src.abstractions.switcher import SwitcherBase
import xml.etree.ElementTree as ET

from servers.web_services.src.exceptions.general import WebExceptions
from servers.web_services.src.modules.auth.contracts.requests import LoginProfileRequest, LoginProfileWithGameIdRequest, LoginRemoteAuthRequest, LoginRemoteAuthWithGameIdRequest, LoginUniqueNickRequest, LoginUniqueNickWithGameIdRequest
from servers.web_services.src.modules.auth.handlers.general import LoginProfileHandler, LoginProfileWithGameIdHandler, LoginRemoteAuthHandler, LoginRemoteAuthWithGameIdHandler, LoginUniqueNickHandler, LoginUniqueNickWithGameIdHandler
from servers.web_services.src.modules.direct2game.contracts.requests import GetPurchaseHistoryRequest, GetStoreAvailabilityRequest
from servers.web_services.src.modules.direct2game.handlers.general import GetPurchaseHistoryHandler, GetStoreAvailabilityHandler
from servers.web_services.src.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, SearchForRecordsRequest
from servers.web_services.src.modules.sake.handlers.general import CreateRecordHandler, GetMyRecordsHandler, SearchForRecordsHandler


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: ClientBase, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        # assert isinstance(client,Client)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        name_node = ET.fromstring(self._raw_request)[0][0]
        if name_node is None:
            raise WebExceptions("name node is missing from soap request")

        name = name_node.text.split("}")[1]

        if len(name) < 4:
            raise WebExceptions("request name invalid")
        self._requests.append((name, self._raw_request))

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, str)
        assert isinstance(raw_request, str)

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
                return LoginRemoteAuthWithGameIdHandler(self._client, LoginRemoteAuthWithGameIdRequest)

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
                raise GetMyRecordsHandler(
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
