from typing import cast

from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.protocols.chat.abstractions.contract import (
    ChatChannelCaches,
    RequestBase,
    ResponseBase,
    ResultBase,
)
from frontends.gamespy.protocols.chat.aggregates.enums import MessageType
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    EXCEPTIONS,
    ChatException,
    IRCException,
    NoSuchNickException,
)
from frontends.gamespy.protocols.chat.applications.client import Client
import frontends.gamespy.library.abstractions.handler as lib


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client
    _request: RequestBase
    _result: ResultBase

    def __init__(self, client: ClientBase, request: RequestBase):
        self._exceptions_mapping = EXCEPTIONS
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
    channel_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ChannelRequestBase":
        data = RequestBase.gs_parse(raw_request)
        if len(data.irc.params) < 1:
            raise ChatException("Channel name is missing.")
        channel_name = data.irc.params[0]
        data = ChannelRequestBase(
            **data.model_dump(), channel_name=channel_name)
        return data


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

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        """
        we handle message broadcasting in backend api
        client -> frontends -> backends -> backends_api -> websocket broadcast. -> frontends.client.brocker.receive -> clients
        """


# region Message
class ChannelMessageRequestBase(ChannelRequestBase):
    type: MessageType
    message: str
    target_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ChannelMessageRequestBase":
        data = ChannelRequestBase.gs_parse(raw_request)
        message = data.irc.trailing
        if "#" in data.channel_name:
            target_name = data.channel_name
            msg_type = MessageType.CHANNEL_MESSAGE
        else:
            if len(data.irc.params) < 1:
                raise ChatException("cmd params is invalid")

            msg_type = MessageType.USER_MESSAGE
            target_name = data.irc.params[0]
        data = ChannelMessageRequestBase(
            **data.model_dump(), type=msg_type, target_name=target_name, message=message
        )
        return data


class ChannelMessageResultBase(ResultBase):
    sender_nick_name: str
    sender_user_name: str
    target_name: str
    message: str


class ChannelMessageHandlerBase(ChannelHandlerBase):
    _request: ChannelMessageRequestBase
    _result: ChannelMessageResultBase

    def __init__(self, client: Client, request: ChannelMessageRequestBase):
        assert isinstance(request, ChannelMessageRequestBase)
        super().__init__(client, request)
