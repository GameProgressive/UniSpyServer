from frontends.gamespy.protocols.chat.abstractions.handler import ChannelResponseBase
from frontends.gamespy.protocols.chat.aggregates.enums import (
    ModeRequestType,
    WhoRequestType,
    ResponseCode,
)
from frontends.gamespy.protocols.chat.contracts.results import (
    GetCKeyResult,
    GetChannelKeyResult,
    JoinResult,
    KickResult,
    ModeResult,
    NamesResult,
    NamesResultData,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
    ATMResult,
    NoticeResult,
    PrivateResult,
    UTMResult,
    GetKeyResult,
    ListResult,
    LoginResult,
    NickResult,
    PingResult,
    UserIPResult,
    WhoIsResult,
    WhoResult,
)
from frontends.gamespy.protocols.chat.contracts.requests import (
    GetCKeyRequest,
    GetChannelKeyRequest,
    JoinRequest,
    KickRequest,
    ModeRequest,
    NamesRequest,
    PartRequest,
    SetCKeyRequest,
    SetChannelKeyRequest,
    TopicRequest,
    ATMRequest,
    NoticeRequest,
    PrivateRequest,
    UTMRequest,
    GetKeyRequest,
    WhoRequest,
)
from frontends.gamespy.protocols.chat.abstractions.contract import (
    SERVER_DOMAIN,
    ResponseBase,
)
from frontends.gamespy.library.encryption.gs_encryption import CLIENT_KEY, SERVER_KEY

# region General


class CdKeyResponse(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self) -> None:
        self.sending_buffer = (
            f":{SERVER_DOMAIN} {ResponseCode.CDKEY.value} * 1 :Authenticated.\r\n"  # noqa
        )


class CryptResponse(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.SECUREKEY.value} * {CLIENT_KEY} {SERVER_KEY}\r\n"  # noqa


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
            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.GETKEY.value} * {self._result.nick_name} {self._request.cookie} {value}\r\n"  # noqa

        self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDGETKEY.value} * {self._request.cookie} * :End Of GETKEY.\r\n"  # noqa


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
            self.sending_buffer += f":{self._result.user_irc_prefix} {ResponseCode.LISTSTART.value} * {channel_name} {total_channel_user} {channel_topic}\r\n"  # noqa
        self.sending_buffer += (
            f":{self._result.user_irc_prefix} {ResponseCode.LISTEND.value}\r\n"  # noqa
        )


class LoginResponse(ResponseBase):
    _result: LoginResult

    def __init__(self, result: LoginResult) -> None:
        assert isinstance(result, LoginResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.LOGIN.value} * {self._result.user_id} {self._result.profile_id}\r\n"  # noqa


class NickResponse(ResponseBase):
    _result: NickResult

    def __init__(self, result: NickResult) -> None:
        assert isinstance(result, NickResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.WELCOME.value} {self._result.nick_name} :Welcome to UniSpy!\r\n"  # noqa


class PingResponse(ResponseBase):
    _result: PingResult

    def __init__(self, result: PingResult) -> None:
        assert isinstance(result, PingResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = (
            f":{self._result.requester_irc_prefix} {ResponseCode.PONG.value}\r\n"
        )


class UserIPResponse(ResponseBase):
    _result: UserIPResult

    def __init__(self, result: UserIPResult) -> None:
        assert isinstance(result, UserIPResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.USRIP.value} :@{self._result.remote_ip_address}\r\n"


class WhoIsResponse(ResponseBase):
    _result: WhoIsResult

    def __init__(self, result: WhoIsResult) -> None:
        assert isinstance(result, UserIPResult)
        self._result = result

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.WHOISUSER.value} {self._result.nick_name} {self._result.name} {self._result.user_name} {self._result.public_ip_address} * :{self._result.user_name}\r\n"  # noqa

        if len(self._result.joined_channel_name) != 0:
            channel_name = ""
            for name in self._result.joined_channel_name:
                channel_name += f"@{name} "  # noqa

            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.WHOISCHANNELS.value} {self._result.nick_name} {self._result.name} :{channel_name}\r\n"  # noqa

        self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDOFWHOIS.value} {self._result.nick_name} {self._result.name} :End of WHOIS list. \r\n"  # noqa


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
            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.WHOREPLY.value} * {channel_name} {user_name} {public_ip_address} * {nick_name} {modes} *\r\n"  # noqa

        if self._request.request_type == WhoRequestType.GET_CHANNEL_USER_INFO:
            # if len(self._result.infos) > 0:
            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDOFWHO.value} * {self._request.channel_name} * :End of WHO.\r\n"  # noqa
        elif self._request.request_type == WhoRequestType.GET_USER_INFO:
            # if len(self._result.infos) > 0:
            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDOFWHO.value} * {self._request.nick_name} * :End of WHO.\r\n"  # noqa


