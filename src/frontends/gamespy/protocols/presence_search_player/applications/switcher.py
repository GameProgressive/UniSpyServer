from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.extentions.string_extentions import (
    get_kv_str_name,
    split_nested_kv_str,
)
from frontends.gamespy.protocols.presence_search_player.abstractions.handler import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.presence_search_player.applications.client import (
    Client,
)
from frontends.gamespy.protocols.presence_search_player.applications.handlers import (
    CheckHandler,
    NewUserHandler,
    NicksHandler,
    OthersHandler,
    OthersListHandler,
    SearchHandler,
    SearchUniqueHandler,
    UniqueSearchHandler,
    ValidHandler,
)
from frontends.gamespy.protocols.presence_search_player.contracts.requests import (
    CheckRequest,
    NewUserRequest,
    NicksRequest,
    OthersListRequest,
    OthersRequest,
    SearchRequest,
    SearchUniqueRequest,
    UniqueSearchRequest,
    ValidRequest,
)


class Switcher(SwitcherBase):
    _raw_request: str
    _client: Client

    def __init__(self, client: Client, raw_request: str):
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self):
        if self._raw_request[0] != "\\":
            self._client.log_info("Invalid request received!")
            return
        raw_requests = split_nested_kv_str(self._raw_request)
        for raw_request in raw_requests:
            name = get_kv_str_name(raw_request)
            if name not in CommandName:
                self._client.log_debug(f"Request: {name} is not a valid request.")
                continue
            self._requests.append((CommandName(name), raw_request))

    def _create_cmd_handlers(
        self, name: CommandName, raw_request: str
    ) -> CmdHandlerBase | None:
        assert isinstance(name, CommandName)
        match name:
            case CommandName.CHECK:
                return CheckHandler(self._client, CheckRequest.gs_parse(raw_request))
            case CommandName.NEWUSER:
                return NewUserHandler(self._client, NewUserRequest.gs_parse(raw_request))
            case CommandName.NICKS:
                return NicksHandler(self._client, NicksRequest.gs_parse(raw_request))
            case CommandName.OTHERS:
                return OthersHandler(self._client, OthersRequest.gs_parse(raw_request))
            case CommandName.OTHERSLIST:
                return OthersListHandler(self._client, OthersListRequest.gs_parse(raw_request))
            case CommandName.PMATCH:
                # Uncomment the line below when PMatchHandler is implemented
                # return PMatchHandler(self._client, PMatchRequest.gs_parse(raw_request))
                raise NotImplementedError("PMatchHandler is not implemented.")
            case CommandName.SEARCH:
                return SearchHandler(self._client, SearchRequest.gs_parse(raw_request))
            case CommandName.SEARCHUNIQUE:
                return SearchUniqueHandler(
                    self._client, SearchUniqueRequest.gs_parse(raw_request)
                )
            case CommandName.UNIQUESearch:
                return UniqueSearchHandler(
                    self._client, UniqueSearchRequest.gs_parse(raw_request)
                )
            case CommandName.VALID:
                return ValidHandler(self._client, ValidRequest.gs_parse(raw_request))
            case _:
                return None
