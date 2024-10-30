import unittest

from library.tests.mock_objects import BrokerMock
from servers.chat.src.aggregates.channel import Channel
from servers.chat.src.aggregates.channel_user import ChannelUser
from servers.chat.tests.mock_objects import create_client


class RoomTests(unittest.TestCase):
    def test_peer_room(self):
        client = create_client()
        client.info.gamename = "test"
        client.info.previously_joined_channel = "stagging"
        client.info.nick_name = "unispy"
        channel = Channel("test", client, brocker_cls=BrokerMock)
        user = ChannelUser(client, channel)
        channel.add_bind_on_user_and_channel(user)
        pass
