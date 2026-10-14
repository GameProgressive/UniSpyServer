from typing import TYPE_CHECKING, Optional, cast

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.server_browser.aggregates.exceptions import (
    ServerBrowserException,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.handlers import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    CommandName,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import (
    SBException,
)
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client
from frontends.gamespy.protocols.server_browser.v2.applications.handlers import (
    P2PGroupRoomListHandler,
    SendMessageHandler,
    ServerFullInfoListHandler,
    ServerMainListHandler,
    UpdateServerInfoHandler,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.requests import (
    SendMessageRequest,
    ServerInfoRequest,
    ServerListRequest,
)


class Switcher(SwitcherBase):
    _raw_request: bytes
    _client: Client

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise SBException("Invalid request")
        name = self._raw_request[2]
        if name not in CommandName:
            self._client.log_debug(f"Request: {name} is not a valid request.")
            return

        self._requests.append((CommandName(name), self._raw_request))

    def _create_cmd_handlers(
        self, name: int, raw_request: bytes
    ) -> CmdHandlerBase | None:
        req = raw_request
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case CommandName.SERVER_LIST_REQUEST:
                handler = self.__check_update_option(req)
                return handler
            case CommandName.SERVER_INFO_REQUEST:
                return UpdateServerInfoHandler(
                    self._client, ServerInfoRequest.gs_parse(req)
                )
            case CommandName.SEND_MESSAGE_REQUEST:
                return SendMessageHandler(
                    self._client, SendMessageRequest.gs_parse(req)
                )
            case _:
                return None

    def __check_update_option(self, request: bytes) -> CmdHandlerBase:
        """
        check update option and create handler
        """
        req = ServerListRequest.gs_parse(request)
        match req.update_option:
            case (
                ServerListUpdateOption.SERVER_MAIN_LIST
                | ServerListUpdateOption.P2P_SERVER_MAIN_LIST
                | ServerListUpdateOption.LIMIT_RESULT_COUNT
            ):
                return ServerMainListHandler(
                    self._client, ServerListRequest.gs_parse(request)
                )
            case ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
                return P2PGroupRoomListHandler(
                    self._client, ServerListRequest.gs_parse(request)
                )
            case ServerListUpdateOption.SERVER_FULL_INFO_LIST:
                return ServerFullInfoListHandler(
                    self._client, ServerListRequest.gs_parse(request)
                )
            case _:
                raise ServerBrowserException("unknown serverlist update option type")

    @staticmethod
    def get_update_option(raw_request: bytes) -> ServerListUpdateOption:
        # todo check if all game follow this pattern, +2 is calc by sdk server info request
        req = ServerListRequest.gs_parse(raw_request)
        return req.update_option
