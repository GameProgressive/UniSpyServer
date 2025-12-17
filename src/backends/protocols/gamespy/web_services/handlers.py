# region altas

# region auth
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.presence_search_player.handlers import NewUserHandler
from backends.protocols.gamespy.presence_search_player.requests import NewUserRequest
import backends.protocols.gamespy.web_services.data as data
from backends.protocols.gamespy.web_services.requests import (
    CreateRecordRequest,
    CreateUserAccountRequest,
    GetMyRecordsRequest,
    GetPurchaseHistoryRequest,
    GetStoreAvailabilityRequest,
    LoginPS3CertRequest,
    LoginProfileRequest,
    LoginRemoteAuthRequest,
    LoginUniqueNickRequest,
    SearchForRecordsRequest,
)
from backends.protocols.gamespy.web_services.responses import CreateRecordResponse, CreateUserAccountResponse, GetMyRecordsResponse, LoginProfileResponse, LoginRemoteAuthRepsonse, LoginUniqueNickResponse, SearchForRecordsResponse
from frontends.gamespy.protocols.web_services.modules.auth.contracts.results import (
    CreateUserAccountResult,
    LoginProfileResult,
    LoginRemoteAuthResult,
    LoginUniqueNickResult,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.results import (
    GetPurchaseHistoryResult,
    GetStoreAvailabilityResult,
)


class LoginProfileHandler(HandlerBase):
    _request: LoginProfileRequest
    response: LoginProfileResponse

    def _data_operate(self) -> None:
        self.data = data.get_info_by_cdkey_email(
            uniquenick=self._request.uniquenick,
            namespace_id=self._request.namespace_id,
            cdkey=self._request.cdkey,
            email=self._request.email,
            session=self._session,
        )

    def _result_construct(self) -> None:
        self._result = LoginProfileResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.version,
            namespace_id=self._request.namespace_id,
            partner_code=self._request.partner_code
        )


class LoginPS3CertHandler(HandlerBase):
    _request: LoginPS3CertRequest

    def _data_operate(self) -> None:
        raise NotImplementedError()


class LoginRemoteAuthHandler(HandlerBase):
    _request: LoginRemoteAuthRequest
    response: LoginRemoteAuthRepsonse

    def _data_operate(self) -> None:
        self.data = data.get_info_by_authtoken(
            auth_token=self._request.auth_token, session=self._session
        )

    def _result_construct(self) -> None:
        self._result = LoginRemoteAuthResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.version,
            namespace_id=self._request.namespace_id,
            partner_code=self._request.partner_code
        )


class LoginUniqueNickHandler(HandlerBase):
    _request: LoginUniqueNickRequest
    response: LoginUniqueNickResponse

    def _data_operate(self) -> None:
        self.data = data.get_info_by_uniquenick(
            uniquenick=self._request.uniquenick,
            namespace_id=self._request.namespace_id,
            session=self._session,
        )

    def _result_construct(self) -> None:
        self._result = LoginUniqueNickResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.version,
            namespace_id=self._request.namespace_id,
            partner_code=self._request.partner_code
        )


class CreateUserAccountHandler(HandlerBase):
    _request: CreateUserAccountRequest
    response: CreateUserAccountResponse

    def _data_operate(self) -> None:
        dump = self._request.model_dump()
        dump["operation_id"] = 0
        dump["product_id"] = 0
        req = NewUserRequest.model_validate(dump)
        h = NewUserHandler(req)
        h.handle()
        self.data = h._result

    def _result_construct(self) -> None:
        self._result = CreateUserAccountResult(
            user_id=self.data.user_id,
            profile_id=self.data.profile_id,
            profile_nick=self._request.nick,
            unique_nick=self._request.uniquenick,
            cdkey_hash="",
            version=3,
            namespace_id=self._request.namespace_id,
            partner_code=self._request.partner_code
        )


# region d2g


class GetPurchaceHistoryHandler(HandlerBase):
    _request: GetPurchaseHistoryRequest

    def _result_construct(self) -> None:
        self._result = GetPurchaseHistoryResult()


class GetStoreAvailabilityHandler(HandlerBase):
    _request: GetStoreAvailabilityRequest

    def _result_construct(self) -> None:
        self._result = GetStoreAvailabilityResult()


# region ingamead


class GetTargettedAdHandler(HandlerBase):
    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


class ReportAdUsageRequest(HandlerBase):
    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        raise NotImplementedError()


# region patching and tracking

# region racing

# region sake


class CreateRecordHandler(HandlerBase):
    _request: CreateRecordRequest
    response: CreateRecordResponse

    def _data_operate(self) -> None:
        raise NotImplementedError()


class GetMyRecordsHandler(HandlerBase):
    _request: GetMyRecordsRequest
    response: GetMyRecordsResponse

    def _data_operate(self):
        self.data = data.get_user_data(self._request.table_id, self._session)
        raise NotImplementedError()


class SearchForRecordsHandler(HandlerBase):
    _request: SearchForRecordsRequest
    response: SearchForRecordsResponse

    def _data_operate(self) -> None:
        raise NotImplementedError()
