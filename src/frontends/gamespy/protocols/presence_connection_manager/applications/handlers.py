from typing import final

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    SdkRevisionType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
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
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    BlockListResult,
    BuddyListResult,
    NewUserResult,
    RegisterNickResult,
    StatusInfoResult,
    StatusResult,
    GetProfileResult,
    NewProfileResult,
    LoginResult,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.responses import (
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

from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    Client,
)
from frontends.gamespy.protocols.presence_connection_manager.abstractions.handlers import (
    CmdHandlerBase,
    LoginedHandlerBase,
)
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    RequestBase,
)
from typing import TYPE_CHECKING


if TYPE_CHECKING:
    from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
        Client,
    )

# region General


class KeepAliveHandler(CmdHandlerBase):
    _request: KeepAliveRequest

    def __init__(self, client: "Client", request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        # we set ip and data to request
        self._request.client_ip = self._client.connection.remote_ip
        self._request.client_port = self._client.connection.remote_port
        super()._data_operate()

    def _response_construct(self) -> None:
        self._response = KeepAliveResponse()


class LoginHandler(CmdHandlerBase):
    _request: LoginRequest
    _result_cls: type[LoginResult]
    _result: LoginResult

    def __init__(self, client: "Client", request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)
        self._result_cls = LoginResult
        self._response_cls = LoginResponse


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

    def __init__(self, client: Client, request: NewUserRequest) -> None:
        assert isinstance(request, NewUserRequest)
        super().__init__(client, request)
        self._result_cls = NewUserResult
        self._response_cls = NewUserResponse


class SdkRevisionHandler(CmdHandlerBase):
    _request: LoginRequest

    def __init__(self, client: "Client", request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _data_operate(self):
        pass

    def _response_construct(self) -> None:
        self._client.info.sdk_revision = self._request.sdk_revision_type
        for operation in self._client.info.sdk_revision:
            if operation == SdkRevisionType.GPINEW_LIST_RETRIEVAL_ON_LOGIN:
                BuddyListHandler(self._client).handle()
                BlockListHandler(self._client).handle()
                request = StatusInfoRequest()
                request.profile_id = self._client.info.profile_id
                request.namespace_id = int(self._client.info.namespace_id)
                StatusInfoHandler(self._client, request).handle()
            # todo: add other revision operations


# region Buddy


class AddBuddyHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class BlockListHandler(CmdHandlerBase):
    _result: BlockListResult

    def __init__(self, client: Client) -> None:
        assert isinstance(client, Client)
        self._is_fetching = False

    def _response_construct(self) -> None:
        self._response = BlockListResponse(self._result)


class BuddyListHandler(LoginedHandlerBase):
    _result: BuddyListResult
    _result_cls: type[BuddyListResult]

    def __init__(self, client):
        self._client = client
        assert isinstance(client, Client)
        self._result_cls = BuddyListResult
        self._response_cls = BuddyListResponse


class BuddyStatusInfoHandler(CmdHandlerBase):
    """
    This is what the message should look like.  Its broken up for easy viewing.

    \\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


class DelBuddyHandler(LoginedHandlerBase):
    _request: DelBuddyRequest

    def __init__(self, client: Client, request: DelBuddyRequest) -> None:
        assert isinstance(request, DelBuddyRequest)
        super().__init__(client, request)
        self._is_uploading = False


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
        self._is_fetching = False


class StatusInfoHandler(LoginedHandlerBase):
    _request: StatusInfoRequest
    _result: StatusInfoResult

    # TODO: check if this implement is correct
    def __init__(self, client: Client, request: StatusInfoRequest) -> None:
        assert isinstance(request, StatusInfoRequest)
        super().__init__(client, request)
        self._result_cls = StatusInfoResult
        self._response_cls = StatusInfoResponse

    def _response_send(self) -> None:
        # todo: check if response is needed
        super()._response_send()


# region Profile


@final
class AddBlockHandler(CmdHandlerBase):
    _request: AddBlockRequest

    def __init__(self, client: Client, request: AddBlockRequest) -> None:
        assert isinstance(request, AddBlockRequest)
        super().__init__(client, request)
        self._is_fetching = False


@final
class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResult

    def __init__(self, client: Client, request: GetProfileRequest) -> None:
        assert isinstance(request, GetProfileRequest)
        super().__init__(client, request)
        self._result_cls = GetProfileResult
        self._response_cls = GetProfileResponse


@final
class NewProfileHandler(CmdHandlerBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, client: Client, request: NewProfileRequest) -> None:
        assert isinstance(request, NewProfileRequest)
        super().__init__(client, request)
        self._result_cls = NewProfileResult
        self._response_cls = NewProfileResponse


@final
class RegisterCDKeyHandler(CmdHandlerBase):
    _request: RegisterCDKeyRequest

    def __init__(self, client: Client, request: RegisterCDKeyRequest) -> None:
        assert isinstance(request, RegisterCDKeyRequest)
        super().__init__(client, request)
        self._is_fetching = False


@final
class RegisterNickHandler(CmdHandlerBase):
    _request: RegisterNickRequest

    def __init__(self, client: Client, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        super().__init__(client, request)
        self._result_cls = RegisterNickResult
        self._response_cls = RegisterNickResponse


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
        raise NotImplementedError()


@final
class UpdateUserInfoHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()
