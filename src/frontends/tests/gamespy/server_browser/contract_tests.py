from datetime import datetime
import unittest
from uuid import UUID

from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import (
    GameServerStatus,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    GameServerFlags,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.applications.switcher import Switcher
from frontends.gamespy.protocols.server_browser.v2.contracts.responses import (
    ServerMainListResponse,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    ServerMainListResult,
)


class ResponseTests(unittest.TestCase):
    def test_main_list(self):
        result = ServerMainListResult(
            client_remote_ip="127.0.0.1",
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key="xxxx",
            keys=["hostname", "gametype", "mapname",
                  "numplayers", "maxplayers"],
            servers_info=[
                GameServerInfo(
                    server_id=UUID("950b7638-a90d-469b-ac1f-861e63c8c613"),
                    instant_key="12356",
                    game_name="gmtest",
                    query_report_port=6900,
                    last_heart_beat_received_time=datetime.now(),
                    status=GameServerStatus.NORMAL,
                    server_data={"hostname": "GameSpy QR2 Sample"},
                    player_data=[{}],
                    team_data=[{}],
                    host_ip_address="127.0.0.1",
                )
            ],
        )
        response = ServerMainListResponse(result)
        response.build()
        correct = b'\xee\x00\x00\xe00000000000\x7f\x00\x00\x01\x19d\x05\x00hostname\x00\x00gametype\x00\x00mapname\x00\x00numplayers\x00\x00maxplayers\x00\x00P\x7f\x00\x00\x01\x1a\xf4\xffGameSpy QR2 Sample\x00\xff\x00\xff\x00\xff\x00\xff\x00\x00\xff\xff\xff\xff'
        self.assertTrue(response.sending_buffer == correct)

    def test_server_info_request(self):
        raw = b'\x00%\x00\x01\x03\x00\x00\x00\x00gmtest\x00gmtest\x00$(A:{<]p\x00\x00\x00\x00\x00\x02\x00\t\x01\xac\x13\x00\x05+g'
        option = Switcher.get_update_option(raw)
        self.assertEqual(option, ServerListUpdateOption.SERVER_FULL_INFO_LIST)
        pass


if __name__ == "__main__":
    ResponseTests().test_server_info_request()
