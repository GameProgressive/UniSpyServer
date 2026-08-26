import unittest

import responses

import backends.protocols.gamespy.presence_connection_manager.handlers as bkh
import backends.protocols.gamespy.presence_connection_manager.requests as bkr
import frontends.gamespy.protocols.presence_connection_manager.contracts.requests as pcm
from backends.tests.utils import add_headers
from frontends.tests.gamespy.presence_connection_manager.request_tests import (
    LOGIN_AUTH_TOKEN,
    LOGIN_UNIQUE_NICK,
    LOGIN_USER,
)


class HandlerTest(unittest.TestCase):
    # region General
    @responses.activate
    def test_login_authtoken(self):
        r = pcm.LoginRequest(LOGIN_AUTH_TOKEN)
        data = add_headers(r)
        request = bkr.LoginRequest(**data)
        handler = bkh.LoginHandler(request)
        handler.handle()
        pass

    @responses.activate
    def test_login_uniquenick(self):
        r = pcm.LoginRequest(LOGIN_UNIQUE_NICK)
        data = add_headers(r)
        request = bkr.LoginRequest(**data)
        handler = bkh.LoginHandler(request)
        handler.handle()
        pass

    @responses.activate
    def test_login_user(self):
        r = pcm.LoginRequest(LOGIN_USER)
        data = add_headers(r)
        request = bkr.LoginRequest(**data)
        handler = bkh.LoginHandler(request)
        handler.handle()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_new_user(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_logout(self):
        raise NotImplementedError()

    # region Buddy

    @unittest.skip("not implemented")
    @responses.activate
    def test_buddy_list(self):
        r = pcm.BuddyListRetriveRequest(profile_id=1, namespace_id=0, operation_id=1)
        data = add_headers(r)
        request = bkr.BuddyListRetriveRequest(**data)
        handler = bkh.BuddyListRetriveHandler(request)
        handler.handle()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_block_list(self):
        r = pcm.BlockListRetriveRequest(profile_id=1, namespace_id=0, operation_id=1)
        data = add_headers(r)
        request = bkr.BlockListRetriveRequest(**data)
        handler = bkh.BlockListRetriveHandler(request)
        handler.handle()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_add_buddy(self):
        # r = pcm.AddBuddyRequest()
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_del_buddy(self):
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_add_block(self):
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_del_block(self):
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_invite_to(self):
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_status_info(self):
        raise NotImplementedError()
        pass

    @unittest.skip("not implemented")
    @responses.activate
    def test_statue(self):
        raise NotImplementedError()
        pass

    # region Profile
    @unittest.skip("not implemented")
    @responses.activate
    def test_get_profile(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_new_profile(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_register_cdkey(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_register_nick(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_update_profile(self):
        raise NotImplementedError()

    @unittest.skip("not implemented")
    @responses.activate
    def test_update_user_info(self):
        raise NotImplementedError()
