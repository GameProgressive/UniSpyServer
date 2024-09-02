from library.src.abstractions.switcher import SwitcherBase
from servers.game_status.src.abstractions.handlers import CmdHandlerBase
from servers.game_status.src.applications.client import Client
from servers.game_status.src.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from servers.game_status.src.handlers.handlers import AuthGameHandler, AuthPlayerHandler, GetPlayerDataHandler, NewGameHandler, SetPlayerDataHandler, UpdateGameHandler


class Switcher(SwitcherBase):
    _raw_request: str

    def __init__(self, client: Client, raw_request: str) -> None:
        super().__init__(client, raw_request)
        assert isinstance(client, Client)
        assert isinstance(raw_request, str)

    def _process_raw_request(self) -> None:
        if self._raw_request[0] != "\\":
            self._client.log_info("Invalid request received!")
            return
        raw_requests = [
            r+"\\final\\" for r in self._raw_request.split("\\final\\") if r]
        for raw_request in raw_requests:
            name = raw_request.strip("\\").split("\\", 1)[0]
            self._requests.append((name, raw_request))

    def _create_cmd_handlers(self, name: object, raw_request: str) -> CmdHandlerBase | None:

        match name:
            case "auth":
                return AuthGameHandler(self._client, AuthGameRequest(raw_request))
            case "authp":
                return AuthPlayerHandler(self._client, AuthPlayerRequest(raw_request))
            case "newgame":
                return NewGameHandler(self._client, NewGameRequest(raw_request))
            case "getpd":
                return GetPlayerDataHandler(self._client, GetPlayerDataRequest(raw_request))
            case "setpd":
                return SetPlayerDataHandler(self._client, SetPlayerDataRequest(raw_request))
            case "updgame":
                return UpdateGameHandler(self._client, UpdateGameRequest(raw_request))
            case _:
                return None
