import unittest

from frontends.gamespy.protocols.chat.aggregates.enums import (
    GetKeyRequestType,
    MessageType,
    ModeName,
    ModeOperation,
)
from frontends.gamespy.protocols.chat.contracts.requests import (
    AtmRequest,
    GetChannelKeyRequest,
    GetCKeyRequest,
    JoinRequest,
    KickRequest,
    ModeRequest,
    NoticeRequest,
    PartRequest,
    PrivateRequest,
    SetChannelKeyRequest,
    SetCKeyRequest,
    TopicRequest,
    UtmRequest,
)

# region General
CD_KEY = "CDKEY XXXX-XXXX-XXXX-XXXX\r\n"
CRYPT = "CRYPT des 1 gmtest\r\n"
GET_KEY = "GETKEY spyguy 004 0 :\\b_firewall\\b_profileid\\b_ipaddress\\b_publicip\\b_privateip\\b_authresponse\\b_gamever\\b_val\r\n"  # CRLF
GET_UDP_RELAY = "GETUDPRELAY\r\n"
INVITE = "INVITE test spyguy\r\n"
LIST_LIMIT = "LISTLIMIT 5 test\r\n"
LIST = "LIST test\r\n"
LOGIN_PRE_AUTH = "LOGINPREAUTH xxxxx yyyyy\r\n"
# TODO: add binary data test [0D][0A]
LOGIN_NICK_AND_EMAIL = "LOGIN 0 * xxxxx :spyguy@spyguy@gamespy.com\r\n"
LOGIN_UNIQUE_NICK = "LOGIN 0 spyguy xxxxx\r\n"
NAMES = "NAMES\r\n"
MODES_GET = "MODES #gmtest"
MODES_SET = "MODES #gmtest +s"
NICK = "NICK :spyguy\r\n"
PING = "PING\r\n"  # TODO: add binary data test [0D][0A]
PONG = "PONG :Pong!\r\n"
QUIT = "QUIT :Later!\r\n"  # TODO: add binary data test [0D][0A]
REGISTER_NICK = "REGISTERNICK 0 spyguy XXXX-XXXX-XXXX-XXXX\r\n"
SET_GROUP = "SETGROUP test\r\n"
SET_KEY = "SETKEY :test\r\n"
USER_IP = "USRIP\r\n"
USER = "USER spyguy 127.0.0.1 peerchat.unispy.org :spyguy2\r\n"
WHO_IS = "WHOIS spyguy\r\n"
WHO_CHANNEL_USERS_INFO = "WHO #room\r\n"
WHO_USER_INFO = "WHO spyguy\r\n"


class GeneralRequestTests(unittest.TestCase):
    def test_get_chann_key(self):
        pass


# region Channel
GET_CHANNEL_KEY = "GETCHANKEY #GSP!room!test 0000 0 :\\username\\nickname\0\r\n"
GET_CKEY_CHANNEL_SPECIFIC_USER = (
    "GETCKEY #GSP!room!test spyguy 0000 0 :\\username\\nickname\0\r\n"
)
GET_CKEY_CHANNEL_ALL_USER = (
    "GETCKEY #GSP!room!test * 0000 0 :\\username\\nickname\0\r\n"
)
JOIN = "JOIN #GSP!room!test\r\n"
JOIN_WITH_PASS = "JOIN #GSP!room!test pass123\r\n"
KICK = "KICK #islabul spyguy :Spam\r\n"
MODE_CHANNEL = "MODE #GSP!room!test +l 2\r\n"
MODE_USER = "MODE spyguy +s\r\n"
PART = "PART #GSP!room!test :test\r\n"
SET_CHANNEL_KEY = "SETCHANKEY #GSP!room!test 0000 :\\b_flags\\sh\0\r\n"
SET_CKEY = "SETCKEY #GSP!room!test spyguy 0000 :\\b_flags\\sh\0\r\n"
TOPIC_GET_CHANNEL_TOPIC = "TOPIC #GSP!room!test\r\n"
TOPIC_SET_CHANNEL_TOPIC = "TOPIC #GSP!room!test :This is a topic message.\r\n"


class ChannelRequestTests(unittest.TestCase):
    def test_get_channel_key(self):
        request = GetChannelKeyRequest.gs_parse(GET_CHANNEL_KEY)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_get_ckey_channel_specific_user(self):
        request = GetCKeyRequest.gs_parse(GET_CKEY_CHANNEL_SPECIFIC_USER)
        self.assertEqual(
            request.request_type, GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE
        )
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.nick_name, "spyguy")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_get_ckey_channel_all_user(self):
        request = GetCKeyRequest.gs_parse(GET_CKEY_CHANNEL_ALL_USER)
        self.assertEqual(
            request.request_type, GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE
        )
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_join(self):
        request = JoinRequest.gs_parse(JOIN)
        self.assertEqual(request.channel_name, "#GSP!room!test")

        request = JoinRequest.gs_parse(JOIN_WITH_PASS)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.password, "pass123")

    def test_kick(self):
        request = KickRequest.gs_parse(KICK)
        self.assertEqual(request.kickee_nick_name, "spyguy")
        self.assertEqual(request.reason, "Spam")

    def test_mode(self):
        request = ModeRequest.gs_parse(MODE_CHANNEL)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(
            request.mode_flags[0], (ModeOperation.SET, ModeName.CHANNEL_USER_LIMITS)
        )
        self.assertEqual(request.limit_number, 2)

        request = ModeRequest.gs_parse("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2")

    def test_part(self):
        request = PartRequest.gs_parse(PART)
        self.assertEqual(request.reason, "test")

    def test_set_channel_key(self):
        request = SetChannelKeyRequest.gs_parse(SET_CHANNEL_KEY)
        # Add assertions as needed

    def test_set_ckey(self):
        request = SetCKeyRequest.gs_parse(SET_CKEY)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.nick_name, "spyguy")
        self.assertEqual(request.key_values, {"b_flags": "sh"})

    def test_topic_get_channel_topic(self):
        request = TopicRequest.gs_parse(TOPIC_GET_CHANNEL_TOPIC)
        self.assertEqual(request.channel_name, "#GSP!room!test")

    def test_topic_set_channel_topic(self):
        request = TopicRequest.gs_parse(TOPIC_SET_CHANNEL_TOPIC)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.channel_topic, "This is a topic message.")


# region Message


ABOVE_THE_TABLE_MSG = "ATM #GSP!room!test :hello this is a test.\r\n"
NOTICE_MSG = "NOTICE #GSP!room!test :hello this is a test.\r\n"
PRIVATE_MSG = "PRIVMSG #GSP!room!test :hello this is a test.\r\n"
UNDER_THE_TABLE_MSG = "UTM #GSP!room!test :hello this is a test.\r\n"
ACTION_MSG = "PRIVMSG #GSP!room!test :\001ACTION hello this is a test.\001\r\n"


class MessageRequestTests(unittest.TestCase):
    def test_atm(self):
        request = AtmRequest.gs_parse(ABOVE_THE_TABLE_MSG)
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_notice(self):
        request = NoticeRequest.gs_parse(NOTICE_MSG)
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_private(self):
        request = PrivateRequest.gs_parse(PRIVATE_MSG)
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_utm(self):
        request = UtmRequest.gs_parse(UNDER_THE_TABLE_MSG)
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)
