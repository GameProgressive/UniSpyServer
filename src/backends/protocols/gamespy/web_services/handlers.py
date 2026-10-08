from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.presence_search_player.handlers import NewUserHandler
from backends.protocols.gamespy.presence_search_player.requests import NewUserRequest
from backends.protocols.gamespy.web_services import data
from backends.protocols.gamespy.web_services.requests import (
    CreateRecordRequest,
    CreateUserAccountRequest,
    DeleteRecordRequest,
    GetMyRecordsRequest,
    GetPurchaseHistoryRequest,
    GetRecordCountRequest,
    GetStoreAvailabilityRequest,
    LoginProfileRequest,
    LoginPs3CertRequest,
    LoginRemoteAuthRequest,
    LoginUniqueNickRequest,
    SearchForRecordsRequest,
    UpdateRecordRequest,
)
from backends.protocols.gamespy.web_services.responses import (
    CreateRecordResponse,
    CreateUserAccountResponse,
    DeleteRecordResponse,
    GetMyRecordsResponse,
    GetRecordCountResponse,
    LoginProfileResponse,
    LoginRemoteAuthRepsonse,
    LoginUniqueNickResponse,
    SearchForRecordsResponse,
    UpdateRecordResponse,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import (
    UserNotFoundException,
)
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
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import (
    CreateRecordResult,
    DeleteRecordResult,
    GetMyRecordsResult,
    GetRecordCountResult,
    SearchForRecordsResult,
    UpdateRecordResult,
)

# region altas

# region auth


class LoginProfileHandler(HandlerBase):
    _request: LoginProfileRequest
    response: LoginProfileResponse

    def _data_operate(self) -> None:
        assert self._request.data.cdkey
        self.data = data.get_info_by_cdkey_email(
            uniquenick=self._request.data.nick,
            namespace_id=self._request.data.namespace_id,
            cdkey=self._request.data.cdkey,
            email=self._request.data.email,
            session=self._session,
        )

        if self.data is None:
            raise UserNotFoundException(
                "No account exists with the provided uniquenick and namespace id.",
                self._request.data.command_name,
            )

    def _result_construct(self) -> None:
        assert self.data is not None
        assert self._request.data.raw_request.headers
        self._result = LoginProfileResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            session_token="example_token",
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.data.version,
            namespace_id=self._request.data.namespace_id,
            partner_code=self._request.data.partner_code,
        )


class LoginPS3CertHandler(HandlerBase):
    _request: LoginPs3CertRequest

    def _data_operate(self) -> None:
        raise NotImplementedError()


class LoginRemoteAuthHandler(HandlerBase):
    _request: LoginRemoteAuthRequest
    response: LoginRemoteAuthRepsonse

    def _data_operate(self) -> None:
        self.data = data.get_info_by_authtoken(
            auth_token=self._request.data.auth_token, session=self._session
        )

        if self.data is None:
            raise UserNotFoundException(
                "No account exists with the provided authtoken.",
                self._request.data.command_name,
            )

    def _result_construct(self) -> None:
        assert self.data is not None
        assert self._request.data.raw_request.headers
        self._result = LoginRemoteAuthResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            session_token="example_token",
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.data.version,
            namespace_id=self._request.data.namespace_id,
            partner_code=self._request.data.partner_code,
        )


class LoginUniqueNickHandler(HandlerBase):
    _request: LoginUniqueNickRequest
    response: LoginUniqueNickResponse

    def _data_operate(self) -> None:
        self.data = data.get_info_by_uniquenick(
            uniquenick=self._request.data.uniquenick,
            namespace_id=self._request.data.namespace_id,
            session=self._session,
        )
        if self.data is None:
            raise UserNotFoundException(
                "No account exists with the provided uniquenick and namespace id.",
                self._request.data.command_name,
            )

    def _result_construct(self) -> None:
        assert self.data is not None
        assert self._request.data.raw_request.headers
        self._result = LoginUniqueNickResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            session_token="example_token",
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4],
            version=self._request.data.version,
            namespace_id=self._request.data.namespace_id,
            partner_code=self._request.data.partner_code,
        )


