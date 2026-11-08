import unittest

from frontends.gamespy.library.encryption.gs_encryption import CLIENT_KEY, SERVER_KEY
from frontends.gamespy.protocols.chat.aggregates.enums import WhoRequestType
from frontends.gamespy.protocols.chat.contracts.responses import (
    AtmResponse,
    CdKeyResponse,
    CryptResponse,
    GetChannelKeyResponse,
    GetCKeyResponse,
    GetKeyResponse,
    JoinResponse,
    KickResponse,
    ListResponse,
    LoginResponse,
    ModeResponse,
    NamesResponse,
    NickResponse,
    NoticeResponse,
    PartResponse,
    PingResponse,
    PrivateResponse,
    SetChannelKeyResponse,
    SetCKeyResponse,
    TopicResponse,
    UserIPResponse,
    UtmResponse,
    WhoIsResponse,
    WhoResponse,
)
from frontends.gamespy.protocols.chat.contracts.results import (
    AtmResult,
    CryptResult,
    GetChannelKeyResult,
    GetCKeyResult,
    GetKeyResult,
    JoinResult,
    KickResult,
    ListResult,
    LoginResult,
    ModeResult,
    NamesResult,
    NamesResultData,
    NickResult,
    NoticeResult,
    PartResult,
    PingResult,
    PrivateResult,
    SetChannelKeyResult,
    SetCKeyResult,
    TopicResult,
    UserIPResult,
    UtmResult,
    WhoIsResult,
    WhoResult,
)


class GeneralResponseTests(unittest.TestCase):
    def test_cdkey(self):
        response = CdKeyResponse()
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 706 * 1 :Authenticated.\r\n",
        )

    def test_crypt(self):
        result = CryptResult(secret_key="secret")
        response = CryptResponse()
        response.build()
        self.assertEqual(
            response.sending_buffer,
            f":unispy.net 705 * {CLIENT_KEY} {SERVER_KEY}\r\n",
        )

    def test_get_key(self):
        result = GetKeyResult(
            nick_name="spyguy",
            values=["\\b_profileid\\123", "\\b_ipaddress\\127.0.0.1"],
            cookie="004",
        )
        response = GetKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 700 * spyguy 004 \\b_profileid\\123\r\n"
            ":unispy.net 700 * spyguy 004 \\b_ipaddress\\127.0.0.1\r\n"
            ":unispy.net 701 * 004 * :End Of GETKEY.\r\n",
        )

    def test_get_key_with_no_values(self):
        result = GetKeyResult(nick_name="spyguy", values=[], cookie="004")
        response = GetKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 701 * 004 * :End Of GETKEY.\r\n",
        )

    def test_list(self):
        result = ListResult(
            invoker_nick_name="spyguy",
            invoker_user_name="user",
            channel_info_list=[
                ListResult.ListInfo(
                    channel_name="#room",
                    total_channel_user=2,
                    channel_topic="Welcome",
                ),
                ListResult.ListInfo(
                    channel_name="#empty",
                    total_channel_user=0,
                    channel_topic="",
                ),
            ],
        )
        response = ListResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net 321 * #room 2 Welcome\r\n"
            ":spyguy!user@unispy.net 321 * #empty 0 \r\n"
            ":spyguy!user@unispy.net 323\r\n",
        )

    def test_list_with_no_channels(self):
        result = ListResult(
            invoker_nick_name="spyguy",
            invoker_user_name="user",
            channel_info_list=[],
        )
        response = ListResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net 323\r\n",
        )

    def test_login(self):
        result = LoginResult(user_id=12, profile_id=34)
        response = LoginResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, ":unispy.net 707 * 12 34\r\n")

    def test_nick(self):
        result = NickResult(nick_name="spyguy")
        response = NickResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 001 spyguy :Welcome to UniSpy!\r\n",
        )

    def test_ping(self):
        result = PingResult(nick_name="spyguy", user_name="user")
        response = PingResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net PONG\r\n",
        )

    def test_user_ip(self):
        result = UserIPResult(remote_ip="192.0.2.1")
        response = UserIPResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, ":unispy.net 302 :@192.0.2.1\r\n")

    def test_who_is_with_joined_channels(self):
        result = WhoIsResult(
            nick_name="spyguy",
            user_name="user",
            public_ip_address="192.0.2.1",
            joined_channels=["#room", "#lobby"],
        )
        response = WhoIsResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 311 spyguy user user 192.0.2.1 * :user\r\n"
            ":unispy.net 319 spyguy user :@#room @#lobby \r\n"
            ":unispy.net 318 spyguy user :End of WHOIS list. \r\n",
        )

    def test_who_is_without_joined_channels(self):
        result = WhoIsResult(
            nick_name="spyguy",
            user_name="user",
            public_ip_address="192.0.2.1",
            joined_channels=[],
        )
        response = WhoIsResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 311 spyguy user user 192.0.2.1 * :user\r\n"
            ":unispy.net 318 spyguy user :End of WHOIS list. \r\n",
        )

    def test_who_for_channel(self):
        result = WhoResult(
            infos=[
                WhoResult.WhoInfo(
                    channel_name="#room",
                    user_name="user",
                    public_ip_addr="192.0.2.1",
                    nick_name="spyguy",
                    modes="H@",
                )
            ],
            request_type=WhoRequestType.GET_CHANNEL_USER_INFO,
            channel_name="#room",
            nick_name="",
        )
        response = WhoResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 352 * #room user 192.0.2.1 * spyguy H@ *\r\n"
            ":unispy.net 315 * #room * :End of WHO.\r\n",
        )

    def test_who_for_user_and_empty_result(self):
        result = WhoResult(
            infos=[],
            request_type=WhoRequestType.GET_USER_INFO,
            channel_name="",
            nick_name="spyguy",
        )
        response = WhoResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 315 * spyguy * :End of WHO.\r\n",
        )


