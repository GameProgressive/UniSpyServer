from uuid import UUID
from pydantic import BaseModel
from frontends.gamespy.library.network.websocket_brocker import WebsocketMessage as WsMessage
from frontends.gamespy.protocols.query_report.v2.contracts.requests import ClientMessageRequest

class WebsocketMessage(WsMessage):
    message: ClientMessageRequest
