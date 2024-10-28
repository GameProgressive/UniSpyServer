from servers.game_status.src.abstractions.handlers import CmdHandlerBase
from servers.game_status.src.applications.client import Client
from servers.game_status.src.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, GetProfileIdRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from servers.game_status.src.contracts.responses import AuthGameResponse, AuthPlayerResponse, GetPlayerDataResponse, GetProfileIdResponse
from servers.game_status.src.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


class AuthGameHandler(CmdHandlerBase):
    _request: AuthGameRequest
    _result: AuthGameResult

    def __init__(self, client: Client, request: AuthGameRequest) -> None:
        self._result_cls = AuthGameResult
        super().__init__(client, request)
        assert isinstance(request, AuthGameRequest)

    def _response_construct(self) -> None:
        self._client.info.session_key = self._result.session_key
        self._client.info.game_name = self._request.game_name
        self._client.info.is_game_authenticated = True
        self._response = AuthGameResponse(self._request, self._result)


class AuthPlayerHandler(CmdHandlerBase):
    _result_cls: type[AuthPlayerResult]

    def __init__(self, client: Client, request: AuthPlayerRequest) -> None:
        assert isinstance(request, AuthPlayerRequest)
        self._result_cls = AuthPlayerResult
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = AuthPlayerResponse(self._request, self._result)


class GetPlayerDataHandler(CmdHandlerBase):
    _result_cls: type[GetPlayerDataResult]

    def __init__(self, client: Client, request: GetPlayerDataRequest) -> None:
        self._result_cls = GetPlayerDataResult
        super().__init__(client, request)
        assert isinstance(request, GetPlayerDataRequest)

    def _response_construct(self) -> None:
        self._response = GetPlayerDataResponse(self._request, self._result)


class GetProfileIdHandler(CmdHandlerBase):
    _result_cls: type[GetProfileIdResult]

    def __init__(self, client: Client, request: GetProfileIdRequest) -> None:
        assert isinstance(request, GetProfileIdRequest)
        super().__init__(client, request)
        self._result_cls = GetProfileIdResult

    def _response_construct(self) -> None:
        self._response = GetProfileIdResponse(self._request, self._result)


class NewGameHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: NewGameRequest) -> None:
        assert isinstance(request, NewGameRequest)
        super().__init__(client, request)
        self._is_feaching = False

    def _response_construct(self) -> None:
        self._response = None


class SetPlayerDataHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: SetPlayerDataRequest) -> None:
        assert isinstance(request, SetPlayerDataRequest)
        super().__init__(client, request)
        self._is_feaching = False

    def _response_construct(self) -> None:
        self._response = None


class UpdateGameHandler(CmdHandlerBase):
    """
        old request "\\updgame\\\\sesskey\\%d\\done\\%d\\gamedata\\%s"

        new request "\\updgame\\\\sesskey\\%d\\connid\\%d\\done\\%d\\gamedata\\%s"
    """
    _request: UpdateGameRequest

    def __init__(self, client: Client, request: UpdateGameRequest) -> None:
        assert isinstance(request, UpdateGameRequest)
        super().__init__(client, request)
        self._is_feaching = False

    def _response_construct(self) -> None:
        self._response = None
