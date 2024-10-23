from typing import Optional
from library.src.abstractions.client import ClientBase
from servers.chat.src.abstractions.contract import *
from servers.chat.src.abstractions.contract import RequestBase
from servers.chat.src.abstractions.handler import PostLoginHandlerBase
from servers.chat.src.aggregates.channel import Channel, ChannelManager
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.src.exceptions.channel import NoSuchChannelException
from servers.chat.src.exceptions.general import ChatException, NoSuchNickException


class ChannelRequestBase(RequestBase):
    channel_name: str

    def parse(self) -> None:
        super().parse()
        if self._cmd_params is None or len(self._cmd_params) < 1:
            raise ChatException("Channel name is missing.")
        self.channel_name = self._cmd_params[0]


class ChannelResponseBase(ResponseBase):
    _request: ChannelRequestBase

    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        super().__init__(request, result)
        assert isinstance(request, RequestBase)
        assert isinstance(result, ResultBase)


class ChannelHandlerBase(PostLoginHandlerBase):
    _channel: Channel
    _user: ChannelUser
    _request: ChannelRequestBase
    _response: ResponseBase

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        # self._channel = None
        # self._response = None

    def _request_check(self) -> None:
        if self._request.raw_request is None:
            return super()._request_check()

        if self._channel is None:
            channel = ChannelManager.get_channel(
                self._request.channel_name
            )
        if channel is None:
            raise NoSuchChannelException(
                f"No such channel {self._request.channel_name}",
            )
        self._channel = channel
        if self._user is None:
            user = self._channel.get_user_by_nick(
                self._client.info.nick_name)

        if user is None:
            raise NoSuchNickException(
                f"Can not find user with nickname: {
                    self._client.info.nick_name} user_name: {self._client.info.user_name}"
            )
        self._user = user

    def handle(self) -> None:
        super().handle()
        try:
            # todo check whether the broadcast message is same as responses
            self._publish_message()
            self._update_channel_cache()
        except Exception as e:
            self._handle_exception(e)
            if ChannelHandlerBase._debug:
                raise e

    def _publish_message(self):
        if self._response is None:
            self._client.log_warn("response is not constructed.")
        if self._channel is None:
            self._client.log_warn("channel is not assined")
            return
        self._channel.send_message_to_brocker(self._response.sending_buffer)

    def _update_channel_cache(self):
        pass
