import unittest
from datetime import datetime, timezone

from pydantic import ValidationError
from sqlalchemy.orm import Session

from backends.library.database.pg_orm import ENGINE, ChatChannelCaches
from backends.protocols.gamespy.query_report import data


class DataFetchTests(unittest.TestCase):
    def test_get_peer_staging_channels(self):
        cache = ChatChannelCaches(
            channel_name="#GSP!unispy_test_game_name!*",
            server_id="b6480a17-5e3d-4da0-aeec-c421620bff71",
            game_name="unispy_test_game_name",
            room_name="unispy_test_room_name",
            group_id=0,
            max_num_user=100,
            update_time=datetime.now(),
        )
        with Session(ENGINE) as session:
            session.add(cache)
            session.commit()
            self.assertRaises(
                ValidationError,
                data.get_peer_staging_channels,
                "unispy_test_game_name",
                0,
                session,
            )
            session.delete(cache)
            session.commit()
