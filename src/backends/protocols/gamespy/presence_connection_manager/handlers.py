from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
import backends.protocols.gamespy.presence_connection_manager.data as data
from backends.protocols.gamespy.presence_connection_manager.requests import (
    AddBlockRequest,
    AddBuddyRequest,
    BlockListRequest,
    BuddyListRequest,
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
    StatusInfoRequest,
    StatusRequest,
    UpdateProfileRequest,
    UpdateUserInfoRequest,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    BlockListResult,
    BuddyListResult,
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

    def _data_operate(self) -> None:
        data.update_online_time(self._request.client_ip, self._request.client_port)


class LoginHandler(HandlerBase):
    _request: LoginRequest

    def _data_operate(self) -> None:
        if self._request.type == LoginType.NICK_EMAIL:
            self._nick_email_login()
        elif self._request.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            self._unique_nick_login()
        elif self._request.type == LoginType.AUTH_TOKEN:
            self._auth_token_login()
        else:
            raise ValueError("Request type not valid")

    def _nick_email_login(self) -> None:
        assert self._request.email is not None
        assert self._request.nick is not None
        is_exsit = data.is_email_exist(self._request.email)
        if not is_exsit:
            raise GPLoginBadEmailException(f"email: {self._request.email} is invalid.")
        self._data = data.get_user_infos_by_nick_email(
            self._request.nick, self._request.email
        )

    def _unique_nick_login(self) -> None:
        assert self._request.unique_nick is not None
        assert self._request.namespace_id is not None
        self._data = data.get_user_infos_by_uniquenick_namespace_id(
            self._request.unique_nick, self._request.namespace_id
        )

    def _auth_token_login(self) -> None:
        assert self._request.auth_token is not None
        self._data = data.get_user_infos_by_authtoken(self._request.auth_token)

    def _result_construct(self) -> None:
        if self._data is None:
            raise GPLoginException("User is not exist.")
        self._result = LoginResult(data=self._data)


class LogoutHandler(HandlerBase):
    _request: LogoutRequest

    def _data_operate(self) -> None:
        # data.update_online_status(user_id=, status=LoginStatus.DISCONNECTED)
        raise NotImplementedError()


class NewUserHandler(HandlerBase):
    def __init__(self, request: NewUserRequest) -> None:
        raise NotImplementedError("Use presence search player newuser router")
        super().__init__(request)
        # region Profile

        # region Buddy


class BuddyListHandler(HandlerBase):
    _request: BuddyListRequest

    def _data_operate(self) -> None:
        self.data = data.get_buddy_list(
            self._request.profile_id, self._request.namespace_id
        )

    def _result_construct(self) -> None:
        self._result = BuddyListResult(profile_ids=self.data)


class BlockListHandler(HandlerBase):
    _request: BlockListRequest

    def _data_operate(self) -> None:
        self.data = data.get_block_list(
            self._request.profile_id, self._request.namespace_id
        )

    def _result_construct(self) -> None:
        self._result = BlockListResult(profile_ids=self.data)


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

    def _data_operate(self) -> None:
        self.data = data.delete_friend_by_profile_id(self._request.target_id)


class AddBuddyHandler(HandlerBase):
    _request: AddBuddyRequest

    def _data_operate(self) -> None:
        data.add_friend_request(
            self._request.profile_id,
            self._request.target_id,
            self._request.namespace_id,
            self._request.reason,
        )


class AddBlockHandler(HandlerBase):
    _request: AddBlockRequest

    def _data_operate(self) -> None:
        data.update_block(
            self._request.profile_id, self._request.taget_id, self._request.session_key
        )


class InviteToHandler(HandlerBase):
    _request: InviteToRequest

    def _data_operate(self) -> None:
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
        raise NotImplementedError()


class StatusHandler(HandlerBase):
    _request: StatusRequest

    def _data_operate(self) -> None:
        data.update_status(
            self._request.session_key,
            self._request.current_status,
            self._request.location_string,
            self._request.status_string,
        )


class StatusInfoHandler(HandlerBase):
    _request: StatusInfoRequest

    def _data_operate(self) -> None:
        raise NotImplementedError()


# region Profile


class GetProfileHandler(HandlerBase):
    _request: GetProfileRequest

    def _data_operate(self) -> None:
        self.data = data.get_profile_infos(
            profile_id=self._request.profile_id, session_key=self._request.session_key
        )

    def _result_construct(self) -> None:
        self._result = GetProfileResult(user_profile=self.data)


class NewProfileHandler(HandlerBase):
    """
    update a exist profile
    """

    _request: NewProfileRequest

    def _data_operate(self) -> None:
        data.update_new_nick(
            self._request.session_key, self._request.old_nick, self._request.new_nick
        )


class RegisterCDKeyHandler(HandlerBase):
    _request: RegisterCDKeyRequest

    def _data_operate(self):
        data.update_cdkey(self._request.session_key, self._request.cdkey_enc)


class RegisterNickHandler(HandlerBase):
    """
    some game will not register uniquenick when create a new account, it will update its uniquenick later
    """

    _request: RegisterNickRequest

    def _data_operate(self):
        data.update_uniquenick(self._request.session_key, self._request.unique_nick)


class RemoveBlockHandler(HandlerBase):
    def _data_operate(self):
        raise NotImplementedError()


class UpdateProfileHandler(HandlerBase):
    _request: UpdateProfileRequest

    def _data_operate(self):
        data.update_profiles(self._request.session_key, self._request.extra_infos)


class UpdateUserInfoHandler(HandlerBase):
    _request: UpdateUserInfoRequest

    def _data_operate(self):
        data.update_profiles(self._request.session_key, self._request.extra_infos)
