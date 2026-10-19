from backends.library.abstractions.contracts import OKResponse, RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.presence_connection_manager import data
from backends.protocols.gamespy.presence_connection_manager.requests import (
    AddBlockRequest,
    AddBuddyRequest,
    AuthAddBuddyRequest,
    BlockListRetriveRequest,
    BuddyListRetriveRequest,
    BuddyMessageFriendAddRequest,
    DelBuddyRequest,
    GetProfileRequest,
    InviteToRequest,
    KeepAliveRequest,
    LoginRequest,
    LogoutRequest,
    NewProfileRequest,
    NewUserRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    RemoveBlockRequest,
    StatusInfoRequest,
    StatusRequest,
    UpdateProfileRequest,
    UpdateUserInfoRequest,
)
from backends.protocols.gamespy.presence_connection_manager.responses import (
    BlockListRetriveResponse,
    BuddyListRetriveResponse,
    BuddyMessageFriendAddResponse,
    GetProfileResponse,
    LoginResponse,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginStatus,
    LoginType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    BlockListResult,
    BuddyListResult,
    BuddyMessageFriendAddResult,
    GetProfileResult,
    LoginResult,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPLoginBadEmailException,
    GPLoginException,
)

# region General


class KeepAliveHandler(HandlerBase):
    _request: KeepAliveRequest
    response: OKResponse

    def _data_operate(self) -> None:
        data.update_online_time(
            self._request.info.client_ip, self._request.info.client_port, self._session
        )


class LoginHandler(HandlerBase):
    _request: LoginRequest
    response: LoginResponse

    def _data_operate(self) -> None:
        if self._request.data.type == LoginType.NICK_EMAIL:
            self._nick_email_login()
        elif self._request.data.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            self._unique_nick_login()
        elif self._request.data.type == LoginType.AUTH_TOKEN:
            self._auth_token_login()
        else:
            raise ValueError("Request type not valid")

    def _nick_email_login(self) -> None:
        assert self._request.data.email is not None
        assert self._request.data.nick is not None
        self._data = data.get_user_infos_by_nick_email(
            self._request.data.nick, self._request.data.email, self._session
        )
        if not self._data:
            raise GPLoginBadEmailException(
                f"email: {self._request.data.email} is invalid."
            )

    def _unique_nick_login(self) -> None:
        assert self._request.data.unique_nick is not None
        assert self._request.data.namespace_id is not None
        self._data = data.get_user_infos_by_uniquenick_namespace_id(
            self._request.data.unique_nick,
            self._request.data.namespace_id,
            self._session,
        )

    def _auth_token_login(self) -> None:
        assert self._request.data.auth_token is not None
        self._data = data.get_user_infos_by_authtoken(
            self._request.data.auth_token, self._session
        )

    def _result_construct(self) -> None:
        if self._data is None:
            raise GPLoginException("login information is incorrect.")

        self._result = LoginResult(
            operation_id=self._request.data.operation_id,
            data=self._data,
            user_data=self._request.data.user_data,
            type=self._request.data.type,
            partner_id=self._request.data.partner_id,
            user_challenge=self._request.data.user_challenge,
            # !FIX ME: gamespy session key is 4 bytes int, not secure enough, so we just replace it with subprofileid for convience
            # TODO in future we authenticate user with JWT token
            subprofile_id=self._data.subprofile_id,
        )


class LogoutHandler(HandlerBase):
    _request: LogoutRequest
    response: OKResponse

    def _data_operate(self) -> None:
        data.update_online_status(
            subprofile_id=self._request.data.subprofile_id,
            status=LoginStatus.DISCONNECTED,
            session=self._session,
        )


class NewUserHandler(HandlerBase):
    response: OKResponse

    def __init__(self, request: NewUserRequest) -> None:
        raise NotImplementedError("Use presence search player newuser router")
        super().__init__(request)
        # region Profile

        # region Buddy


class BuddyListRetriveHandler(HandlerBase):
    _request: BuddyListRetriveRequest
    response: BuddyListRetriveResponse

    def _data_operate(self) -> None:
        self.data = data.get_buddy_list(
            self._request.data.profile_id,
            self._request.data.namespace_id,
            self._session,
        )

    def _result_construct(self) -> None:
        self._result = BuddyListResult(
            profile_ids=self.data, operation_id=self._request.data.operation_id
        )


class BlockListRetriveHandler(HandlerBase):
    _request: BlockListRetriveRequest
    response: BlockListRetriveResponse

    def _data_operate(self) -> None:
        self.data = data.get_block_list(
            self._request.data.profile_id,
            self._request.data.namespace_id,
            self._session,
        )

    def _result_construct(self) -> None:
        self._result = BlockListResult(
            profile_ids=self.data, operation_id=self._request.data.operation_id
        )


class BuddyStatusInfoHandler(HandlerBase):
    """
    This is what the message should look like.  Its broken up for easy viewing.

    "\\bsi\\\\state\\\\profile\\\\bip\\\\bport\\\\hostip\\\\hprivip\\"
    "\\qport\\\\hport\\\\sessflags\\\\rstatus\\\\gameType\\"
    "\\gameVnt\\\\gameMn\\\\product\\\\qmodeflags\\"
    """

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


