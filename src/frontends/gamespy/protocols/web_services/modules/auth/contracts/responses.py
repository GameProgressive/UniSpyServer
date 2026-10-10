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
    pass


class LoginPs3CertResponse(LoginResponseBase):
    _result: LoginPs3CertResult

    def build(self) -> None:
        self._content.add("responseCode", self._result.response_code)
        self._content.add("authToken", self._result.auth_token)
        self._content.add("partnerChallenge", self._result.partner_challenge)
        super().build()


class LoginRemoteAuthResponse(LoginResponseBase):
    pass


class LoginUniqueNickResponse(LoginResponseBase):
    pass


class CreateUserAccountResponse(LoginResponseBase):
    pass
