from frontends.gamespy.protocols.query_report.aggregates.game_server_info import GameServerInfo
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import PeerRoomInfo
from frontends.gamespy.protocols.server_browser.v1.abstractions.contracts import ResultBase


class ServerInfoResult(ResultBase):
    servers: list[GameServerInfo]


class ServerListCompressResult(ResultBase):
    servers: list[GameServerInfo]


class GroupListResult(ResultBase):
    peer_rooms: list[PeerRoomInfo]
