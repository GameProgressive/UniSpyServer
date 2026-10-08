import unittest

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    AddBlockRequest,
    AddBuddyRequest,
    DelBuddyRequest,
    GetProfileRequest,
    InviteToRequest,
    LoginRequest,
    NewProfileRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    StatusRequest,
    UpdateProfileRequest,
)

ADD_BLOCK = "\\addblock\\\\sesskey\\1234\\profileid\\0\\final\\"
GET_PROFILE = "\\getprofile\\\\sesskey\\xxxx\\profileid\\0\\final\\"
NEW_PROFILE = "\\newprofile\\\\sesskey\\xxxx\\nick\\spyguy\\id\\1\\final\\"
NEW_PROFILE_REPLACE = "\\newprofile\\\\sesskey\\xxxx\\nick\\spyguy2\\replace\\1\\oldnick\\spyguy\\id\\1\\final\\"
REGISTER_CD_KEY = "\\registercdkey\\\\sesskey\\xxxx\\cdkeyenc\\xxxx\\id\\1\\final\\"
REGISTER_NICK = (
    "\\registernick\\\\sesskey\\xxxx\\uniquenick\\spyguy\\partnerid\\0\\id\\1\\final\\"
)


LOGIN_AUTH_TOKEN = "\\login\\\\challenge\\xxxx\\authtoken\\example_auth\\userid\\1\\profileid\\1\\partnerid\\0\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"
LOGIN_UNIQUE_NICK = "\\login\\\\challenge\\xxxx\\uniquenick\\spyguy\\userid\\1\\profileid\\1\\namespaceid\\1\\partnerid\\0\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"
LOGIN_USER = "\\login\\\\challenge\\xxxx\\user\\spyguy@spyguy@gamespy.com\\userid\\0\\profileid\\0\\partnerid\\0\\namespaceid\\1\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"

ADD_BUDDY = "\\addbuddy\\\\sesskey\\0\\newprofileid\\0\\reason\\test\\final\\"
DEL_BUDDY = "\\delbuddy\\\\sesskey\\0\\delprofileid\\0\\final\\"
INVITE_TO = "\\inviteto\\\\sesskey\\0\\productid\\0\\profileid\\0\\final\\"
STATUS = [
    "\\status\\0\\sesskey\\1111\\statstring\\test\\locstring\\test\\final\\",
    "\\status\\1\\sesskey\\1111\\statstring\\Not Ready\\locstring\\gptestc\\final\\",
]


class RequestTests(unittest.TestCase):
    # region General
    def test_login_auth_token(self) -> None:
        request = LoginRequest.gs_parse(LOGIN_AUTH_TOKEN)
        self.assertEqual(LoginType.AUTH_TOKEN, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("example_auth", request.auth_token)
        self.assertEqual(1, request.user_id)
        self.assertEqual(1, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(request.game_port, 0)
        self.assertEqual(request.product_id, 0)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision[0]
        )
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)

    def test_login_unique_nick(self) -> None:
        request = LoginRequest.gs_parse(LOGIN_UNIQUE_NICK)
        self.assertEqual(LoginType.UNIQUENICK_NAMESPACE_ID, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("spyguy", request.unique_nick)
        self.assertEqual(1, request.namespace_id)
        self.assertEqual(1, request.user_id)
        self.assertEqual(1, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(0, request.game_port)
        self.assertEqual(0, request.product_id)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision[0]
        )
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)

    def test_login_user(self) -> None:
        request = LoginRequest.gs_parse(LOGIN_USER)
        self.assertEqual(LoginType.NICK_EMAIL, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("spyguy@gamespy.com", request.email)
        self.assertEqual(1, request.namespace_id)
        self.assertEqual(0, request.user_id)
        self.assertEqual(0, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(0, request.game_port)
        self.assertEqual(0, request.product_id)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision[0]
        )
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)
        # region Profile

    def test_add_block(self) -> None:
        request = AddBlockRequest.gs_parse(ADD_BLOCK)
        self.assertEqual(0, request.target_profile_id)

    def test_get_profile(self) -> None:
        request = GetProfileRequest.gs_parse(GET_PROFILE)
        self.assertEqual("xxxx", request.subprofile_id)
        self.assertEqual(0, request.profile_id)

    def test_new_profile(self) -> None:
        request = NewProfileRequest.gs_parse(NEW_PROFILE)
        self.assertEqual("xxxx", request.subprofile_id)
        self.assertEqual("spyguy", request.new_nick)

    def test_new_profile_replace(self) -> None:
        request = NewProfileRequest.gs_parse(NEW_PROFILE_REPLACE)
        self.assertEqual("xxxx", request.subprofile_id)
        self.assertEqual("spyguy2", request.new_nick)
        self.assertEqual("spyguy", request.old_nick)

    def test_register_cd_key(self) -> None:
        request = RegisterCDKeyRequest.gs_parse(REGISTER_CD_KEY)
        self.assertEqual("xxxx", request.subprofile_id)
        self.assertEqual("xxxx", request.cdkey_enc)

    def test_register_nick(self) -> None:
        request = RegisterNickRequest.gs_parse(REGISTER_NICK)
        self.assertEqual("xxxx", request.subprofile_id)
        self.assertEqual("spyguy", request.unique_nick)
        self.assertEqual(0, request.partner_id)

    def test_update_profile(self) -> None:
        crysisWarsRaw = "\\updatepro\\\\sesskey\\111111\\countrycode\\DE\\birthday\\168232912\\partnerid\\0\\final\\"
        request = UpdateProfileRequest.gs_parse(crysisWarsRaw)

    # region Buddy
    def test_add_buddy(self) -> None:
        request = AddBuddyRequest.gs_parse(ADD_BUDDY)
        self.assertEqual(0, request.target_profile_id)
        self.assertEqual("test", request.reason)

    def test_del_buddy(self) -> None:
        request = DelBuddyRequest.gs_parse(DEL_BUDDY)
        self.assertEqual(0, request.target_profile_id)

    def test_invite_to(self) -> None:
        request = InviteToRequest.gs_parse(INVITE_TO)
        self.assertEqual(0, request.product_id)
        self.assertEqual(0, request.profile_id)

    def test_status_test(self) -> None:
        request = StatusRequest.gs_parse(STATUS[0])
        self.assertEqual("test", request.status_string)
        self.assertEqual("test", request.location_string)
        request = StatusRequest.gs_parse(STATUS[1])
