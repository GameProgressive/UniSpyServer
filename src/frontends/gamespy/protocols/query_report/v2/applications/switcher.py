from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v2.abstractions.handlers import CmdHandlerBase

from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageAckRequest,
    EchoRequest,
    HeartbeatRequest,
    KeepAliveRequest,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import RequestType
from frontends.gamespy.protocols.query_report.v2.applications.handlers import (
    AvailableHandler,
    ChallengeHanler,
    ClientMessageAckHandler,
    EchoHandler,
    HeartbeatHandler,
    KeepAliveHandler,
)


class Switcher(SwitcherBase):
    _raw_request: bytes

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise QRException("Invalid request")
        name = self._raw_request[0]
        if name not in RequestType:
            self._client.log_debug(
                f"Request: {name} is not a valid request.")
            return
        raw_request = self._raw_request
        self._requests.append((RequestType(name), raw_request))

    def _create_cmd_handlers(self, name: RequestType, raw_request: bytes) -> CmdHandlerBase | None:
        assert isinstance(name, RequestType)
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case RequestType.HEARTBEAT:
                return HeartbeatHandler(self._client, HeartbeatRequest(raw_request))
            case RequestType.CHALLENGE:
                return ChallengeHanler(self._client, ChallengeRequest(raw_request))
            case RequestType.AVALIABLE_CHECK:
                return AvailableHandler(self._client, AvaliableRequest(raw_request))
            case RequestType.CLIENT_MESSAGE_ACK:
                return ClientMessageAckHandler(self._client, ClientMessageAckRequest(raw_request))
            case RequestType.ECHO:
                return EchoHandler(self._client, EchoRequest(raw_request))
            case RequestType.KEEP_ALIVE:
                return KeepAliveHandler(self._client, KeepAliveRequest(raw_request))
            case _:
                return None
