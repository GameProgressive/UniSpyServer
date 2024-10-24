from typing import final

from servers.presence_connection_manager.src.contracts.requests import (
    AddBlockRequest,
    GetProfileRequest,
    NewProfileRequest,
    NewUserRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    UpdateProfileRequest,
    DelBuddyRequest,
    StatusInfoRequest,
    StatusRequest,
    KeepAliveRequest,
    LoginRequest,
    LogoutRequest,
)
from servers.presence_connection_manager.src.abstractions.handlers import CmdHandlerBase
from servers.presence_connection_manager.src.contracts.results import (
    BlockListResult,
    BuddyListResult,
    NewUserResult,
    StatusInfoResult,
    StatusResult,
    GetProfileResult,
    NewProfileResult,
    LoginResult
)
from servers.presence_connection_manager.src.contracts.responses import (
    BlockListResponse,
    BuddyListResponse,
    NewUserResponse,
    StatusInfoResponse,
    GetProfileResponse,
    NewProfileResponse,
    RegisterNickResponse,
    KeepAliveResponse,
    LoginResponse,
)

from servers.presence_connection_manager.src.applications.client import Client
from servers.presence_connection_manager.src.abstractions.handlers import (
    CmdHandlerBase,
    LoginedHandlerBase,
)
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from multiprocessing.pool import Pool
from typing import TYPE_CHECKING
from servers.presence_connection_manager.src.aggregates.sdk_revision import SdkRevision


if TYPE_CHECKING:
    from servers.presence_connection_manager.src.applications.client import Client

# region General


class KeepAliveHandler(CmdHandlerBase):
    _request: KeepAliveRequest

    def __init__(self, client: "Client", request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = KeepAliveResponse(self._request)


class LoginHandler(CmdHandlerBase):

    _request: LoginRequest
    _result_cls: type[LoginResult]
    _result: LoginResult

    def __init__(self, client: "Client", request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)
        self._result_cls = LoginResult

    def _response_construct(self) -> None:
        self._response = LoginResponse(self._request, self._result)


class LogoutHandler(LoginedHandlerBase):
    _request: LogoutRequest

    def __init__(self, client: "Client", request: LogoutRequest) -> None:
        assert isinstance(request, LogoutRequest)
        super().__init__(client, request)


# todo create new handler
class NewUserHandler(CmdHandlerBase):
    _request: NewUserRequest
    _result_cls: type[NewUserResult]
    _result: NewUserResult
    # todo create seperate request and result

    def _response_construct(self):
        self._response = NewUserResponse(self._request, self._result)


class SdkRevisionHandler(CmdHandlerBase):
    _request: LoginRequest

    def __init__(self, client: "Client", request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._client.info.sdk_revision = SdkRevision(
            self._request.sdk_revision_type)
        if self._client.info.sdk_revision.is_support_gpi_new_status_notification:
            BuddyListHandler(self._client).handle()
            BlockListHandler(self._client).handle()

# region Buddy


class AddBuddyHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class BlockListHandler(CmdHandlerBase):
    _result: BlockListResult

    def __init__(self, client: Client) -> None:
        assert isinstance(client, Client)

    def _response_construct(self) -> None:
        self._response = BlockListResponse(self._result)


class BuddyListHandler(LoginedHandlerBase):
    _result: BuddyListResult
    _result_cls = BuddyListResult

    def __init__(self, client: Client):
        assert isinstance(client, Client)
        self._client = client

    def response_construct(self):
        self._response = BuddyListResponse(self._request, self._result)

    def handle_status_info(self, profile_id):
        request = StatusInfoRequest()
        request.profile_id = profile_id
        request.namespace_id = int(self._client.info.namespace_id)
        # request.is_get_status_info = True

        StatusInfoHandler(self._client, request).handle()

    def _response_send(self) -> None:
        super()._response_send()

        if not self._client.info.sdk_revision.is_support_gpi_new_status_notification:
            return

        with Pool() as pool:
            pool.map(self.handle_status_info, self._result.profile_ids)


class BuddyStatusInfoHandler(CmdHandlerBase):
    """
    This is what the message should look like.  Its broken up for easy viewing.

    \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class DelBuddyHandler(LoginedHandlerBase):
    _request: DelBuddyRequest

    def __init__(self, client: Client, request: DelBuddyRequest) -> None:
        assert isinstance(request, DelBuddyRequest)
        super().__init__(client, request)


class InviteToHandler(LoginedHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)

    pass


class StatusHandler(CmdHandlerBase):
    _request: StatusRequest
    _result: StatusResult

    def __init__(self, client: Client, request: StatusRequest) -> None:
        assert isinstance(request, StatusRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        # TODO check if statushandler need send response
        raise NotImplementedError()


class StatusInfoHandler(LoginedHandlerBase):
    _request: StatusInfoRequest
    _result: StatusInfoResult

    def __init__(self, client: Client, request: StatusInfoRequest) -> None:
        assert isinstance(request, StatusInfoRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        if self._request is not None:
            self._response = StatusInfoResponse(self._request, self._result)
            super()._response_send()


# region Profile


@final
class AddBlockHandler(CmdHandlerBase):
    _request: AddBlockRequest

    def __init__(self, client: Client, request: AddBlockRequest) -> None:
        assert isinstance(request, AddBlockRequest)
        super().__init__(client, request)


@final
class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResult

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
        self._response = RegisterNickResponse(self._request)


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
