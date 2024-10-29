from typing import TYPE_CHECKING, Optional, cast
from library.src.abstractions.switcher import SwitcherBase
from library.src.exceptions.general import UniSpyException
from servers.server_browser.src.v2.abstractions.handlers import CmdHandlerBase
from servers.server_browser.src.v2.aggregations.enums import RequestType
from servers.server_browser.src.v2.applications.client import Client
from servers.server_browser.src.v2.applications.handlers import SendMessageHandler, ServerInfoHandler, ServerListHandler
from servers.server_browser.src.v2.contracts.requests import SendMessageRequest, ServerInfoRequest, ServerListRequest


class CmdSwitcher(SwitcherBase):
    _raw_request: bytes

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise UniSpyException("Invalid request")
        name = RequestType(self._raw_request[2])
        self._requests.append((name, self._raw_request))

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
