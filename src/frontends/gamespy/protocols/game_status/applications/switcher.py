from typing import cast
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.game_status.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.game_status.aggregations.enums import RequestType
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.contracts.requests import AuthGameRequest, AuthPlayerRequest, GetPlayerDataRequest, GetProfileIdRequest, NewGameRequest, SetPlayerDataRequest, UpdateGameRequest
from frontends.gamespy.protocols.game_status.applications.handlers import AuthGameHandler, AuthPlayerHandler, GetPlayerDataHandler, GetProfileIdHandler, NewGameHandler, SetPlayerDataHandler, UpdateGameHandler


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
            if name not in RequestType:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                return
            self._requests.append((RequestType(name), raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, RequestType)
        self._client = cast(Client, self._client)
        match name:
            case RequestType.AUTH:
                return AuthGameHandler(self._client, AuthGameRequest(raw_request))
            case RequestType.AUTHP:
                return AuthPlayerHandler(self._client, AuthPlayerRequest(raw_request))
            case RequestType.NEWGAME:
                return NewGameHandler(self._client, NewGameRequest(raw_request))
            case RequestType.GETPD:
                return GetPlayerDataHandler(self._client, GetPlayerDataRequest(raw_request))
            case RequestType.SETPD:
                return SetPlayerDataHandler(self._client, SetPlayerDataRequest(raw_request))
            case RequestType.UPDGAME:
                return UpdateGameHandler(self._client, UpdateGameRequest(raw_request))
            case RequestType.GETPID:
                return GetProfileIdHandler(self._client, GetProfileIdRequest(raw_request))
            case _:
                return None
