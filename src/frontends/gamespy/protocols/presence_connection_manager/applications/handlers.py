from typing import final

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginStatus,
    SdkRevisionType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    AddBlockRequest,
    AddBuddyRequest,
    AuthAddBuddyRequest,
    BlockListRequest,
    BuddyListRequest,
    BuddyMessageFriendAddRequest,
    GetProfileRequest,
    NewProfileRequest,
    NewUserRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    RemoveBlockRequest,
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
    BuddyMessageFriendAddResult,
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
    BuddyMessageFriendAddResponse,
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


@final
class KeepAliveHandler(CmdHandlerBase):
    _request: KeepAliveRequest

    def __init__(self, client: Client, request: KeepAliveRequest) -> None:
        assert isinstance(request, KeepAliveRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = KeepAliveResponse()


@final
class LoginHandler(CmdHandlerBase):
    _request: LoginRequest
    _result: LoginResult
    _response: LoginResponse

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)

    def _response_send(self) -> None:
        super()._response_send()
        self._client.info.profile_id = self._result.data.profile_id
        self._client.info.namespace_id = self._result.data.namespace_id
        self._client.info.sdk_revision = self._request.sdk_revision_type
        self._client.info.user_id = self._result.data.user_id
        self._client.info.login_status = LoginStatus.COMPLETED
        handler = SdkRevisionHandler(self._client, self._request)
        handler.handle()


@final
class LogoutHandler(LoginedHandlerBase):
    _request: LogoutRequest

    def __init__(self, client: Client, request: LogoutRequest) -> None:
        assert isinstance(request, LogoutRequest)
        super().__init__(client, request)

    def _request_check(self) -> None:
        super()._request_check()
        self._request.user_id = self._client.info.user_id


@final
# todo create new handler
class NewUserHandler(CmdHandlerBase):
    _request: NewUserRequest
    _result: NewUserResult
    _response: NewUserResponse
    # todo create seperate request and result

    def __init__(self, client: Client, request: NewUserRequest) -> None:
        assert isinstance(request, NewUserRequest)
        super().__init__(client, request)


@final
class SdkRevisionHandler(CmdHandlerBase):
    _request: LoginRequest

    def __init__(self, client: Client, request: LoginRequest) -> None:
        assert isinstance(request, LoginRequest)
        super().__init__(client, request)
        self._is_fetching = False
        self._is_uploading = False

    def _response_construct(self) -> None:
        self._client.info.sdk_revision = self._request.sdk_revision_type
        # todo check whether need to send friend add requests
        # bmh = BuddyMessageFriendAddHandler(self._client, BuddyMessageFriendAddRequest(
        #     self._client.info.profile_id,
        #     self._client.info.namespace_id,
        #     self._request.operation_id
        # ))
        # bmh.handle()
        if SdkRevisionType.GPINEW_LIST_RETRIEVAL_ON_LOGIN in self._client.info.sdk_revision:
            bdy = BuddyListHandler(self._client, BuddyListRequest(
                self._client.info.profile_id,
                self._client.info.namespace_id,
                self._request.operation_id))
            bdy.handle()
            bl = BlockListHandler(self._client, BlockListRequest(
                self._client.info.profile_id,
                self._client.info.namespace_id,
                self._request.operation_id))
            bl.handle()

            bs = BuddyStatusHandler(self._client, BuddyStatusRequest(
                self._client.info.profile_id,
                self._client.info.namespace_id,
                self._request.operation_id
            ))
            bs.handle()
            # request = StatusInfoRequest()
            # request.profile_id = self._client.info.profile_id
            # request.namespace_id = int(self._client.info.namespace_id)
            # StatusInfoHandler(self._client, request).handle()
            # todo: add other revision operations


# region Buddy


