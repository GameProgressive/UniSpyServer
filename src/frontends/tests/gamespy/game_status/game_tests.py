

import unittest

import responses

from frontends.tests.gamespy.game_status.mock_objects import create_client


class GameTest(unittest.TestCase):
    @responses.activate
    def test_worm3d_20230331(self):
        raws1 = [
            "\\auth\\\\gamename\\worms3\\response\\bc3ca727a7825879eb9f13d9fd51bbb9\\port\\0\\id\\1\\final\\",
            "\\newgame\\\\connid\\0\\sesskey\\144562\\final\\",
            "\\authp\\\\pid\\1\\resp\\7b6658e99f448388fbeddc93654e6dd4\\lid\\2\\final\\",
            "\\setpd\\\\pid\\1\\ptype\\1\\dindex\\0\\kv\\1\\lid\\2\\length\\111\\data\\\\report\\|title||victories|0|timestamp|66613|league|Team17|winner||crc|-1|player_0|spyguy|ip_0||pid_0|0|auth_0|[00]\\final\\",
        ]
        client = create_client()
        for raw in raws1:
            client.on_received(raw.encode())

    @responses.activate
    def test_gmtest(self):
        raws = [
            "\\auth\\\\gamename\\crysis2\\response\\xxxxx\\port\\30\\id\\1\\final\\",
            "\\getpd\\\\pid\\0\\ptype\\0\\dindex\\1\\keys\\hello\x01hi\\lid\\1\\final\\",
            "\\getpid\\\\nick\\xiaojiuwo\\keyhash\\00000\\lid\\1\\final\\",
            "\\newgame\\\\connid\\123\\sesskey\\123456\\lid\\1\\final\\",
            "\\newgame\\\\connid\\123\\sesskey\\2020\\lid\\1\\final\\",
            "\\newgame\\\\connid\\123\\sesskey\\123456\\challenge\\123456789\\lid\\1\\final\\",
            "\\newgame\\\\connid\\123\\sesskey\\2020\\challenge\\123456789\\lid\\1\\final\\",
            "\\setpd\\\\pid\\123\\ptype\\0\\dindex\\1\\kv\\%d\\lid\\1\\length\\5\\data\\11\\lid\\1\\final\\",
            "\\updgame\\\\sesskey\\0\\done\\1\\gamedata\\hello\\lid\\1\\final\\",
            "\\updgame\\\\sesskey\\2020\\done\\1\\gamedata\\hello\\lid\\1\\final\\",
            "\\updgame\\\\sesskey\\2020\\connid\\1\\done\\1\\gamedata\\hello\\lid\\1\\final\\",
            "\\updgame\\\\sesskey\\0\\connid\\1\\done\\1\\gamedata\\hello\\lid\\1\\final\\"]
        client = create_client()
        for raw in raws:
            client.on_received(raw.encode())
