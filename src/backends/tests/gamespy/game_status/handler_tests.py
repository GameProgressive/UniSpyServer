import unittest

from backends.protocols.gamespy.game_status.handlers import AuthGameHandler
from backends.protocols.gamespy.game_status.requests import AuthGameRequest
import backends.tests.gamespy.game_status.request_dumps as req_dp
from backends.tests.utils import add_headers


class HandlerTests(unittest.TestCase):
    def test_auth_game(self):
        request = add_headers(req_dp.test_auth_game_raw)
        req = AuthGameRequest(**request)
        h = AuthGameHandler(req)
        h.handle()
        pass
