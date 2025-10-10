from backends.library.database.pg_orm import ChatChannelCaches
from frontends.gamespy.protocols.chat.aggregates.enums import MessageType
from frontends.gamespy.protocols.chat.abstractions.contract import (
    BrockerMessage,
    RequestBase,
    ResponseBase,
    ResultBase,
)
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    ChatException,
    NoSuchNickException,
)
from frontends.gamespy.library.abstractions.client import ClientBase
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

    def _request_check(self) -> None:
        super()._request_check()
        assert self._client.brocker
        self._request.websocket_address = self._client.brocker.ip_port

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
    channel_name: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)

    def parse(self) -> None:
        super().parse()
        if self._cmd_params is None or len(self._cmd_params) < 1:
            raise ChatException("Channel name is missing.")
        self.channel_name = self._cmd_params[0]


class ChannelResponseBase(ResponseBase):
    def __init__(self, result: ResultBase) -> None:
        assert isinstance(result, ResultBase)
        super().__init__(result)

    @staticmethod
    def build_value_str(keys: list, kv: dict) -> str:
        v_str = ""
        for k in keys:
            v_str += "\\"
            if k in kv:
                v_str += kv[k]
        return v_str

    @staticmethod
    def build_key_value_str(kv: dict) -> str:
        kv_str = ""
        for k, v in kv.items():
            kv_str += f"\\{k}\\{v}"
        return kv_str


class ChannelHandlerBase(PostLoginHandlerBase):
    _request: ChannelRequestBase
    _response: ResponseBase
    _result: ResultBase
    _channel: ChatChannelCaches
    _is_broadcast: bool

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        """
        we handle message broadcasting in backend api
        frontends -> backends -> backends_api -> websocket broadcast. -> frontends.client.brocker.receive
        """
        self._is_broadcast = False

    def _response_send(self) -> None:
        if self._is_broadcast:
            # send message to backend websocket
            self.broadcast()
        else:
            super()._response_send()

    def broadcast(self):
        self._response.build()
        assert self._request.channel_name
        msg = BrockerMessage(
            server_id=self._client.server_config.server_id,
            channel_name=self._request.channel_name,
            sender_ip_address=self._client.connection.remote_ip,
            sender_port=self._client.connection.remote_port,
            message=self._response.sending_buffer,
        )
        assert self._client.brocker
        self._client.brocker.publish_message(msg.model_dump_json())
        self._client.log_network_broadcast(msg)


# region Message
class MessageRequestBase(ChannelRequestBase):
    type: MessageType
    nick_name: str
    message: str
    target_name: str

    def parse(self):
        super().parse()
        if self.channel_name is None:
            raise NoSuchNickException(
                "the channel name is missing from the request")
        if "#" in self.channel_name:
            self.type = MessageType.CHANNEL_MESSAGE
            self.target_name = self.channel_name
        else:
            if self._cmd_params is None or len(self._cmd_params) < 1:
                raise ChatException("cmd params is invalid")
            self.type = MessageType.USER_MESSAGE
            self.nick_name = self._cmd_params[0]
            self.target_name = self.nick_name
        if self._long_param is None:
            raise ChatException("long param is invalid")
        self.message = self._long_param


class MessageResultBase(ResultBase):
    sender_nick_name: str
    sender_user_name: str
    target_name: str
    message: str


class MessageHandlerBase(ChannelHandlerBase):
    _request: MessageRequestBase
    _result: MessageResultBase

    def __init__(self, client: ClientBase, request: MessageRequestBase):
        assert isinstance(request, MessageRequestBase)
        super().__init__(client, request)
        self._is_broadcast = True
