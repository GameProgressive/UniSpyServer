from servers.query_report.aggregates.game_server_info import GameServerInfo
from servers.query_report.aggregates.peer_room_info import PeerRoomInfo
from servers.server_browser.v2.abstractions.contracts import (
    ResultBase,
    ServerListUpdateOptionResultBase,
)


class AdHocResult(ResultBase):
    game_server_info: GameServerInfo


class P2PGroupRoomListResult(ResultBase):
    peer_room_infos: list[PeerRoomInfo]


class ServerMainListResult(ServerListUpdateOptionResultBase):
    servers_info: list[GameServerInfo] = []
