# the total requests tests
import unittest

from backends.library.database.pg_orm import get_engine
import backends.protocols.gamespy.presence_search_player.handlers as bkh
import backends.protocols.gamespy.presence_search_player.requests as bkr
import frontends.gamespy.protocols.presence_search_player.contracts.requests as psp
from backends.tests.utils import add_headers
from frontends.tests.gamespy.presence_search_player.handler_tests import (
    CHECK1,
    NEWUSER,
    NICKS,
    SEARCH_1,
    SEARCH_2,
    SEARCH_3,
    SEARCH_4,
    SEARCH_UNIQUENICK,
    VALID,
)


class HandlerTest(unittest.TestCase):
    """
    test backend and server request compability
    """

    # region PCM
    # region PSP

    def test_search(self):
        for raw in [SEARCH_1, SEARCH_2, SEARCH_3, SEARCH_4]:
            r = psp.SearchRequest.gs_parse(raw)
            data = add_headers(r.model_dump())
            request = bkr.SearchRequest(**data)
            handler = bkh.SearchHandler(request)
            handler.handle()
            pass

    def test_chenck(self):
        r = psp.CheckRequest.gs_parse(CHECK1)
        data = add_headers(r.model_dump())
        request = bkr.CheckRequest(**data)
        handler = bkh.CheckHandler(request)
        handler.handle()
        pass

    def test_new_user(self):
        r = psp.NewUserRequest.gs_parse(NEWUSER)
        data = add_headers(r.model_dump())
        request = bkr.NewUserRequest(**data)
        handler = bkh.NewUserHandler(request)
        handler.handle()
        engine = get_engine()
        from sqlalchemy.orm.session import Session
        from backends.library.database.pg_orm import Users,Profiles,SubProfiles
        with Session(engine) as session:
            user = session.query(Users).where(Users.email == "xiaojiuwo@gamespy.com")
            profile = session.query(Profiles).where(Profiles.nick == "xiaojiuwo")
            subprofile = session.query(SubProfiles).where(SubProfiles.uniquenick == "xiaojiuwo")
            self.assertIsNotNone(user)
            self.assertIsNotNone(profile)
            self.assertIsNotNone(subprofile)
            subprofile.delete()
            profile.delete()
            user.delete()
            session.commit()
        pass

    def test_nick(self):
        r = psp.NicksRequest.gs_parse(NICKS)
        data = add_headers(r.model_dump())
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
        r = psp.SearchUniqueRequest.gs_parse(SEARCH_UNIQUENICK)
        data = add_headers(r.model_dump())
        request = bkr.SearchUniqueRequest(**data)
        handler = bkh.SearchUniqueHandler(request)
        handler.handle()
        pass

    def test_valid(self):
        r = psp.ValidRequest.gs_parse(VALID)
        data = add_headers(r.model_dump())
        request = bkr.ValidRequest(**data)
        handler = bkh.ValidHandler(request)
        handler.handle()
        self.assertTrue(handler._result.is_account_valid, True)
        pass
