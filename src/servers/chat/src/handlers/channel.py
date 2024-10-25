from library.src.abstractions.client import ClientBase
from servers.chat.src.abstractions.channel import ChannelHandlerBase
from servers.chat.src.aggregates.channel import Channel
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.src.aggregates.managers import ChannelManager
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
from servers.chat.src.contracts.responses.channel import (
    GetCKeyResponse,
    JoinResponse,
    KickResponse,
    ModeResponse,
    NamesResponse,
    PartResponse,
    SetCKeyResponse,
    SetChannelKeyResponse,
    TopicResponse,
)
from servers.chat.src.contracts.results.channel import (
    GetCKeyResult,
    GetChannelKeyResult,
    JoinResult,
    KickResult,
    ModeResult,
    NamesResult,
    PartResult,
    SetChannelKeyResult,
    TopicResult,
)
from servers.chat.src.enums.general import ModeRequestType, TopicRequestType
from servers.chat.src.exceptions.general import ChatException


class GetChannelKeyHandler(ChannelHandlerBase):
    _request: GetChannelKeyRequest
    _result: GetChannelKeyResult

    def __init__(self, client: ClientBase, request: GetChannelKeyRequest):
        assert isinstance(request, GetChannelKeyRequest)
        self._is_fetching_data = True
        super().__init__(client, request)

    def _publish_message(self):
        pass

    def _update_channel_cache(self):
        pass


class GetCKeyHandler(ChannelHandlerBase):
    _request: GetCKeyRequest
    _result: GetCKeyResult

    def __init__(self, client: ClientBase, request: GetCKeyRequest):
        assert isinstance(request, GetCKeyRequest)
        super().__init__(client, request)

    def _publish_message(self):
        pass

    def _update_channel_cache(self):
        pass

    def _response_construct(self):
        self._response = GetCKeyResponse(self._request, self._result)


class JoinHandler(ChannelHandlerBase):
    _request: JoinRequest
    _result: JoinResult

    def __init__(self, client: ClientBase, request: JoinRequest):
        assert isinstance(request, JoinRequest)
        super().__init__(client, request)

    def _check_user_in_remote(self):
        """
        todo maybe do not need because there are nick handler?
        """
        pass

    def _check_user_in_local(self):
        channel = ChannelManager.get_channel(
            self._request.channel_name)
        if channel is None:
            self._channel = Channel(
                self._request.channel_name, self._client, self._request.password)
            ChannelManager.add_channel(self._channel)
        else:
            self._channel = channel
            if self._client.info.nick_name in self._channel.users:
                raise ChatException("user is already in channel")

    def _request_check(self) -> None:
        # todo check if user already in local channel
        # self._check_user_in_remote()
        self._check_user_in_local()
        self._user = ChannelUser(self._client, self._channel)
        self._channel.add_bind_on_user_and_channel(self._user)

        super()._request_check()

    def _response_construct(self):
        self._response = JoinResponse(self._request, self._result)

    def _response_send(self):
        # for join request we need to send to our self
        self._channel.multicast(self._user.client, self._response, False)


class KickHandler(ChannelHandlerBase):
    _request: KickRequest
    _result: KickResult

    def __init__(self, client: ClientBase, request: KickRequest):
        assert isinstance(request, KickRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = KickResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class ModeHandler(ChannelHandlerBase):
    _request: ModeRequest
    _result: ModeResult

    def __init__(self, client: ClientBase, request: ModeRequest):
        assert isinstance(request, ModeRequest)
        super().__init__(client, request)

    def _response_construct(self):
        match self._request.request_type:
            case (
                ModeRequestType.GET_CHANNEL_AND_USER_MODES,
                ModeRequestType.GET_CHANNEL_MODES,
            ):
                self._response = ModeResponse(self._request, self._result)
            case ModeRequestType.SET_CHANNEL_MODES:
                pass
            case _:
                raise ChatException("Unknown mode request type")

    def _publish_message(self):
        if self._request.request_type == ModeRequestType.SET_CHANNEL_MODES:
            super()._publish_message()

    def _update_channel_cache(self):
        if self._request.request_type == ModeRequestType.SET_CHANNEL_MODES:
            super()._update_channel_cache()

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class NamesHandler(ChannelHandlerBase):
    _request: NamesRequest
    _result: NamesResult

    def __init__(self, client: ClientBase, request: NamesRequest):
        assert isinstance(request, NamesRequest)
        self._is_fetching_data = True
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NamesResponse(self._request, self._result)


class PartHandler(ChannelHandlerBase):
    _request: PartRequest
    _result: PartResult

    def __init__(self, client: ClientBase, request: PartRequest):
        assert isinstance(request, PartRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = PartResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class SetChannelKeyHandler(ChannelHandlerBase):
    _request: SetChannelKeyRequest
    _result: SetChannelKeyResult

    def __init__(self, client: ClientBase, request: SetChannelKeyRequest):
        assert isinstance(self._request, SetChannelKeyRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SetChannelKeyResponse(self._request, self._result)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class SetCKeyHandler(ChannelHandlerBase):
    _request: SetCKeyRequest

    def __init__(self, client: ClientBase, request: SetCKeyRequest):
        assert isinstance(request, SetCKeyRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = SetCKeyResponse(self._request)

    def _response_send(self):
        self._channel.multicast(self._user.client, self._response, True)


class TopicHandler(ChannelHandlerBase):
    _request: TopicRequest
    _result: TopicResult

    def __init__(self, client: ClientBase, request: TopicRequest):
        assert isinstance(request, TopicRequest)
        super().__init__(client, request)

    def _response_construct(self) -> None:
        self._response = TopicResponse(self._request, self._result)

    def _response_send(self) -> None:
        match self._request.request_type:
            case TopicRequestType.GET_CHANNEL_TOPIC:
                self._client.send(self._response)
            case TopicRequestType.SET_CHANNEL_TOPIC:
                self._channel.multicast(self._client, self._response)
