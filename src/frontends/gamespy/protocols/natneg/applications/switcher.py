from frontends.gamespy.library.abstractions.switcher import SwitcherBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.natneg.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.natneg.aggregations.enums import CommandName
from frontends.gamespy.protocols.natneg.applications.client import Client
from frontends.gamespy.protocols.natneg.applications.handlers import (
    AddressCheckHandler,
    ConnectAckHandler,
    ConnectHandler,
    ErtAckHandler,
    InitHandler,
    NatifyHandler,
    ReportHandler,
)
from frontends.gamespy.protocols.natneg.contracts.requests import (
    AddressCheckRequest,
    ConnectAckRequest,
    ConnectRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
    ReportRequest,
)


class Switcher(SwitcherBase):
    _raw_request: bytes
    _client: Client

    def __init__(self, client: Client, raw_request: bytes) -> None:
        super().__init__(client, raw_request)
        assert issubclass(type(client), Client)
        assert isinstance(raw_request, bytes)

    def _process_raw_request(self) -> None:
        if len(self._raw_request) < 8:
            self._client.log_debug("incoming buffer length is not valid")
            return
        name = self._raw_request[7]
        if name not in CommandName:
            self._client.log_debug(f"Request: {name} is not a valid request.")
            return
        self._requests.append((CommandName(name), self._raw_request))

    def _create_cmd_handlers(
        self, name: CommandName, raw_request: bytes
    ) -> CmdHandlerBase | None:
        assert isinstance(name, CommandName)
        assert isinstance(raw_request, bytes)
        match name:
            case CommandName.NATIFY_REQUEST:
                return NatifyHandler(self._client, NatifyRequest.gs_parse(raw_request))
            case CommandName.INIT:
                return InitHandler(self._client, InitRequest.gs_parse(raw_request))
            # case RequestType.
            case CommandName.ERT_ACK:
                return ErtAckHandler(self._client, ErtAckRequest.gs_parse(raw_request))
            case CommandName.CONNECT:
                return ConnectHandler(self._client, ConnectRequest.gs_parse(raw_request))
            case CommandName.CONNECT_ACK:
                return ConnectAckHandler(self._client, ConnectAckRequest.gs_parse(raw_request))
            case CommandName.ADDRESS_CHECK:
                return AddressCheckHandler(
                    self._client, AddressCheckRequest.gs_parse(raw_request)
                )
            case CommandName.REPORT:
                return ReportHandler(self._client, ReportRequest.gs_parse(raw_request))
            case CommandName.PRE_INIT:
                return None
            case _:
                return None
