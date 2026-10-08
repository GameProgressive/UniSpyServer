from typing import TYPE_CHECKING, cast

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.extentions.string_extentions import (
    get_kv_str_name,
    split_nested_kv_str,
)
from frontends.gamespy.protocols.presence_connection_manager.abstractions.handlers import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    CommandName,
)
from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    Client,
)
from frontends.gamespy.protocols.presence_connection_manager.applications.handlers import (
    AddBlockHandler,
    AddBuddyHandler,
    AuthAddBuddyHandler,
    GetProfileHandler,
    KeepAliveHandler,
    LoginHandler,
    LogoutHandler,
    NewProfileHandler,
    NewUserHandler,
    RegisterCDKeyHandler,
    RegisterNickHandler,
    RemoveBlockHandler,
    StatusHandler,
    StatusInfoHandler,
    UpdateProfileHandler,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    AddBlockRequest,
    AddBuddyRequest,
    AuthAddBuddyRequest,
    GetProfileRequest,
    KeepAliveRequest,
    LoginRequest,
    LogoutRequest,
    NewProfileRequest,
    NewUserRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    RemoveBlockRequest,
    StatusInfoRequest,
    StatusRequest,
    UpdateProfileRequest,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)
import frontends.gamespy.protocols.presence_search_player.applications.switcher as psp
class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: Client, raw_request: str) -> None:
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        if self._raw_request[0] != "\\":
            raise GPParseException("Request format is invalid")
        raw_requests = split_nested_kv_str(self._raw_request)
        for raw_request in raw_requests:
            name = RequestBase.gs_parse(raw_request).command_name
            self._requests.append((name, raw_request))

    def _create_cmd_handlers(
        self, name: CommandName, raw_request: str
    ) -> CmdHandlerBase | None:
        assert isinstance(name, CommandName)
        assert isinstance(raw_request, str)
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case CommandName.KA:
                return KeepAliveHandler(
                    self._client, KeepAliveRequest.gs_parse(raw_request)
                )
            case CommandName.LOGIN:
                return LoginHandler(self._client, LoginRequest.gs_parse(raw_request))
            case CommandName.LOGOUT:
                return LogoutHandler(self._client, LogoutRequest.gs_parse(raw_request))
            case CommandName.NEWUSER:
                return NewUserHandler(
                    self._client, NewUserRequest.gs_parse(raw_request)
                )
            case CommandName.ADDBLOCK:
                return AddBlockHandler(
                    self._client, AddBlockRequest.gs_parse(raw_request)
                )
            case CommandName.GETPROFILE:
                return GetProfileHandler(
                    self._client, GetProfileRequest.gs_parse(raw_request)
                )
            case CommandName.NEWPROFILE:
                return NewProfileHandler(
                    self._client, NewProfileRequest.gs_parse(raw_request)
                )
            case CommandName.REGISTERCDKEY:
                return RegisterCDKeyHandler(
                    self._client, RegisterCDKeyRequest.gs_parse(raw_request)
                )
            case CommandName.REGISTERNICK:
                return RegisterNickHandler(
                    self._client, RegisterNickRequest.gs_parse(raw_request)
                )
            case CommandName.UPDATEPRO:
                return UpdateProfileHandler(
                    self._client, UpdateProfileRequest.gs_parse(raw_request)
                )
            case CommandName.STATUS:
                return StatusHandler(self._client, StatusRequest.gs_parse(raw_request))
            case CommandName.STATUSINFO:
                return StatusInfoHandler(
                    self._client, StatusInfoRequest.gs_parse(raw_request)
                )
            case CommandName.INVITETO:
                raise NotImplementedError("InviteToHandler is not implemented.")
            case CommandName.PEER_AUTH:
                raise NotImplementedError("peer auth is not implemented")
            case CommandName.ADDBUDDY:
                return AddBuddyHandler(
                    self._client, AddBuddyRequest.gs_parse(raw_request)
                )
            case CommandName.ADDBLOCK:
                return AddBlockHandler(
                    self._client, AddBlockRequest.gs_parse(raw_request)
                )
            case CommandName.AUTHADD:
                return AuthAddBuddyHandler(
                    self._client, AuthAddBuddyRequest.gs_parse(raw_request)
                )
            case CommandName.REMOVEBLOCK:
                return RemoveBlockHandler(
                    self._client, RemoveBlockRequest.gs_parse(raw_request)
                )
            case _:
                return None