@final
class AddBuddyHandler(CmdHandlerBase):
    _request: AddBuddyRequest

    def __init__(self, client: Client, request: AddBuddyRequest) -> None:
        assert isinstance(request, AddBuddyRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        self._request.sender_profile_id = self._client.info.profile_id
        self._request.namespace_id = self._client.info.namespace_id
        self._client.connection.send(
            "\\bm\\2\\f\\3\\date\\1777530486\\msg\\|signed|11111111111111111111111111111111\\final\\".encode())
        super()._data_operate()


@final
class AuthAddBuddyHandler(CmdHandlerBase):
    _request: AuthAddBuddyRequest

    def __init__(self, client: Client, request: AuthAddBuddyRequest) -> None:
        assert isinstance(request, AuthAddBuddyRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        self._request.sender_profile_id = self._client.info.profile_id
        self._request.namespace_id = self._client.info.namespace_id
        super()._data_operate()


@final
class BlockListHandler(CmdHandlerBase):
    _result: BlockListResult
    _response: BlockListResponse


@final
class BuddyListHandler(LoginedHandlerBase):
    _result: BuddyListResult
    _response: BuddyListResponse


@final
class BuddyMessageFriendAddHandler(LoginedHandlerBase):
    """
    get the friend add request from server
    """
    _result: BuddyMessageFriendAddResult
    _response: BuddyMessageFriendAddResponse


@final
class BuddyStatusInfoHandler(CmdHandlerBase):
    """
    This is what the message should look like.  Its broken up for easy viewing.

    \\bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)


@final
class DelBuddyHandler(LoginedHandlerBase):
    _request: DelBuddyRequest

    def __init__(self, client: Client, request: DelBuddyRequest) -> None:
        assert isinstance(request, DelBuddyRequest)
        super().__init__(client, request)
        self._is_uploading = False

    def _data_operate(self) -> None:
        self._request.sender_profile_id = self._client.info.profile_id
        self._request.namespace_id = self._client.info.namespace_id
        super()._data_operate()


@final
class InviteToHandler(LoginedHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        raise NotImplementedError()
        super().__init__(client, request)

    pass


@final
class StatusHandler(CmdHandlerBase):
    _request: StatusRequest
    _result: StatusResult

    def __init__(self, client: Client, request: StatusRequest) -> None:
        assert isinstance(request, StatusRequest)
        super().__init__(client, request)
        self._is_fetching = False


@final
class StatusInfoHandler(LoginedHandlerBase):
    _request: StatusInfoRequest
    _result: StatusInfoResult
    _response: StatusInfoResponse
    # TODO: check if this implement is correct

    def __init__(self, client: Client, request: StatusInfoRequest) -> None:
        assert isinstance(request, StatusInfoRequest)
        super().__init__(client, request)

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

    def _data_operate(self) -> None:
        self._request.sender_profile_id = self._client.info.profile_id
        self._request.namespace_id = self._client.info.namespace_id
        super()._data_operate()


class RemoveBlockHandler(CmdHandlerBase):
    _request: RemoveBlockRequest

    def __init__(self, client: Client, request: RemoveBlockRequest) -> None:
        assert isinstance(request, RemoveBlockRequest)
        super().__init__(client, request)
        self._is_fetching = False

    def _data_operate(self) -> None:
        self._request.sender_profile_id = self._client.info.profile_id
        self._request.namespace_id = self._client.info.namespace_id
        super()._data_operate()


@final
class GetProfileHandler(CmdHandlerBase):
    _request: GetProfileRequest
    _result: GetProfileResult
    _response: GetProfileResponse

    def __init__(self, client: Client, request: GetProfileRequest) -> None:
        assert isinstance(request, GetProfileRequest)
        super().__init__(client, request)


@final
class NewProfileHandler(CmdHandlerBase):
    _request: NewProfileRequest
    _result: NewProfileResult
    _response: NewProfileResponse

    def __init__(self, client: Client, request: NewProfileRequest) -> None:
        assert isinstance(request, NewProfileRequest)
        super().__init__(client, request)


@final
class RegisterCDKeyHandler(CmdHandlerBase):
    _request: RegisterCDKeyRequest

    def __init__(self, client: Client, request: RegisterCDKeyRequest) -> None:
        assert isinstance(request, RegisterCDKeyRequest)
        super().__init__(client, request)


@final
class RegisterNickHandler(CmdHandlerBase):
    _request: RegisterNickRequest
    _result: RegisterNickResult
    _response: RegisterNickResponse

    def __init__(self, client: Client, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        super().__init__(client, request)


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
