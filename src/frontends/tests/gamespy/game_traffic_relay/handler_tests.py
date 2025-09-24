import unittest

from frontends.gamespy.protocols.game_traffic_relay.applications.handlers import (
    PingHandler,
)
from frontends.gamespy.protocols.natneg.contracts.requests import PingRequest
from frontends.tests.gamespy.game_traffic_relay.mock_objects import create_client


class HandlerTests(unittest.TestCase):
    def test_ping(self):
        """
        test whether 2 clients can be binding togather with ping command
        """
        ping_raw = (
            b"\xfd\xfc\x1efj\xb2\x03\x07\x00\x00\x02\x9a\xc0\xa8\x01gl\xfd\x00\x00"
        )
        client1 = create_client(("127.0.0.1", 1234))
        client1.connection.remote_ip = "127.0.0.1"
        client1.connection.remote_port = 1234
        client1._log_prefix = "[127.0.0.1:1234]"
        client2 = create_client(("127.0.0.1", 1235))
        client2.connection.remote_ip = "127.0.0.1"
        client2.connection.remote_port = 1235
        client2._log_prefix = "[127.0.0.1:1235]"
        handler = PingHandler(client1, PingRequest(ping_raw))
        handler.handle()
        handler = PingHandler(client2, PingRequest(ping_raw))
        handler.handle()
        # cookie length check
        self.assertEqual(len(client1.listener.pool), 1)
        clients = list(client1.listener.pool.values())[0]
        self.assertEqual(len(clients), 2)
        client1.on_received(ping_raw)
        pass
