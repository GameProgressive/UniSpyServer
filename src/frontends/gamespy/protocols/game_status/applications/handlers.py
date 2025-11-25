from frontends.gamespy.protocols.game_status.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, GetProfileIdRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from frontends.gamespy.protocols.game_status.contracts.responses import AuthGameResponse, AuthPlayerResponse, GetPlayerDataResponse, GetProfileIdResponse, SetPlayerDataResponse
from frontends.gamespy.protocols.game_status.contracts.results import AuthGameResult, AuthPlayerResult, GetPlayerDataResult, GetProfileIdResult, SetPlayerDataResult


class AuthGameHandler(CmdHandlerBase):
    _request: AuthGameRequest
    _result: AuthGameResult
    _response: AuthGameResponse

    def __init__(self, client: Client, request: AuthGameRequest) -> None:
        assert isinstance(request, AuthGameRequest)
        super().__init__(client, request)

    def _data_operate(self) -> None:
        super()._data_operate()
        self._client.info.session_key = self._result.session_key
        self._client.info.game_name = self._result.game_name
        self._client.info.is_game_authenticated = True


class AuthPlayerHandler(CmdHandlerBase):
    _request: AuthPlayerRequest
    _result: AuthPlayerResult
    _response: AuthPlayerResponse

    def __init__(self, client: Client, request: AuthPlayerRequest) -> None:
        assert isinstance(request, AuthPlayerRequest)
        super().__init__(client, request)


class GetPlayerDataHandler(CmdHandlerBase):
    _result: GetPlayerDataResult
    _response: GetPlayerDataResponse

    def __init__(self, client: Client, request: GetPlayerDataRequest) -> None:
        assert isinstance(request, GetPlayerDataRequest)
        super().__init__(client, request)


class GetProfileIdHandler(CmdHandlerBase):
    _result: GetProfileIdResult
    _response: GetProfileIdResponse

    def __init__(self, client: Client, request: GetProfileIdRequest) -> None:
        assert isinstance(request, GetProfileIdRequest)
        super().__init__(client, request)


class NewGameHandler(CmdHandlerBase):
    _request: NewGameRequest

    def __init__(self, client: Client, request: NewGameRequest) -> None:
        assert isinstance(request, NewGameRequest)
        super().__init__(client, request)


class SetPlayerDataHandler(CmdHandlerBase):
    _request: SetPlayerDataRequest

    def __init__(self, client: Client, request: SetPlayerDataRequest) -> None:
        assert isinstance(request, SetPlayerDataRequest)
        super().__init__(client, request)


class UpdateGameHandler(CmdHandlerBase):
    """
        old request "\\updgame\\\\sesskey\\%d\\done\\%d\\gamedata\\%s"

        new request "\\updgame\\\\sesskey\\%d\\connid\\%d\\done\\%d\\gamedata\\%s"
    """
    _request: UpdateGameRequest

    def __init__(self, client: Client, request: UpdateGameRequest) -> None:
        assert isinstance(request, UpdateGameRequest)
        super().__init__(client, request)
