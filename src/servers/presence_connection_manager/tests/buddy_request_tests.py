import unittest

from servers.presence_connection_manager.src.contracts.requests import (
    AddBuddyRequest,
    DelBuddyRequest,
    InviteToRequest,
    StatusRequest,
)
ADD_BUDDY = "\\addbuddy\\\\sesskey\\0\\newprofileid\\0\\reason\\test\\final\\"
DEL_BUDDY = "\\delbuddy\\\\sesskey\\0\\delprofileid\\0\\final\\"
INVITE_TO = "\\inviteto\\\\sesskey\\0\\productid\\0\\profileid\\0\\final\\"
STATUS = "\\status\\0\\statstring\\test\\locstring\\test\\final\\"

class BuddyRequestTest(unittest.TestCase):


    def test_add_buddy(self) -> None:
        request = AddBuddyRequest(ADD_BUDDY)
        request.parse()
        self.assertEqual(0, request.friend_profile_id)
        self.assertEqual("test", request.reason)

    def test_del_buddy(self) -> None:
        request = DelBuddyRequest(DEL_BUDDY)
        request.parse()
        self.assertEqual(0, request.friend_profile_id)

    def test_invite_to(self) -> None:
        request = InviteToRequest(INVITE_TO)
        request.parse()
        self.assertEqual(0, request.product_id)
        self.assertEqual(0, request.profile_id)

    def test_status_test(self) -> None:
        request = StatusRequest(STATUS)
        request.parse()
        self.assertEqual("test", request.status.status_string)
        self.assertEqual("test", request.status.location_string)


if __name__ == "__main__":
    unittest.main()
