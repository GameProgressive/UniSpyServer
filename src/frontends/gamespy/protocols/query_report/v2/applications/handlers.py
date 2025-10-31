from typing import final
from frontends.gamespy.protocols.query_report.applications.client import Client
from frontends.gamespy.protocols.query_report.v2.abstractions.cmd_handler_base import (
    CmdHandlerBase,
)
from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
    AvaliableRequest,
    ChallengeRequest,
    ClientMessageAckRequest,
    ClientMessageRequest,
    EchoRequest,
    HeartbeatRequest,
    KeepAliveRequest,
)
from frontends.gamespy.protocols.query_report.v2.contracts.responses import (
    AvailableResponse,
    ChallengeResponse,
    ClientMessageResponse,
    HeartbeatResponse,
)
from frontends.gamespy.protocols.query_report.v2.contracts.results import (
    AvailableResult,
    ChallengeResult,
    ClientMessageResult,
    HeartbeatResult,
)


@final
class AvailableHandler(CmdHandlerBase):
    _request: AvaliableRequest

    def __init__(self, client: Client, request: AvaliableRequest) -> None:
        assert isinstance(request, AvaliableRequest)
        super().__init__(client, request)
        

    def _response_construct(self):
        self._result = AvailableResult(
            command_name=self._request.command_name,
            instant_key=self._request.instant_key)
        self._response = AvailableResponse(self._result)


@final
class ChallengeHanler(CmdHandlerBase):
    _request: ChallengeRequest
    _result: ChallengeResult

    def __init__(self, client: Client, request: ChallengeRequest) -> None:
        assert isinstance(request, ChallengeRequest)
        super().__init__(client, request)
        self._result_cls = ChallengeResult
        self._response_cls = ChallengeResponse


@final
class ClientMessageAckHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: ClientMessageAckRequest) -> None:
        assert isinstance(request, ClientMessageAckRequest)
        super().__init__(client, request)
        

    def _response_construct(self):
        self._client.log_info("Get client message ack")


@final
class ClientMessageHandler(CmdHandlerBase):

    def __init__(self, client: Client, request: ClientMessageRequest) -> None:
        assert isinstance(request, ClientMessageRequest)
        super().__init__(client, request)
        self._result_cls = ClientMessageResult
        self._response_cls = ClientMessageResponse


@final
class EchoHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: EchoRequest) -> None:
        assert isinstance(request, EchoRequest)
        super().__init__(client, request)


@final
class HeartbeatHandler(CmdHandlerBase):
    _request: HeartbeatRequest

    def __init__(self, client: Client, request: HeartbeatRequest) -> None:
        assert isinstance(request, HeartbeatRequest)
        super().__init__(client, request)
        

    def _response_construct(self) -> None:
        self._result = HeartbeatResult(
            remote_ip=self._client.connection.remote_ip,
            remote_port=self._client.connection.remote_port,
            instant_key=self._request.instant_key,
            command_name=self._request.command_name
        )
        self._response = HeartbeatResponse(self._result)


@final
class KeepAliveHandler(CmdHandlerBase):

    def __init__(self, client: Client, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)
        
