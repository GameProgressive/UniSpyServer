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
import unittest

from fastapi.testclient import TestClient

from backends.routers.home import app
import backends.tests.gamespy.chat.request_dumps as req_dump

client = TestClient(app)


class RoomTest(unittest.TestCase):
    def test_sdk(self):

        for c1, c2, route in zip(
            req_dump.test_sdk_client1_msg,
            req_dump.test_sdk_client2_msg,
            req_dump.test_sdk_api,
        ):
            client.post(url=route, json=c1)
            client.post(url=route, json=c2)

    def test_peer(self):
        """
        peer test in gamespy sdk
        """

        for m, route in zip(req_dump.test_peer_msg, req_dump.test_peer_api):
            print(route)
            client.post(url=route, json=m)


if __name__ == "__main__":
    test = RoomTest()
    test.test_sdk()
