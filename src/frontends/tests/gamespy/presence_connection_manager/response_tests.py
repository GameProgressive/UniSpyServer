import re
import unittest
from datetime import datetime, timezone

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.responses import (
    BlockListResponse,
    BuddyListResponse,
    BuddyMessageFriendAddResponse,
    BuddyMessageResponse,
    BuddyMessageUTMResponse,
    GetProfileResponse,
    KeepAliveResponse,
    LoginResponse,
    NewProfileResponse,
    NewUserResponse,
    RegisterCDKeyResposne,
    RegisterNickResponse,
    StatusInfoResponse,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    BlockListResult,
    BuddyListResult,
    BuddyMessageFriendAddResult,
    BuddyMessageResult,
    BuddyMessageUTMResult,
    GetProfileData,
    GetProfileResult,
    LoginData,
    LoginResult,
    NewProfileResult,
    NewUserResult,
    RegisterCDKeyResult,
    RegisterNickResult,
    StatusInfoResult,
)


class ResponseTests(unittest.TestCase):
    def test_keep_alive(self):
        response = KeepAliveResponse()
        response.build()
        self.assertEqual(response.sending_buffer, "\\ka\\final\\")

    def test_login(self):
        result = LoginResult(
            operation_id=1,
            data=LoginData(
                user_id=2,
                profile_id=3,
                subprofile_id=4,
                nick="player",
                email="player@example.com",
                unique_nick="player_unique",
                password_hash="password_hash",
                email_verified_flag=True,
                banned_flag=False,
                namespace_id=5,
            ),
            user_data="player@example.com",
            type=LoginType.NICK_EMAIL,
            partner_id=0,
            user_challenge="challenge",
            subprofile_id=4,
        )
        response = LoginResponse(result)
        response.build()
        self.assertRegex(
            response.sending_buffer,
            r"\\lc\\2\\sesskey\\4\\proof\\[0-9a-f]{32}"
            r"\\userid\\2\\profileid\\3\\uniquenick\\player_unique"
            r"\\lt\\0000000000000000000000__\\id\\1\\final\\",
        )

    def test_new_user(self):
        response = NewUserResponse(
            NewUserResult(operation_id=7, user_id=2, profile_id=3)
        )
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "\\nur\\userid\\2\\profileid\\3\\id\\7\\final\\",
        )

    def test_buddy_and_block_lists(self):
        block_response = BlockListResponse(
            BlockListResult(operation_id=7, profile_ids=[1, 2, 3])
        )
        block_response.build()
        self.assertEqual(block_response.sending_buffer,
                         "\\blk\\3\\list\\1,2,3\\final\\")

        empty_block_response = BlockListResponse(
            BlockListResult(operation_id=7, profile_ids=[])
        )
        empty_block_response.build()
        self.assertEqual(empty_block_response.sending_buffer,
                         "\\blk\\0\\list\\\\final\\")

        buddy_response = BuddyListResponse(
            BuddyListResult(operation_id=7, profile_ids=[4, 5])
        )
        buddy_response.build()
        self.assertEqual(buddy_response.sending_buffer,
                         "\\bdy\\2\\list\\4,5\\final\\")

    def test_buddy_messages(self):
        message_date = datetime(2026, 1, 2, 3, 4, 5, tzinfo=timezone.utc)
        friend_add = BuddyMessageFriendAddResult(
            operation_id=1,
            data=[
                BuddyMessageFriendAddResult.BuddyMessageFriendAddData(
                    from_profile_id=10,
                    date=message_date,
                    message="hello",
                    signature="signed-data",
                )
            ]
        )
        response = BuddyMessageFriendAddResponse(friend_add)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "\\bm\\2\\f\\10\\date\\1767323045\\msg\\hello|signed|signed-data\\final\\",
        )

        message = BuddyMessageResult(
            operation_id=2,
            data=[
                BuddyMessageResult.BuddyMessageData(
                    from_profile_id=11, date=message_date, message="message"
                )
            ]
        )
        response = BuddyMessageResponse(message)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "\\bm\\2\\f\\11\\date\\1767323045\\message\\message\\final\\",
        )

        utm = BuddyMessageUTMResult(
            operation_id=3,
            data=[
                BuddyMessageUTMResult.BuddyMessageUTMData(
                    from_profile_id=12, date=message_date, message="utm"
                )
            ]
        )
        response = BuddyMessageUTMResponse(utm)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "\\bm\\2\\f\\12\\date\\1767323045\\message\\utm\\final\\",
        )

    def test_status_info(self):
        result = StatusInfoResult(
            operation_id=4,
            profile_id=1,
            product_id=2,
            status_state="3",
            buddy_ip="192.0.2.1",
            host_ip="192.0.2.2",
            host_private_ip="10.0.0.2",
            query_report_port=3,
            host_port=4,
            session_flags="5",
            rich_status="ready",
            game_type="type",
            game_variant="variant",
            game_map_name="map",
            quiet_mode_flags="6",
        )
        response = StatusInfoResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "\\bsi\\state\\3\\profile\\1\\bip\\192.0.2.1\\hostIp\\192.0.2.2"
            "\\hprivIp\\10.0.0.2\\qport\\3\\hport\\4\\sessflags\\5"
            "\\rstatus\\ready\\gameType\\type\\gameVnt\\variant\\gameMn\\map"
            "\\product\\2\\qmodeflags\\6\\final\\",
        )

    def test_get_profile(self):
        result = GetProfileResult(
            operation_id=5,
            user_profile=GetProfileData(
                nick="player",
                profile_id=9,
                unique_nick="player_unique",
                email="player@example.com",
                extra_infos={"countrycode": "US", "birthday": "20000101"},
            )
        )
        response = GetProfileResponse(result)
        response.build()
        self.assertRegex(
            response.sending_buffer,
            re.escape(
                "\\pi\\profileid\\9\\nick\\player\\uniquenick\\player_unique"
                "\\email\\player@example.com\\countrycode\\US\\birthday\\20000101"
                "\\sig\\+"
            )
            + r"[0-9a-f]{10}"
            + re.escape("\\id\\5\\final\\"),
        )

    def test_profile_creation_and_registration(self):
        new_profile = NewProfileResponse(
            NewProfileResult(operation_id=8, profile_id=42)
        )
        new_profile.build()
        self.assertEqual(
            new_profile.sending_buffer,
            "\\npr\\\\profileid\\42\\id\\8\\final\\",
        )

        register_cd_key = RegisterCDKeyResposne(
            RegisterCDKeyResult(operation_id=9))
        register_cd_key.build()
        self.assertEqual(
            register_cd_key.sending_buffer, "\\rc\\\\id\\9\\final\\"
        )

        register_nick = RegisterNickResponse(
            RegisterNickResult(operation_id=10))
        register_nick.build()
        self.assertEqual(register_nick.sending_buffer,
                         "\\rn\\\\id\\10\\final\\")


if __name__ == "__main__":
    unittest.main()
