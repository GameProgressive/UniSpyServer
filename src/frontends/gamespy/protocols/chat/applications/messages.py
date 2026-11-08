from frontends.gamespy.protocols.chat.applications.client import Client
from frontends.gamespy.protocols.chat.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.chat.abstractions.contract import IrcParam, RequestBase


class MessageBroadcastRequest(RequestBase):
    """
    this class is use to send broadcast message to all backend
    """
    irc: IrcParam | None = None
    target_name: str
    message: str


class MessageBroadcastHandler(CmdHandlerBase):
    _request: MessageBroadcastRequest

    def __init__(self, client: Client, request: MessageBroadcastRequest):
        assert isinstance(request, MessageBroadcastRequest)
        super().__init__(client, request)
