import unittest
import responses

from frontends.gamespy.protocols.game_status.aggregations.gscrypt import GSCrypt
from frontends.gamespy.protocols.game_status.contracts.requests import (
    AuthGameRequest,
    AuthPlayerRequest,
    GetPlayerDataRequest,
    GetProfileIdRequest,
    NewGameRequest,
    SetPlayerDataRequest,
    UpdateGameRequest,
)
from frontends.gamespy.protocols.game_status.aggregations.enums import (
    PersistStorageType,
)
from frontends.gamespy.protocols.game_status.applications.handlers import (
    AuthPlayerHandler,
    SetPlayerDataHandler,
    UpdateGameHandler,
)
from frontends.tests.gamespy.game_status.mock_objects import create_client


class HandlerTests(unittest.TestCase):
    @responses.activate
    # @unittest.skip("not implemented")
    def test_set_player_data_20230329(self):
        raw = "\\setpd\\\\pid\\1\\ptype\\1\\dindex\\0\\kv\\1\\lid\\2\\length\\111\\data\\\\report\\|title||victories|0|timestamp|37155|league|Team17|winner||crc|-1|player_0|spyguy|ip_0||pid_0|0|auth_0|[00]\\final\\"
        client = create_client()
        request = SetPlayerDataRequest(raw)
        request.parse()
        self.assertEqual(1, request.profile_id)
        self.assertEqual(PersistStorageType.PRIVATE_READ_WRITE, request.storage_type)
        self.assertEqual(0, request.data_index)
        self.assertEqual("", request.data)
        self.assertEqual(111, request.length)
        self.assertEqual(
            "|title||victories|0|timestamp|37155|league|Team17|winner||crc|-1|player_0|spyguy|ip_0||pid_0|0|auth_0|[00]",
            request.report,
        )

        handler = SetPlayerDataHandler(client, request)
        handler.handle()

    @responses.activate
    # @unittest.skip("not implemented")
    def test_gamespysdk_update_game_20230329(self):
        raw1 = "\\updgame\\\\sesskey\\20298203\\connid\\0\\done\\0\\gamedata\\\u0001hostname\u0001My l33t Server\u0001mapname\u0001Level 33\u0001gametype\u0001hunter\u0001gamever\u00011.230000\u0001player_0\u0001Bob!\u0001points_0\u00014\u0001deaths_0\u00012\u0001pid_0\u000132432423\u0001auth_0\u00017cca8e60a13781eebc820a50754f57cd\u0001player_1\u0001Joey\u0001points_1\u00012\u0001deaths_1\u00014\u0001pid_1\u0001643423\u0001auth_1\u000119ea14d9d92a7fcc635cf5716944d9bc\\final\\"
        raw2 = "\\updgame\\\\sesskey\\20298203\\connid\\0\\done\\1\\gamedata\\\u0001hostname\u0001My l33t Server\u0001mapname\u0001Level 33\u0001gametype\u0001hunter\u0001gamever\u00011.230000\u0001player_0\u0001Bob!\u0001points_0\u00016\u0001deaths_0\u00013\u0001pid_0\u000132432423\u0001auth_0\u00017cca8e60a13781eebc820a50754f57cd\u0001player_1\u0001Joey\u0001points_1\u00013\u0001deaths_1\u00016\u0001pid_1\u0001643423\u0001auth_1\u000119ea14d9d92a7fcc635cf5716944d9bc\\final\\"
        client = create_client()
        request = UpdateGameRequest(raw1)
        handler = UpdateGameHandler(client, request)
        handler.handle()
        self.assertEqual("20298203", request.session_key)
        self.assertEqual(0, request.connection_id)
        self.assertEqual(False, request.is_done)
        self.assertEqual(
            "\u0001hostname\u0001My l33t Server\u0001mapname\u0001Level 33\u0001gametype\u0001hunter\u0001gamever\u00011.230000\u0001player_0\u0001Bob!\u0001points_0\u00014\u0001deaths_0\u00012\u0001pid_0\u000132432423\u0001auth_0\u00017cca8e60a13781eebc820a50754f57cd\u0001player_1\u0001Joey\u0001points_1\u00012\u0001deaths_1\u00014\u0001pid_1\u0001643423\u0001auth_1\u000119ea14d9d92a7fcc635cf5716944d9bc",
            request.game_data,
        )
        request = UpdateGameRequest(raw2)
        handler = UpdateGameHandler(client, request)
        handler.handle()

    @responses.activate
    @unittest.skip("Encrypted request is not correct")
    def test_worm3d_auth_player(self):
        raw = b"2\\x0F\\x16\\x10]%+=veKaB3a(UC`b$\\x1CO\\x11VZX\\x09w\\x1Cu\\x08L@\\x13=X!\\x1E{\\x0EL\\x1DLf[qN \\x04G\\x130[#N'\\x09(IC`b$\\final\\"
        plaintext = GSCrypt().decrypt(raw)
        request = AuthPlayerRequest(plaintext)
        client = create_client()
        handler = AuthPlayerHandler(client, request)
        handler.handle()

        # self.assertEqual()

    def test_auth(self):
        raw = "\\auth\\\\gamename\\crysis2\\response\\xxxxx\\port\\30\\id\\1\\final\\"
        request = AuthGameRequest(raw)
        request.parse()
        self.assertEqual("crysis2", request.game_name)
        self.assertEqual(30, request.port)
        self.assertEqual(1, request.local_id)

    def test_get_player_data(self):
        raw = (
            "\\getpd\\\\pid\\0\\ptype\\0\\dindex\\1\\keys\\hello\x01hi\\lid\\1\\final\\"
        )

        request = GetPlayerDataRequest(raw)
        request.parse()
        self.assertEqual(0, request.profile_id)
        self.assertEqual(PersistStorageType.PRIVATE_READ_ONLY, request.storage_type)
        self.assertEqual(1, request.data_index)
        self.assertEqual(2, len(request.keys))
        self.assertEqual("hello", request.keys[0])
        self.assertEqual("hi", request.keys[1])

    def test_get_profile_id(self):
        raw = "\\getpid\\\\nick\\xiaojiuwo\\keyhash\\00000\\lid\\1\\final\\"
        request = GetProfileIdRequest(raw)
        request.parse()
        self.assertEqual("xiaojiuwo", request.nick)
        self.assertEqual("00000", request.keyhash)
        self.assertEqual(1, request.local_id)

    def test_new_game(self):
        raw1 = "\\newgame\\\\connid\\123\\sesskey\\123456\\lid\\1\\final\\"
        request1 = NewGameRequest(raw1)
        request1.parse()

        self.assertEqual(123, request1.connection_id)
        self.assertEqual("123456", request1.session_key)
        self.assertEqual(1, request1.local_id)

        raw2 = "\\newgame\\\\connid\\123\\sesskey\\123456\\challenge\\123456789\\lid\\1\\final\\"
        request2 = NewGameRequest(raw2)
        request2.parse()
        self.assertEqual(123, request2.connection_id)
        self.assertEqual("123456", request2.session_key)
        self.assertEqual("123456789", request2.challenge)
        self.assertEqual(1, request2.local_id)

    def test_update_game(self):
        raw1 = "\\updgame\\\\sesskey\\0\\done\\1\\gamedata\\hello\\lid\\1\\final\\"
        request1 = UpdateGameRequest(raw1)
        request1.parse()
        self.assertEqual("0", request1.session_key)
        self.assertEqual(True, request1.is_done)
        self.assertEqual("hello", request1.game_data)
        self.assertEqual(None, request1.connection_id)
        raw2 = "\\updgame\\\\sesskey\\0\\connid\\1\\done\\1\\gamedata\\hello\\lid\\1\\final\\"
        request2 = UpdateGameRequest(raw2)
        request2.parse()
        self.assertEqual("0", request2.session_key)
        self.assertEqual(True, request2.is_done)
        self.assertEqual("hello", request2.game_data)
        self.assertEqual(1, request2.connection_id)


if __name__ == "__main__":
    unittest.main()
