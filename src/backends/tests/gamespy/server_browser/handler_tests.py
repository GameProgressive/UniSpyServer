import unittest
from backends.protocols.gamespy.server_browser.handlers import (
    ServerInfoHandler,
    ServerMainListHandler,
)
from backends.protocols.gamespy.server_browser.requests import (
    ServerInfoRequest,
    ServerListRequest,
)
from backends.tests.utils import add_headers
import frontends.gamespy.protocols.server_browser.v2.contracts.requests as fnt


class HandlerTests(unittest.TestCase):
    def test_server_main_list(self):
        raw = b"\x00\x9a\x00\x01\x03\x8fU\x00\x00anno1701\x00anno1701\x00D:@o)Okhgroupid is null\x00\\hostname\\gamemode\\gamever\\gametype\\password\\mapname\\numplayers\\numaiplayers\\openslots\\gamevariant\x00\x00\x00\x00\x04"
        r = fnt.ServerListRequest(raw)
        data = add_headers(r)
        request = ServerListRequest(**data)
        handler = ServerMainListHandler(request)
        handler.handle()
        pass

    def test_p2p_group_room_list(self):
        raise NotImplementedError()

    def test_server_network_info_list(self):
        raise NotImplementedError()

    def test_server_info(self):
        raw = b"\x00\t\x01\xc0\xa8z\xe2+g"
        r = fnt.ServerInfoRequest(raw)
        data = add_headers(r)
        request = ServerInfoRequest(**data)
        handler = ServerInfoHandler(request)
        handler.handle()
        pass
