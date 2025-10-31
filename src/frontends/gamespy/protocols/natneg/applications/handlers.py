from frontends.gamespy.protocols.natneg.abstractions.handlers import CmdHandlerBase
from frontends.gamespy.protocols.natneg.aggregations.enums import ConnectPacketStatus, RequestType
from frontends.gamespy.protocols.natneg.applications.client import Client
from frontends.gamespy.protocols.natneg.contracts.requests import (
    AddressCheckRequest,
    ConnectAckRequest,
    ConnectRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
    ReportRequest,
)
from frontends.gamespy.protocols.natneg.contracts.responses import (
    AddressCheckResponse,
    ConnectResponse,
    ErcAckResponse,
    InitResponse,
    NatifyResponse,
)
from frontends.gamespy.protocols.natneg.contracts.results import (
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
        self._is_uploading = False

    def _response_construct(self) -> None:
        """
        address check did not require restapi backend, \n
        just send the remote ip back to the client
        """
        self._result = AddressCheckResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port,
            version=self._request.version,
            cookie=self._request.cookie,
            client_index=self._request.client_index,
            use_game_port=self._request.use_game_port,
            port_type=self._request.port_type
        )
        self._result.public_ip_addr = self._client.connection.remote_ip
        self._result.public_port = self._client.connection.remote_port
        self._response = AddressCheckResponse(self._result)


class ConnectAckHandler(CmdHandlerBase):
    _request: ConnectAckRequest

    def __init__(self, client: Client, request: ConnectAckRequest) -> None:
        assert isinstance(request, ConnectAckRequest)
        super().__init__(client, request)
        self._is_uploading = False

    def _response_construct(self) -> None:
        self._client.log_info(
            f"client: {self._client.connection.remote_ip} index:{self._request.client_index} is received the connect packet.")


class ConnectHandler(CmdHandlerBase):
    _request: ConnectRequest
    _result: ConnectResult
    _response: ConnectResponse

    def __init__(self, client: Client, request: ConnectRequest) -> None:
        assert isinstance(request, ConnectRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        if not self._result.is_both_client_ready:
            self._client.log_warn(
                f"init cache is not enough for cookie: {self._request.cookie}")
            return
        super()._response_construct()


class ErtAckHandler(CmdHandlerBase):
    _request: ErtAckRequest
    _response: ErcAckResponse

    def __init__(self, client: Client, request: ErtAckRequest) -> None:
        assert isinstance(request, ErtAckRequest)
        super().__init__(client, request)

    def _data_operate(self) -> None:
        """
        Natneg require fast response, so we do not wait for upload data.
        """
        self._result = ErtAckResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port,
            version=self._request.version,
            cookie=self._request.cookie,
            client_index=self._request.client_index,
            use_game_port=self._request.use_game_port,
            port_type=self._request.port_type
        )


class InitHandler(CmdHandlerBase):
    """
    In init process, we need response the initresponse first to make client not timeout
    """

    _request: InitRequest
    _client: Client
    _result: InitResult
    # _response: InitResponse

    def __init__(self, client: Client, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(client, request)

    def _response_construct(self):
        """
        Natneg require fast response, so we do not wait for upload data.
        """
        self._result = InitResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port,
            version=self._request.version,
            cookie=self._request.cookie,
            client_index=self._request.client_index,
            use_game_port=self._request.use_game_port,
            port_type=self._request.port_type
        )

        self._response = InitResponse(self._result)

    def handle(self) -> None:
        try:
            # we first log this class
            self._log_current_class()
            # then we handle it
            self._request_check()
            self._response_construct()
            # first send the response
            if self._response is None:
                return
            self._response_send()
            # then send to backends
            self._data_operate()
            self._invoke_connect()
        except Exception as ex:
            self._handle_exception(ex)

    def _invoke_connect(self) -> None:
        connect_raw = ConnectRequest.build(
            version=self._request.version,
            command_name=RequestType.CONNECT,
            cookie=self._request.cookie,
            port_type=self._request.port_type,
            client_index=self._request.client_index,
            use_game_port=self._request.use_game_port
        )
        request = ConnectRequest(connect_raw)
        handler = ConnectHandler(self._client, request)
        handler.handle()


class NatifyHandler(CmdHandlerBase):
    _request: NatifyRequest

    def __init__(self, client: Client, request: NatifyRequest) -> None:
        assert isinstance(request, NatifyRequest)
        super().__init__(client, request)
        self._is_uploading = False

    def _response_construct(self):
        """
        Natneg require fast response, so we do not wait for upload data.
        """
        self._result = NatifyResult(
            public_ip_addr=self._client.connection.remote_ip,
            public_port=self._client.connection.remote_port,
            version=self._request.version,
            cookie=self._request.cookie,
            client_index=self._request.client_index,
            use_game_port=self._request.use_game_port,
            port_type=self._request.port_type
        )
        self._response = NatifyResponse(self._result)


class ReportHandler(CmdHandlerBase):
    _request: ReportRequest

    def __init__(self, client: Client, request: ReportRequest) -> None:
        assert isinstance(request, ReportRequest)
        super().__init__(client, request)
