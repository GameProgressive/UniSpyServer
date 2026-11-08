import unittest

from frontends.gamespy.protocols.query_report.v1.aggregates.enums import (
    ServerStatus,
)
from frontends.gamespy.protocols.query_report.v1.contracts.responses import (
    HeartbeatPreResponse,
)
from frontends.gamespy.protocols.query_report.v1.contracts.results import (
    HeartbeatPreResult,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import (
    CommndName,
)
from frontends.gamespy.protocols.query_report.v2.contracts.responses import (
    AvailableResponse,
    ChallengeResponse,
    ClientMessageResponse,
    HeartbeatResponse,
)
from frontends.gamespy.protocols.query_report.v2.contracts.results import (
    AvailableResult,
    ChallengeResult,
    ClientMessageResult,
    HeartbeatResult,
)


class ResponseTests(unittest.TestCase):
    def test_heartbeat_pre(self):
        result = HeartbeatPreResult(
            status=ServerStatus.START,
            game_name="test",
        )
        response = HeartbeatPreResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, "\\status\\")

    def test_available(self):
        result = AvailableResult(
            command_name=CommndName.AVALIABLE_CHECK,
            instant_key="123",
        )
        response = AvailableResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, b"\xfe\xfd\x09\x00\x00\x00\x00")

    def test_challenge(self):
        result = ChallengeResult(
            command_name=CommndName.CHALLENGE,
            instant_key="16909060",
        )
        response = ChallengeResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfe\xfd\x01\x01\x02\x03\x04UniSpy echo!",
        )

    def test_client_message(self):
        result = ClientMessageResult(
            command_name=CommndName.CLIENT_MESSAGE,
            instant_key="16909060",
            message_key=0x12345678,
            natneg_message=b"\x05\x06\x07",
        )
        response = ClientMessageResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfe\xfd\x06\x01\x02\x03\x04\x78\x56\x34\x12\x05\x06\x07",
        )

    def test_heartbeat(self):
        result = HeartbeatResult(
            command_name=CommndName.HEARTBEAT,
            instant_key="16909060",
            remote_ip="192.0.2.1",
            remote_port=27900,
        )
        response = HeartbeatResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            b"\xfe\xfd\x03\x01\x02\x03\x04"
            b"\x54\x54\x54\x00\x00\xc0\x00\x02\x01"
            b"\x00\x00\x00\x00\xfc\x6c",
        )


if __name__ == "__main__":
    unittest.main()