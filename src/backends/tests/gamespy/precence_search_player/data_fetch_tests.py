# the tests related to database operations
from unittest import TestCase
from backends.library.database.pg_orm import ENGINE, Profiles, Users
import backends.protocols.gamespy.presence_search_player.data as data
from sqlalchemy.orm import Session


class DataFetchTests(TestCase):
    def test_verify_email(self) -> None:
        with Session(ENGINE) as session:
            result1 = data.verify_email("spyguy@unispy.net", session)
            self.assertFalse(result1)
            result2 = data.verify_email("spyguy@gamespy.com", session)
            self.assertTrue(result2)

    def test_verify_email_and_password(self):
        with Session(ENGINE) as session:
            result1 = data.verify_email_and_password(
                "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d91", session
            )
            self.assertFalse(result1)
            result2 = data.verify_email_and_password(
                "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d9b", session
            )
            self.assertTrue(result2)

    def test_get_profile_id(self):
        with Session(ENGINE) as session:
            result1 = data.get_profile_id(
                "spyguy@gamespy.com",
                "4a7d1ed414474e4033ac29ccb8653d9b",
                "spyguy1",
                1,
                session,
            )
            self.assertIsNone(result1)
            result2 = data.get_profile_id(
                "spyguy@gamespy.com",
                "4a7d1ed414474e4033ac29ccb8653d9b",
                "spyguy",
                1,
                session,
            )
            self.assertIsNotNone(result2)
            self.assertEqual(result2, 1)

    def test_get_users(self):
        with Session(ENGINE) as session:
            result1 = data.get_user("spyguy@gamespy.com", session)
            self.assertIsNotNone(result1)
            self.assertEqual(type(result1), Users)
            result2 = data.get_user("spyguy_not_user@gamespy.com", session)
            self.assertIsNone(result2)

    def test_get_profile(self):
        with Session(ENGINE) as session:
            result1 = data.get_profile(1, "spyguy", session)
            self.assertEqual(type(result1), Profiles)

            result2 = data.get_profile(1, "spyguy_not_profile", session)
            self.assertIsNone(result2)

    def test_get_sub_profile(self):
        pass

    def test_get_nick_and_unique_nick_list(self):
        with Session(ENGINE) as session:
            result = data.get_nick_and_unique_nick_list(
                "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d9b", 0, session
            )
            self.assertIsInstance(result, list)
            self.assertEqual(len(result), 1)
            self.assertIsInstance(result[0], tuple)

    def test_get_matched_profile_info_list(self):
        with Session(ENGINE) as session:
            result = data.get_matched_profile_info_list([1], 0, session)
            self.assertIsInstance(result, list)
            self.assertNotEqual(len(result), 0)

    def test_get_matched_info_by_nick(self):
        with Session(ENGINE) as session:
            result = data.get_matched_info_by_nick("spyguy", session)
            self.assertIsNotNone(result)
            self.assertNotEqual(len(result), 0)

    def test_get_matched_info_by_email(self):
        with Session(ENGINE) as session:
            result = data.get_matched_info_by_email("spyguy@gamespy.com", session)
            self.assertEqual(result[0]["nick"], "spyguy")
            self.assertEqual(result[0]["profile_id"], 1)

    def test_is_uniquenick_exist(self):
        with Session(ENGINE) as session:
            result1 = data.is_uniquenick_exist("spyguy_test", 0, "gmtests", session)
            self.assertTrue(result1)

            result2 = data.is_uniquenick_exist(
                "spyguy_not_uniquenick", 0, "gmtests", session
            )
            self.assertFalse(result2)

    def test_is_email_exist(self):
        with Session(ENGINE) as session:
            result1 = data.is_email_exist("spyguy@gamespy.com", session)
            self.assertTrue(result1)
            result2 = data.is_email_exist("spyguy@gamespy.net", session)
            self.assertFalse(result2)
