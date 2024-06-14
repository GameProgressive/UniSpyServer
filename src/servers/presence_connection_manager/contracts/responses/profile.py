from library.extentions.gamespy_ramdoms import StringType, generate_random_string
from servers.chat.contracts.requests.general import RegisterNickRequest
from servers.presence_connection_manager.abstractions.contracts import ResponseBase
from servers.presence_connection_manager.contracts.requests.profile import GetProfileRequest, NewProfileRequest
from servers.presence_connection_manager.contracts.results.profile import GetProfileResult, NewProfileResult


class GetProfileResponse(ResponseBase):
    _result: GetProfileResult
    _request: GetProfileRequest

    def __init__(self, request: GetProfileRequest, result: GetProfileResult):
        assert isinstance(request, GetProfileRequest)
        assert isinstance(result, GetProfileResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = (
            "\\pi\\profileid\\"
            + str(self._result.user_profile.profile_id)
            + "\\nick\\"
            + self._result.user_profile.nick
            + "\\uniquenick\\"
            + self._result.user_profile.unique_nick
            + "\\email\\"
            + self._result.user_profile.email
            + "\\firstname\\"
            + self._result.user_profile.firstname
            + "\\lastname\\"
            + self._result.user_profile.lastname
            + "\\icquin\\"
            + self._result.user_profile.icquin
            + "\\homepage\\"
            + self._result.user_profile.homepage
            + "\\zipcode\\"
            + self._result.user_profile.zipcode
            + "\\countrycode\\"
            + self._result.user_profile.countrycode
            + "\\lon\\"
            + str(self._result.user_profile.longitude)
            + "\\lat\\"
            + str(self._result.user_profile.latitude)
            + "\\loc\\"
            + self._result.user_profile.location
        )

        birth_str = (
            (self._result.user_profile.birthday << 24)
            | (self._result.user_profile.birthmonth << 16)
            | self._result.user_profile.birthyear
        )
        self.sending_buffer += "\\birthday\\" + str(birth_str)

        self.sending_buffer += "\\sex\\" + self._result.user_profile.sex
        self.sending_buffer += "\\publicmask\\" + self._result.user_profile.publicmask
        self.sending_buffer += "\\aim\\" + self._result.user_profile.aim
        self.sending_buffer += "\\picture\\" + self._result.user_profile.picture
        self.sending_buffer += "\\ooc" + str(self._result.user_profile.occupationid)
        self.sending_buffer += "\\ind\\" + str(self._result.user_profile.industryid)
        self.sending_buffer += "\\inc\\" + str(self._result.user_profile.incomeid)
        self.sending_buffer += "\\mar\\" + str(self._result.user_profile.marriedid)
        self.sending_buffer += "\\chc\\" + str(self._result.user_profile.childcount)
        self.sending_buffer += "\\i1\\" + self._result.user_profile.interests1
        self.sending_buffer += "\\o1\\" + self._result.user_profile.ownership1
        self.sending_buffer += "\\conn\\" + self._result.user_profile.connectiontype
        self.sending_buffer += "\\sig\\+" + generate_random_string(10, StringType.HEX)
        self.sending_buffer += "\\id\\" + str(self._request.operation_id) + "\\final\\"


class NewProfileResponse(ResponseBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, request: NewProfileRequest, result: NewProfileResult):
        assert isinstance(request, NewProfileRequest)
        assert isinstance(result, NewProfileResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = f"\\npr\\\\profileid\\{self.sending_buffer}\\id\\{self._request.operation_id}\\final\\"


class RegisterCDKeyResposne(ResponseBase):
    def __init__(self) -> None:
        pass

    def build(self):
        self.sending_buffer = "\\rc\\\\final\\"


class RegisterNickResponse(ResponseBase):
    _request: RegisterNickRequest

    def __init__(self, request: RegisterNickRequest) -> None:
        assert isinstance(request, RegisterNickRequest)
        self._request = request

    def build(self) -> None:
        self.sending_buffer = f"\\rn\\\\id\\{self._request.operation_id}\\final\\"