# region Channel


class GetChannelKeyResponse(ChannelResponseBase):
    _request: GetChannelKeyRequest
    _result: GetChannelKeyResult

    def build(self) -> None:
        self.sending_buffer = f":{self._result.channel_user_irc_prefix} {ResponseCode.GETCHANKEY.value} * {self._result.channel_name} {self._request.cookie} {self._result.values}\r\n"


class GetCKeyResponse(ResponseBase):
    _request: GetCKeyRequest
    _result: GetCKeyResult

    def __init__(self, request: GetCKeyRequest, result: GetCKeyResult) -> None:
        assert isinstance(request, GetCKeyRequest)
        assert isinstance(result, GetCKeyResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = ""
        for nick_name, user_values in self._result.infos:
            self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.GETCKEY.value} * {self._request.channel_name} {nick_name} {self._request.cookie} {user_values}\r\n"  # noqa

        self.sending_buffer += f"{SERVER_DOMAIN} {ResponseCode.ENDGETCKEY.value} * {self._request.channel_name} {self._request.cookie} :End Of GETCKEY.\r\n"  # noqa


class JoinResponse(ResponseBase):
    _request: JoinRequest
    _result: JoinResult

    def __init__(self, request: JoinRequest, result: JoinResult) -> None:
        assert isinstance(request, JoinRequest)
        assert isinstance(result, JoinResult)
        super().__init__(request, result)

    def build(self) -> None:
        joiner_irc_prefix = f"{self._result.joiner_nick_name}!{self._result.joiner_user_name}@{SERVER_DOMAIN}"
        self.sending_buffer = f"{joiner_irc_prefix} {ResponseCode.JOIN.value} {self._request.channel_name}\r\n"


class KickResponse(ResponseBase):
    _request: KickRequest
    _result: KickResult

    def __init__(self, request: KickRequest, result: KickResult) -> None:
        assert isinstance(request, KickRequest)
        assert isinstance(result, KickResult)
        super().__init__(request, result)

    def build(self) -> None:
        kicker_irc_prefix = f"{self._result.kicker_nick_name}!{self._result.kicker_user_name}@{SERVER_DOMAIN}"
        self.sending_buffer = f"{kicker_irc_prefix} {ResponseCode.KICK.value} {self._result.channel_name} {self._result.kickee_nick_name} :{self._request.reason}\r\n"


class ModeResponse(ResponseBase):
    _request: ModeRequest
    _result: ModeResult

    def __init__(self, request: ModeRequest, result: ModeResult) -> None:
        assert isinstance(request, ModeRequest)
        assert isinstance(result, ModeResult)
        super().__init__(request, result)

    def build(self) -> None:
        if self._request.request_type == ModeRequestType.GET_CHANNEL_MODES:
            self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.CHANNELMODEIS.value} * {self._result.channel_modes} {self._result.channel_modes}\r\n"


class NamesResponse(ChannelResponseBase):
    _request: NamesRequest
    _result: NamesResult

    def __init__(self, request: NamesRequest, result: NamesResult) -> None:
        assert isinstance(request, NamesRequest)
        assert isinstance(result, NamesResult)
        super().__init__(request, result)

    @staticmethod
    def get_nicks_list(data: list[NamesResultData]):
        nicks_str = ""
        for i in range(len(data)):
            user = data[i]
            assert isinstance(user.is_channel_creator, bool)
            assert isinstance(user.nick_name, str)
            if user.is_channel_creator:
                nicks_str += f"@{user.nick_name}"
            else:
                nicks_str += user.nick_name
            # use space as seperator
            if i != (len(data) - 1):
                nicks_str += " "
        return nicks_str

    def build(self) -> None:
        nicks_str = NamesResponse.get_nicks_list(self._result.channel_nicks)
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.NAMEREPLY.value} {self._result.requester_nick_name} = {self._result.channel_name} :{nicks_str}\r\n"
        self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDOFNAMES.value} {self._result.requester_nick_name} {self._result.channel_name} :End of NAMES list. \r\n"


