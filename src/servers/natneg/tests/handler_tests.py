import unittest

from library.src.abstractions.handler import CmdHandlerBase
from library.tests.mock_objects.general import create_mock_url
from servers.natneg.src.contracts.requests import InitRequest
from servers.natneg.src.handlers.handlers import AddressCheckHandler, ErtAckHandler, InitHandler, NatifyHandler
import responses
from servers.natneg.src.contracts.requests import (
    AddressCheckRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
    PreInitRequest,
)
from servers.natneg.src.enums.general import (
    NatClientIndex,
    NatPortType,
    PreInitState,
    RequestType,
)

from servers.natneg.tests.mock_objects import create_client
CmdHandlerBase._debug = True


class HandlerTests(unittest.TestCase):
    @responses.activate
    def test_init(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x00,
                0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip

        client = create_client()

        create_mock_url(client, InitHandler, {"message": "ok"})

        # test request parsing
        request = InitRequest(raw)
        request.parse()
        cookie = 151191552
        self.assertEqual(cookie, request.cookie)
        self.assertEqual(RequestType.INIT, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(3, request.version)
        self.assertEqual(NatPortType.NN1, request.port_type)
        handler = InitHandler(client, request)
        handler.handle()

        # test response constructing
        self.assertTrue(handler._response.sending_buffer ==
                        b'\xfd\xfc\x1efj\xb2\x03\x01\t\x03\x00\x00\x01\x00\xc0\xa8\x00\x01\x00\x00')

    @responses.activate
    def test_address_check(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03, 0x0a, 0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip

        request = AddressCheckRequest(raw)
        request.parse()
        self.assertEqual(151191552, request.cookie)
        self.assertEqual(RequestType.ADDRESS_CHECK, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(3, request.version)
        self.assertEqual(NatPortType.NN1, request.port_type)

        client = create_client()
        create_mock_url(client, AddressCheckHandler, {"message": "ok"})
        handler = AddressCheckHandler(client, request)
        handler.handle()

        self.assertTrue(handler._response.sending_buffer ==
                        b'\xfd\xfc\x1efj\xb2\x03\x0b\t\x03\x00\x00\x01\x00\xc0\xa8\x00\x01\x00\x00')

    @responses.activate
    def test_ert_ack(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x03,
                0x00, 0x00, 0x03, 0x09,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip
        request = ErtAckRequest(raw)
        request.parse()
        self.assertEqual(151191552, request.cookie)
        self.assertEqual(RequestType.ERT_ACK, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(3, request.version)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(NatPortType.NN1, request.port_type)
        client = create_client()
        create_mock_url(client, AddressCheckHandler, {"message": "ok"})

        handler = ErtAckHandler(client, request)
        handler.handle()
        self.assertTrue(handler._response.sending_buffer ==
                        b'\xfd\xfc\x1efj\xb2\x03\x03\t\x03\x00\x00\x01\x00\xc0\xa8\x00\x01\x00\x00')

    @responses.activate
    def test_natify(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x0c,
                0x00, 0x00, 0x03, 0x09,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip
        request = NatifyRequest(raw)
        request.parse()
        self.assertEqual(151191552, request.cookie)
        self.assertEqual(RequestType.NATIFY_REQUEST, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(3, request.version)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(NatPortType.NN1, request.port_type)
        client = create_client()
        create_mock_url(client, NatifyHandler, {"message": "ok"})

        handler = NatifyHandler(client, request)
        handler.handle()
        self.assertTrue(handler._response.sending_buffer ==
                        b'\xfd\xfc\x1efj\xb2\x03\x02\t\x03\x00\x00\x01\x00\xc0\xa8\x00\x01\x00\x00')

    @responses.activate
    def test_preinit(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x04, 0x0f, 0xb5, 0xe0, 0x95, 0x2a, 0x00, 0x24, 0x38, 0xb2, 0xb3, 0x5e
            ]
        )  # fmt: skip

        req = PreInitRequest(raw)
        req.parse()
        self.assertEqual(714465461, req.cookie)
        self.assertEqual(RequestType.PRE_INIT, req.command_name)
        self.assertEqual(4, req.version)
        self.assertEqual(NatPortType.GP, req.port_type)
        self.assertEqual(PreInitState.WAITING_FOR_CLIENT, req.state)


if __name__ == "__main__":
    unittest.main()
