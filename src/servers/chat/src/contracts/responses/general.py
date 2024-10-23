from library.src.abstractions.contracts import RequestBase, ResultBase
from library.src.encryption.gs_encryption import CLIENT_KEY, SERVER_KEY
from servers.chat.src.abstractions.contract import SERVER_DOMAIN, ResponseBase
from servers.chat.src.aggregates.response_name import *
from servers.chat.src.contracts.requests.general import GetKeyRequest, WhoRequest
from servers.chat.src.contracts.results.general import (
    GetKeyResult,
    ListResult,
    LoginResult,
    NickResult,
    PingResult,
    UserIPResult,
    WhoIsResult,
    WhoResult,
)
from servers.chat.src.enums.general import WhoRequestType


class CdKeyResponse(ResponseBase):
    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {CD_KEY} * 1 :Authenticated.\r\n"  # noqa


class CryptResponse(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {SECURE_KEY} * {CLIENT_KEY} {SERVER_KEY}\r\n"  # noqa


class GetKeyResponse(ResponseBase):
    _request: GetKeyRequest
    _result: GetKeyResult

    def __init__(self, request: GetKeyRequest, result: GetKeyResult) -> None:
        assert isinstance(request, GetKeyRequest)
        assert isinstance(result, GetKeyResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = ""

        for value in self._result.values:
            self.sending_buffer += f":{SERVER_DOMAIN} {GET_KEY} * {self._result.nick_name} {self._request.cookie} {value}\r\n"  # noqa

        self.sending_buffer += f":{SERVER_DOMAIN} {END_GET_KEY} * {self._request.cookie} * :End Of GETKEY.\r\n"  # noqa


class ListResponse(ResponseBase):
    _result: ListResult

    def __init__(self, result: ListResult) -> None:
        assert isinstance(result, ListResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = ""
        for (
            channel_name,
            total_channel_user,
            channel_topic,
        ) in self._result.channel_info_list:
            self.sending_buffer += f":{self._result.user_irc_prefix} {LIST_START} * {channel_name} {total_channel_user} {channel_topic}\r\n"  # noqa
        self.sending_buffer += f":{self._result.user_irc_prefix} {LIST_END}\r\n"  # noqa


class LoginResponse(ResponseBase):
    _result: LoginResult

    def __init__(self, result: LoginResult) -> None:
        assert isinstance(result, LoginResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {LOGIN} * {self._result.user_id} {self._result.profile_id}\r\n"  # noqa


class NickResponse(ResponseBase):
    _result: NickResult

    def __init__(self, result: NickResult) -> None:
        assert isinstance(result, NickResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {WELCOME} {self._result.nick_name} :Welcome to UniSpy!\r\n"  # noqa


class PingResponse(ResponseBase):
    _result: PingResult

    def __init__(self, result: PingResult) -> None:
        assert isinstance(result, PingResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{
            self._result.requester_irc_prefix} {PONG}\r\n"


class UserIPResponse(ResponseBase):
    _result: UserIPResult

    def __init__(self, result: UserIPResult) -> None:
        assert isinstance(result, UserIPResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {USER_IP} :@{self._result.remote_ip_address}\r\n"  # noqa


class WhoIsResponse(ResponseBase):
    _result: WhoIsResult

    def __init__(self, result: WhoIsResult) -> None:
        assert isinstance(result, UserIPResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {WHO_IS_USER} {self._result.nick_name} {self._result.name} {self._result.user_name} {self._result.public_ip_address} * :{self._result.user_name}\r\n"  # noqa

        if len(self._result.joined_channel_name) != 0:
            channel_name = ""
            for name in self._result.joined_channel_name:
                channel_name += f"@{name} "  # noqa

            self.sending_buffer += f":{SERVER_DOMAIN} {WHO_IS_CHANNELS} {self._result.nick_name} {self._result.name} :{channel_name}\r\n"  # noqa

        self.sending_buffer += f":{SERVER_DOMAIN} {END_OF_WHO_IS} {self._result.nick_name} {self._result.name} :End of WHOIS list. \r\n"  # noqa


class WhoResponse(ResponseBase):
    _request: WhoRequest
    _result: WhoResult

    def __init__(self, request: WhoRequest, result: WhoResult) -> None:
        assert isinstance(request, WhoRequest)
        assert isinstance(result, WhoResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = ""
        for (
            channel_name,
            user_name,
            public_ip_address,
            nick_name,
            modes,
        ) in self._result.infos:
            self.sending_buffer += f":{SERVER_DOMAIN} {WHO_REPLY} * {channel_name} {user_name} {public_ip_address} * {nick_name} {modes} *\r\n"  # noqa

        if self._request.request_type == WhoRequestType.GET_CHANNEL_USER_INFO:
            if len(self._result.infos) > 0:
                self.sending_buffer += f":{SERVER_DOMAIN} {END_OF_WHO} * {self._request.channel_name} * :End of WHO.\r\n"  # noqa
        elif self._request.request_type == WhoRequestType.GET_USER_INFO:
            if len(self._result.infos) > 0:
                self.sending_buffer += f":{SERVER_DOMAIN} {END_OF_WHO} * {self._request.nick_name} * :End of WHO.\r\n"  # noqa
