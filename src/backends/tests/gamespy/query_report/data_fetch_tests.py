from datetime import datetime, timezone
import unittest

from pydantic import ValidationError
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches
import backends.protocols.gamespy.query_report.data as data
from servers.query_report.src.aggregates.game_server_info import GameServerInfo


class DataFetchTests(unittest.TestCase):
    def test_get_all_groups(self):
        self.assertIsNotNone(data.PEER_GROUP_LIST)
        self.assertIsInstance(data.PEER_GROUP_LIST, dict)

    def test_get_peer_staging_channels(self):
        cache = ChatChannelCaches(channel_name="#GSP!unispy_test_game_name!*", server_id="b6480a17-5e3d-4da0-aeec-c421620bff71", game_name="unispy_test_game_name",
                                  room_name="unispy_test_room_name", group_id=0, max_num_user=100, key_values={}, invited_nicks={}, update_time=datetime.now(timezone.utc))
        # PG_SESSION.add(cache)
        # PG_SESSION.commit()
        self.assertRaises(ValidationError,data.get_peer_staging_channels,"unispy_test_game_name", 0)


    