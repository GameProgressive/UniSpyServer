from servers.chat.src.contracts.requests.general import RegisterNickRequest
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.abstractions.handlers import CmdHandlerBase
from servers.presence_connection_manager.src.applications.client import Client
from servers.presence_connection_manager.src.contracts.requests.profile import (
    AddBlockRequest,
    GetProfileRequest,
    NewProfileRequest,
    RegisterCDKeyRequest,
    UpdateProfileRequest,
)
from servers.presence_connection_manager.src.contracts.responses.profile import (
    GetProfileResponse,
    NewProfileResponse,
    RegisterNickResponse,
)
from servers.presence_connection_manager.src.contracts.results.profile import NewProfileResult


class AddBlockHandler(CmdHandlerBase):
    _request: AddBlockRequest

    def __init__(self, client: Client, request: AddBlockRequest) -> None:
        assert isinstance(request, AddBlockRequest)
        super().__init__(client, request)


class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResponse

    def __init__(self, client: Client, request: GetProfileRequest) -> None:
        assert isinstance(request, GetProfileRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetProfileResponse(self._request, self._result)


class NewProfileHandler(CmdHandlerBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, client: Client, request: NewProfileRequest) -> None:
        assert isinstance(request, NewProfileRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = NewProfileResponse(self._request, self._result)


class RegisterCDKeyHandler(CmdHandlerBase):
    _request: RegisterCDKeyRequest

    def __init__(self, client: Client, request: RegisterCDKeyRequest) -> None:
        assert isinstance(request, RegisterCDKeyRequest)
        super().__init__(client, request)


class RegisterNickHandler(CmdHandlerBase):

    _request: RegisterNickRequest

    def __init__(self, client: Client, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = RegisterNickResponse(self._request, self._result)


class RemoveBlockHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()

        super().__init__(client, request)


class UpdateProfileHandler(CmdHandlerBase):
    _request: UpdateProfileRequest

    def __init__(self, client: Client, request: UpdateProfileRequest) -> None:
        assert isinstance(request, UpdateProfileRequest)
        super().__init__(client, request)


class UpdateUserInfoHandler(CmdHandlerBase):
    raise NotImplementedError()
