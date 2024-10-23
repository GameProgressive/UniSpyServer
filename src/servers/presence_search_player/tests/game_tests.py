from typing import cast
import unittest

from library.src.extentions.password_encoder import process_password
from library.tests.mock_objects.general import create_mock_url
from servers.presence_search_player.src.contracts.requests import CheckRequest
from servers.presence_search_player.src.handlers.handlers import CheckHandler
from servers.presence_search_player.src.handlers.switcher import CmdSwitcher
import responses

from servers.presence_search_player.tests.mock_objects import create_client


class GameTest(unittest.TestCase):
    @responses.activate
    def test_check(self):
        raw = "\\check\\\\nick\\spyguy\\email\\spyguy@gamespy.com\\pass\\0000\\final\\"
        client = create_client()
        create_mock_url(client, CheckHandler, {"profile_id": 0})

        switcher = CmdSwitcher(client, raw)
        switcher.handle()
        request: CheckRequest = cast(
            CheckRequest, switcher._handlers[0]._request)
        response = switcher._handlers[0]._response
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("spyguy@gamespy.com", request.email)
        self.assertEqual("4a7d1ed414474e4033ac29ccb8653d9b", request.password)
        self.assertEqual("\\cur\\0\\pid\\0\\final\\", response.sending_buffer)


if __name__ == "__main__":
    unittest.main()
