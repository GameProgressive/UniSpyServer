from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.applications.client import Client
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
    LoginPs3CertResponse,
    LoginPs3CertWithGameIdResponse,
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

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = LoginProfileResult
        self._response_cls = LoginProfileResponse


class LoginProfileWithGameIdHandler(CmdHandlerBase):
    _request: LoginProfileWithGameIdRequest
    _result: LoginProfileResult

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = LoginProfileResult
        self._response_cls = LoginProfileWithGameIdResponse


class LoginPs3CertHandler(CmdHandlerBase):
    _request: LoginPs3CertRequest
    _result: LoginPs3CertResult

    def __init__(self, client: Client, request: LoginPs3CertRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginPs3CertResult
        self._response_cls = LoginPs3CertResponse


class LoginPs3CertWithGameIdHandler(CmdHandlerBase):
    _request: LoginPs3CertWithGameIdRequest
    _result: LoginPs3CertResult

    def __init__(self, client: Client, request: LoginPs3CertRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginPs3CertResult
        self._response_cls = LoginPs3CertWithGameIdResponse


class LoginRemoteAuthHandler(CmdHandlerBase):
    _request: LoginRemoteAuthRequest
    _result: LoginRemoteAuthResult

    def __init__(self, client: Client, request: LoginRemoteAuthRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginRemoteAuthResult
        self._response_cls = LoginRemoteAuthResponse


class LoginRemoteAuthWithGameIdHandler(CmdHandlerBase):
    _request: LoginRemoteAuthWithGameIdRequest
    _result: LoginRemoteAuthResult

    def __init__(self, client: Client, request: LoginRemoteAuthWithGameIdRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginRemoteAuthResult
        self._response_cls = LoginRemoteAuthWithGameIdResponse


class LoginUniqueNickHandler(CmdHandlerBase):
    _request: LoginUniqueNickRequest
    _result: LoginUniqueNickResult

    def __init__(self, client: Client, request: LoginUniqueNickRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginUniqueNickResult
        self._response_cls = LoginUniqueNickResponse


class LoginUniqueNickWithGameIdHandler(CmdHandlerBase):
    _request: LoginUniqueNickWithGameIdRequest
    _result: LoginUniqueNickResult

    def __init__(self, client: Client, request: LoginUniqueNickWithGameIdRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginUniqueNickResult
        self._response_cls = LoginUniqueNickWithGameIdResponse
