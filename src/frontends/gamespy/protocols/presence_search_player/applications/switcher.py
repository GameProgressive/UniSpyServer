from typing import TYPE_CHECKING, Optional, cast
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import RequestType
from frontends.gamespy.protocols.presence_search_player.contracts.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest

from frontends.gamespy.protocols.presence_search_player.applications.handlers import CheckHandler, NewUserHandler, NicksHandler, OthersHandler, OthersListHandler, SearchHandler, SearchUniqueHandler, UniqueSearchHandler, ValidHandler

from frontends.gamespy.protocols.presence_search_player.abstractions.handler import CmdHandlerBase

from frontends.gamespy.protocols.presence_search_player.applications.client import Client


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: Client, raw_request: str):
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self):
        if self._raw_request[0] != "\\":
            self._client.log_info("Invalid request received!")
            return
        raw_requests = [
            r+"\\final\\" for r in self._raw_request.split("\\final\\") if r]
        for raw_request in raw_requests:
            name = raw_request.strip("\\").split("\\", 1)[0]
            if name not in RequestType:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                continue
            self._requests.append((RequestType(name), raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, RequestType)
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)

        match name:
            case RequestType.CHECK:
                return CheckHandler(self._client, CheckRequest(raw_request))
            case RequestType.NEWUSER:
                return NewUserHandler(self._client, NewUserRequest(raw_request))
            case RequestType.NICKS:
                return NicksHandler(self._client, NicksRequest(raw_request))
            case RequestType.OTHERS:
                return OthersHandler(self._client, OthersRequest(raw_request))
            case RequestType.OTHERSLIST:
                return OthersListHandler(self._client, OthersListRequest(raw_request))
            case RequestType.PMATCH:
                # Uncomment the line below when PMatchHandler is implemented
                # return PMatchHandler(self._client, PMatchRequest(raw_request))
                raise NotImplementedError("PMatchHandler is not implemented.")
            case RequestType.SEARCH:
                return SearchHandler(self._client, SearchRequest(raw_request))
            case RequestType.SEARCHUNIQUE:
                return SearchUniqueHandler(self._client, SearchUniqueRequest(raw_request))
            case RequestType.UNIQUESearch:
                return UniqueSearchHandler(self._client, UniqueSearchRequest(raw_request))
            case RequestType.VALID:
                return ValidHandler(self._client, ValidRequest(raw_request))
            case _:
                return None
