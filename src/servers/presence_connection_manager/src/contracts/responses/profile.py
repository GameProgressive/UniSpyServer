from library.src.extentions.gamespy_ramdoms import StringType, generate_random_string
from servers.presence_connection_manager.src.abstractions.contracts import ResponseBase
from servers.presence_connection_manager.src.contracts.requests.profile import (
    GetProfileRequest,
    NewProfileRequest,
    RegisterNickRequest,
)
from servers.presence_connection_manager.src.contracts.results.profile import (
    GetProfileResult,
    NewProfileResult,
)


class GetProfileResponse(ResponseBase):
    _result: GetProfileResult
    _request: GetProfileRequest

    def __init__(self, request: GetProfileRequest, result: GetProfileResult):
        assert isinstance(request, GetProfileRequest)
        assert isinstance(result, GetProfileResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = (
            f"\\pi\\profileid\\{self._result.user_profile.profile_id}"
            + f"\\nick\\{self._result.user_profile.nick}"
            + f"\\uniquenick\\{self._result.user_profile.unique_nick}"
            + f"\\email\\{self._result.user_profile.email}"
            + f"\\firstname\\{self._result.user_profile.firstname}"
            + f"\\lastname\\{self._result.user_profile.lastname}"
            + f"\\icquin\\{self._result.user_profile.icquin}"
            + f"\\homepage\\{self._result.user_profile.homepage}"
            + f"\\zipcode\\{self._result.user_profile.zipcode}"
            + f"\\countrycode\\{self._result.user_profile.countrycode}"
            + f"\\lon\\{self._result.user_profile.longitude}"
            + f"\\lat\\{self._result.user_profile.latitude}"
            + f"\\loc\\{self._result.user_profile.location}"
        )

        birth_str = (
            (self._result.user_profile.birthday << 24)
            or (self._result.user_profile.birthmonth << 16)
            or self._result.user_profile.birthyear
        )
        self.sending_buffer += (
            f"\\birthday\\{birth_str}"
            f"\\sex\\{self._result.user_profile.sex}"
            + f"\\publicmask\\{self._result.user_profile.publicmask}"
            + f"\\aim\\{self._result.user_profile.aim}"
            + f"\\picture\\{self._result.user_profile.picture}"
            + f"\\ooc{self._result.user_profile.occupationid}"
            + f"\\ind\\{self._result.user_profile.industryid}"
            + f"\\inc\\{self._result.user_profile.incomeid}"
            + f"\\mar\\{self._result.user_profile.marriedid}"
            + f"\\chc\\{self._result.user_profile.childcount}"
            + f"\\i1\\{self._result.user_profile.interests1}"
            + f"\\o1\\{self._result.user_profile.ownership1}"
            + f"\\conn\\{self._result.user_profile.connectiontype}"
            + f"\\sig\\+{generate_random_string(10, StringType.HEX)}"
            + f"\\id\\{self._request.operation_id}\\final\\"
        )


class NewProfileResponse(ResponseBase):
    _request: NewProfileRequest
    _result: NewProfileResult

    def __init__(self, request: NewProfileRequest, result: NewProfileResult):
        assert isinstance(request, NewProfileRequest)
        assert isinstance(result, NewProfileResult)
        super().__init__(request, result)

    def build(self):
        # fmt: off
        self.sending_buffer = f"\\npr\\\\profileid\\{self.sending_buffer}\\id\\{self._request.operation_id}\\final\\"
        # fmt: on


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
        # fmt: off
        self.sending_buffer = f"\\rn\\\\id\\{self._request.operation_id}\\final\\" 
        # fmt: on
