import unittest
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import LoginRequest
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
LOGIN_AUTH_TOKEN = "\\login\\\\challenge\\xxxx\\authtoken\\xxxx\\userid\\0\\profileid\\0\\partnerid\\0\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"
LOGIN_UNIQUE_NICK = "\\login\\\\challenge\\xxxx\\uniquenick\\spyguy\\userid\\0\\profileid\\0\\namespaceid\\0\\partnerid\\0\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"
LOGIN_USER = "\\login\\\\challenge\\xxxx\\user\\spyguy@spyguy@unispy.org\\userid\\0\\profileid\\0\\partnerid\\0\\namespaceid\\0\\response\\xxxxx\\firewall\\1\\port\\0000\\productid\\0\\gamename\\gmtest\\sdkrevision\\4\\quiet\\0\\id\\1\\final\\"


class GeneralRequestTest(unittest.TestCase):

    def test_login_auth_token(self) -> None:
        request = LoginRequest(LOGIN_AUTH_TOKEN)
        request.parse()
        self.assertEqual(LoginType.AUTH_TOKEN, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("xxxx", request.auth_token)
        self.assertEqual(0, request.user_id)
        self.assertEqual(0, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(request.game_port, 0)
        self.assertEqual(request.product_id, 0)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision_type)
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)

    def test_login_unique_nick(self) -> None:
        request = LoginRequest(LOGIN_UNIQUE_NICK)
        request.parse()
        self.assertEqual(LoginType.UNIQUENICK_NAMESPACE_ID, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("spyguy", request.unique_nick)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual(0, request.user_id)
        self.assertEqual(0, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(0, request.game_port)
        self.assertEqual(0, request.product_id)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision_type)
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)

    def test_login_user(self) -> None:
        request = LoginRequest(LOGIN_USER)
        request.parse()
        self.assertEqual(LoginType.NICK_EMAIL, request.type)
        self.assertEqual("xxxx", request.user_challenge)
        self.assertEqual("spyguy", request.nick)
        self.assertEqual("spyguy@unispy.org", request.email)
        self.assertEqual(0, request.namespace_id)
        self.assertEqual(0, request.user_id)
        self.assertEqual(0, request.profile_id)
        self.assertEqual(0, request.partner_id)
        self.assertEqual("xxxxx", request.response)
        self.assertEqual(True, request.firewall)
        self.assertEqual(0, request.game_port)
        self.assertEqual(0, request.product_id)
        self.assertEqual("gmtest", request.game_name)
        self.assertEqual(
            SdkRevisionType.GPINEW_REVOKE_NOTIFICATION, request.sdk_revision_type)
        self.assertEqual(QuietModeType.SILENCE_NONE, request.quiet_mode_flags)


if __name__ == "__main__":
    unittest.main()
