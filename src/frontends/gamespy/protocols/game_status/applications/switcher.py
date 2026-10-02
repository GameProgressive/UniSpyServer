from typing import cast

from frontends.gamespy.protocols.game_status.abstractions.contracts import RequestBase
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.extentions.string_extentions import get_kv_str_name
from frontends.gamespy.protocols.game_status.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.game_status.aggregations.enums import CommandName
from frontends.gamespy.protocols.game_status.applications.client import Client
from frontends.gamespy.protocols.game_status.applications.handlers import (
    AuthGameHandler,
    AuthPlayerHandler,
    GetPlayerDataHandler,
    GetProfileIdHandler,
    NewGameHandler,
    SetPlayerDataHandler,
    UpdateGameHandler,
)
from frontends.gamespy.protocols.game_status.contracts.requests import (
    AuthGameRequest,
    AuthPlayerRequest,
    GetPlayerDataRequest,
    GetProfileIdRequest,
    NewGameRequest,
    SetPlayerDataRequest,
    UpdateGameRequest,
)


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
            name = RequestBase.gs_parse(raw_request).command_name
            if name not in CommandName:
                self._client.log_debug(
                    f"Request: {name} is not a valid request.")
                return
            self._requests.append((CommandName(name), raw_request))

    def _create_cmd_handlers(self, name: CommandName, raw_request: str) -> CmdHandlerBase | None:
        assert isinstance(name, CommandName)
        self._client = cast(Client, self._client)
        match name:
            case CommandName.AUTH:
                return AuthGameHandler(self._client, AuthGameRequest.gs_parse(raw_request))
            case CommandName.AUTHP:
                return AuthPlayerHandler(self._client, AuthPlayerRequest.gs_parse(raw_request))
            case CommandName.NEWGAME:
                return NewGameHandler(self._client, NewGameRequest.gs_parse(raw_request))
            case CommandName.GETPD:
                return GetPlayerDataHandler(self._client, GetPlayerDataRequest.gs_parse(raw_request))
            case CommandName.SETPD:
                return SetPlayerDataHandler(self._client, SetPlayerDataRequest.gs_parse(raw_request))
            case CommandName.UPDGAME:
                return UpdateGameHandler(self._client, UpdateGameRequest.gs_parse(raw_request))
            case CommandName.GETPID:
                return GetProfileIdHandler(self._client, GetProfileIdRequest.gs_parse(raw_request))
            case _:
                return None
