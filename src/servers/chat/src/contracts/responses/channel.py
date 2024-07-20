from library.src.abstractions.contracts import RequestBase, ResultBase
from servers.chat.src.abstractions.channel import ChannelResponseBase
from servers.chat.src.abstractions.contract import (
    SERVER_DOMAIN,
    RequestBase,
    ResponseBase,
    ResultBase,
)
from servers.chat.src.aggregates.response_name import *
from servers.chat.src.contracts.requests.channel import (
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
)
from servers.chat.src.contracts.results.channel import (
    GetCKeyResult,
    GetChannelKeyResult,
    JoinResult,
    KickResult,
    ModeResult,
    NamesResult,
    PartResult,
    TopicResult,
)
from servers.chat.src.enums.general import ModeRequestType


class GetChannelKeyResponse(ChannelResponseBase):
    _request: GetChannelKeyRequest
    _result: GetChannelKeyResult

    def build(self) -> None:
        self.sending_buffer = f":{self._result.channel_user_irc_prefix} {GET_CHAN_KEY} * {self._result.channel_name} {self._request.cookie} {self._result.values}\r\n"


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
            self.sending_buffer += f":{SERVER_DOMAIN} {GET_CKEY} * {self._request.channel_name} {nick_name} {self._request.cookie} {user_values}\r\n"

        self.sending_buffer += f"{SERVER_DOMAIN} {END_GET_CKEY} * {self._request.channel_name} {self._request.cookie} :End Of GETCKEY.\r\n"


class JoinResponse(ResponseBase):
    _request: JoinRequest
    _result: JoinResult

    def __init__(self, request: JoinRequest, result: JoinResult) -> None:
        assert isinstance(request, JoinRequest)
        assert isinstance(result, JoinResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = (
            f"{self._result.joiner_prefix} {JOIN} {self._request.channel_name}\r\n"
        )


class KickResponse(ResponseBase):
    _request: KickRequest
    _result: KickResult

    def __init__(self, request: KickRequest, result: KickResult) -> None:
        assert isinstance(request, KickRequest)
        assert isinstance(result, KickResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f"{self._result.kicker_irc_prefix} {KICK} {self._result.channel_name} {self._result.kickee_nick_name} :{self._request.reason}\r\n"


class ModeResponse(ResponseBase):
    _request: ModeRequest
    _result: ModeResult

    def __init__(self, request: ModeRequest, result: ModeResult) -> None:
        assert isinstance(request, ModeRequest)
        assert isinstance(result, ModeResult)
        super().__init__(request, result)

    def build(self) -> None:
        if self._request.request_type == ModeRequestType.GET_CHANNEL_MODES:
            self.sending_buffer = f":{SERVER_DOMAIN} {CHANNEL_MODELS} * {self._result.channel_modes} {self._result.channel_modes}\r\n"


class NamesResponse(ChannelResponseBase):
    _request: NamesRequest
    _result: NamesResult

    def __init__(self, request: NamesRequest, result: NamesResult) -> None:
        assert isinstance(request, NamesRequest)
        assert isinstance(result, NamesResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {NAME_REPLY} {self._result.requester_nick_name} = {self._result.channel_name} :{self._result.all_channel_user_nicks}\r\n"

        self.sending_buffer = f":{SERVER_DOMAIN} {END_OF_NAMES} {self._result.requester_nick_name} {self._result.channel_name} :End of NAMES list. \r\n"


class PartResponse(ResponseBase):
    _request: PartRequest
    _result: PartResult

    def __init__(self, request: PartRequest, result: PartResult) -> None:
        assert isinstance(request, PartRequest)
        assert isinstance(result, PartResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.leaver_irc_prefix} {PART} {self._result.channel_name} :{self._request.reason}\r\n"


class SetChannelKeyResponse(ChannelResponseBase):
    _request: SetChannelKeyRequest
    _result: GetChannelKeyResult

    def __init__(
        self, request: SetChannelKeyRequest, result: GetChannelKeyResult
    ) -> None:
        assert isinstance(request, SetChannelKeyRequest)
        assert isinstance(result, GetChannelKeyResult)
        super().__init__(request, result)

    def build(self) -> None:
        self.sending_buffer = f":{self._result.channel_user_irc_prefix} {GET_CHAN_KEY} * {self._request.channel_name} BCAST {self._request.key_value_string}\r\n"


class SetCKeyResponse(ResponseBase):
    _request: SetCKeyRequest

    def __init__(self, request: SetCKeyRequest) -> None:
        assert isinstance(request, SetCKeyRequest)
        super().__init__(request, None)

    def build(self) -> None:
        self.sending_buffer = f":{SERVER_DOMAIN} {GET_CKEY} * {self._request.channel_name} {self._request.nick_name} {self._request.cookie} {self._request.key_value_string}\r\n"

        self.sending_buffer = f":{SERVER_DOMAIN} {END_GET_CKEY} * {self._request.channel_name} {self._request.nick_name} {self._request.cookie} :End Of SETCKEY.\r\n"


class TopicResponse(ResponseBase):
    _request: TopicRequest
    _result: TopicResult

    def __init__(self, request: TopicRequest, result: TopicResult) -> None:
        assert isinstance(request, TopicRequest)
        assert isinstance(result, TopicResult)
        super().__init__(request, result)

    def build(self) -> None:
        if self._result.channel_topic == "" or self._result.channel_topic is None:
            self.sending_buffer = (
                f":{SERVER_DOMAIN} {NO_TOPIC} * {self._result.channel_name}\r\n"
            )
        else:
            self.sending_buffer = f":{SERVER_DOMAIN} {TOPIC} * {self._result.channel_name} :{self._result.channel_topic}\r\n"
