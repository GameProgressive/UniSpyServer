from library.src.abstractions.switcher import SwitcherBase
from library.src.exceptions.general import UniSpyException
from servers.presence_connection_manager.src.contracts.requests.general import (
    KeepAliveRequest,
)
from servers.query_report.src.applications.client import Client
from servers.query_report.src.v2.abstractions.cmd_handler_base import CmdHandlerBase

from servers.query_report.src.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageRequest,
    EchoRequest,
    HeartBeatRequest,
)
from servers.query_report.src.v2.enums.general import RequestType
from servers.query_report.src.v2.handlers.handlers import (
    AvailableHandler,
    ChallengeHanler,
    ClientMessageAckHandler,
    EchoHandler,
    HeartBeatHandler,
    KeepAliveHandler,
)


class CmdSwitcher(SwitcherBase):
    def __init__(self, client: Client, raw_request: bytes):
        super().__init__(client, raw_request)

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise UniSpyException("Invalid request")

        name = RequestType(self._raw_request[0])
        raw_request = self._raw_request
        self._requests.append((name, raw_request))

    def _create_cmd_handlers(self, name: int, raw_request: bytes) -> CmdHandlerBase:
        req = raw_request
        match name:
            case RequestType.HEARTBEAT:
                return HeartBeatHandler(self._client, HeartBeatRequest(req))
            case RequestType.CHALLENGE:
                return ChallengeHanler(self._client, ChallengeRequest(req))
            case RequestType.AVALIABLE_CHECK:
                return AvailableHandler(self._client, AvaliableRequest(req))
            case RequestType.CLIENT_MESSAGE_ACK:
                return ClientMessageAckHandler(self._client, ClientMessageRequest(req))
            case RequestType.ECHO:
                return EchoHandler(self._client, EchoRequest(req))
            case RequestType.KEEP_ALIVE:
                return KeepAliveHandler(self._client, KeepAliveRequest(req))
            case _:
                return None
