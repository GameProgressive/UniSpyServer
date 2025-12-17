import unittest

import responses

from frontends.tests.gamespy.chat.mock_objects import create_client


class GameTests(unittest.TestCase):
    @responses.activate
    def test_civilization4(self):
        raws = [
            "USRIP",
            "USER X419pGl4sX|18 127.0.0.1 peerchat.gamespy.com :aa3041ada9385b28fc4d4e47db288769",
            "NICK a1701-5",
            "CDKEY 81123-67814-77652-27631-11723-47707-22638-10701",
            "JOIN #GSP!anno1701 ",
            "MODE #GSP!anno1701",
            "GETCKEY #GSP!anno1701 * 008 0 :\\b_flags",
            "WHO a1701-5",
            "JOIN #GSP!anno1701!M9zK0KJaKM ",
            "MODE #GSP!anno1701!M9zK0KJaKM",
            "SETCKEY #GSP!anno1701 a1701-5 :\\b_flags\\s",
            "SETCKEY #GSP!anno1701!M9zK0KJaKM a1701-5 :\\b_flags\\sh",
            "GETCKEY #GSP!anno1701!M9zK0KJaKM * 009 0 :\\b_flags",
            "TOPIC #GSP!anno1701!M9zK0KJaKM :test",
            "MODE #GSP!anno1701!M9zK0KJaKM +l 4",
            "MODE #GSP!anno1701!M9zK0KJaKM -i-p-s+m+n+t+l+e 4",
            "PART #GSP!anno1701 :",
        ]
        client = create_client()
        for raw in raws:
            client.on_received(raw.encode())

    @responses.activate
    def test_worm3d(self):
        raws = [
            "CRYPT des 1 worms3\r\n",
            "USRIP\r\n",
            "USER X419pGl4sX|6 127.0.0.1 peerchat.gamespy.com :aa3041ada9385b28fc4d4e47db288769\r\n",
            "NICK worms10\r\n",
            "JOIN #GPG!622\r\n",
            "MODE #GPG!622\r\n",
            "GETCKEY #GPG!622 * 024 0 :\\username\\b_flags\r\n",
            "JOIN #GSP!worms3!Ml4lz344lM\r\n",
            "MODE #GSP!worms3!Ml4lz344lM\r\n",
            "SETCKEY #GPG!622 worms10 :\\b_flags\\s" + "\r\n",
            "SETCKEY #GSP!worms3!Ml4lz344lM worms10 :\\b_flags\\sh" + "\r\n",
            "GETCKEY #GSP!worms3!Ml4lz344lM * 025 0 :\\username\\b_flags\r\n",
            "TOPIC #GSP!worms3!Ml4lz344lM :tesr\r\n",
            "MODE #GSP!worms3!Ml4lz344lM +l 2\r\n",
            "PART #GPG!622 :Joined staging room\r\n",
            "SETCKEY #GSP!worms3!Ml4lz344lM worms10 :\\b_firewall\\1\\b_profileid\\6\\b_ipaddress\\b_publicip\\255.255.255.255\\b_privateip\\192.168.0.60\b_authresponse\\b_gamever\\1073\\b_val\\0\r\n",
            "WHO worms10\r\n",
            "SETCHANKEY #GSP!worms3!Ml4lz344lM :\\b_hostname\\test\\b_hostport\\b_MaxPlayers\\2\\b_NumPlayers\\1\\b_SchemeChanging\\0\\b_gamever\\1073\\b_gametype\\b_mapname\\Random\\b_firewall\\1\\b_publicip\\255.255.255.255\\b_privateip\\192.168.0.60\\b_gamemode\\openstaging\\b_val\\0\\b_password\\1\r\n",
            "GETKEY worms20 026 0 :\\b_firewall\b_profileid\\b_ipaddress\\b_publicip\\b_privateip\\b_authresponse\\b_gamever\\b_val\r\n",
            "GETCKEY #GSP!worms3!Ml4lz344lM worms20 027 0 :\\b_firewall\\b_profileid\\b_ipaddress\\b_publicip\\b_privateip\\b_authresponse\\b_gamever\\b_val\r\n",
            "SETCHANKEY #GSP!worms3!Ml4lz344lM :\\b_hostname\\test\\b_hostport\\b_MaxPlayers\\2\\b_NumPlayers\\1\\b_SchemeChanging\\0\\b_gamever\\1073\\b_gametype\\b_mapname\\Random\\b_firewall\\1\\b_publicip\\255.255.255.255\\b_privateip\\192.168.0.60\\b_gamemode\\openstaging\\b_val\0\\b_password\\1\r\n",
            "SETCHANKEY #GSP!worms3!Ml4lz344lM :\\b_hostname\\test\\b_hostport\\b_MaxPlayers\\2\\b_NumPlayers\\1\\b_SchemeChanging\\0\\b_gamever\\1073\\b_gametype\\b_mapname\\Random\\b_firewall\\1\\b_publicip\\255.255.255.255\\b_privateip\\192.168.0.60\\b_gamemode\\openstaging\\b_val\\0\\b_password\\1\r\n",
            "UTM #GSP!worms3!Ml4lz344lM :MDM |Obj|3|Land.Time|0|LogicalSeed|3891226431|GraphicalSeed|3269271590|Land.RealSeed|3281489942|Land.Theme|Pirate.Lumps|LevelToUse|FE.Level.RandomLand|Land.Ind|0|Wormpot.Reel1|17|Wormpot.Reel2|17|Wormpot.Reel3|17|TimeStamp|6206364\r\n",
            "UTM #GSP!worms3!Ml4lz344lM :TDM aA\r\n",
            "UTM #GSP!worms3!Ml4lz344lM :SDM ASFE.Scheme.StandardCUnAACADCBBCACBBFFBKBB8C/C3C!A!A*C*C<D*B*B*B*B*B*B3C*B<A*C3CEC!A-C5C-C3C<C*A*B*B*C*B<CEC*B*C<B<D!A*B*B3B3C<D!A<D/C<C<D*C*A\r\n",
            "UTM worms20 :APE [01]privateip[02]192.168.0.60[01]publicip[02]255.255.255.255\r\n",
        ]
        client = create_client()
        for raw in raws:
            client.on_received(raw.encode())
            client.crypto = None

    @responses.activate
    def test_crysis2_20230926(self):
        raws = [
            "CRYPT des 1 capricorn\r\n",
            "LOGIN 95 Sporesirius baec04ae6862e941b948c8a1a361c096\r\n",
            "USRIP\r\n",
            "USER XpaGflff1X|39 127.0.0.1 peerchat.unispy.dev :e51130924b08de265d9ae69113103f78\r\nNICK *\r\n",
            "QUIT :Later!\r\n",
        ]
        client = create_client()
        for raw in raws:
            client.on_received(raw.encode())
            client.crypto = None
