from frontends.gamespy.protocols.web_services.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import (
    LoginProfileRequest,
    LoginProfileWithGameIdRequest,
    LoginPs3CertRequest,
    LoginPs3CertWithGameIdRequest,
    LoginRemoteAuthRequest,
    LoginRemoteAuthWithGameIdRequest,
    LoginUniqueNickRequest,
    LoginUniqueNickWithGameIdRequest,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.responses import (
    LoginProfileResponse,
    LoginProfileWithGameIdResponse,
    LoginRemoteAuthResponse,
    LoginRemoteAuthWithGameIdResponse,
    LoginUniqueNickResponse,
    LoginUniqueNickWithGameIdResponse,
)
from frontends.gamespy.protocols.web_services.modules.auth.contracts.results import (
    LoginProfileResult,
    LoginPs3CertResult,
    LoginRemoteAuthResult,
    LoginUniqueNickResult,
)


class LoginProfileHandler(CmdHandlerBase):
    _request: LoginProfileRequest
    _result: LoginProfileResult

    def _response_construct(self) -> None:
        self._response = LoginProfileResponse(self._result)


class LoginProfileWithGameIdHandler(CmdHandlerBase):
    _request: LoginProfileWithGameIdRequest
    _result: LoginProfileResult

    def _response_construct(self) -> None:
        self._response = LoginProfileWithGameIdResponse(self._result)


class LoginPs3CertHandler(CmdHandlerBase):
    _request: LoginPs3CertRequest
    _result: LoginPs3CertResult


class LoginPs3CertWithGameIdHandler(CmdHandlerBase):
    _request: LoginPs3CertWithGameIdRequest
    _result: LoginPs3CertResult


class LoginRemoteAuthHandler(CmdHandlerBase):
    _request: LoginRemoteAuthRequest
    _result: LoginRemoteAuthResult

    def _response_construct(self) -> None:
        self._response = LoginRemoteAuthResponse(self._result)


class LoginRemoteAuthWithGameIdHandler(CmdHandlerBase):
    _request: LoginRemoteAuthWithGameIdRequest
    _result: LoginRemoteAuthResult

    def _response_construct(self) -> None:
        self._response = LoginRemoteAuthWithGameIdResponse(self._result)


class LoginUniqueNickHandler(CmdHandlerBase):
    _request: LoginUniqueNickRequest
    _result: LoginUniqueNickResult

    def _response_construct(self) -> None:
        self._response = LoginUniqueNickResponse(self._result)


class LoginUniqueNickWithGameIdHandler(CmdHandlerBase):
    _request: LoginUniqueNickWithGameIdRequest
    _result: LoginUniqueNickResult

    def _response_construct(self) -> None:
        self._response = LoginUniqueNickWithGameIdResponse(self._result)
