# the total requests tests
import unittest

from backends.library.database.pg_orm import PG_SESSION
from backends.tests.utils import add_headers
import frontends.gamespy.protocols.presence_search_player.contracts.requests as psp
from frontends.tests.gamespy.presence_search_player.handler_tests import (
    NICKS, SEARCH_1, SEARCH_2, SEARCH_3, SEARCH_4, CHECK1, NEWUSER, SEARCH_UNIQUENICK, VALID)
import backends.protocols.gamespy.presence_search_player.requests as bkr
import backends.protocols.gamespy.presence_search_player.handlers as bkh


class HandlerTest(unittest.TestCase):
    """
    test backend and server request compability
    """
    # region PCM
    # region PSP

    def test_search(self):
        for raw in [SEARCH_1, SEARCH_2, SEARCH_3, SEARCH_4]:
            r = psp.SearchRequest(raw)
            data = add_headers(r)
            request = bkr.SearchRequest(**data)
            handler = bkh.SearchHandler(request)
            handler.handle()
            pass

    def test_chenck(self):
        r = psp.CheckRequest(CHECK1)
        data = add_headers(r)
        request = bkr.CheckRequest(**data)
        handler = bkh.CheckHandler(request)
        handler.handle()
        pass

    def test_new_user(self):
        r = psp.NewUserRequest(NEWUSER)
        data = add_headers(r)
        request = bkr.NewUserRequest(**data)
        handler = bkh.NewUserHandler(request)
        handler.handle()
        pass

    def test_nick(self):
        r = psp.NicksRequest(NICKS)
        data = add_headers(r)
        request = bkr.NicksRequest(**data)
        handler = bkh.NicksHandler(request)
        handler.handle()
        pass

    def test_others_list(self):
        # r = psp.OthersListRequest()
        pass

    def test_others(self):
        pass

    def test_search_unique(self):
        r = psp.SearchUniqueRequest(SEARCH_UNIQUENICK)
        data = add_headers(r)
        request = bkr.SearchUniqueRequest(**data)
        handler = bkh.SearchUniqueHandler(request)
        handler.handle()
        pass

    def test_valid(self):
        r = psp.ValidRequest(VALID)
        data = add_headers(r)
        request = bkr.ValidRequest(**data)
        handler = bkh.ValidHandler(request)
        handler.handle()
        self.assertTrue(handler._result.is_account_valid, True)
        pass
