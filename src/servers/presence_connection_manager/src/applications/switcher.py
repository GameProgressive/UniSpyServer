from library.src.abstractions.switcher import SwitcherBase
from servers.presence_connection_manager.src.contracts.requests import KeepAliveRequest, LoginRequest, LogoutRequest, StatusInfoRequest, StatusRequest, AddBlockRequest, GetProfileRequest, NewProfileRequest, RegisterCDKeyRequest, NewUserRequest, RegisterNickRequest, UpdateProfileRequest
from servers.presence_connection_manager.src.applications.handlers import AddBlockHandler, GetProfileHandler, KeepAliveHandler, LoginHandler, LogoutHandler, NewProfileHandler, NewUserHandler, RegisterCDKeyHandler, RegisterNickHandler, StatusHandler, StatusInfoHandler, UpdateProfileHandler
from servers.presence_search_player.src.aggregates.exceptions import GPParseException

from servers.presence_connection_manager.src.abstractions.handlers import CmdHandlerBase
from typing import TYPE_CHECKING, Optional, cast

if TYPE_CHECKING:
    from servers.presence_connection_manager.src.applications.client import Client


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: Client, raw_request: str) -> None:
        super().__init__(client, raw_request)
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)

    def _process_raw_request(self) -> None:
        if self._raw_request[0] != "\\":
            raise GPParseException("Request format is invalid")
        raw_requests = [
            r+"\\final\\" for r in self._raw_request.split("\\final\\") if r]
        for raw_request in raw_requests:
            name = raw_request.strip("\\").split("\\")[0]
            self._requests.append((name, raw_request))

    def _create_cmd_handlers(self, name: str, raw_request: str) -> Optional[CmdHandlerBase]:
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case "ka":
                return KeepAliveHandler(self._client, KeepAliveRequest(raw_request))
            case "login":
                return LoginHandler(self._client, LoginRequest(raw_request))
            case "logout":
                return LogoutHandler(self._client, LogoutRequest(raw_request))
            case "newuser":
                return NewUserHandler(self._client, NewUserRequest(raw_request))
            case "addblock":
                return AddBlockHandler(self._client, AddBlockRequest(raw_request))
            case "getprofile":
                return GetProfileHandler(self._client, GetProfileRequest(raw_request))
            case "newprofile":
                return NewProfileHandler(self._client, NewProfileRequest(raw_request))
            case "registercdkey":
                return RegisterCDKeyHandler(self._client, RegisterCDKeyRequest(raw_request))
            case "registernick":
                return RegisterNickHandler(self._client, RegisterNickRequest(raw_request))
            case "updatepro":
                return UpdateProfileHandler(self._client, UpdateProfileRequest(raw_request))
            case "status":
                return StatusHandler(self._client, StatusRequest(raw_request))
            case "statusinfo":
                return StatusInfoHandler(self._client, StatusInfoRequest(raw_request))
            case _:
                return None
