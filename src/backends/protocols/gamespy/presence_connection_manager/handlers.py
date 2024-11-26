from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.presence_connection_manager.requests import *
import backends.protocols.gamespy.presence_connection_manager.data as data
from servers.presence_connection_manager.src.aggregates.enums import LoginStatus
from servers.presence_connection_manager.src.contracts.results import LoginResult
from servers.presence_search_player.src.aggregates.exceptions import GPLoginBadEmailException


class KeepAliveHandler(HandlerBase):
    _request: KeepAliveRequest

    async def _data_operate(self) -> None:
        data.update_online_time(self._request.client_ip,
                                self._request.client_port)


class LoginHandler(HandlerBase):
    _request: LoginRequest

    async def _data_operate(self) -> None:
        if self._request.type == LoginType.NICK_EMAIL:
            self._nick_email_login()
        elif self._request.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            self._unique_nick_login()
        elif self._request.type == LoginType.AUTH_TOKEN:
            self._auth_token_login()
        else:
            raise ValueError("Request type not valid")

    def _nick_email_login(self) -> None:
        is_exsit = data.is_email_exist(self._request.email)
        if not is_exsit:
            raise GPLoginBadEmailException(
                f"email: {self._request.email} is invalid.")
        self._data = data.get_user_infos_by_nick_email(
            self._request.nick, self._request.email)

    def _unique_nick_login(self) -> None:
        self.data = data.get_user_infos_by_uniquenick_namespace_id(
            self._request.unique_nick, self._request.namespace_id)

    def _auth_token_login(self) -> None:
        self._data = data.get_user_infos_by_authtoken(
            self._request.auth_token)

    async def _result_construct(self) -> None:
        if self.data is not None:
            self._result = LoginResult(data=self.data)


class LogoutHandler(HandlerBase):
    _request: LogoutRequest

    async def _data_operate(self) -> None:
        # data.update_online_status(user_id=, status=LoginStatus.DISCONNECTED)
        raise NotImplementedError()
