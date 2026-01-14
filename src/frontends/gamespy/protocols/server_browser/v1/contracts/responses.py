from frontends.gamespy.protocols.server_browser.v1.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.server_browser.v1.contracts.results import GroupListResult, ServerListCompressResult, ServerInfoResult


class ServerInfoResponse(ResponseBase):
    _result: ServerInfoResult
    """
    \\fieldcount\\137\\ip\\AdminEMail\\AdminName\\backend_id\\balanceteams\\changelevels\\friendlyfire\\gamemode\\gamename\\gamerevision\\gamestyle\\gametype\\gamever\\goalteamscore\\hostname\\hostport\\listenserver\\location\\mapname\\maptitle\\maxplayers\\maxteams\\minnetver\\minplayers\\mutators\\numplayers\\password\\playersbalanceteams\\timelimit\\tournament\\wantworldlog\\worldlog\\botskill\\face_0\\face_1\\fraglimit\\frags_0\\frags_1\\mesh_0\\mesh_1\\ngsecret_0\\ngsecret_1\\ping_0\\ping_1\\player_0\\player_1\\skin_0\\skin_1\\team_0\\team_1\\face_2\\frags_2\\mesh_2\\ngsecret_2\\ping_2\\player_2\\skin_2\\team_2\\cwmode\\explositionff\\firstkill\\gameended\\gameperiod\\lastwinningteam\\outdated\\respawngame\\roundnumber\\tname_0\\tname_1\\tname_2\\tname_3\\tostver\\tscore_0\\tscore_1\\tscore_2\\tscore_3\\tsize_0\\tsize_1\\tsize_2\\tsize_3\\warhitbox\\face_3\\face_4\\face_5\\face_6\\face_7\\face_8\\face_9\\frags_3\\frags_4\\frags_5\\frags_6\\frags_7\\frags_8\\frags_9\\mesh_3\\mesh_4\\mesh_5\\mesh_6\\mesh_7\\mesh_8\\mesh_9\\ngsecret_3\\ngsecret_4\\ngsecret_5\\ngsecret_6\\ngsecret_7\\ngsecret_8\\ngsecret_9\\ping_3\\ping_4\\ping_5\\ping_6\\ping_7\\ping_8\\ping_9\\player_3\\player_4\\player_5\\player_6\\player_7\\player_8\\player_9\\skin_3\\skin_4\\skin_5\\skin_6\\skin_7\\skin_8\\skin_9\\team_3\\team_4\\team_5\\team_6\\team_7\\team_8\\team_9
    """

    def build(self) -> None:
        header = f"\\fieldcount\\{len(self._result.servers)}"
        server = self._result.servers[0]
        keys = "\\".join(server.data.keys())
        values = ""
        for s in self._result.servers:
            value = "\\".join(s.data.values())
            values += value
        self.sending_buffer = f"{header}\\{keys}\\{values}\\final\\"


class ServerListCompressResponse(ResponseBase):
    _result: ServerListCompressResult

    def build(self) -> None:
        # 6 bytes <serverip,serverport>
        buffer = bytearray()
        for server in self._result.servers:
            ip = server.host_ip_address_bytes
            port = server.query_report_port_bytes
            ip_port = ip + port
            buffer.extend(ip_port)


class GroupListResponse(ResponseBase):
    _result: GroupListResult
    """
    \\fieldcount\\8\\groupid\\hostname\\numplayers\\maxwaiting\\numwaiting\\numservers\\password\\other\\final\\
    """

    def build(self) -> None:
        header = f"\\fieldcount\\{len(self._result.peer_rooms)}"
        peer_room = self._result.peer_rooms[0]
        keys = "\\".join(peer_room.get_gamespy_dict().keys())
        values = ""
        for p in self._result.peer_rooms:
            value = "\\".join(p.get_gamespy_dict().values())
            values += value
        self.sending_buffer = f"{header}\\{keys}\\{values}\\final\\"
