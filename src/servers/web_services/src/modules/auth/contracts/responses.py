from servers.web_services.src.modules.auth.abstractions.general import LoginResponseBase
from servers.web_services.src.modules.auth.contracts.requests import (
    LoginProfileRequest,
    LoginProfileWithGameIdRequest,
)
from servers.web_services.src.modules.auth.contracts.results import (
    LoginProfileResult,
    LoginPs3CertResult,
)


class LoginProfileResponse(LoginResponseBase):
    _request: LoginProfileRequest
    _result: LoginProfileResult

    def build(self) -> None:
        self._content.add("LoginProfileResult")
        super().build()


class LoginProfileWithGameIdResponse(LoginResponseBase):
    _request: LoginProfileWithGameIdRequest

    def build(self) -> None:
        self._content.add("LoginProfileWithGameIdResult")
        super().build()


class LoginPs3CertResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content.add("LoginPs3CertResult")
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginPs3CertWithGameIdResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content.add("LoginPs3CertWithGameIdResult")
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginRemoteAuthResponse(LoginResponseBase):
    def build(self) -> None:
        self._content.add("LoginRemoteAuthResult")
        super().build()


class LoginRemoteAuthWithGameIdResponse(LoginResponseBase):
    def build(self) -> None:
        self._content.add("LoginRemoteAuthWithGameIdResult")
        super().build()


class LoginUniqueNickResponse(LoginResponseBase):
    def build(self) -> None:
        self._content.add("LoginUniqueNickResult")
        super().build()


class LoginUniqueNickWithGameIdResponse(LoginResponseBase):
    def build(self) -> None:
        self._content.add("LoginUniqueNickWithGameIdResult")
        super().build()
