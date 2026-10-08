import unittest

import responses

from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortType,
    PreInitState,
    CommandName,
)
from frontends.gamespy.protocols.natneg.applications.handlers import (
    AddressCheckHandler,
    ErtAckHandler,
    InitHandler,
    NatifyHandler,
)
from frontends.gamespy.protocols.natneg.contracts.requests import (
    AddressCheckRequest,
    ErtAckRequest,
    InitRequest,
    NatifyRequest,
    PreInitRequest,
)
from frontends.tests.gamespy.natneg.mock_objects import create_client

CONFIG.unittest.is_raise_except = True


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

        # test request parsing
        data = InitRequest.gs_parse(raw)
        request = InitRequest.model_validate(data)
        self.assertEqual(777, request.cookie)
        self.assertEqual(CommandName.INIT, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(3, request.version)
        self.assertEqual(NatPortType.NN1, request.port_type)
        handler = InitHandler(client, request)
        handler.handle()

        # test response constructing
        self.assertTrue(
            handler._response.sending_buffer
            == b"\xfd\xfc\x1efj\xb2\x03\x01\x00\x00\x03\t\x01\x00\x00\xc0\xa8\x00\x01\x00\x00"
        )

    @responses.activate
    def test_address_check(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03, 0x0a, 0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip

        request = AddressCheckRequest.gs_parse(raw)
        self.assertEqual(777, request.cookie)
        self.assertEqual(CommandName.ADDRESS_CHECK, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(3, request.version)
        self.assertEqual(NatPortType.NN1, request.port_type)

        client = create_client()
        handler = AddressCheckHandler(client, request)
        handler.handle()

        self.assertTrue(
            handler._response.sending_buffer
            == b"\xfd\xfc\x1efj\xb2\x03\x0b\x00\x00\x03\t\x01\x00\x00\xc0\xa8\x00\x01\x00\x00"
        )

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
        data = ErtAckRequest.gs_parse(raw)
        request = ErtAckRequest.model_validate(data)
        self.assertEqual(777, request.cookie)
        self.assertEqual(CommandName.ERT_ACK, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(3, request.version)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(NatPortType.NN1, request.port_type)
        client = create_client()

        handler = ErtAckHandler(client, request)
        handler.handle()
        self.assertTrue(
            handler._response.sending_buffer
            == b"\xfd\xfc\x1efj\xb2\x03\x03\x00\x00\x03\t\x01\x00\x00\xc0\xa8\x00\x01\x00\x00"
        )

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
        date = NatifyRequest.gs_parse(raw)
        request = NatifyRequest.model_validate(date)
        self.assertEqual(777, request.cookie)
        self.assertEqual(CommandName.NATIFY_REQUEST, request.command_name)
        self.assertEqual(NatClientIndex.GAME_CLIENT, request.client_index)
        self.assertEqual(3, request.version)
        self.assertEqual(False, request.use_game_port)
        self.assertEqual(NatPortType.NN1, request.port_type)
        client = create_client()

        handler = NatifyHandler(client, request)
        handler.handle()
        self.assertTrue(
            handler._response.sending_buffer
            == b"\xfd\xfc\x1efj\xb2\x03\x02\x00\x00\x03\t\x01\x00\x00\xc0\xa8\x00\x01\x00\x00"
        )

    @responses.activate
    def test_preinit(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x04, 0x0f, 0xb5, 0xe0, 0x95, 0x2a, 0x00, 0x24, 0x38, 0xb2, 0xb3, 0x5e
            ]
        )  # fmt: skip

        data = PreInitRequest.gs_parse(raw)
        request = PreInitRequest.model_validate(data)
        self.assertEqual(3051394346, request.cookie)
        self.assertEqual(CommandName.PRE_INIT, request.command_name)
        self.assertEqual(4, request.version)
        self.assertEqual(36, request.state)


if __name__ == "__main__":
    unittest.main()
