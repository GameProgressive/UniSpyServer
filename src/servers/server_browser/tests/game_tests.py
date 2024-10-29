import unittest

import responses
# from servers.server_browser.tests.mock_objects import create_v2_client
from servers.query_report.tests.mock_objects import create_client
from servers.server_browser.tests.mock_objects import create_v2_client


class GameTest(unittest.TestCase):
    @responses.activate
    def test_gmtest_20200309(self):
        qr_raw = b'\x03\xea+\xafPlocalip0\x00192.168.122.226\x00localport\x0011111\x00natneg\x001\x00statechanged\x003\x00gamename\x00gmtest\x00hostname\x00GameSpy QR2 Sample\x00gamever\x002.00\x00hostport\x0025000\x00mapname\x00gmtmap1\x00gametype\x00arena\x00numplayers\x0010\x00numteams\x002\x00maxplayers\x0032\x00gamemode\x00openplaying\x00teamplay\x001\x00fraglimit\x000\x00timelimit\x0040\x00gravity\x00800\x00rankingon\x001\x00\x00\x00\nplayer_\x00score_\x00deaths_\x00ping_\x00team_\x00time_\x00\x00Joe Player\x004\x002\x0077\x000\x00185\x00L33t 0n3\x006\x0024\x0068\x001\x00820\x00Raptor\x0010\x0029\x00216\x001\x00664\x00Gr81\x008\x006\x00327\x001\x00697\x00Flubber\x0015\x002\x00179\x000\x0048\x00Sarge\x009\x0012\x00337\x000\x00296\x00Void\x0027\x0029\x0045\x000\x00355\x00runaway\x0024\x004\x00197\x001\x00428\x00Ph3ar\x0030\x0030\x00339\x001\x00525\x00wh00t\x0031\x0028\x00269\x001\x0077\x00\x00\x02team_t\x00score_t\x00avgping_t\x00\x00Red\x00487\x00336\x00Blue\x0082\x00458\x00'
        qr_client = create_client()
        qr_client.on_received(qr_raw)
        sb_raw = b'\x00\t\x01\xc0\xa8z\xe2+g'
        sb_client = create_v2_client()
        sb_client.on_received(sb_raw)