class DelBuddyHandler(HandlerBase):
    _request: DelBuddyRequest
    response: OKResponse

    def _data_operate(self) -> None:
        self.data = data.del_buddy(
            self._request.data.target_profile_id,
            self._request.data.subprofile_id,
            self._session,
        )


class AddBuddyHandler(HandlerBase):
    _request: AddBuddyRequest
    response: OKResponse

    def _data_operate(self) -> None:
        data.add_buddy(
            self._request.data.subprofile_id,
            self._request.data.target_profile_id,
            self._request.data.reason,
            self._session,
        )


class BuddyMessageFriendAddHandler(HandlerBase):
    _request: BuddyMessageFriendAddRequest
    response: BuddyMessageFriendAddResponse

    def _data_operate(self) -> None:
        self._data = data.get_friend_add_info(
            self._request.data.profile_id,
            self._request.data.namespace_id,
            self._session,
        )

    def _result_construct(self) -> None:
        self._result = BuddyMessageFriendAddResult(
            operation_id=self._request.data.operation_id, data=self._data
        )


class AuthAddBuddyHandler(HandlerBase):
    """
    authenticate the add buddy request
    """

    _request: AuthAddBuddyRequest

    def _data_operate(self) -> None:
        self._data = data.set_friendship_bidirectional(
            self._request.data.subprofile_id,
            self._request.data.receiver_profile_id,
            self._session,
        )


class AddBlockHandler(HandlerBase):
    _request: AddBlockRequest
    response: OKResponse

    def _data_operate(self) -> None:
        # TODO make session key functional!!
        data.add_block(
            self._request.data.subprofile_id,
            self._request.data.target_profile_id,
            self._session,
        )


class InviteToHandler(HandlerBase):
    _request: InviteToRequest

    def _data_operate(self) -> None:
        """
        todo
        tell service which product IDs the current session can invite other players to
        """
        print("invite to handler is not implemented yet")
        # user is offline
        # if (client is null)
        # {
        #     return;
        # }
        # else
        # {

        # }
        # TODO
        # parse user to buddy message system
        # raise NotImplementedError("whether need chat server to interact?")


class StatusHandler(HandlerBase):
    _request: StatusRequest
    response: OKResponse

    def _data_operate(self) -> None:
        data.update_status(
            self._request.data.subprofile_id,
            self._request.data.current_status,
            self._request.data.location_string,
            self._request.data.status_string,
            self._session,
        )


class StatusInfoHandler(HandlerBase):
    _request: StatusInfoRequest
    response: OKResponse

    def _data_operate(self) -> None:
        raise NotImplementedError()


# region Profile


class GetProfileHandler(HandlerBase):
    _request: GetProfileRequest
    response: GetProfileResponse

    def _data_operate(self) -> None:
        self.data = data.get_profile_infos(
            profile_id=self._request.data.profile_id,
            subprofile_id=self._request.data.subprofile_id,
            session=self._session,
        )

    def _result_construct(self) -> None:
        self._result = GetProfileResult(
            user_profile=self.data, operation_id=self._request.data.operation_id
        )


class NewProfileHandler(HandlerBase):
    """
    update a exist profile
    """

    _request: NewProfileRequest
    response: OKResponse

    def _data_operate(self) -> None:
        # TODO finish old nick is none, create a nick function
        assert self._request.data.old_nick

        data.update_new_nick(
            self._request.data.subprofile_id,
            self._request.data.old_nick,
            self._request.data.new_nick,
            self._session,
        )


class RegisterCDKeyHandler(HandlerBase):
    _request: RegisterCDKeyRequest
    response: OKResponse

    def _data_operate(self):
        data.update_cdkey(
            self._request.data.subprofile_id, self._request.data.cdkey_enc, self._session
        )


class RegisterNickHandler(HandlerBase):
    """
    some game will not register uniquenick when create a new account, it will update its uniquenick later
    """

    _request: RegisterNickRequest
    response: OKResponse

    def _data_operate(self):
        data.update_uniquenick(
            self._request.data.subprofile_id,
            self._request.data.unique_nick,
            self._session,
        )


class RemoveBlockHandler(HandlerBase):
    _request: RemoveBlockRequest

    def _data_operate(self) -> None:
        data.remove_block(
            self._request.data.subprofile_id,
            self._request.data.target_profile_id,
            self._session,
        )


class UpdateProfileHandler(HandlerBase):
    _request: UpdateProfileRequest
    response: OKResponse

    def _data_operate(self):
        data.update_profiles(
            self._request.data.subprofile_id,
            self._request.data.extra_infos,
            self._session,
        )


class UpdateUserInfoHandler(HandlerBase):
    _request: UpdateUserInfoRequest
    response: OKResponse

    def _data_operate(self):

        data.update_profiles(
            self._request.data.subprofile_id,
            # TODO confirm param type ExtraInfo?
            self._request.data.extra_infos,
            self._session,
        )