class PartResponse(ResponseBase):
    _request: PartRequest
    _result: PartResult

    def __init__(self, request: PartRequest, result: PartResult) -> None:
        assert isinstance(request, PartRequest)
        assert isinstance(result, PartResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.leaver_irc_prefix} {ResponseCode.PART.value} {self._result.channel_name} :{self._request.reason}\r\n"


class SetChannelKeyResponse(ChannelResponseBase):
    _request: SetChannelKeyRequest
    _result: SetChannelKeyResult

    def __init__(
        self, request: SetChannelKeyRequest, result: SetChannelKeyResult
    ) -> None:
        assert isinstance(request, SetChannelKeyRequest)
        assert isinstance(result, SetChannelKeyResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.channel_user_irc_prefix} {ResponseCode.GETCHANKEY.value} * {self._request.channel_name} BCAST {self._request.key_value_string}\r\n"


class SetCKeyResponse(ResponseBase):
    _request: SetCKeyRequest

    def __init__(self, request: SetCKeyRequest) -> None:
        assert isinstance(request, SetCKeyRequest)
        super().__init__(request, None)

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.GETCKEY.value} * {self._request.channel_name} {self._request.nick_name} {self._request.cookie} {self._request.key_value_string}\r\n"

        self.sending_buffer += f":{SERVER_DOMAIN} {ResponseCode.ENDGETCKEY.value} * {self._request.channel_name} {self._request.nick_name} {self._request.cookie} :End Of SETCKEY.\r\n"


class TopicResponse(ResponseBase):
    _request: TopicRequest
    _result: TopicResult

    def __init__(self, request: TopicRequest, result: TopicResult) -> None:
        assert isinstance(request, TopicRequest)
        assert isinstance(result, TopicResult)
        super().__init__(request, result)

    def build(self) -> None:
        if self._result.channel_topic == "" or self._result.channel_topic is None:
            self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.NOTOPIC.value} * {self._result.channel_name}\r\n"
        else:
            self.sending_buffer = f":{SERVER_DOMAIN} {ResponseCode.TOPIC.value} * {self._result.channel_name} :{self._result.channel_topic}\r\n"


# region Message


class ATMResponse(ResponseBase):
    _request: ATMRequest
    _result: ATMResult

    def __init__(self, request: ATMRequest, result: ATMResult) -> None:
        assert isinstance(request, ATMRequest)
        assert isinstance(result, ATMResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {ResponseCode.ATM.value} {self._result.target_name} :{self._request.message}\r\n"


class NoticeResponse(ResponseBase):
    _request: NoticeRequest
    _result: NoticeResult

    def __init__(self, request: NoticeRequest, result: NoticeResult) -> None:
        assert isinstance(result, NoticeResult)
        assert isinstance(request, NoticeRequest)

        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {ResponseCode.NOTICE.value} {self._result.target_name} {self._request.message}\r\n"


class PrivateResponse(ResponseBase):
    _request: PrivateRequest
    _result: PrivateResult

    def __init__(self, request: PrivateRequest, result: PrivateResult) -> None:
        assert isinstance(result, PrivateRequest)
        assert isinstance(request, PrivateResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {ResponseCode.PRIVMSG.value} {self._result.target_name} :{self._request.message}\r\n"


class UTMResponse(ResponseBase):
    _request: UTMRequest
    _result: UTMResult

    def __init__(self, request: UTMRequest, result: UTMResult) -> None:
        assert isinstance(request, UTMRequest)
        assert isinstance(result, UTMResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.user_irc_prefix} {
            ResponseCode.UTM.value
        } {self._result.target_name} :{self._request.message}"
