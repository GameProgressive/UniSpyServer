import unittest

import frontends.gamespy.protocols.natneg.contracts.requests as fnt
from backends.protocols.gamespy.natneg.handlers import (
    ConnectHandler,
    InitHandler,
    ReportHandler,
)
from backends.protocols.gamespy.natneg.requests import (
    ConnectRequest,
    InitRequest,
    ReportRequest,
)
from backends.tests.utils import add_headers


class HandlerTests(unittest.TestCase):
    @unittest.skip("")
    def test_report(self):
        raw=b"\\xfd\\xfc\x1efj\\xb2\x04\r\x00\x00\x02\\x9a\x00\x00\x00\x00\x00\x00\x00\x01\x00\x00\x00gmtest\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00"
        r = fnt.ReportRequest.gs_parse(raw)
        data = r.model_dump(mode="json")
        req_dict = add_headers(data)
        request = ReportRequest.model_validate(req_dict)
        handler = ReportHandler(request)
        handler.handle()

    def test_connect(self):
        raw = b"\xfd\xfc\x1efj\xb2\x03\x00\x00\x00\x03\t\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00"
        r = fnt.ConnectRequest.gs_parse(raw)
        data = r.model_dump(mode="json")
        req_dict = add_headers(data)
        request = ConnectRequest(**req_dict)
        handler = ConnectHandler(request)
        handler.handle()

    def test_init(self):
        raw = b"\xfd\xfc\x1efj\xb2\x03\x00\x00\x00\x03\t\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00"
        r = fnt.InitRequest.gs_parse(raw)
        data = r.model_dump(mode="json")
        req_dict = add_headers(data)
        request = InitRequest(**req_dict)
        handler = InitHandler(request)
        handler.handle()
        from backends.library.database.pg_orm import get_engine,InitPacketCaches,Session,NatClientIndex
        with Session(get_engine()) as session:
            session.query(InitPacketCaches).where(InitPacketCaches.version == 3,InitPacketCaches.cookie == 777,InitPacketCaches.client_index == NatClientIndex.GAME_CLIENT).delete()
            session.commit()
