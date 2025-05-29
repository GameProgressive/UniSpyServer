from typing import TYPE_CHECKING, Optional, cast
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.server_browser.aggregates.exceptions import (
    ServerBrowserException,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.handlers import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    RequestType,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client
from frontends.gamespy.protocols.server_browser.v2.applications.handlers import (
    P2PGroupRoomListHandler,
    SendMessageHandler,
    ServerNetworkInfoListHandler,
    ServerInfoHandler,
    ServerMainListHandler,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.requests import (
    SendMessageRequest,
    ServerInfoRequest,
    ServerListRequest,
)


class Switcher(SwitcherBase):
    _raw_request: bytes

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise UniSpyException("Invalid request")
        name = self._raw_request[2]
        if name not in RequestType:
            self._client.log_debug(f"Request: {name} is not a valid request.")
            return

        self._requests.append((RequestType(name), self._raw_request))

    def _create_cmd_handlers(
        self, name: int, raw_request: bytes
    ) -> Optional[CmdHandlerBase]:
        req = raw_request
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case RequestType.SERVER_LIST_REQUEST:
                update_option_index = raw_request.find(b"\x00\x00\x00\x00") + 1
                update_option_bytes = raw_request[
                    update_option_index : update_option_index + 4
                ]
                update_option = ServerListUpdateOption(
                    int.from_bytes(update_option_bytes)
                )
                if update_option in [
                    ServerListUpdateOption.SERVER_MAIN_LIST,
                    ServerListUpdateOption.P2P_SERVER_MAIN_LIST,
                    ServerListUpdateOption.LIMIT_RESULT_COUNT,
                ]:
                    return ServerMainListHandler(self._client, ServerListRequest(req))
                elif update_option == ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
                    return P2PGroupRoomListHandler(self._client, ServerListRequest(req))
                elif update_option == ServerListUpdateOption.SERVER_FULL_MAIN_LIST:
                    return ServerNetworkInfoListHandler(
                        self._client, ServerListRequest(req)
                    )
                else:
                    raise ServerBrowserException(
                        "unknown serverlist update option type"
                    )
            case RequestType.SERVER_INFO_REQUEST:
                return ServerInfoHandler(self._client, ServerInfoRequest(req))
            case RequestType.SEND_MESSAGE_REQUEST:
                return SendMessageHandler(self._client, SendMessageRequest(req))
            case _:
                return None
