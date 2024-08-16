import unittest

from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import create_mock_url
from servers.natneg.src.contracts.requests import InitRequest
from servers.natneg.src.handlers.handlers import AddressCheckHandler, InitHandler
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

from servers.natneg.tests.mock_objects import ClientMock, create_client


class HandlerTests(unittest.TestCase):
    @responses.activate
    def init_test(self):
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
    def address_check_test(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03, 0x0a, 0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip

        request = AddressCheckRequest(raw)
        request.parse()
        cookie = 151191552
        self.assertEqual(cookie, request.cookie)
        self.assertEqual(RequestType.ADDRESS_CHECK, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(3, request.version)
        self.assertEqual(NatPortType.NN1, request.port_type)

        client = create_client()
        create_mock_url(client, AddressCheckHandler,{"message": "ok"})
        handler = AddressCheckHandler(client, request)
        handler.handle()

        self.assertTrue(handler._response.sending_buffer == b'')


if __name__ == "__main__":
    test = HandlerTests()
    test.init_test()
