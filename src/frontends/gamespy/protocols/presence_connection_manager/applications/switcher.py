from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import RequestType
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import KeepAliveRequest, LoginRequest, LogoutRequest, StatusInfoRequest, StatusRequest, AddBlockRequest, GetProfileRequest, NewProfileRequest, RegisterCDKeyRequest, NewUserRequest, RegisterNickRequest, UpdateProfileRequest
from frontends.gamespy.protocols.presence_connection_manager.applications.handlers import AddBlockHandler, GetProfileHandler, KeepAliveHandler, LoginHandler, LogoutHandler, NewProfileHandler, NewUserHandler, RegisterCDKeyHandler, RegisterNickHandler, StatusHandler, StatusInfoHandler, UpdateProfileHandler
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import GPParseException

from frontends.gamespy.protocols.presence_connection_manager.abstractions.handlers import CmdHandlerBase
from typing import TYPE_CHECKING, Optional, cast

from frontends.gamespy.protocols.presence_connection_manager.applications.client import Client


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: Client, raw_request: str) -> None:
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        if self._raw_request[0] != "\\":
            raise GPParseException("Request format is invalid")
        raw_requests = [
            r+"\\final\\" for r in self._raw_request.split("\\final\\") if r]
        for raw_request in raw_requests:
            name = raw_request.strip("\\").split("\\")[0]
            if name not in RequestType:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                continue
            self._requests.append((RequestType(name), raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, RequestType)
        assert isinstance(raw_request, str)
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case RequestType.KA:
                return KeepAliveHandler(self._client, KeepAliveRequest(raw_request))
            case RequestType.LOGIN:
                return LoginHandler(self._client, LoginRequest(raw_request))
            case RequestType.LOGOUT:
                return LogoutHandler(self._client, LogoutRequest(raw_request))
            case RequestType.NEWUSER:
                return NewUserHandler(self._client, NewUserRequest(raw_request))
            case RequestType.ADDBLOCK:
                return AddBlockHandler(self._client, AddBlockRequest(raw_request))
            case RequestType.GETPROFILE:
                return GetProfileHandler(self._client, GetProfileRequest(raw_request))
            case RequestType.NEWPROFILE:
                return NewProfileHandler(self._client, NewProfileRequest(raw_request))
            case RequestType.REGISTERCDKEY:
                return RegisterCDKeyHandler(self._client, RegisterCDKeyRequest(raw_request))
            case RequestType.REGISTERNICK:
                return RegisterNickHandler(self._client, RegisterNickRequest(raw_request))
            case RequestType.UPDATEPRO:
                return UpdateProfileHandler(self._client, UpdateProfileRequest(raw_request))
            case RequestType.STATUS:
                return StatusHandler(self._client, StatusRequest(raw_request))
            case RequestType.STATUSINFO:
                return StatusInfoHandler(self._client, StatusInfoRequest(raw_request))
            case RequestType.INVITETO:
                raise NotImplementedError(
                    "InviteToHandler is not implemented.")
            case _:
                return None