class CreateUserAccountHandler(HandlerBase):
    _request: CreateUserAccountRequest
    response: CreateUserAccountResponse

    def _data_operate(self) -> None:
        dump = self._request.data.model_dump()
        req = NewUserRequest.model_validate(dump)
        h = NewUserHandler(req)
        h.handle()
        self.data = h._result

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = CreateUserAccountResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            session_token="example_token",
            user_id=self.data.user_id,
            profile_id=self.data.profile_id,
            profile_nick=self._request.data.nick,
            unique_nick=self._request.data.uniquenick,
            version=3,
            namespace_id=self._request.data.namespace_id,
            partner_code=self._request.data.partner_code,
        )


# region d2g


class GetPurchaceHistoryHandler(HandlerBase):
    _request: GetPurchaseHistoryRequest

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = GetPurchaseHistoryResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
        )


class GetStoreAvailabilityHandler(HandlerBase):
    _request: GetStoreAvailabilityRequest

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = GetStoreAvailabilityResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
        )


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

        self._record_id: int = data.create_records(
            table_id=self._request.data.table_id,
            records=self._request.data.records,
            command_name=self._request.data.command_name,
            session=self._session,
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = CreateRecordResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            login_ticket=self._request.data.login_ticket,
            table_id=self._request.data.table_id,
            record_id=self._record_id,
        )


class UpdateRecordHandler(HandlerBase):
    _request: UpdateRecordRequest
    response: UpdateRecordResponse

    def _data_operate(self) -> None:
        self._record_id = data.update_record(
            self._request.data.table_id,
            self._request.data.records,
            self._request.data.command_name,
            self._session,
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = UpdateRecordResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            login_ticket=self._request.data.login_ticket,
            table_id=self._request.data.table_id,
            record_id=self._record_id,
        )


class DeleteRecordHandler(HandlerBase):
    _request: DeleteRecordRequest
    response: DeleteRecordResponse

    def _data_operate(self) -> None:
        data.delete_record(
            self._request.data.table_id, self._request.data.command_name, self._session
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = DeleteRecordResult(
            headers=self._request.data.raw_request.headers,
            login_ticket=self._request.data.login_ticket,
            command_name=self._request.data.command_name,
        )


class GetMyRecordsHandler(HandlerBase):
    """
    todo find sub profile id by login ticket
    ! the records number should match the fileds number
    """

    _request: GetMyRecordsRequest
    response: GetMyRecordsResponse

    def _data_operate(self):
        self._data = data.get_my_records(
            self._request.data.table_id,
            self._request.data.fields,
            self._request.data.command_name,
            self._session,
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = GetMyRecordsResult(
            headers=self._request.data.raw_request.headers,
            command_name=self._request.data.command_name,
            login_ticket=self._request.data.login_ticket,
            records=self._data,
            fields=self._request.data.fields,
        )


class SearchForRecordsHandler(HandlerBase):
    _request: SearchForRecordsRequest
    response: SearchForRecordsResponse

    def _data_operate(self) -> None:
        self._data = data.search_for_record(
            self._request.data.table_id,
            self._request.data.max_value,
            self._request.data.filter,
            self._request.data.fields,
            self._request.data.command_name,
            self._session,
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = SearchForRecordsResult(
            headers=self._request.data.raw_request.headers,
            login_ticket=self._request.data.login_ticket,
            command_name=self._request.data.command_name,
            records_list=self._data,
            fields=self._request.data.fields,
        )


class GetRecordCountHandler(HandlerBase):
    _request: GetRecordCountRequest
    response: GetRecordCountResponse

    def _data_operate(self) -> None:
        self._count = data.count_for_record(
            self._request.data.table_id, self._request.data.command_name, self._session
        )

    def _result_construct(self) -> None:
        assert self._request.data.raw_request.headers
        self._result = GetRecordCountResult(
            headers=self._request.data.raw_request.headers,
            login_ticket=self._request.data.login_ticket,
            command_name=self._request.data.command_name,
            count=self._count,
        )
