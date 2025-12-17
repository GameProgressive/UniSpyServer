from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import LoginResponseBase
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import (
    LoginProfileRequest,
    LoginProfileWithGameIdRequest,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.results import (
    LoginProfileResult,
    LoginPs3CertResult,
)


class LoginProfileResponse(LoginResponseBase):
    _request: LoginProfileRequest
    _result: LoginProfileResult

    def build(self) -> None:
        self._content = SoapEnvelop("LoginProfileResult")
        super().build()


class LoginProfileWithGameIdResponse(LoginResponseBase):
    _request: LoginProfileWithGameIdRequest

    def build(self) -> None:
        self._content = SoapEnvelop("LoginProfileWithGameIdResult")
        super().build()


class LoginPs3CertResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content = SoapEnvelop("LoginPs3CertResult")
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginPs3CertWithGameIdResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content = SoapEnvelop("LoginPs3CertWithGameIdResult")
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginRemoteAuthResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop("LoginRemoteAuthResult")
        super().build()


class LoginRemoteAuthWithGameIdResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop("LoginRemoteAuthWithGameIdResult")
        super().build()


class LoginUniqueNickResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop("LoginUniqueNickResult")
        super().build()


class LoginUniqueNickWithGameIdResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop("LoginUniqueNickWithGameIdResult")
        super().build()


class CreateUserAccountResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop("CreateUserAccountResult")
        super().build()
