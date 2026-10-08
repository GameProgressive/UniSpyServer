from frontends.gamespy.library.network.http_data import HttpData
import frontends.gamespy.protocols.web_services.applications.switcher as web
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.web_services.modules.sake.applications.handlers import (
    CreateRecordHandler,
    GetMyRecordsHandler,
    GetRecordCountHandler,
    SearchForRecordsHandler,
    UpdateRecordHandler,
)
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import (
    CreateRecordRequest,
    GetMyRecordsRequest,
    GetRecordCountRequest,
    SearchForRecordsRequest,
    UpdateRecordRequest,
)


class Switcher(web.Switcher):
    _raw_request: HttpData

    def _create_cmd_handlers(
        self, name: str, raw_request: HttpData
    ) -> CmdHandlerBase | None:
        cmd = CommandName(name)
        match cmd:
            case CommandName.GET_SPECIFIC_RECORD:
                raise NotImplementedError()
            case CommandName.CREATE_RECORD:
                return CreateRecordHandler(
                    self._client, CreateRecordRequest.gs_parse(raw_request)
                )
            case CommandName.DELETE_RECORD:
                raise NotImplementedError()
            case CommandName.GET_RECORD_COUNT:
                return GetRecordCountHandler(
                    self._client, GetRecordCountRequest.gs_parse(raw_request)
                )
            case CommandName.GET_MY_RECORD:
                return GetMyRecordsHandler(
                    self._client, GetMyRecordsRequest.gs_parse(raw_request)
                )
            case CommandName.GET_RAMDOM_RECORD:
                raise NotImplementedError()
            case CommandName.GET_RECORD_LIMIT:
                raise NotImplementedError()
            case CommandName.RATE_RECORD:
                raise NotImplementedError()
            case CommandName.SEARCH_FOR_RECORD:
                return SearchForRecordsHandler(
                    self._client, SearchForRecordsRequest.gs_parse(raw_request)
                )
            case CommandName.UPDATE_RECORD:
                return UpdateRecordHandler(
                    self._client, UpdateRecordRequest.gs_parse(raw_request)
                )
            case _:
                self._client.log_error(f"Unknown {name} request received")
                return None
