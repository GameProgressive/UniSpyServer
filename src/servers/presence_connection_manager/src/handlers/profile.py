from typing import final
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.abstractions.handlers import CmdHandlerBase
from servers.presence_connection_manager.src.applications.client import Client
from servers.presence_connection_manager.src.contracts.requests.profile import (
    AddBlockRequest,
    GetProfileRequest,
    NewProfileRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    UpdateProfileRequest,
)
from servers.presence_connection_manager.src.contracts.responses.profile import (
    GetProfileResponse,
    NewProfileResponse,
    RegisterNickResponse,
)
from servers.presence_connection_manager.src.contracts.results.profile import NewProfileResult

@final
class AddBlockHandler(CmdHandlerBase):
    _request: AddBlockRequest

    def __init__(self, client: Client, request: AddBlockRequest) -> None:
        assert isinstance(request, AddBlockRequest)
        super().__init__(client, request)


@final
class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResponse

    def __init__(self, client: Client, request: GetProfileRequest) -> None:
        assert isinstance(request, GetProfileRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = GetProfileResponse(self._request, self._result)


@final
class NewProfileHandler(CmdHandlerBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, client: Client, request: NewProfileRequest) -> None:
        assert isinstance(request, NewProfileRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = NewProfileResponse(self._request, self._result)


@final
class RegisterCDKeyHandler(CmdHandlerBase):
    _request: RegisterCDKeyRequest

    def __init__(self, client: Client, request: RegisterCDKeyRequest) -> None:
        assert isinstance(request, RegisterCDKeyRequest)
        super().__init__(client, request)


@final
class RegisterNickHandler(CmdHandlerBase):
    _request: RegisterNickRequest

    def __init__(self, client: Client, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = RegisterNickResponse(self._request, self._result)


@final
class RemoveBlockHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()


@final
class UpdateProfileHandler(CmdHandlerBase):
    _request: UpdateProfileRequest

    def __init__(self, client: Client, request: UpdateProfileRequest) -> None:
        assert isinstance(request, UpdateProfileRequest)
        super().__init__(client, request)


@final
class UpdateUserInfoHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()
