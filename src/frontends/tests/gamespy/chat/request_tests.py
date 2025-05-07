import unittest

from frontends.gamespy.protocols.chat.aggregates.enums import MessageType
from frontends.gamespy.protocols.chat.contracts.requests import *
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
GET_CKEY_CHANNEL_SPECIFIC_USER = "GETCKEY #GSP!room!test spyguy 0000 0 :\\username\\nickname\0\r\n"
GET_CKEY_CHANNEL_ALL_USER = "GETCKEY #GSP!room!test * 0000 0 :\\username\\nickname\0\r\n"
JOIN = "JOIN #GSP!room!test\r\n"
JOIN_WITH_PASS = "JOIN #GSP!room!test pass123\r\n"
KICK = "KICK #islabul spyguy :Spam\r\n"
MODE_CHANNEL = "MODE #GSP!room!test +l 2\r\n"
MODE_USER = "MODE spyguy +s\r\n"
PART = "PART #GSP!room!test :test\r\n"
SET_CHANNEL_KEY = "SETCHANNELKEY #GSP!room!test 0000 0:\\b_flags\\sh\0\r\n"
SET_CKEY = "SETCKEY #GSP!room!test spyguy 0000 0:\\b_flags\\sh\0\r\n"
TOPIC_GET_CHANNEL_TOPIC = "TOPIC #GSP!room!test\r\n"
TOPIC_SET_CHANNEL_TOPIC = "TOPIC #GSP!room!test :This is a topic message.\r\n"


class ChannelRequestTests(unittest.TestCase):
    def test_get_channel_key(self):
        request = GetChannelKeyRequest(GET_CHANNEL_KEY)
        request.parse()
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_get_ckey_channel_specific_user(self):
        request = GetCKeyRequest(GET_CKEY_CHANNEL_SPECIFIC_USER)
        request.parse()
        self.assertEqual(request.request_type,
                         GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.nick_name, "spyguy")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_get_ckey_channel_all_user(self):
        request = GetCKeyRequest(GET_CKEY_CHANNEL_ALL_USER)
        request.parse()
        self.assertEqual(request.request_type,
                         GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.cookie, "0000")
        self.assertEqual(request.keys[0], "username")
        self.assertEqual(request.keys[1], "nickname")

    def test_join(self):
        request = JoinRequest(JOIN)
        request.parse()
        self.assertEqual(request.channel_name, "#GSP!room!test")

        request = JoinRequest(JOIN_WITH_PASS)
        request.parse()
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.password, "pass123")

    def test_kick(self):
        request = KickRequest(KICK)
        request.parse()
        self.assertEqual(request.kickee_nick_name, "spyguy")
        self.assertEqual(request.reason, "Spam")

    def test_mode(self):
        request = ModeRequest(MODE_CHANNEL)
        request.parse()
        self.assertEqual(
            request.mode_operations[0], ModeOperationType.ADD_CHANNEL_USER_LIMITS)
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.mode_flag, "+l")
        self.assertEqual(request.limit_number, 2)

        request = ModeRequest("MODE #GSP!gmtest!MlNK4q4l1M -i-p-s+m-n+t+l+e 2")
        request.parse()

    def test_part(self):
        request = PartRequest(PART)
        request.parse()
        self.assertEqual(request.reason, "test")

    def test_set_channel_key(self):
        request = SetChannelKeyRequest(SET_CHANNEL_KEY)
        request.parse()
        # Add assertions as needed

    def test_set_ckey(self):
        request = SetCKeyRequest(SET_CKEY)
        request.parse()
        self.assertEqual(request.channel_name, "#GSP!room!test")
        self.assertEqual(request.nick_name, "spyguy")
        self.assertEqual(request.key_values, {"b_flags": "sh"})

    def test_topic_get_channel_topic(self):
        request = TopicRequest(TOPIC_GET_CHANNEL_TOPIC)
        request.parse()
        self.assertEqual(request.channel_name, "#GSP!room!test")

    def test_topic_set_channel_topic(self):
        request = TopicRequest(TOPIC_SET_CHANNEL_TOPIC)
        request.parse()
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
        request = ATMRequest(ABOVE_THE_TABLE_MSG)
        request.parse()
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_notice(self):
        request = NoticeRequest(NOTICE_MSG)
        request.parse()
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_private(self):
        request = PrivateRequest(PRIVATE_MSG)
        request.parse()
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)

    def test_utm(self):
        request = UTMRequest(UNDER_THE_TABLE_MSG)
        request.parse()
        self.assertEqual(MessageType.CHANNEL_MESSAGE, request.type)
        self.assertEqual(False, hasattr(request, "nick_name"))
        self.assertEqual("#GSP!room!test", request.channel_name)
