from pydantic import BaseModel
from frontends.gamespy.library.network.websocket_brocker import WebsocketMessage as WsMessage


class ChannelMessage(BaseModel):
    target_name: str
    message: str


class WebsocketMessage(WsMessage):
    message: ChannelMessage
