from typing import Optional
from library.src.abstractions.switcher import SwitcherBase
from servers.natneg.src.abstractions.handlers import CmdHandlerBase
from servers.natneg.src.applications.client import Client
from servers.natneg.src.contracts.requests import (
    AddressCheckRequest,
    ConnectRequest,
    ErtAckRequest,
    InitRequest,
    PingRequest,
    ReportRequest,
)
from servers.natneg.src.aggregations.enums import RequestType
from servers.natneg.src.applications.handlers import (
    AddressCheckHandler,
    ConnectHandler,
    ErtAckHandler,
    InitHandler,
    PingHandler,
    ReportHandler,
)


class CmdSwitcher(SwitcherBase):
    _raw_request: bytes
    _client: Client

    def __init__(self, client: Client, raw_request: bytes) -> None:
        super().__init__(client, raw_request)
        assert issubclass(type(client), Client)
        assert isinstance(raw_request, bytes)

    def _process_raw_request(self) -> None:

        name = self._raw_request[7]
        if name not in RequestType:
            self._client.log_debug(
                f"Request: {name} is not a valid request.")
            return
        self._requests.append((RequestType(name), self._raw_request))

    def _create_cmd_handlers(
        self, name: RequestType, raw_request: bytes
    ) -> Optional[CmdHandlerBase | None]:
        assert isinstance(name, RequestType)
        assert isinstance(raw_request, bytes)
        match name:
            case RequestType.INIT:
                return InitHandler(self._client, InitRequest(raw_request))
            # case RequestType.
            case RequestType.ERT_ACK:
                return ErtAckHandler(self._client, ErtAckRequest(raw_request))
            case RequestType.CONNECT:
                return ConnectHandler(self._client, ConnectRequest(raw_request))
            case RequestType.PING:
                return PingHandler(self._client, PingRequest(raw_request))
            case RequestType.ADDRESS_CHECK:
                return AddressCheckHandler(
                    self._client, AddressCheckRequest(raw_request)
                )
            case RequestType.REPORT:
                return ReportHandler(self._client, ReportRequest(raw_request))
            case RequestType.PRE_INIT:
                return None
            case _:
                return None
