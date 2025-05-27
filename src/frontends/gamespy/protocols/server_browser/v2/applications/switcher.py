from typing import TYPE_CHECKING, Optional, cast
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.server_browser.v2.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import RequestType
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client
from frontends.gamespy.protocols.server_browser.v2.applications.handlers import SendMessageHandler, ServerInfoHandler, ServerListHandler
from frontends.gamespy.protocols.server_browser.v2.contracts.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest


class Switcher(SwitcherBase):
    _raw_request: bytes

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise UniSpyException("Invalid request")
        name = self._raw_request[2]
        if name not in RequestType:
            self._client.log_debug(
                f"Request: {name} is not a valid request.")
            return

        self._requests.append((RequestType(name), self._raw_request))

    def _create_cmd_handlers(self, name: int, raw_request: bytes) -> Optional[CmdHandlerBase]:
        req = raw_request
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case RequestType.SERVER_LIST_REQUEST:
                return ServerListHandler(self._client, ServerListRequest(req))
            case RequestType.SERVER_INFO_REQUEST:
                return ServerInfoHandler(self._client, ServerInfoRequest(req))
            case RequestType.SEND_MESSAGE_REQUEST:
                return SendMessageHandler(self._client, SendMessageRequest(req))
            case _:
                return None
