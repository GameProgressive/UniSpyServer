from typing import Optional
from library.src.abstractions.switcher import SwitcherBase
from servers.presence_search_player.src.contracts.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest

from servers.presence_search_player.src.handlers.handlers import CheckHandler, NewUserHandler, NicksHandler, OthersHandler, OthersListHandler, SearchHandler, SearchUniqueHandler, UniqueSearchHandler, ValidHandler

from servers.presence_search_player.src.abstractions.handler import CmdHandlerBase

from servers.presence_search_player.src.applications.client import Client


class CmdSwitcher(SwitcherBase):

    def __init__(self, client: Client, raw_request: str):
        super().__init__(client, raw_request)
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)

    def _process_raw_request(self):
        if self._raw_request[0] != "\\":
            self._client.log_info("Invalid request received!")
            return
        raw_requests = [
            r+"\\final\\" for r in self._raw_request.split("\\final\\") if r]
        for raw_request in raw_requests:
            name = raw_request.strip("\\").split("\\", 1)[0]
            self._requests.append((name, raw_request))

    def _create_cmd_handlers(self, name: str, raw_request: str) -> Optional[CmdHandlerBase]:
        match name:
            case "check":
                return CheckHandler(self._client, CheckRequest(raw_request))
            case "newuser":
                return NewUserHandler(self._client, NewUserRequest(raw_request))
            case "nicks":
                return NicksHandler(self._client, NicksRequest(raw_request))
            case "others":
                return OthersHandler(self._client, OthersRequest(raw_request))
            case "otherslist":
                return OthersListHandler(self._client, OthersListRequest(raw_request))
            case "pmatch":
                # return PMatchHandler(self._client, PMatchRequest(raw_request))
                raise NotImplementedError()
            case "search":
                return SearchHandler(self._client, SearchRequest(raw_request))
            case "searchunique":
                return SearchUniqueHandler(
                    self._client, SearchUniqueRequest(raw_request)
                )
            case "uniquesearch":
                return UniqueSearchHandler(
                    self._client, UniqueSearchRequest(raw_request)
                )
            case "valid":
                return ValidHandler(self._client, ValidRequest(raw_request))
            case _:
                return None
