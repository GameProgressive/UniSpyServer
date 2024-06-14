from servers.webservices.abstractions.handler import CmdHandlerBase
from servers.webservices.modules.auth.abstractions.contracts import LoginResultBase
from servers.webservices.modules.auth.contracts.requests import (
    LoginProfileRequest,
    LoginProfileWithGameIdRequest,
    LoginPs3CertRequest,
    LoginPs3CertWithGameIdRequest,
    LoginRemoteAuthRequest,
    LoginRemoteAuthWithGameIdRequest,
    LoginUniqueNickRequest,
    LoginUniqueNickWithGameIdRequest,
)
from servers.webservices.modules.auth.contracts.responses import (
    LoginProfileResponse,
    LoginProfileWithGameIdResponse,
    LoginRemoteAuthResponse,
    LoginRemoteAuthWithGameIdResponse,
    LoginUniqueNickResponse,
    LoginUniqueNickWithGameIdResponse,
)
from servers.webservices.modules.auth.contracts.results import (
    LoginProfileResult,
    LoginPs3CertResult,
    LoginRemoteAuthResult,
)


class LoginProfileHandler(CmdHandlerBase):
    _request: LoginProfileRequest
    _result: LoginProfileResult

    def _response_construct(self) -> None:
        self._response = LoginProfileResponse(self._request, self._result)


class LoginProfileWithGameIdHandler(CmdHandlerBase):
    _request: LoginProfileWithGameIdRequest
    _result: LoginProfileResult

    def _response_construct(self) -> None:
        self._response = LoginProfileWithGameIdResponse(self._request, self._result)


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
        self._response = LoginRemoteAuthResponse(self._request, self._result)


class LoginRemoteAuthWithGameIdHandler(CmdHandlerBase):
    _request: LoginRemoteAuthWithGameIdRequest

    def _response_construct(self) -> None:
        self._response = LoginRemoteAuthWithGameIdResponse(self._request, self._result)


class LoginUniqueNickHandler(CmdHandlerBase):
    _request: LoginUniqueNickRequest

    def _response_construct(self) -> None:
        self._response = LoginUniqueNickResponse(self._request, self._result)


class LoginUniqueNickWithGameIdHandler(CmdHandlerBase):
    _request: LoginUniqueNickWithGameIdRequest

    def _response_construct(self) -> None:
        self._response = LoginUniqueNickWithGameIdResponse(self._request, self._result)
