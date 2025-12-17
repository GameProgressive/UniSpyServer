from datetime import datetime
import unittest

from backends.library.database.pg_orm import InitPacketCaches
from backends.protocols.gamespy.natneg.helpers import NatProtocolHelper
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortType,
    NatType,
)


class NatDetectionTests(unittest.TestCase):
    def test_public_ip(self):
        # Create a list of InitPacketCache instances
        packet_caches: list[InitPacketCaches] = [
            InitPacketCaches(
                port_type=NatPortType.GP,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=1,
                private_ip="10.0.0.1",
                private_port=0,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN1,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="10.0.0.1",
                private_port=0,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN2,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="10.0.0.1",
                private_port=2,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN3,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="10.0.0.1",
                private_port=2,
            ),
        ]

        # Create an instance of NatInitInfo with the list of packet caches
        init_info = NatProtocolHelper(packet_caches)

        # Determine NAT type
        NatProtocolHelper._determine_nat_type_version4(init_info)

        # Assert that the NAT type is NoNat
        self.assertEqual(init_info.nat_type, NatType.NO_NAT)

    def test_full_cone(self):
        # Create a list of InitPacketCache instances
        packet_caches: list[InitPacketCaches] = [
            InitPacketCaches(
                port_type=NatPortType.GP,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=1,
                private_ip="192.168.1.1",
                private_port=0,

            ),
            InitPacketCaches(
                port_type=NatPortType.NN1,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="192.168.1.1",
                private_port=0,

            ),
            InitPacketCaches(
                port_type=NatPortType.NN2,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="192.168.1.1",
                private_port=2,

            ),
            InitPacketCaches(
                port_type=NatPortType.NN3,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="192.168.1.1",
                private_port=2,

            ),
        ]

        # Create an instance of NatInitInfo with the list of packet caches
        init_info = NatProtocolHelper(packet_caches)

        # Determine NAT type
        NatProtocolHelper._determine_nat_type_version4(init_info)
        self.assertEqual(init_info.nat_type, NatType.FULL_CONE)

    # Test method
    def test_symmetric(self):
        # Create a list of InitPacketCaches instances
        packet_caches = [
            InitPacketCaches(
                port_type=NatPortType.GP,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=1,
                private_ip="192.168.1.1",
                private_port=1,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN1,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=2,
                private_ip="192.168.1.1",
                private_port=2,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN2,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=3,
                private_ip="192.168.1.1",
                private_port=2,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN3,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="10.0.0.1",
                public_port=4,
                private_ip="192.168.1.1",
                private_port=2,
            ),
        ]

        # Create an instance of NatInitInfo with the list of packet caches
        init_info = NatProtocolHelper(packet_caches)

        # Determine NAT type
        NatProtocolHelper._determine_nat_type_version4(init_info)

        # Assert the NAT type is symmetric
        self.assertEqual(init_info.nat_type, NatType.SYMMETRIC)

    def test_sposirius_network(self):
        # Create a list of InitPacketCaches instances
        packet_caches = [
            InitPacketCaches(
                port_type=NatPortType.GP,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="91.52.105.210",
                public_port=51520,
                private_ip="192.168.0.60",
                private_port=0,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN1,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="91.52.105.210",
                public_port=51521,
                private_ip="192.168.0.60",
                private_port=0,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN2,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="91.52.105.210",
                public_port=49832,
                private_ip="192.168.0.60",
                private_port=49832,
            ),
            InitPacketCaches(
                port_type=NatPortType.NN3,
                client_index=NatClientIndex.GAME_CLIENT,
                cookie=123,
                version=3,
                game_name="gmtest",
                public_ip="91.52.105.210",
                public_port=49832,
                private_ip="192.168.0.60",
                private_port=49832,
            ),
        ]

        # Create an instance of NatInitInfo with the list of packet caches
        init_info = NatProtocolHelper(packet_caches)

        # Determine NAT type
        NatProtocolHelper._determine_nat_type_version4(init_info)

        # Assert the NAT type is symmetric
        self.assertEqual(init_info.nat_type, NatType.SYMMETRIC)
