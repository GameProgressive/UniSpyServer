from library.abstractions.client import ClientBase
from servers.chat.abstractions.channel import ChannelHandlerBase, ChannelRequestBase
from servers.chat.abstractions.contract import ResultBase
from servers.chat.enums.general import MessageType


class MessageRequestBase(ChannelRequestBase):
    type: MessageType
    nick_name: str
    message: str

    def parse(self):
        super().parse()
        if "#" in self.channel_name:
            self.type = MessageType.CHANNEL_MESSAGE
        else:
            self.type = MessageType.USER_MESSAGE
            self.channel_name = None
            self.nick_name = self._cmd_params[0]

        self.message = self._longParam


class MessageResultBase(ResultBase):
    user_irc_prefix: str
    target_name: str


class MessageHandlerBase(ChannelHandlerBase):
    _request: MessageRequestBase
    _result: MessageResultBase
    _receiver: ChannelUser

    def __init__(self, client: ClientBase, request: MessageRequestBase):
        assert isinstance(request, MessageRequestBase)
        super().__init__(client, request)

    def _update_channel_cache(self):
        """we do nothing here, channel message do not need to update channel cache"""
        pass


