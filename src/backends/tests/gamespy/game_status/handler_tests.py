import unittest

from backends.protocols.gamespy.game_status.handlers import AuthGameHandler
from backends.protocols.gamespy.game_status.requests import AuthGameRequest


class HandlerTests(unittest.TestCase):
    def test_auth_game(self):
        raw = {
            "raw_request": "\\auth\\\\gamename\\gmtest\\response\\b7f8b7f83dcc4427c35864c5d53c5fe5\\port\\2667\\id\\1\\final\\",
            "request_dict": {
                "auth": "",
                "gamename": "gmtest",
                "response": "b7f8b7f83dcc4427c35864c5d53c5fe5",
                "port": "2667",
                "id": "1",
            },
            "local_id": 1,
            "game_name": "gmtest",
            "response": "b7f8b7f83dcc4427c35864c5d53c5fe5",
            "port": 2667,
            "client_ip": "172.19.0.5",
            "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
            "client_port": 38996,
        }
        req = AuthGameRequest(**raw)
        h = AuthGameHandler(req)
        h.handle()
        pass
