# the total requests tests
import unittest

import frontends.gamespy.protocols.presence_search_player.contracts.requests as psp
from frontends.tests.gamespy.presence_search_player.handler_tests import (
    NICKS, SEARCH_1, SEARCH_2, SEARCH_3, SEARCH_4, CHECK1, NEWUSER, SEARCH_UNIQUENICK, SUGGEST_UNIQUE, VALID)
import backends.protocols.gamespy.presence_search_player.requests as bk


def add_nessesary_info(request) -> dict:
    request.parse()
    data = request.to_dict()
    data["client_ip"] = "192.168.0.1"
    data["server_id"] = "950b7638-a90d-469b-ac1f-861e63c8c613"
    data["client_port"] = 1234
    return data


class RequestsCompatibleTests(unittest.TestCase):
    """
    test backend and server request compability
    """
    # region PCM
    # region PSP

    def test_search(self):
        for raw in [SEARCH_1, SEARCH_2, SEARCH_3, SEARCH_4]:
            r = psp.SearchRequest(raw)
            data = add_nessesary_info(r)
            bk.SearchRequest(**data)

    def test_chenck(self):
        r = psp.CheckRequest(CHECK1)
        data = add_nessesary_info(r)
        bk.CheckRequest(**data)
        pass

    def test_new_user(self):
        pass
        r = psp.NewUserRequest(NEWUSER)
        data = add_nessesary_info(r)
        bk.NewUserRequest(**data)

    def test_nick(self):
        r = psp.NicksRequest(NICKS)
        data = add_nessesary_info(r)
        bk.NicksRequest(**data)
        pass

    def test_others_list(self):
        # r = psp.OthersListRequest()
        pass

    def test_others(self):
        pass

    def test_search_unique(self):
        r = psp.SearchUniqueRequest(SEARCH_UNIQUENICK)
        data = add_nessesary_info(r)
        bk.SearchUniqueRequest(**data)
        pass

    def test_valid(self):
        r = psp.ValidRequest(VALID)
        data = add_nessesary_info(r)
        bk.ValidRequest(**data)
        pass
