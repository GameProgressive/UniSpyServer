from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.network.brockers import WebsocketBrocker
from frontends.gamespy.protocols.chat.aggregates.enums import MessageType
from frontends.gamespy.protocols.chat.abstractions.contract import ResultBase
from frontends.gamespy.protocols.chat.aggregates.exceptions import ChatException, NoSuchNickException, NoSuchChannelException
from frontends.gamespy.protocols.chat.abstractions.contract import RequestBase
from frontends.gamespy.protocols.chat.abstractions.contract import *
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.protocols.chat.abstractions.contract import RequestBase, ResultBase
from frontends.gamespy.protocols.chat.applications.client import Client
from frontends.gamespy.protocols.chat.aggregates.exceptions import IRCException
import frontends.gamespy.library.abstractions.handler as lib
from typing import cast


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client
    _request: RequestBase
    _result: ResultBase

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        assert issubclass(type(request), RequestBase)

    def _handle_exception(self, ex: Exception) -> None:
        t_ex = type(ex)
        if t_ex is IRCException:
            ex = cast(IRCException, ex)
            self._client.connection.send(ex.message.encode())
        super()._handle_exception(ex)


class PostLoginHandlerBase(CmdHandlerBase):
    pass


# region Channel


class ChannelRequestBase(RequestBase):
    channel_name: Optional[str]

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.channel_name = None

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


def handle_brocker_message(message: str):
    raise NotImplementedError()


class ChannelHandlerBase(PostLoginHandlerBase):
    _request: ChannelRequestBase
    _response: ResponseBase
    _result: ResultBase
    _b_msg: BrockerMessage
    """
    broadcast message
    """
    _brocker: BrockerBase = WebsocketBrocker(
        "channel", CONFIG.backend.url, handle_brocker_message)

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        # self._channel = None

    def _message_construct(self):
        """
        broadcast message construct
        """
        assert self._request.channel_name is not None
        self._response.build()
        self._b_msg = BrockerMessage(server_id=self._client.server_config.server_id,
                                     channel_name=self._request.channel_name,
                                     sender_ip_address=self._client.connection.remote_ip,
                                     sender_port=self._client.connection.remote_port,
                                     message=self._response.sending_buffer)

    def _publish_to_brocker(self):
        """
        send message to backend, let backend to broadcast for us
        """
        self._message_construct()
        j_str = self._b_msg.model_dump_json()
        self._client.log_network_broadcast(j_str)
        ChannelHandlerBase._brocker.publish_message(j_str)

    def _response_send(self) -> None:
        super()._response_send()
        self._publish_to_brocker()


# region Message


class MessageRequestBase(ChannelRequestBase):
    type: MessageType
    nick_name: str
    message: str

    def parse(self):
        super().parse()
        if self.channel_name is None:
            raise NoSuchNickException(
                "the channel name is missing from the request")
        if "#" in self.channel_name:
            self.type = MessageType.CHANNEL_MESSAGE
        else:
            if self._cmd_params is None or len(self._cmd_params) < 1:
                raise ChatException("cmd params is invalid")
            self.type = MessageType.USER_MESSAGE
            self.nick_name = self._cmd_params[0]
        if self._long_param is None:
            raise ChatException("long param is invalid")
        self.message = self._long_param


class MessageResultBase(ResultBase):
    user_irc_prefix: str
    target_name: str


class MessageHandlerBase(ChannelHandlerBase):
    _request: MessageRequestBase
    _result: MessageResultBase

    def __init__(self, client: ClientBase, request: MessageRequestBase):
        assert isinstance(request, MessageRequestBase)
        super().__init__(client, request)

    def _update_channel_cache(self):
        """we do nothing here, channel message do not need to update channel cache"""
        pass
