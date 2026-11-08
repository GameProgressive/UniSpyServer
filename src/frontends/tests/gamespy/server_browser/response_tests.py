import unittest
from datetime import datetime
from uuid import UUID

from frontends.gamespy.protocols.query_report.aggregates.enums import (
    GameServerStatus,
)
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import (
    PeerRoomInfo,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    GameServerFlags,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.applications.switcher import (
    Switcher,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.responses import (
    DeleteServerInfoResponse,
    P2PGroupRoomListResponse,
    ServerFullInfoListResponse,
    ServerInfoResponse,
    ServerMainListResponse,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    ServerFullInfoListResult,
    ServerInfoResult,
    ServerMainListResult,
)


def create_server_info(data: dict[str, str], port: int = 6500) -> GameServerInfo:
    return GameServerInfo(
        server_id=UUID("950b7638-a90d-469b-ac1f-861e63c8c613"),
        instant_key="12356",
        game_name="gmtest",
        query_report_port=port,
        update_time=datetime(2023, 10, 1, 12, 0),
        status=GameServerStatus.NORMAL,
        data=data,
        host_ip_address="192.0.2.10",
    )


def create_server_list_result(
    servers_info: list[GameServerInfo],
    keys: list[str],
) -> ServerMainListResult:
    return ServerMainListResult(
        client_remote_ip="127.0.0.1",
        flag=GameServerFlags.HAS_KEYS_FLAG,
        game_secret_key="xxxx",
        keys=keys,
        servers_info=servers_info,
    )


class ResponseTests(unittest.TestCase):
    def test_server_main_list(self):
        response = ServerMainListResponse(
            create_server_list_result(
                [create_server_info({"hostname": "Room"})],
                ["hostname"],
            )
        )

        response.build()

        expected = (
            b"\xee\x00\x00\xe0"
            b"0000000000"
            b"\x7f\x00\x00\x01\x19\x64"
            b"\x01\x00hostname\x00\x00"
            b"\x40\xc0\x00\x02\x0a"
            b"\xffRoom\x00"
            b"\x00\xff\xff\xff\xff"
        )
        self.assertEqual(response.sending_buffer, expected)

    def test_server_info(self):
        response = ServerInfoResponse(
            ServerInfoResult(
                game_server_info=create_server_info(
                    {"hostname": "Room", "mapname": "arena"}
                )
            )
        )

        response.build()

        self.assertEqual(
            response.sending_buffer,
            b"\x00\x22\x02\x80\xc0\x00\x02\x0a"
            b"hostname\x00Room\x00mapname\x00arena\x00",
        )

    def test_delete_server_info(self):
        response = DeleteServerInfoResponse(
            ServerInfoResult(
                game_server_info=create_server_info({"hostname": "Room"})
            )
        )

        response.build()

        self.assertEqual(response.sending_buffer, b"\x04\xc0\x00\x02\x0a")

    def test_server_full_info_list(self):
        result = ServerFullInfoListResult(
            client_remote_ip="127.0.0.1",
            flag=GameServerFlags.HAS_FULL_RULES_FLAG,
            game_secret_key="xxxx",
            keys=[],
            servers_info=[
                create_server_info({"hostname": "Room", "mapname": "arena"})
            ],
        )
        response = ServerFullInfoListResponse(result)

        response.build()

        self.assertEqual(
            response.sending_buffer,
            b"\xee\x00\x00\xe0"
            b"0000000000"
            b"\x7f\x00\x00\x01\x19\x64"
            b"\x00\x22\x02\x80\xc0\x00\x02\x0a"
            b"hostname\x00Room\x00mapname\x00arena\x00",
        )

    def test_p2p_group_room_list(self):
        result = P2PGroupRoomListResult(
            client_remote_ip="127.0.0.1",
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key="xxxx",
            keys=["hostname"],
            peer_room_info=[
                PeerRoomInfo(game_name="gmtest", groupid=7, hostname="Room")
            ],
        )
        response = P2PGroupRoomListResponse(result)

        response.build()

        self.assertEqual(
            response.sending_buffer,
            b"\xee\x00\x00\xe0"
            b"0000000000"
            b"\x7f\x00\x00\x01\x19\x64"
            b"\x01\x00hostname\x00\x00"
            b"\x40\x00\x00\x00\x07\xffRoom\x00"
            b"\x40\x00\x00\x00\x00\xff\x00"
            b"\x00\x00\x00\x00",
        )

    def test_server_info_request_update_option(self):
        raw = (
            b"\x00%\x00\x01\x03\x00\x00\x00\x00gmtest\x00gmtest\x00"
            b"$(A:{<]p\x00\x00\x00\x00\x00\x02\x00\t\x01\xac\x13\x00\x05+g"
        )

        self.assertEqual(
            Switcher.get_update_option(raw),
            ServerListUpdateOption.SERVER_FULL_INFO_LIST,
        )


if __name__ == "__main__":
    unittest.main()
