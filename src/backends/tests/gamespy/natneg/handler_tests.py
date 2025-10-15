import unittest
from backends.protocols.gamespy.natneg.handlers import ConnectHandler, InitHandler, ReportHandler
from backends.protocols.gamespy.natneg.requests import InitRequest, ConnectRequest, ReportRequest
from backends.tests.utils import add_headers
import frontends.gamespy.protocols.natneg.contracts.requests as fnt


class HandlerTests(unittest.TestCase):

    @unittest.skip("")
    def test_report(self):
        req_dict = {"raw_request": "\\xfd\\xfc\u001efj\\xb2\u0004\r\u0000\u0000\u0002\\x9a\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0001\u0000\u0000\u0000gmtest\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000",
                    "version": 4, "command_name": 13, "cookie": 666, "port_type": 0, "client_index": 0, "use_game_port": False, "is_nat_success": False, "nat_type": 0, "mapping_scheme": 0, "game_name": "gmtest", "client_ip": "172.19.0.5", "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613", "client_port": 36229}
        req = ReportRequest.model_validate(req_dict)
        handler = ReportHandler(req)
        handler.handle()
        pass

    def test_connect(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x00,
                0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt:skip
        r = fnt.ConnectRequest(raw)
        data = add_headers(r)
        data["raw_request"] = data["raw_request"].decode(
            "ascii", "backslashreplace")
        request = ConnectRequest(**data)
        handler = ConnectHandler(request)
        handler.handle()
        pass

    def test_init(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x00,
                0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt:skip
        r = fnt.InitRequest(raw)
        data = add_headers(r)
        data["raw_request"] = data["raw_request"].decode(
            "ascii", "backslashreplace")
        request = InitRequest(**data)
        handler = InitHandler(request)
        handler.handle()
        pass
