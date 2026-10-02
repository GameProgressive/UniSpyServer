from typing import TYPE_CHECKING, cast

from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v2.abstractions.handlers import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import CommndName
from frontends.gamespy.protocols.query_report.v2.applications.handlers import (
    AvailableHandler,
    ChallengeHanler,
    ClientMessageAckHandler,
    EchoHandler,
    HeartbeatHandler,
    KeepAliveHandler,
)
from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageAckRequest,
    EchoRequest,
    HeartbeatRequest,
    KeepAliveRequest,
)


class Switcher(SwitcherBase):
    _raw_request: bytes

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 4:
            raise QRException("Invalid request")
        name = self._raw_request[0]
        if name not in CommndName:
            self._client.log_debug(
                f"Request: {name} is not a valid request.")
            return
        raw_request = self._raw_request
        self._requests.append((CommndName(name), raw_request))

    def _create_cmd_handlers(self, name: CommndName, raw_request: bytes) -> CmdHandlerBase | None:
        assert isinstance(name, CommndName)
        if TYPE_CHECKING:
            self._client = cast(Client, self._client)
        match name:
            case CommndName.HEARTBEAT:
                return HeartbeatHandler(self._client, HeartbeatRequest.gs_parse(raw_request))
            case CommndName.CHALLENGE:
                return ChallengeHanler(self._client, ChallengeRequest.gs_parse(raw_request))
            case CommndName.AVALIABLE_CHECK:
                return AvailableHandler(self._client, AvaliableRequest.gs_parse(raw_request))
            case CommndName.CLIENT_MESSAGE_ACK:
                return ClientMessageAckHandler(self._client, ClientMessageAckRequest.gs_parse(raw_request))
            case CommndName.ECHO:
                return EchoHandler(self._client, EchoRequest.gs_parse(raw_request))
            case CommndName.KEEP_ALIVE:
                return KeepAliveHandler(self._client, KeepAliveRequest.gs_parse(raw_request))
            case _:
                return None
