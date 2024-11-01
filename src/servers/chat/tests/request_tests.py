import unittest
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
LOGIN_NICK_AND_EMAIL = "LOGIN 0 * xxxxx :spyguy@spyguy@unispy.org\r\n"
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
    def test_get_chann_key(self):
        pass

# region Message


ABOVE_THE_TABLE_MSG = "ATM #GSP!room!test :hello this is a test.\r\n"
NOTICE = "NOTICE #GSP!room!test :hello this is a test.\r\n"
PRIVATE_MSG = "PRIVMSG #GSP!room!test :hello this is a test.\r\n"
UNDER_THE_TABLE_MSG = "UTM #GSP!room!test :hello this is a test.\r\n"
ACTION_MSG = "PRIVMSG #GSP!room!test :\001ACTION hello this is a test.\001\r\n"


class MessageRequestTests(unittest.TestCase):
    def test_get_chann_key(self):
        pass
