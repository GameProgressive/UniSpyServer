from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, GetRecordCountRequest, SearchForRecordsRequest, UpdateRecordRequest
from frontends.gamespy.protocols.web_services.modules.sake.applications.handlers import CreateRecordHandler, GetMyRecordsHandler, GetRecordCountHandler, SearchForRecordsHandler, UpdateRecordHandler
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import CommandName


class Switcher(web.Switcher):
    _raw_request: str

    def _create_cmd_handlers(self, name: str, raw_request: str) -> CmdHandlerBase | None:
        cmd = CommandName(name)
        match cmd:
            case CommandName.GET_SPECIFIC_RECORD:
                raise NotImplementedError()
            case CommandName.CREATE_RECORD:
                return CreateRecordHandler(self._client, CreateRecordRequest(raw_request))
            case CommandName.DELETE_RECORD:
                raise NotImplementedError()
            case CommandName.GET_RECORD_COUNT:
                return GetRecordCountHandler(self._client, GetRecordCountRequest(raw_request))
            case CommandName.GET_MY_RECORD:
                return GetMyRecordsHandler(
                    self._client, GetMyRecordsRequest(raw_request))
            case CommandName.GET_RAMDOM_RECORD:
                raise NotImplementedError()
            case CommandName.GET_RECORD_LIMIT:
                raise NotImplementedError()
            case CommandName.RATE_RECORD:
                raise NotImplementedError()
            case CommandName.SEARCH_FOR_RECORD:
                return SearchForRecordsHandler(self._client, SearchForRecordsRequest(raw_request))
            case CommandName.UPDATE_RECORD:
                return UpdateRecordHandler(self._client, UpdateRecordRequest(raw_request))
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
