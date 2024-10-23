
from servers.query_report.src.applications.client import Client
from servers.query_report.src.v2.abstractions.cmd_handler_base import CmdHandlerBase
from servers.query_report.src.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageAckRequest,
    ClientMessageRequest,
    EchoRequest,
    HeartBeatRequest,
    KeepAliveRequest,
)
from servers.query_report.src.v2.contracts.responses import (
    AvaliableResponse,
    ChallengeResponse,
    ClientMessageResponse,
    HeartBeatResponse,
)
from servers.query_report.src.v2.contracts.results import ChallengeResult, HeartBeatResult


class AvailableHandler(CmdHandlerBase):
    _request: AvaliableRequest

    def __init__(self, client: Client, request: AvaliableRequest) -> None:
        assert isinstance(request, AvaliableRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = AvaliableResponse(self._request)


class ChallengeHanler(CmdHandlerBase):
    _request: ChallengeRequest
    _result: ChallengeResult

    def __init__(self, client: Client, request: ChallengeRequest) -> None:
        assert isinstance(request, ChallengeRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = ChallengeResponse(self._request, self._result)


class ClientMessageAckHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: ClientMessageAckRequest) -> None:
        assert isinstance(request, ClientMessageAckRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._client.log_info("Get client message ack")


class ClientMessageHandler(CmdHandlerBase):
    _request: ClientMessageRequest

    def __init__(self, client: Client, request: ClientMessageRequest) -> None:
        assert isinstance(request, ClientMessageRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = ClientMessageResponse(self._request)


class EchoHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: EchoRequest) -> None:
        assert isinstance(request, EchoRequest)
        super().__init__(client, request)


class HeartBeatHandler(CmdHandlerBase):
    _request: HeartBeatRequest
    _result: HeartBeatResult
    _result_cls: type[HeartBeatResult]

    def __init__(self, client: Client, request: HeartBeatRequest) -> None:
        assert isinstance(request, HeartBeatRequest)
        super().__init__(client, request)
        self._result_cls = HeartBeatResult

    def _response_construct(self) -> None:
        self._response = HeartBeatResponse(self._request, self._result)


class KeepAliveHandler(CmdHandlerBase):
    _request: KeepAliveRequest

    def __init__(self, client: Client, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)
