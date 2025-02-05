from typing import TYPE_CHECKING, cast
import unittest

from frontends.gamespy.protocols.presence_search_player.contracts.requests import CheckRequest
from frontends.gamespy.protocols.presence_search_player.applications.handlers import CheckHandler
from frontends.gamespy.protocols.presence_search_player.applications.switcher import CmdSwitcher
import responses

from frontends.gamespy.protocols.presence_search_player.contracts.responses import CheckResponse
from frontends.tests.gamespy.presence_search_player.mock_objects import create_client


class GameTest(unittest.TestCase):
    @responses.activate
    def test_check(self):
        raw = "\\check\\\\nick\\spyguy\\email\\spyguy@gamespy.com\\pass\\0000\\final\\"
        client = create_client()

        switcher = CmdSwitcher(client, raw)
        switcher.handle()
        request = switcher._handlers[0]._request
        if TYPE_CHECKING:
            request = cast(CheckRequest, request)
        response = switcher._handlers[0]._response
        if TYPE_CHECKING:
            response = cast(CheckResponse, response)
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("spyguy@gamespy.com", request.email)
        self.assertEqual("4a7d1ed414474e4033ac29ccb8653d9b", request.password)
        self.assertEqual("\\cur\\0\\pid\\0\\final\\", response.sending_buffer)


if __name__ == "__main__":
    unittest.main()
