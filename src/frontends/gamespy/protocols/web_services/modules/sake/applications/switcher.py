from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.library.network.http_handler import HttpData
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, GetRecordCountRequest, SearchForRecordsRequest, UpdateRecordRequest
from frontends.gamespy.protocols.web_services.modules.sake.applications.handlers import CreateRecordHandler, GetMyRecordsHandler, GetRecordCountHandler, SearchForRecordsHandler, UpdateRecordHandler


class Switcher(web.Switcher):
    _raw_request: str

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        match name:
            case "GetSpecificRecords":
                raise NotImplementedError()
            case "CreateRecord":
                return CreateRecordHandler(self._client, CreateRecordRequest(raw_request))
            case "DeleteRecord":
                raise NotImplementedError()
            case "GetRecordCount":
                return GetRecordCountHandler(self._client, GetRecordCountRequest(raw_request))
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
                return UpdateRecordHandler(self._client, UpdateRecordRequest(raw_request))
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
