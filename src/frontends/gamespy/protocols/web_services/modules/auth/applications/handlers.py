from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.modules.auth.contracts.requests import (
    CreateUserAccountRequest,
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
    CreateUserAccountResponse,
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
    CreateUserAccountResult,
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
    _response: LoginProfileWithGameIdResponse

    def __init__(self, client: Client, request: LoginProfileWithGameIdRequest) -> None:
        super().__init__(client, request)


class LoginPs3CertHandler(CmdHandlerBase):
    _request: LoginPs3CertRequest
    _result: LoginPs3CertResult
    _response: LoginPs3CertResponse

    def __init__(self, client: Client, request: LoginPs3CertRequest) -> None:
        super().__init__(client, request)


class LoginPs3CertWithGameIdHandler(CmdHandlerBase):
    _request: LoginPs3CertWithGameIdRequest
    _result: LoginPs3CertResult
    _response: LoginPs3CertWithGameIdResponse

    def __init__(self, client: Client, request: LoginPs3CertRequest) -> None:
        super().__init__(client, request)


class LoginRemoteAuthHandler(CmdHandlerBase):
    _request: LoginRemoteAuthRequest
    _result: LoginRemoteAuthResult
    _response: LoginRemoteAuthResponse

    def __init__(self, client: Client, request: LoginRemoteAuthRequest) -> None:
        super().__init__(client, request)
        self._result_cls = LoginRemoteAuthResult
        self._response_cls = LoginRemoteAuthResponse


class LoginRemoteAuthWithGameIdHandler(CmdHandlerBase):
    _request: LoginRemoteAuthWithGameIdRequest
    _result: LoginRemoteAuthResult
    _response: LoginRemoteAuthWithGameIdResponse

    def __init__(self, client: Client, request: LoginRemoteAuthWithGameIdRequest) -> None:
        super().__init__(client, request)


class LoginUniqueNickHandler(CmdHandlerBase):
    _request: LoginUniqueNickRequest
    _result: LoginUniqueNickResult
    _response: LoginUniqueNickResponse

    def __init__(self, client: Client, request: LoginUniqueNickRequest) -> None:
        super().__init__(client, request)


class LoginUniqueNickWithGameIdHandler(CmdHandlerBase):
    _request: LoginUniqueNickWithGameIdRequest
    _result: LoginUniqueNickResult
    _response: LoginUniqueNickWithGameIdResponse

    def __init__(self, client: Client, request: LoginUniqueNickWithGameIdRequest) -> None:
        assert isinstance(request, LoginUniqueNickWithGameIdRequest)
        super().__init__(client, request)


class CreateUserAccountHandler(CmdHandlerBase):
    _request: CreateUserAccountRequest
    _result: CreateUserAccountResult
    _response: CreateUserAccountResponse

    def __init__(self, client: Client, request: CreateUserAccountRequest) -> None:
        assert isinstance(request, CreateUserAccountRequest)
        super().__init__(client, request)
