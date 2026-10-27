import unittest
from frontends.gamespy.protocols.presence_search_player.contracts.responses import (
    CheckResponse,
    NewUserResponse,
    NicksResponse,
    OthersListResponse,
    OthersResponse,
)
from frontends.gamespy.protocols.presence_search_player.contracts.results import (
    CheckResult,
    NewUserResult,
    NickResultData,
    NicksResult,
    OthersListData,
    OthersListResult,
    OthersResult,
)


class ResponseTests(unittest.TestCase):
    def test_check(self):
        result1 = CheckResult(profile_id=1)
        resp1 = CheckResponse(result1)
        resp1.build()
        self.assertEqual(resp1.sending_buffer, "\\cur\\0\\pid\\1\\final\\")

        result2 = CheckResult(profile_id=None)
        resp2 = CheckResponse(result2)
        resp2.build()
        self.assertEqual(resp2.sending_buffer, "\\cur\\1\\final\\")

    def test_new_user(self):
        result = NewUserResult(user_id=1, profile_id=2)
        resp = NewUserResponse(result)
        resp.build()
        self.assertEqual(resp.sending_buffer, f"\\nur\\\\pid\\2\\final\\")

    def test_nicks(self):
        result1 = NicksResult(
            data=[NickResultData(nick="nick1", uniquenick="uniquenick1")],
            is_require_uniquenicks=True,
        )
        resp1 = NicksResponse(result1)
        resp1.build()
        self.assertEqual(
            resp1.sending_buffer,
            "\\nr\\nick\\nick1\\uniquenick\\uniquenick1\\ndone\\final\\",
        )

        result2 = NicksResult(
            data=[],
            is_require_uniquenicks=True,
        )
        resp2 = NicksResponse(result2)
        resp2.build()
        self.assertEqual(resp2.sending_buffer, "\\nr\\ndone\\final\\")

    def test_otherslist(self):
        result1 = OthersListResult(data=[])
        response1 = OthersListResponse(result1)
        response1.build()
        self.assertEqual(response1.sending_buffer, "\\otherslist\\\\oldone\\final\\")

        result2 = OthersListResult(
            data=[OthersListData(profile_id=1, unique_nick="spyguy")]
        )
        response2 = OthersListResponse(result2)
        response2.build()
        self.assertEqual(
            response2.sending_buffer,
            "\\otherslist\\\\o\\1\\uniquenick\\spyguy\\oldone\\final\\",
        )

    def test_others(self):
        result = OthersResult(data=[])
        response = OthersResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, "\\others\\\\odone\\final\\")
