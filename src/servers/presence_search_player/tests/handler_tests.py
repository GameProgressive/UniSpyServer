import unittest

import responses

from library.tests.mock_objects import create_mock_url
from servers.presence_search_player.src.contracts.requests import SearchRequest
from servers.presence_search_player.src.applications.handlers import SearchHandler
from servers.presence_search_player.tests.mock_objects import create_client
CHECK1 = "\\check\\\\nick\\spyguy\\email\\spyguy@gamespy.com\\pass\\0000\\final\\"

SEARCH_1 = "\\search\\\\sesskey\\xxxx\\profileid\\0\\namespaceid\\0\\uniquenick\\spyguy\\firstname\\spy\\lastname\\guy\\icquin\\123\\skip\\0\\gamename\\gmtest\\final\\"
SEARCH_2 = "\\search\\\\sesskey\\xxxx\\profileid\\0\\nick\\spyguy\\email\\spyguy@unispy.org\\firstname\\spy\\lastname\\guy\\icquin\\123\\skip\\0\\gamename\\gmtest\\final\\"
SEARCH_3 = "\\search\\\\sesskey\\xxxx\\profileid\\0\\nick\\spyguy\\firstname\\spy\\lastname\\guy\\icquin\\123\\skip\\0\\gamename\\gmtest\\final\\"
SEARCH_4 = "\\search\\\\sesskey\\xxxx\\profileid\\0\\email\\spyguy@unispy.org\\firstname\\spy\\lastname\\guy\\icquin\\123\\skip\\0\\gamename\\gmtest\\final\\"

SEARCH_UNIQUENICK = "\\searchunique\\\\sesskey\\xxxx\\profileid\\0\\uniquenick\\spyguy\\namespaces\\1,2,3,4,5\\gamename\\gmtest\\final\\"

VALID = "\\valid\\\\email\\spyguy@unispy.org\\partnerid\\1\\gamename\\gmtest\\final\\"

NICKS = "\\nicks\\\\email\\spyguy@unispy.org\\passenc\\xxxxx\\namespaceid\\0\\partnerid\\0\\gamename\\gmtest\\final\\"


PMATCH = "\\pmatch\\\\sesskey\\123456\\profileid\\0\\productid\\0\\final\\"

NEWUSER = "\\newuser\\\\nick\\xiaojiuwo\\email\\xiaojiuwo@gamespy.com\\passenc\\xxxx\\productID\\0\\namespaceid\\0\\uniquenick\\xiaojiuwo\\cdkey\\xxx-xxx-xxx-xxx\\partnerid\\0\\gamename\\gmtest\\final\\"

OTHER_BUDDY = "\\others\\\\sesskey\\123456\\profileid\\0\\namespaceid\\0\\gamename\\gmtest\\final\\"

OTHERS_BUDDY_LIST = "\\otherlist\\\\sesskey\\123456\\profileid\\0\\numopids\\2\\opids\\1|2\\namespaceid\\0\\gamename\\gmtest\\final\\"

SUGGEST_UNIQUE = "\\uniquesearch\\\\preferrednick\\xiaojiuwo\\namespaceid\\0\\gamename\\gmtest\\final\\"


class HandlerTests(unittest.TestCase):

    @responses.activate
    def test_profile(self):
        client = create_client()
        request = SearchRequest(SEARCH_1)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual("spyguy", request.uniquenick)
        self.assertEqual(0, request.profile_id)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual(0, request.skip_num)
        self.assertEqual("spy", request.firstname)
        self.assertEqual("guy", request.lastname)
        self.assertEqual("123", request.icquin)

        request = SearchRequest(SEARCH_2)
        request.parse()
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("spyguy@unispy.org", request.email)

        request = SearchRequest(SEARCH_3)
        request.parse()
        self.assertEqual("spyguy", request.nick)

        request = SearchRequest(SEARCH_4)
        request.parse()
        self.assertEqual("spyguy@unispy.org", request.email)

        handler = SearchHandler(client, request)
        handler.handle()
        self.assertEqual("\\bsr\\0\\nick\\spyguy\\uniquenick\\spyguy\\namespaceid\\0\\firstname\\spy\\lastname\\guy\\email\\spyguy@unispy.org\\bsrdone\\\\more\\0\\final\\",
                         handler._response.sending_buffer)


if __name__ == "__main__":
    unittest.main()
