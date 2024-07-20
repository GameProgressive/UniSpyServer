from library.src.abstractions.contracts import RequestBase
from library.src.extentions.string_extentions import IPEndPoint
from servers.natneg.src.abstractions.handlers import CmdHandlerBase
from servers.natneg.src.applications.client import Client
from servers.natneg.src.contracts.requests import AddressCheckRequest, ConnectAckRequest, ConnectRequest, ErtAckRequest, InitRequest, NatifyRequest, ReportRequest
from servers.natneg.src.contracts.responses import InitResponse
from servers.natneg.src.contracts.results import AddressCheckResult, ConnectResult, ErtAckResult, InitResult, NatifyResult, ReportResult


class AddressCheckHandler(CmdHandlerBase):
    _request: AddressCheckRequest
    _result: AddressCheckResult = AddressCheckResult()
    _response: InitResponse

    def __init__(self, client: Client, request: AddressCheckRequest) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert isinstance(request, AddressCheckRequest)

    def _data_operate(self) -> None:
        self._result.ip_endpoint = IPEndPoint(
            self._client.connection.ip, self._client.connection.port
        )

    def _response_construct(self) -> None:
        self._response = InitResponse(self._request, self._result)


class ConnectAckHandler(CmdHandlerBase):
    _request: ConnectAckRequest

    def _data_operate(self) -> None:
        self._client.log_info(
            f"client:{self._request.client_index} aknowledged connect request."
        )


class ConnectHandler(CmdHandlerBase):
    _request: ConnectRequest
    _result: ConnectResult

    def __init__(self, client: Client, request: ConnectRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, ConnectRequest)


class ErtAckHandler(CmdHandlerBase):
    _request: ErtAckRequest
    _result: ErtAckResult = ErtAckResult()
    _response: InitResponse

    def __init__(self, client: Client, request: ErtAckRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, ErtAckRequest)

    def _data_operate(self) -> None:
        self._result.ip_endpoint = IPEndPoint(
            self._client.connection.ip, self._client.connection.port
        )

    def _response_construct(self) -> None:
        self._response = InitResponse(self._request, self._result)


class InitHandler(CmdHandlerBase):
    """
    In init process, we need response the initresponse first to make client not timeout
    """

    _request: InitRequest
    _result: InitResult
    _response: InitResponse

    def __init__(self, client: Client, request: InitRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, InitRequest)

    def _response_construct(self):
        self._response = InitResponse(self._request, self._result)


class NatifyHandler(CmdHandlerBase):
    _request: NatifyRequest
    _result: NatifyResult = NatifyResult()
    _response: InitResponse

    def __init__(self, client: Client, request: NatifyRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, NatifyRequest)

    def _data_operate(self):
        self._result.ip_endpoint = IPEndPoint(
            self._client.connection.ip, self._client.connection.port
        )

    def _response_construct(self):
        self._response = InitResponse(self._request, self._result)


class PingHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()


class ReportHandler(CmdHandlerBase):
    _request: ReportRequest
    _result: ReportResult

    def __init__(self, client: Client, request: ReportRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, ReportRequest)
