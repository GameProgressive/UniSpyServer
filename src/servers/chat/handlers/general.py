from typing import Type
from library.abstractions.client import ClientBase
from servers.chat.abstractions.contract import RequestBase
from servers.chat.abstractions.handler import CmdHandlerBase, PostLoginHandlerBase
from servers.chat.contracts.requests.channel import GetUdpRelayRequest
from servers.chat.contracts.requests.general import (
    CdkeyRequest,
    CryptRequest,
    GetKeyRequest,
    InviteRequest,
    ListRequest,
    LoginRequest,
    NickRequest,
    PingRequest,
    QuitRequest,
    SetKeyRequest,
    UserIPRequest,
    WhoIsRequest,
    WhoRequest,
)
from servers.chat.contracts.responses.general import (
    CdKeyResponse,
    CryptResponse,
    GetKeyResponse,
    ListResponse,
    LoginResponse,
    NickResponse,
    PingResponse,
    UserIPResponse,
    WhoIsResponse,
    WhoResponse,
)
from servers.chat.contracts.results.general import (
    CryptResult,
    GetKeyResult,
    ListResult,
    LoginResult,
    NickResult,
    PingResult,
    UserIPResult,
    WhoIsResult,
    WhoResult,
)


class CdKeyHandler(CmdHandlerBase):
    _request: CdkeyRequest

    def __init__(self, client: ClientBase, request: CdkeyRequest):
        assert isinstance(request, CdkeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = CdKeyResponse(self._request, self._result)


class CryptHandler(CmdHandlerBase):
    _request: CryptRequest
    _result: CryptResult

    def __init__(self, client: ClientBase, request: RequestBase):
        assert isinstance(request, CryptRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = CryptResponse()


class GetKeyHandler(CmdHandlerBase):
    _request: GetKeyRequest
    _result: GetKeyResult

    def __init__(self, client: ClientBase, request: GetKeyRequest):
        assert isinstance(request, GetKeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetKeyResponse(self._request, self._result)


class GetUdpRelayHandler(CmdHandlerBase):
    _request: GetUdpRelayRequest

    def __init__(self, client: ClientBase, request: GetUdpRelayRequest):
        assert isinstance(request, GetUdpRelayRequest)
        super().__init__(client, request)


class InviteHandler(CmdHandlerBase):
    _request: InviteRequest

    def __init__(self, client: ClientBase, request: InviteRequest):
        assert isinstance(request, InviteRequest)
        super().__init__(client, request)


class ListHandler(PostLoginHandlerBase):
    _request: ListRequest
    _result: ListResult

    def __init__(self, client: ClientBase, request: ListRequest):
        assert isinstance(request, ListRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = ListResponse(self._request, self._result)


class LoginHandler(CmdHandlerBase):
    _request: LoginRequest
    _result: LoginResult

    def __init__(self, client: ClientBase, request: LoginRequest):
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = LoginResponse(self._result)


class NickHandler(CmdHandlerBase):
    _request: NickRequest
    _result: NickResult

    def __init__(self, client: ClientBase, request: NickRequest):
        assert isinstance(request, NickRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = NickResponse(self._request, self._result)


class PingHandler(CmdHandlerBase):
    _request: PingRequest
    _result: PingResult

    def __init__(self, client: ClientBase, request: PingRequest):
        assert isinstance(request, PingRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = PingResponse(self._result)


class QuitHandler(CmdHandlerBase):
    _request: QuitRequest

    def __init__(self, client: ClientBase, request: QuitRequest):
        assert isinstance(request, QuitHandler)
        super().__init__(client, request)


class SetKeyHandler(PostLoginHandlerBase):
    _request: SetKeyRequest

    def __init__(self, client: ClientBase, request: SetKeyRequest):
        assert isinstance(request, SetKeyRequest)
        super().__init__(client, request)


class UserHandler(CmdHandlerBase):
    _request: SetKeyRequest

    def __init__(self, client: ClientBase, request: SetKeyRequest):
        assert isinstance(request, SetKeyRequest)
        super().__init__(client, request)


class UserIPHandler(CmdHandlerBase):
    _request: UserIPRequest
    _result: UserIPResult

    def __init__(self, client: ClientBase, request: UserIPRequest):
        assert isinstance(request, UserIPRequest)
        super().__init__(client, request)

    def _request_check(self) -> None:
        super()._request_check()
        self._request.remote_ip_address = (
            f"{self._client.connection.remote_ip}:{self._client.connection.remote_port}"
        )

    def _response_construct(self) -> None:
        self._response = UserIPResponse(self._result)


class WhoHandler(CmdHandlerBase):
    _request: WhoRequest
    _result: WhoResult

    def __init__(self, client: ClientBase, request: WhoRequest):
        assert isinstance(request, WhoRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = WhoResponse(self._request, self._result)


class WhoIsHandler(CmdHandlerBase):
    _request: WhoIsRequest
    _result: WhoIsResult
    _result_type: Type = WhoIsResult

    def __init__(self, client: ClientBase, request: WhoIsRequest):
        assert isinstance(request, WhoIsRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = WhoIsResponse(self._result)
