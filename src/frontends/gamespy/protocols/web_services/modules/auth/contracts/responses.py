from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import (
    SoapEnvelop,
)
from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import (
    LoginResponseBase,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import (
    LoginProfileRequest,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.results import (
    LoginProfileResult,
    LoginPs3CertResult,
)


class LoginProfileResponse(LoginResponseBase):
    _request: LoginProfileRequest
    _result: LoginProfileResult

    def build(self) -> None:
        self._content = SoapEnvelop(f"{self._result.command_name}Result")
        super().build()


class LoginPs3CertResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content = SoapEnvelop(f"{self._result.command_name}Result")
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginRemoteAuthResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop(f"{self._result.command_name}Result")
        super().build()


class LoginUniqueNickResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop(f"{self._result.command_name}Result")
        super().build()


class CreateUserAccountResponse(LoginResponseBase):
    def build(self) -> None:
        self._content = SoapEnvelop(f"{self._result.command_name}Result")
        super().build()
