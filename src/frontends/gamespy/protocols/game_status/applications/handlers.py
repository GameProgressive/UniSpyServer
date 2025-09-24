from frontends.gamespy.protocols.game_status.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, GetProfileIdRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from frontends.gamespy.protocols.game_status.contracts.responses import AuthGameResponse, AuthPlayerResponse, GetPlayerDataResponse, GetProfileIdResponse
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult


class AuthGameHandler(CmdHandlerBase):
    _request: AuthGameRequest
    _result: AuthGameResult

    def __init__(self, client: Client, request: AuthGameRequest) -> None:
        assert isinstance(request, AuthGameRequest)
        super().__init__(client, request)
        self._result_cls = AuthGameResult
        self._response_cls = AuthGameResponse

    def _response_construct(self) -> None:
        self._client.info.session_key = self._result.session_key
        self._client.info.game_name = self._result.game_name
        self._client.info.is_game_authenticated = True


class AuthPlayerHandler(CmdHandlerBase):
    _result_cls: type[AuthPlayerResult]

    def __init__(self, client: Client, request: AuthPlayerRequest) -> None:
        assert isinstance(request, AuthPlayerRequest)
        super().__init__(client, request)
        self._result_cls = AuthPlayerResult

    def _response_construct(self) -> None:
        self._response = AuthPlayerResponse(self._result)


class GetPlayerDataHandler(CmdHandlerBase):
    _result_cls: type[GetPlayerDataResult]

    def __init__(self, client: Client, request: GetPlayerDataRequest) -> None:
        assert isinstance(request, GetPlayerDataRequest)
        super().__init__(client, request)
        self._result_cls = GetPlayerDataResult

    def _response_construct(self) -> None:
        self._response = GetPlayerDataResponse(self._result)


class GetProfileIdHandler(CmdHandlerBase):
    _result_cls: type[GetProfileIdResult]

    def __init__(self, client: Client, request: GetProfileIdRequest) -> None:
        assert isinstance(request, GetProfileIdRequest)
        super().__init__(client, request)
        self._result_cls = GetProfileIdResult

    def _response_construct(self) -> None:
        self._response = GetProfileIdResponse(self._result)


class NewGameHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: NewGameRequest) -> None:
        assert isinstance(request, NewGameRequest)
        super().__init__(client, request)
        self._is_fetching = False


class SetPlayerDataHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: SetPlayerDataRequest) -> None:
        assert isinstance(request, SetPlayerDataRequest)
        super().__init__(client, request)
        self._is_fetching = False




class UpdateGameHandler(CmdHandlerBase):
    """
        old request "\\updgame\\\\sesskey\\%d\\done\\%d\\gamedata\\%s"

        new request "\\updgame\\\\sesskey\\%d\\connid\\%d\\done\\%d\\gamedata\\%s"
    """
    _request: UpdateGameRequest

    def __init__(self, client: Client, request: UpdateGameRequest) -> None:
        assert isinstance(request, UpdateGameRequest)
        super().__init__(client, request)
        self._is_fetching = False

