# the tests related to database operations
from unittest import TestCase
from backends.library.database.pg_orm import Profiles, Users
import backends.protocols.gamespy.presence_search_player.data as data


class DataFetchTests(TestCase):
    def test_verify_email(self):
        result1 = data.verify_email("spyguy@unispy.net")
        self.assertFalse(result1)
        result2 = data.verify_email("spyguy@gamespy.com")
        self.assertTrue(result2)

    def test_verify_email_and_password(self):
        result1 = data.verify_email_and_password(
            "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d91")
        self.assertFalse(result1)
        result2 = data.verify_email_and_password(
            "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d9b")
        self.assertTrue(result2)

    def test_get_profile_id(self):
        result1 = data.get_profile_id(
            "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d9b", "spyguy1", 1)
        self.assertIsNone(result1)
        result2 = data.get_profile_id(
            "spyguy@gamespy.com", "4a7d1ed414474e4033ac29ccb8653d9b", "spyguy", 1)
        self.assertIsNotNone(result2)
        self.assertEqual(result2, 1)

    def test_get_users(self):
        result1 = data.get_user("spyguy@gamespy.com")
        self.assertIsNotNone(result1)
        self.assertEqual(type(result1), Users)
        result2 = data.get_user("spyguy_not_user@gamespy.com")
        self.assertIsNone(result2)

    def test_get_profile(self):
        result1 = data.get_profile(1, "spyguy")
        self.assertEqual(type(result1), Profiles)

        result2 = data.get_profile(1, "spyguy_not_profile")
        self.assertIsNone(result2)
