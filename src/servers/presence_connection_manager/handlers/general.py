from servers.chat.contracts.requests.general import LoginRequest
from servers.chat.contracts.results.general import LoginResult
from servers.presence_connection_manager.abstractions.handler import (
    CmdHandlerBase,
    LoginHandlerBase,
)
from servers.presence_connection_manager.aggregates.sdk_revision import SdkRevision
from servers.presence_connection_manager.applications.client import Client
from servers.presence_connection_manager.contracts.requests.general import (
    KeepAliveRequest,
    LogoutRequest,
)
from servers.presence_connection_manager.contracts.responses.general import (
    KeepAliveResponse,
    LoginResponse,
)
from servers.presence_connection_manager.handlers.buddy import (
    BlockListHandler,
    BuddyListHandler,
)
from servers.presence_search_player.contracts.responses import NewUserResponse


class KeepAliveHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = KeepAliveResponse(self._request)


class LoginHandelr(CmdHandlerBase):

    _request: LoginRequest
    _result: LoginResult

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = LoginResponse(self._request, self._result)


class LogoutHandler(LoginHandlerBase):
    _request: LogoutRequest

    def __init__(self, client: Client, request: LogoutRequest) -> None:
        assert isinstance(request, LogoutRequest)
        super().__init__(client, request)


import servers.presence_search_player.handlers.handlers


class NewUserHandler(servers.presence_search_player.handlers.handlers.NewUserHandler):

    def _response_construct(self):
        self._response = NewUserResponse(self._request, self._response)


class SdkRevisionHandler(CmdHandlerBase):
    _request: LoginRequest

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._client.info.sdk_revision = SdkRevision(self._request.sdk_revision_type)
        if self._client.info.sdk_revision.is_support_gpi_new_status_notification:
            BuddyListHandler(self._client).handle()
            BlockListHandler(self._client).handle()
