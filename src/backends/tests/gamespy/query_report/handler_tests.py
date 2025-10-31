import unittest

from backends.protocols.gamespy.query_report.handlers import HeartbeatHandler, AvaliableHandler, KeepAliveHandler
from backends.protocols.gamespy.query_report.requests import HeartBeatRequest, AvaliableRequest, KeepAliveRequest


class HandlerTests(unittest.IsolatedAsyncioTestCase):
    def test_heartbeat(self):
        request = {"server_id": "950b7638-a90d-469b-ac1f-861e63c8c613", "raw_request": "\\u0003\\\\xe5\\\\xcfaZlocalip0\\u0000172.19.0.5\\u0000localport\\u000011111\\u0000natneg\\u00001\\u0000statechanged\\u00003\\u0000gamename\\u0000gmtest\\u0000hostname\\u0000GameSpy QR2 Sample\\u0000gamever\\u00002.00\\u0000hostport\\u000025000\\u0000mapname\\u0000gmtmap1\\u0000gametype\\u0000arena\\u0000numplayers\\u00005\\u0000numteams\\u00002\\u0000maxplayers\\u000032\\u0000gamemode\\u0000openplaying\\u0000teamplay\\u00001\\u0000fraglimit\\u00000\\u0000timelimit\\u000040\\u0000gravity\\u0000800\\u0000rankingon\\u00001\\u0000\\u0000\\u0000\\u0005player_\\u0000score_\\u0000deaths_\\u0000ping_\\u0000team_\\u0000time_\\u0000\\u0000Joe Player\\u000030\\u000012\\u0000411\\u00000\\u000010\\u0000L33t 0n3\\u00000\\u00006\\u0000233\\u00001\\u0000325\\u0000Raptor\\u000015\\u000025\\u000063\\u00001\\u0000462\\u0000Gr81\\u00000\\u000016\\u0000294\\u00000\\u0000870\\u0000Flubber\\u000017\\u000012\\u0000232\\u00001\\u0000384\\u0000\\u0000\\u0002team_t\\u0000score_t\\u0000avgping_t\\u0000\\u0000Red\\u0000294\\u0000357\\u0000Blue\\u0000498\\u0000454\\u0000", "client_ip": "172.19.0.5", "client_port": 11111,
                   "instant_key": "3855573338", "command_name": 3, "server_data": {"localip0": "172.19.0.5", "localport": "11111", "natneg": "1", "statechanged": "3", "gamename": "gmtest", "hostname": "GameSpy QR2 Sample", "gamever": "2.00", "hostport": "25000", "mapname": "gmtmap1", "gametype": "arena", "numplayers": "5", "numteams": "2", "maxplayers": "32", "gamemode": "openplaying", "teamplay": "1", "fraglimit": "0", "timelimit": "40", "gravity": "800", "rankingon": "1"}, "player_data": [{"player_0": "Joe Player", "score_0": "30", "deaths_0": "12", "ping_0": "411", "team_0": "0", "time_0": "10"}, {"player_1": "L33t 0n3", "score_1": "0", "deaths_1": "6", "ping_1": "233", "team_1": "1", "time_1": "325"}, {"player_2": "Raptor", "score_2": "15", "deaths_2": "25", "ping_2": "63", "team_2": "1", "time_2": "462"}, {"player_3": "Gr81", "score_3": "0", "deaths_3": "16", "ping_3": "294", "team_3": "0", "time_3": "870"}, {"player_4": "Flubber", "score_4": "17", "deaths_4": "12", "ping_4": "232", "team_4": "1", "time_4": "384"}], "team_data": [{"team_t0": "Red", "score_t0": "294", "avgping_t0": "357"}, {"team_t1": "Blue", "score_t1": "498", "avgping_t1": "454"}], "server_status": 3, "group_id": None, "game_name": "gmtest"}
        req = HeartBeatRequest(**request)
        handler = HeartbeatHandler(req)
        handler.handle()
        handler.response

    def test_available(self):
        request = {"raw_request": "\\t\\u0000\\u0000\\u0000\\u0000\\t\\u0000\\u0000\\u0000\\u0000gamespy\\u0000", "command_name": 9,
                   "instant_key": "0", "client_ip": "127.0.0.1", "server_id": "a8893d8a-664e-4302-bb55-41b3a9229bd1", "client_port": "1234"}
        new_req = AvaliableRequest(**request)
        handler = AvaliableHandler(new_req)
        handler.handle()
        pass

    def test_keep_alive(self):
        request = {"raw_request": "\bg\\xd4\\xcbl", "command_name": 8, "instant_key": "1741998956",
                   "client_ip": "172.19.0.4", "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613", "client_port": 11111}
        new_req = KeepAliveRequest(**request)
        handler = KeepAliveHandler(new_req)
        handler.handle()
        self.assertIsNotNone(new_req.instant_key)
