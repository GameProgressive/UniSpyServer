import unittest

import responses
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

    @responses.activate
    def test_anno1701_20220620(self):
        qr_raws = [
            # available request
            b'\t\x00\x00\x00\x00anno1701\x00',
            b"\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00statechanged\x003\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00377563076\x00numaiplayers\x00\x00openslots\x00\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00",
            b"\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00statechanged\x003\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00377563076\x00numaiplayers\x00\x00openslots\x00\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00",
            b'\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00statechanged\x001\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00109127620\x00numaiplayers\x000\x00openslots\x004\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00',
            b"\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00statechanged\x001\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00109127620\x00numaiplayers\x000\x00openslots\x004\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00",
            b"\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00109127620\x00numaiplayers\x000\x00openslots\x004\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00",
            b'\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00109127620\x00numaiplayers\x000\x00openslots\x004\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00',
            b"\x03\x1dU\xcc\xcalocalip0\x00192.168.0.80\x00localport\x0021701\x00natneg\x001\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00109127620\x00numaiplayers\x000\x00openslots\x004\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00anno1701_220\x000\x000\x00\x00\x01\x00",
            # client message
            b"\xfe\xfd\x03\x1dU\xcc\xcaTTT\x00\x00[+2\xba\x00\x00\x00\x00\xc5T\x00\x00",
            # keep alive
            b"\x08\x1dU\xcc\xca"
        ]
        sb_raws = [
            b"\x00\x9a\x00\x01\x03\x8fU\x00\x00anno1701\x00anno1701\x00D:@o)Okhgroupid is null\x00\\hostname\\gamemode\\gamever\\gametype\\password\\mapname\\numplayers\\numaiplayers\\openslots\\gamevariant\x00\x00\x00\x00\x04",
            b"\x00\x9a\x00\x01\x03\x8fU\x00\x00anno1701\x00anno1701\x00AHl='lhIgroupid is null\x00\\hostname\\gamemode\\gamever\\gametype\\password\\mapname\\numplayers\\numaiplayers\\openslots\\gamevariant\x00\x00\x00\x00\x04",
            b"\x00\x9a\x00\x01\x03\x8fU\x00\x00anno1701\x00anno1701\x00TsFhHjvQgroupid is null\x00\\hostname\\gamemode\\gamever\\gametype\\password\\mapname\\numplayers\\numaiplayers\\openslots\\gamevariant\x00\x00\x00\x00\x04",
            b"\x00\t\x01[+2\xbaT\xc5",
            b"\xfd\xfc\x1efj\xb2\x00\x00\x171"
        ]

        qr_client = create_client()
        for raw in qr_raws:
            qr_client.on_received(raw)

        sb_client = create_v2_client()
        for raw in sb_raws:
            sb_client.on_received(raw)

    def test_anno1701_20221104(self):
        qr_raw = {"qr1":
                  b"\t\x00\x00\x00\x00anno1701\x00",
                  "qr2": b"\x03\x98\x92%\xa0localip0\x00192.168.0.50\x00localip1\x00192.168.122.1\x00localport\x0021701\x00natneg\x001\x00statechanged\x003\x00gamename\x00anno1701\x00publicip\x000\x00publicport\x000\x00hostname\x00(unknown game)\x00gamemode\x00openstaging\x00numplayers\x001\x00maxplayers\x004\x00gamever\x0021903\x00mapname\x00Random map\x00gametype\x00Easy\x00password\x000\x00settings_options\x00369174468\x00numaiplayers\x00\x00openslots\x00\x00gamevariant\x00PvP\x00settings_winconditions\x000\x00settings_usercontent_mapname\x00\x00\x00\x00\x01player_\x00ping_\x00ping_\x00\x00sporesirius\x000\x000\x00\x00\x01\x00",
                  "sb1": b"\x00\x9a\x00\x01\x03\x8fU\x00\x00anno1701\x00anno1701\x00RcX;M({Ggroupid is null\x00\\hostname\\gamemode\\gamever\\gametype\\password\\mapname\\numplayers\\numaiplayers\\openslots\\gamevariant\x00\x00\x00\x00\x04",
                  "qr3": b"\x07\x98\x92%\xa0\x00\x00\x00\x00"
                  }

    @responses.activate
    def test_aarts_20230618(self):
        raw = b"\x00\xb8\x00\x01\x03\x00\x00\x00\x00aarts\x00aarts\x00F|Cy9!&w\x00\\hostname\\gamemode\\hostport\\hostname\\gamename\\gametype\\gamever\\mapname\\numplayers\\maxplayers\\gamemode\\password\\groupid\\mapsessiontype\\mapids\\internet\x00\x00\x00\x00\x04"
        client = create_v2_client()
        client.on_received(raw)
