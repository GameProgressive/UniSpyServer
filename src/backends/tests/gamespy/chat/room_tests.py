# import unittest

# from frontends.gamespy.library.tests.mock_objects import BrokerMock
# from frontends.gamespy.protocols.chat.aggregates.channel import Channel
# from frontends.gamespy.protocols.chat.aggregates.channel_user import ChannelUser
# from frontends.gamespy.protocols.chat.tests.mock_objects import create_client


# class RoomTests(unittest.TestCase):
#     def test_peer_room(self):
#         client = create_client()
#         client.info.gamename = "test"
#         client.info.previously_joined_channel = "stagging"
#         client.info.nick_name = "unispy"
#         channel = Channel("test", client, brocker_cls=BrokerMock)
#         user = ChannelUser(client, channel)
#         channel.add_bind_on_user_and_channel(user)
#         pass

#     def test_single_join(self, user_name="unispy", nick_name="unispy", channel_name="#GSP!room!test"):
#         pass


# testclient.TestClient.get()
from fastapi.testclient import TestClient
from backends.routers.home import app
import unittest

client = TestClient(app)


class RoomTest(unittest.TestCase):
    def test_sdk(self):
        client1_msg = [
            {
                "raw_request": "CRYPT des 1 gmtest",
                "command_name": "CRYPT",
                "version_id": "1",
                "gamename": "gmtest",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "USRIP",
                "command_name": "USRIP",
                "websocket_address": "127.0.0.1:60720",
                "remote_ip": "172.19.0.5",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "USER ChatCUser 127.0.0.1 unispy_server_dev :ChatCName",
                "command_name": "USER",
                "user_name": "ChatCUser",
                "local_ip_address": "127.0.0.1",
                "server_name": "unispy_server_dev",
                "name": "ChatCName",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "NICK ChatC660",
                "command_name": "NICK",
                "nick_name": "ChatC660",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "JOIN #GSP!gmtest ",
                "broad_cast_raw": None,
                "password": None,
                "command_name": "JOIN",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "MODE #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "MODE",
                "channel_name": "#GSP!gmtest",
                "mode_operations": {},
                "request_type": 0,
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Hi",
                "broad_cast_raw": "ChatC660!ChatCUser@unispy.net PRIVMSG #GSP!gmtest :Hi",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "message": "Hi",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "WHOIS ChatC660",
                "command_name": "WHOIS",
                "nick_name": "ChatC660",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Bye",
                "broad_cast_raw": "ChatC660!ChatCUser@unispy.net PRIVMSG #GSP!gmtest :Bye",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "message": "Bye",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "PART #GSP!gmtest :",
                "broad_cast_raw": None,
                "command_name": "PART",
                "channel_name": "#GSP!gmtest",
                "reason": "",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
            {
                "raw_request": "QUIT :Later!",
                "command_name": "QUIT",
                "reason": "Later!",
                "websocket_address": "127.0.0.1:60720",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40394,
            },
        ]

        client2_msg = [
            {
                "raw_request": "CRYPT des 1 gmtest",
                "command_name": "CRYPT",
                "version_id": "1",
                "gamename": "gmtest",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "USRIP",
                "command_name": "USRIP",
                "websocket_address": "127.0.0.1:60721",
                "remote_ip": "172.19.0.5",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "USER ChatCUser1 127.0.0.1 unispy_server_dev :ChatCName",
                "command_name": "USER",
                "user_name": "ChatCUser1",
                "local_ip_address": "127.0.0.1",
                "server_name": "unispy_server_dev",
                "name": "ChatCName",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "NICK ChatC661",
                "command_name": "NICK",
                "nick_name": "ChatC661",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "JOIN #GSP!gmtest ",
                "broad_cast_raw": None,
                "password": None,
                "command_name": "JOIN",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "MODE #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "MODE",
                "channel_name": "#GSP!gmtest",
                "mode_operations": {},
                "request_type": 0,
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Hi",
                "broad_cast_raw": "ChatC661!ChatCUser1@unispy.net PRIVMSG #GSP!gmtest :Hi",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "message": "Hi",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "broad_cast_raw": None,
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "WHOIS ChatC661",
                "command_name": "WHOIS",
                "nick_name": "ChatC661",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Bye",
                "broad_cast_raw": "ChatC661!ChatCUser1@unispy.net PRIVMSG #GSP!gmtest :Bye",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "message": "Bye",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "PART #GSP!gmtest :",
                "broad_cast_raw": None,
                "command_name": "PART",
                "channel_name": "#GSP!gmtest",
                "reason": "",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
            {
                "raw_request": "QUIT :Later!",
                "command_name": "QUIT",
                "reason": "Later!",
                "websocket_address": "127.0.0.1:60721",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 40395,
            },
        ]

        api = [
            "GameSpy/Chat/CryptHandler",
            "GameSpy/Chat/UserIPHandler",
            "GameSpy/Chat/UserHandler",
            "GameSpy/Chat/NickHandler",
            "GameSpy/Chat/JoinHandler",
            "GameSpy/Chat/NamesHandler",
            "GameSpy/Chat/ModeHandler",
            "GameSpy/Chat/PrivateHandler",
            "GameSpy/Chat/NamesHandler",
            "GameSpy/Chat/WhoIsHandler",
            "GameSpy/Chat/PrivateHandler",
            "GameSpy/Chat/PartHandler",
            "GameSpy/Chat/QuitHandler",
        ]
        for c1, c2, route in zip(client1_msg, client2_msg, api):
            try:
                client.post(url=route, json=c1)
                client.post(url=route, json=c2)
            except Exception as e:
                print(e)
        pass

    def test_peer(self):
        """
        peer test in gamespy sdk
        """
        test_msg = [
            {
                "raw_request": "CRYPT des 1 gmtest",
                "command_name": "CRYPT",
                "version_id": "1",
                "gamename": "gmtest",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "USRIP",
                "command_name": "USRIP",
                "websocket_address": "127.0.0.1:59754",
                "remote_ip": "172.19.0.5",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "USER ChatCUser 127.0.0.1 unispy_server_dev :ChatCName",
                "command_name": "USER",
                "user_name": "ChatCUser",
                "local_ip_address": "127.0.0.1",
                "server_name": "unispy_server_dev",
                "name": "ChatCName",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "NICK ChatC238",
                "command_name": "NICK",
                "nick_name": "ChatC238",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "JOIN #GSP!gmtest ",
                "password": None,
                "command_name": "JOIN",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "MODE #GSP!gmtest",
                "command_name": "MODE",
                "channel_name": "#GSP!gmtest",
                "mode_operations": {},
                "request_type": 0,
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Hi",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "target_name": "#GSP!gmtest",
                "message": "Hi",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "NAMES #GSP!gmtest",
                "command_name": "NAMES",
                "channel_name": "#GSP!gmtest",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "WHOIS ChatC238",
                "command_name": "WHOIS",
                "nick_name": "ChatC238",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "PRIVMSG #GSP!gmtest :Bye",
                "command_name": "PRIVMSG",
                "channel_name": "#GSP!gmtest",
                "type": 0,
                "target_name": "#GSP!gmtest",
                "message": "Bye",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "PART #GSP!gmtest :",
                "command_name": "PART",
                "channel_name": "#GSP!gmtest",
                "reason": "",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            },
            {
                "raw_request": "QUIT :Later!",
                "command_name": "QUIT",
                "reason": "Later!",
                "websocket_address": "127.0.0.1:59754",
                "client_ip": "172.19.0.5",
                "server_id": "950b7638-a90d-469b-ac1f-861e63c8c613",
                "client_port": 59258
            }
        ]
        test_api = ["GameSpy/Chat/CryptHandler",
                    "GameSpy/Chat/UserIPHandler",
                    "GameSpy/Chat/UserHandler",
                    "GameSpy/Chat/NickHandler",
                    "GameSpy/Chat/JoinHandler",
                    "GameSpy/Chat/NamesHandler",
                    "GameSpy/Chat/ModeHandler",
                    "GameSpy/Chat/PrivateHandler",
                    "GameSpy/Chat/NamesHandler",
                    "GameSpy/Chat/WhoIsHandler",
                    "GameSpy/Chat/PrivateHandler",
                    "GameSpy/Chat/PartHandler",
                    "GameSpy/Chat/QuitHandler"]
        for m, route in zip(test_msg, test_api):
            try:
                print(route)
                client.post(url=route, json=m)
            except:
                pass


if __name__ == "__main__":
    test = RoomTest()
    test.test_sdk()
