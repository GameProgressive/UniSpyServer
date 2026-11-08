import unittest

from frontends.gamespy.protocols.natneg.aggregations.enums import (
    ConnectPacketStatus,
    NatClientIndex,
    NatPortType,
)
from frontends.gamespy.protocols.natneg.contracts.responses import (
    AddressCheckResponse,
    ConnectResponse,
    ErcAckResponse,
    InitResponse,
    NatifyResponse,
)
from frontends.gamespy.protocols.natneg.contracts.results import (
    AddressCheckResult,
    ConnectResult,
    ErtAckResult,
    InitResult,
    NatifyResult,
)


class ResponseTests(unittest.TestCase):
    def test_init(self):
        result = InitResult(
            version=3,
            cookie=777,
            public_ip_addr="192.168.0.1",
            public_port=1234,
            port_type=NatPortType.NN1,
            client_index=NatClientIndex.GAME_CLIENT,
            use_game_port=False,
        )
        response = InitResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfd\xfc\x1e\x66\x6a\xb2\x03\x01\x00\x00\x03\x09"
            b"\x01\x00\x00\xc0\xa8\x00\x01\x04\xd2",
        )

    def test_ert_ack(self):
        result = ErtAckResult(
            version=3,
            cookie=777,
            public_ip_addr="192.168.0.1",
            public_port=1234,
            port_type=NatPortType.NN1,
            client_index=NatClientIndex.GAME_CLIENT,
            use_game_port=False,
        )
        response = ErcAckResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfd\xfc\x1e\x66\x6a\xb2\x03\x03\x00\x00\x03\x09"
            b"\x01\x00\x00\xc0\xa8\x00\x01\x04\xd2",
        )

    def test_natify(self):
        result = NatifyResult(
            version=3,
            cookie=777,
            public_ip_addr="192.168.0.1",
            public_port=1234,
            port_type=NatPortType.NN1,
            client_index=NatClientIndex.GAME_CLIENT,
            use_game_port=False,
        )
        response = NatifyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfd\xfc\x1e\x66\x6a\xb2\x03\x02\x00\x00\x03\x09"
            b"\x01\x00\x00\xc0\xa8\x00\x01\x04\xd2",
        )

    def test_address_check(self):
        result = AddressCheckResult(
            version=3,
            cookie=777,
            public_ip_addr="192.168.0.1",
            public_port=1234,
            port_type=NatPortType.NN1,
            client_index=NatClientIndex.GAME_CLIENT,
            use_game_port=False,
        )
        response = AddressCheckResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfd\xfc\x1e\x66\x6a\xb2\x03\x0b\x00\x00\x03\x09"
            b"\x01\x00\x00\xc0\xa8\x00\x01\x04\xd2",
        )

    def test_connect(self):
        result = ConnectResult(
            version=3,
            cookie=777,
            is_both_client_ready=True,
            status=ConnectPacketStatus.BAD_HEART_BEAT,
            ip="192.0.2.1",
            port=4660,
        )
        response = ConnectResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfd\xfc\x1e\x66\x6a\xb2\x03\x05\x00\x00\x03\x09"
            b"\xc0\x00\x02\x01\x12\x34\x01\x01",
        )