class ChannelResponseTests(unittest.TestCase):
    def test_get_channel_key(self):
        result = GetChannelKeyResult(
            nick_name="spyguy",
            user_name="user",
            channel_name="#room",
            key_values={"username": "spyguy", "b_flags": "s"},
            cookie="004",
        )
        response = GetChannelKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net 704 * #room 004 "
            "{'username': 'spyguy', 'b_flags': 's'}\r\n",
        )

    def test_get_ckey(self):
        result = GetCKeyResult(
            infos=[
                GetCKeyResult.GetCKeyInfos(
                    nick_name="spyguy",
                    key_values={"username": "spyguy", "b_flags": "s"},
                ),
                GetCKeyResult.GetCKeyInfos(
                    nick_name="other",
                    key_values={"username": "other"},
                ),
            ],
            channel_name="#room",
            cookie="004",
            keys=["username", "b_flags"],
        )
        response = GetCKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 702 * #room spyguy 004 \\spyguy\\s\r\n"
            ":unispy.net 702 * #room other 004 \\other\\\r\n"
            "unispy.net 703 * #room 004 :End Of GETCKEY.\r\n",
        )

    def test_get_ckey_with_no_infos(self):
        result = GetCKeyResult(
            infos=[],
            channel_name="#room",
            cookie="004",
            keys=[],
        )
        response = GetCKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "unispy.net 703 * #room 004 :End Of GETCKEY.\r\n",
        )

    def test_join(self):
        result = JoinResult(
            joiner_nick_name="spyguy",
            joiner_user_name="user",
            channel_name="#room",
        )
        response = JoinResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net JOIN #room\r\n",
        )

    def test_kick(self):
        result = KickResult(
            channel_name="#room",
            kicker_nick_name="admin",
            kicker_user_name="admin_user",
            kickee_nick_name="spyguy",
            reason="Spam",
        )
        response = KickResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            "admin!admin_user@unispy.net KICK #room spyguy :Spam\r\n",
        )

    def test_mode(self):
        result = ModeResult(
            channel_name="#room",
            channel_modes=["n", "m", "t"],
            joiner_nick_name="spyguy",
        )
        response = ModeResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 324 * #room +nmt\r\n",
        )
        self.assertEqual(ModeResponse.get_mode_str([]), "")

    def test_names(self):
        result = NamesResult(
            channel_nicks=[
                NamesResultData(nick_name="admin", is_channel_creator=True),
                NamesResultData(nick_name="spyguy"),
            ],
            channel_name="#room",
            requester_nick_name="spyguy",
        )
        response = NamesResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 353 spyguy = #room :@admin spyguy\r\n"
            ":unispy.net 366 spyguy #room :End of NAMES list. \r\n",
        )

    def test_names_with_no_users(self):
        result = NamesResult(
            channel_nicks=[],
            channel_name="#room",
            requester_nick_name="spyguy",
        )
        response = NamesResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 353 spyguy = #room :\r\n"
            ":unispy.net 366 spyguy #room :End of NAMES list. \r\n",
        )

    def test_part(self):
        result = PartResult(
            leaver_nick_name="spyguy",
            leaver_user_name="user",
            is_channel_creator=False,
            channel_name="#room",
            reason="Later",
        )
        response = PartResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net PART #room :Later\r\n",
        )

    def test_set_channel_key(self):
        result = SetChannelKeyResult(
            setter_nick_name="admin",
            setter_user_name="admin_user",
            channel_name="#room",
            key_value={"b_flags": "s", "username": "admin"},
        )
        response = SetChannelKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":admin!admin_user@unispy.net 704 * #room BCAST "
            "\\b_flags\\s\\username\\admin\r\n",
        )

    def test_set_ckey(self):
        result = SetCKeyResult(
            setter_nick_name="admin",
            setter_user_name="admin_user",
            channel_name="#room",
            cookie="004",
            key_value={"b_flags": "s"},
        )
        response = SetCKeyResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 702 * #room admin 004 \\b_flags\\s\r\n"
            ":unispy.net 703 * #room admin 004 :End Of SETCKEY.\r\n",
        )

    def test_topic(self):
        result = TopicResult(channel_name="#room", channel_topic="A room topic")
        response = TopicResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":unispy.net 332 * #room :A room topic\r\n",
        )

    def test_topic_with_no_topic(self):
        result = TopicResult(channel_name="#room", channel_topic="")
        response = TopicResponse(result)
        response.build()
        self.assertEqual(response.sending_buffer, ":unispy.net 331 * #room\r\n")


class MessageResponseTests(unittest.TestCase):
    def test_atm(self):
        result = AtmResult(
            sender_nick_name="spyguy",
            sender_user_name="user",
            target_name="#room",
            message="Hello there",
        )
        response = AtmResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net ATM #room :Hello there\r\n",
        )

    def test_notice(self):
        result = NoticeResult(
            sender_nick_name="admin",
            sender_user_name="admin_user",
            target_name="spyguy",
            message="Maintenance soon",
        )
        response = NoticeResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":admin!admin_user@unispy.net NOTICE spyguy Maintenance soon\r\n",
        )

    def test_private(self):
        result = PrivateResult(
            sender_nick_name="spyguy",
            sender_user_name="user",
            target_name="other",
            message="Hello",
        )
        response = PrivateResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net PRIVMSG other :Hello\r\n",
        )

    def test_utm(self):
        result = UtmResult(
            sender_nick_name="spyguy",
            sender_user_name="user",
            target_name="#room",
            message="Game data",
        )
        response = UtmResponse(result)
        response.build()
        self.assertEqual(
            response.sending_buffer,
            ":spyguy!user@unispy.net UTM #room :Game data\r\n",
        )