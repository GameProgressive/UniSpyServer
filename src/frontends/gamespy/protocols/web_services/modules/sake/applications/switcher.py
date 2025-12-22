from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.applications.client import Client
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, SearchForRecordsRequest
from frontends.gamespy.protocols.web_services.modules.sake.applications.handlers import CreateRecordHandler, GetMyRecordsHandler, SearchForRecordsHandler


class Switcher(web.Switcher):

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        match name:
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
