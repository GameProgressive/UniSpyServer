from unittest import TestCase

from frontends.gamespy.protocols.natneg.aggregations.enums import NatClientIndex
from frontends.gamespy.protocols.natneg.applications.switcher import Switcher
from frontends.gamespy.protocols.natneg.contracts.requests import ConnectAckRequest
from frontends.tests.gamespy.natneg.mock_objects import create_client


class ContractTests(TestCase):
    def test_connect_ack(self):
        raws = [
            b'\xfd\xfc\x1efj\xb2\x04\x06\x00\x00\x02\x9a\x00\x01\x00\x00\xd8\xf2@\x00\x00',
            b'\xfd\xfc\x1efj\xb2\x04\x06\x00\x00\x02\x9aj\x01\x04\x07\x00\x00\x02\x9a\xac',
            b'\xfd\xfc\x1efj\xb2\x04\x06\x00\x00\x02\x9aj\x01\x04\x07\x00\x00\x02\x9a\xac',
            b'\xfd\xfc\x1efj\xb2\x04\x06\x00\x00\x02\x9a@\x01\x00\x00\xc0\xf28o\xfd',
        ]
        for raw in raws:
            r = ConnectAckRequest(raw)
            r.parse()
            self.assertEqual(r.client_index, NatClientIndex.GAME_SERVER)
