
from typing import TYPE_CHECKING
import servers.presence_connection_manager.src.abstractions.handlers
from servers.presence_connection_manager.src.aggregates.sdk_revision import SdkRevision
from servers.presence_connection_manager.src.contracts.requests.general import (
    KeepAliveRequest,
    LoginRequest,
    LogoutRequest,
)
from servers.presence_connection_manager.src.contracts.responses.general import (
    KeepAliveResponse,
    LoginResponse,
)
from servers.presence_connection_manager.src.contracts.results.general import LoginResult
from servers.presence_connection_manager.src.handlers.buddy import (
    BlockListHandler,
    BuddyListHandler,
)
from servers.presence_search_player.src.contracts.responses import NewUserResponse
import servers.presence_search_player.src.handlers.handlers

if TYPE_CHECKING:
    from servers.presence_connection_manager.src.applications.client import Client


class KeepAliveHandler(servers.presence_connection_manager.src.abstractions.handlers.CmdHandlerBase):
    def __init__(self, client: "Client", request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = KeepAliveResponse(self._request)


class LoginHandler(servers.presence_connection_manager.src.abstractions.handlers.CmdHandlerBase):

    _request: LoginRequest
    _result: LoginResult

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = LoginResponse(self._request, self._result)


class LogoutHandler(servers.presence_connection_manager.src.abstractions.handlers.LoginedHandlerBase):
    _request: LogoutRequest

    def __init__(self, client: Client, request: LogoutRequest) -> None:
        assert isinstance(request, LogoutRequest)
        super().__init__(client, request)


class NewUserHandler(servers.presence_search_player.src.handlers.handlers.NewUserHandler):

    def _response_construct(self):
        self._response = NewUserResponse(self._request, self._response)


class SdkRevisionHandler(servers.presence_connection_manager.src.abstractions.handlers.CmdHandlerBase):
    _request: LoginRequest

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._client.info.sdk_revision = SdkRevision(
            self._request.sdk_revision_type)
        if self._client.info.sdk_revision.is_support_gpi_new_status_notification:
            BuddyListHandler(self._client).handle()
            BlockListHandler(self._client).handle()
