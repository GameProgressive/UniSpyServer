# from typing import Optional
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