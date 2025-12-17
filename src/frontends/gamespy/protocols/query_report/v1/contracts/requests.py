from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import NESSESARY_KEYS
from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import ServerStatus


class HeartbeatPreRequest(RequestBase):
    """
    heartbeat do not have final command
    \\heartbeat\\26900\\gamename\\gmtest
    """
    game_port: int
    game_name: str
    status: ServerStatus

    def parse(self) -> None:
        super().parse()
        if 'gamename' not in self._request_dict:
            raise QRException("gamename is missing")
        self.game_name = self._request_dict['gamename']
        if 'heartbeat' not in self._request_dict:
            raise QRException("game port is missing")
        self.game_port = int(self._request_dict['heartbeat'])

        if 'statechanged' in self._request_dict:
            self.status = ServerStatus(int(self._request_dict['statechanged']))
        else:
            self.status = ServerStatus.START


class LegacyHeartbeatRequest(RequestBase):
    """
    heartbeat ack request is start with gamename:
    \\gamename\\gmtest\\gamever\\2.00\\location\\1\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\4\\maxplayers\\32\\gamemode\\openplaying\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\player_0\\Joe Player\\frags_0\\25\\deaths_0\\6\\skill_0\\272\\ping_0\\119\\team_0\\\\player_1\\L33t 0n3\\frags_1\\12\\deaths_1\\24\\skill_1\\136\\ping_1\\98\\team_1\\\\player_2\\Raptor\\frags_2\\11\\deaths_2\\26\\skill_2\\44\\ping_2\\445\\team_2\\Blue\\player_3\\Gr81\\frags_3\\23\\deaths_3\\18\\skill_3\\173\\ping_3\\129\\team_3\\Red\\final\\\\queryid\\2.1
    """
    query_id: str
    data: dict[str, str]
    game_name: str
    group_id: int | None

    def parse(self) -> None:
        super().parse()
        if "queryid" not in self._request_dict:
            raise QRException("queryid is missing")
        self.query_id = self._request_dict['queryid']
        for key in NESSESARY_KEYS:
            if key not in self._request_dict:
                raise QRException(f"key:<{key}> is missing")
        # currently we put basic, info, rules, players, teams into data
        self.data = self._request_dict.copy()
        self.game_name = self.data['gamename']
        if "groupid" in self.data:
            self.group_id = int(self.data["groupid"])
        else:
            self.group_id = None

if __name__ == "__main__":
    heartbeat = '\\heartbeat\\26900\\gamename\\gmtest'
    basic = '\\gamename\\gmtest\\gamever\\2.00\\location\\1\\queryid\\2.1'
    info = '\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\5\\maxplayers\\32\\gamemode\\openplaying\\queryid\\2.2'
    rules = '\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\queryid\\2.3'
    players = '\\player_0\\Joe Player\\frags_0\\3\\deaths_0\\6\\skill_0\\225\\ping_0\\137\\team_0\\Blue\\player_1\\L33t 0n3\\frags_1\\26\\deaths_1\\18\\skill_1\\829\\ping_1\\247\\team_1\\\\player_2\\Raptor\\frags_2\\30\\deaths_2\\17\\skill_2\\888\\ping_2\\233\\team_2\\Red\\player_3\\Gr81\\frags_3\\23\\deaths_3\\19\\skill_3\\415\\ping_3\\409\\team_3\\\\player_4\\Flubber\\frags_4\\23\\deaths_4\\31\\skill_4\\837\\ping_4\\443\\team_4\\\\final\\\\queryid\\2.4'

    status = "\\gamename\\gmtest\\gamever\\2.00\\location\\1\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\4\\maxplayers\\32\\gamemode\\openplaying\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\player_0\\Joe Player\\frags_0\\25\\deaths_0\\6\\skill_0\\272\\ping_0\\119\\team_0\\\\player_1\\L33t 0n3\\frags_1\\12\\deaths_1\\24\\skill_1\\136\\ping_1\\98\\team_1\\\\player_2\\Raptor\\frags_2\\11\\deaths_2\\26\\skill_2\\44\\ping_2\\445\\team_2\\Blue\\player_3\\Gr81\\frags_3\\23\\deaths_3\\18\\skill_3\\173\\ping_3\\129\\team_3\\Red\\final\\\\queryid\\2.1"
    req = LegacyHeartbeatRequest(status)
    req.parse()
