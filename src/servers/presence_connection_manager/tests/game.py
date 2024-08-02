import unittest

from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import RequestHandlerMock, LogMock, ConnectionMock
from servers.presence_connection_manager.src.contracts.requests.buddy import StatusRequest
from servers.natneg.tests.mock_objects import ClientMock

def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceConnectionManager"], t_client=ClientMock,
        logger=logger)

    return conn._client

class GameTest(unittest.TestCase):
    def test_civilization_4(self) -> None:
        raw_requests = [
            "\\newuser\\\\email\\civ4@unispy.org\\nick\\civ4-tk\\passwordenc\\JMHGwQ__\\productid\\10435\\gamename\\civ4\\namespaceid\\17\\uniquenick\\civ4-tk\\id\\1\\final\\",
            "\\login\\\\challenge\\xMsHUXuWNXL3KMwmhoQZJrP0RVsArCYT\\uniquenick\\civ4-tk\\userid\\25\\profileid\\26\\response\\7f2c9c6685570ea18b7207d2cbd72452\\firewall\\1\\port\\0\\productid\\10435\\gamename\\civ4\\namespaceid\\17\\sdkrevision\\1\\id\\1\\final\\",
        ]
        client = create_client()
        
        for x in raw_requests:
            client.on_received(x)
            pass

    def test_conflict_global_storm(self) -> None:
        raw_requests = [
            "\\lc\\1\\challenge\\NRNUJLZMLX\\id\\1\\final\\",
            "\\login\\\\challenge\\KMylyQbZfqzKn9otxx32q4076sOUnKif\\user\\cgs1@cgs1@rs.de\\response\\c1a6638bbcfe130e4287bfe4aa792949\\port\\-15737\\productid\\10469\\gamename\\conflictsopc\\namespaceid\\1\\id\\1\\final\\",
            "\\inviteto\\\\sesskey\\58366\\products\\1038\\final\\",
        ]
        client = create_client()
        
        for x in raw_requests:
            client.on_received(x)
            pass

    def test_sbwfrontps2(self) -> None:
        raw = "\\status\\1\\sesskey\\1111\\statstring\\EN LIGNE\\locstring\\\\final\\"
        request = StatusRequest(raw)
        request.parse()
        self.assertTrue(request.status.location_string == "")
        self.assertTrue(request.status.status_string == "EN LIGNE")