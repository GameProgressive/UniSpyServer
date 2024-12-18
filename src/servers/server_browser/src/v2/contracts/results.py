from datetime import datetime
from uuid import UUID

from pydantic import BaseModel
from servers.query_report.src.aggregates.game_server_info import GameServerInfo
from servers.query_report.src.aggregates.peer_room_info import PeerRoomInfo
from servers.query_report.src.v2.aggregates.enums import GameServerStatus
from servers.server_browser.src.v2.abstractions.contracts import (
    AdHocResultBase,
    ResultBase,
    ServerListUpdateOptionResultBase,
)


class ServerInfoResult(AdHocResultBase):
    pass


class P2PGroupRoomListResult(ServerListUpdateOptionResultBase):
    peer_room_info: list[PeerRoomInfo]


class ServerMainListResult(ServerListUpdateOptionResultBase):
    servers_info: list[GameServerInfo]


class SendMessageResult(ResultBase):
    sb_sender_id: UUID
    natneg_message: str
    server_info: GameServerInfo
