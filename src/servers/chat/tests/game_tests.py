import unittest

from servers.chat.tests.mock_objects import create_client


class GameTests(unittest.TestCase):
    def test_civilization4(self):
        raws = ["USRIP",
                "USER X419pGl4sX|18 127.0.0.1 peerchat.gamespy.com :aa3041ada9385b28fc4d4e47db288769",
                "NICK a1701-5",
                "CDKEY 81123-67814-77652-27631-11723-47707-22638-10701",
                "JOIN #GSP!anno1701 ",
                "MODE #GSP!anno1701",
                "GETCKEY #GSP!anno1701 * 008 0 :\\b_flags", "WHO a1701-5",
                "JOIN #GSP!anno1701!M9zK0KJaKM ",
                "MODE #GSP!anno1701!M9zK0KJaKM",
                "SETCKEY #GSP!anno1701 a1701-5 :\\b_flags\\s",
                "SETCKEY #GSP!anno1701!M9zK0KJaKM a1701-5 :\\b_flags\\sh",
                "GETCKEY #GSP!anno1701!M9zK0KJaKM * 009 0 :\\b_flags",
                "TOPIC #GSP!anno1701!M9zK0KJaKM :test",
                "MODE #GSP!anno1701!M9zK0KJaKM +l 4",
                "MODE #GSP!anno1701!M9zK0KJaKM -i-p-s+m+n+t+l+e 4",
                "PART #GSP!anno1701 :"]
        client = create_client()
        for raw in raws:
            client.on_received(raw)
