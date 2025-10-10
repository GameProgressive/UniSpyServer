from uuid import UUID

from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import (
    PeerRoomInfo,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
    AdHocResultBase,
    ResultBase,
    ServerListUpdateOptionResultBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import GameServerFlags


class ServerInfoResult(AdHocResultBase):
    pass


class P2PGroupRoomListResult(ServerListUpdateOptionResultBase):
    peer_room_info: list[PeerRoomInfo]


class ServerMainListResult(ServerListUpdateOptionResultBase):
    servers_info: list[GameServerInfo]
    flag: GameServerFlags = GameServerFlags.HAS_KEYS_FLAG


class ServerFullInfoListResult(ServerListUpdateOptionResultBase):
    servers_info: list[GameServerInfo]
    flag: GameServerFlags = GameServerFlags.HAS_FULL_RULES_FLAG


class SendMessageResult(ResultBase):
    sb_sender_id: UUID
    natneg_message: str
    server_info: GameServerInfo
