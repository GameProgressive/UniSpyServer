import unittest

import responses
from frontends.tests.gamespy.query_report.mock_objects import create_client


class GameTests(unittest.TestCase):
    @responses.activate
    def test_faltout2(self):
        raw = b'\x03\xc2\x94\x1c\xe7localip0\x00192.168.0.50\x00localip1\x00172.16.74.1\x00localip2\x00172.17.0.1\x00localip3\x00192.168.122.1\x00localip4\x00172.16.65.1\x00localport\x0023756\x00natneg\x001\x00statechanged\x001\x00gamename\x00flatout2pc\x00publicip\x000\x00publicport\x000\x00hostkey\x00-820966322\x00hostname\x00Spore\x00gamever\x00FO14\x00gametype\x00race\x00gamevariant\x00normal_race\x00gamemode\x00openwaiting\x00numplayers\x001\x00maxplayers\x008\x00mapname\x00Timberlands_1\x00timelimit\x000\x00password\x000\x00car_type\x000\x00car_class\x000\x00races_p\x00100\x00derbies_p\x000\x00stunts_p\x000\x00normal_race_p\x00100\x00pong_race_p\x000\x00wreck_derby_p\x000\x00survivor_derby_p\x000\x00frag_derby_p\x000\x00tag_p\x000\x00upgrades\x002\x00nitro_regeneration\x002\x00damage_level\x002\x00derby_damage_level\x001\x00next_race_type\x00normal_race\x00laps_or_timelimit\x004\x00num_races\x001\x00num_derbies\x000\x00num_stunts\x000\x00datachecksum\x003546d58093237eb33b2a96bb813370d846ffcec8\x00\x00\x00\x00\x00\x00\x00\x00'
        client = create_client()
        client.on_received(raw)

    @responses.activate
    def test_worm3d(self):
        raw = b'\x03Q]\xa0\xe8localip0\x00192.168.0.60\x00localport\x006500\x00natneg\x001\x00statechanged\x003\x00gamename\x00worms3\x00hostname\x00test\x00gamemode\x00openstaging\x00groupid\x00622\x00numplayers\x001\x00maxplayers\x002\x00hostname\x00test\x00hostport\x00\x00maxplayers\x002\x00numplayers\x001\x00SchemeChanging\x000\x00gamever\x001073\x00gametype\x00\x00mapname\x00\x00firewall\x000\x00publicip\x00255.255.255.255\x00privateip\x00192.168.0.60\x00gamemode\x00openstaging\x00val\x000\x00password\x000\x00\x00\x00\x01player_\x00ping_\x00hostname\x00hostport\x00maxplayers\x00numplayers\x00SchemeChanging\x00gamever\x00gametype\x00mapname\x00firewall\x00publicip\x00privateip\x00gamemode\x00val\x00password\x00\x00worms10\x000\x00\x00\x00\x00\x00\x001073\x00\x00\x001\x00255.255.255.255\x00192.168.0.60\x00\x000\x00\x00\x00\x00hostname\x00hostport\x00maxplayers\x00numplayers\x00SchemeChanging\x00gamever\x00gametype\x00mapname\x00firewall\x00publicip\x00privateip\x00gamemode\x00val\x00password\x00\x00'
        client = create_client()
        client.on_received(raw)
