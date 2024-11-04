from servers.natneg.src.abstractions.handlers import CmdHandlerBase
from servers.natneg.src.applications.client import Client
from servers.natneg.src.contracts.requests import (
    AddressCheckRequest,
    ConnectAckRequest,
    ConnectRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
    PingRequest,
    ReportRequest,
)
from servers.natneg.src.contracts.responses import (
    AddressCheckResponse,
    ErcAckResponse,
    InitResponse,
    NatifyResponse,
)
from servers.natneg.src.contracts.results import (
    AddressCheckResult,
    ConnectResult,
    ErtAckResult,
    InitResult,
    NatifyResult,
    ReportResult,
)


class AddressCheckHandler(CmdHandlerBase):
    _request: AddressCheckRequest
    _result: AddressCheckResult
    _response: AddressCheckResponse

    def __init__(self, client: Client, request: AddressCheckRequest) -> None:
        assert isinstance(client, Client)
        assert isinstance(request, AddressCheckRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        """
        address check did not require restapi backend, \n
        just send the remote ip back to the client
        """
        self._result = AddressCheckResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port)
        self._result.public_ip_addr = self._client.connection.remote_ip
        self._result.public_port = self._client.connection.remote_port

    def _response_construct(self) -> None:
        self._response = AddressCheckResponse(self._request, self._result)


class ConnectAckHandler(CmdHandlerBase):
    _request: ConnectAckRequest

    def __init__(self, client: Client, request: ConnectAckRequest) -> None:
        assert isinstance(request, ConnectAckRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        self._client.log_info(
            f"client:{self._request.client_index} aknowledged connect request.")


class ConnectHandler(CmdHandlerBase):
    _result_cls: type[ConnectResult]

    def __init__(self, client: Client, request: ConnectRequest) -> None:
        assert isinstance(request, ConnectRequest)
        super().__init__(client, request)
        self._is_fetching = False


class ErtAckHandler(CmdHandlerBase):
    _request: ErtAckRequest
    _result: ErtAckResult
    _response: ErcAckResponse

    def __init__(self, client: Client, request: ErtAckRequest) -> None:
        assert isinstance(request, ErtAckRequest)
        super().__init__(client, request)

    def _feach_data(self) -> None:
        self._result = ErtAckResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port)

    def _response_construct(self) -> None:
        self._response = ErcAckResponse(self._request, self._result)


class InitHandler(CmdHandlerBase):
    """
    In init process, we need response the initresponse first to make client not timeout
    """
    _request: InitRequest
    _result: InitResult
    _response: InitResponse

    def __init__(self, client: Client, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(client, request)

    def _feach_data(self) -> None:
        self._result = InitResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port)

    def _response_construct(self):
        self._response = InitResponse(self._request, self._result)

    def _response_send(self) -> None:
        """we need first to send the response to client in case of the time expire"""
        super()._response_send()
        super()._data_operate()


class NatifyHandler(CmdHandlerBase):
    _request: NatifyRequest
    _result: NatifyResult
    _response: NatifyResponse

    def __init__(self, client: Client, request: NatifyRequest) -> None:
        assert isinstance(request, NatifyRequest)
        super().__init__(client, request)

    def _feach_data(self):
        self._result = NatifyResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port)

    def _response_construct(self):
        self._response = NatifyResponse(self._request, self._result)

    def _response_send(self) -> None:
        super()._response_send()
        super()._data_operate()


class PingHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: PingRequest) -> None:
        super().__init__(client, request)
        raise NotImplementedError()


class ReportHandler(CmdHandlerBase):
    _request: ReportRequest
    _result: ReportResult

    def __init__(self, client: Client, request: ReportRequest) -> None:
        super().__init__(client, request)
        assert isinstance(request, ReportRequest)
