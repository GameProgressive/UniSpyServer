import unittest

from servers.presence_connection_manager.contracts.requests.profile import (
    AddBlockRequest,
    GetProfileRequest,
    NewProfileRequest,
    RegisterCDKeyRequest,
    RegisterNickRequest,
    UpdateProfileRequest,
)


class ProfileRequestTest(unittest.TestCase):
    ADD_BLOCK = "\\addblock\\profileid\\0\\final\\"
    GET_PROFILE = "\\getprofile\\sesskey\\xxxx\\profileid\\0\\final\\"
    NEW_PROFILE = "\\newprofile\\sesskey\\xxxx\\nick\\spyguy\\id\\1\\final\\"
    NEW_PROFILE_REPLACE = "\\newprofile\\sesskey\\xxxx\\nick\\spyguy2\\replace\\1\\oldnick\\spyguy\\id\\1\\final\\"
    REGISTER_CD_KEY = "\\registercdkey\\sesskey\\xxxx\\cdkeyenc\\xxxx\\id\\1\\final\\"
    REGISTER_NICK = "\\registernick\\sesskey\\xxxx\\uniquenick\\spyguy\\partnerid\\0\\id\\1\\final\\"

    def test_add_block(self) -> None:
        request = AddBlockRequest(ProfileRequestTest.ADD_BLOCK)
        request.parse()
        self.assertEqual(0, request.taget_id)

    def test_get_profile(self) -> None:
        request = GetProfileRequest(ProfileRequestTest.GET_PROFILE)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual(0, request.profile_id)

    def test_new_profile(self) -> None:
        request = NewProfileRequest(ProfileRequestTest.NEW_PROFILE)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual("spyguy", request.new_nick)

    def test_new_profile_replace(self) -> None:
        request = NewProfileRequest(ProfileRequestTest.NEW_PROFILE_REPLACE)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual("spyguy2", request.new_nick)
        self.assertEqual("spyguy", request.old_nick)

    def test_register_cd_key(self) -> None:
        request = RegisterCDKeyRequest(ProfileRequestTest.REGISTER_CD_KEY)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual("xxxx", request.cdkey_enc)

    def test_register_nick(self) -> None:
        request = RegisterNickRequest(ProfileRequestTest.REGISTER_NICK)
        request.parse()
        self.assertEqual("xxxx", request.session_key)
        self.assertEqual("spyguy", request.unique_nick)
        self.assertEqual(0, request.partner_id)

    def test_update_profile(self) -> None:
        crysisWarsRaw = "\\updatepro\\sesskey\\1111\\countrycode\\DE\\birthday\\168232912\\partnerid\\0\\final\\"
        request = UpdateProfileRequest(crysisWarsRaw)
        request.Parse()