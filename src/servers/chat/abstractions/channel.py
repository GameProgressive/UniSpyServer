from servers.chat.abstractions.contract import *
from servers.chat.abstractions.handler import PostLoginHandlerBase
from servers.chat.exceptions.channel import NoSuchChannelException
from servers.chat.exceptions.general import ChatException, NoSuchNickException


class ChannelHandlerBase(PostLoginHandlerBase):
    _channel: Channel
    _user: ChannelUser
    _request: ChannelRequestBase

    def _request_check(self) -> None:
        if self._request.raw_request is None:
            return super()._request_check()

        if self._channel is None:
            self._channel = self._client.info.get_local_channel(
                self._request.channel_name
            )
            if self._channel is None:
                raise NoSuchChannelException(
                    f"No such channel {self._request.channel_name}",
                    self._request.channel_name,
                )

        if self._user is None:
            self._user = self._channel.get_user(self._client)

            if self._user is None:
                raise NoSuchNickException(
                    f"Can not find user with nickname: {self._client.info.nickname} username: {self._client.info.username}"
                )

    def handle(self) -> None:
        super().handle()
        try:
            # we do not publish message when the message is received from remote client
            if self._client.is_remote_client:
                return
            if self._channel is None:
                return

            if self.request.raw_request is None:
                return

            publish_message()
            update_channel_cache()
        except Exception as e:
            self._handle_exception(e)

    def publish_message(self):
        meg = RemoteMessage(self._request, self._client.get_remote_client())
        self._channel.broker.publish_message(msg)

    def update_channel_cache(self):
        pass


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
