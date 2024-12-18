# region altas

# region auth
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.web_services.requests import *
import backends.protocols.gamespy.web_services.data as data
from servers.web_services.src.modules.auth.contracts.results import LoginProfileResult
from servers.web_services.src.modules.direct2game.contracts.results import GetPurchaseHistoryResult, GetStoreAvailabilityResult


class LoginProfileHandler(HandlerBase):
    _request: LoginProfileRequest

    async def _data_operate(self) -> None:
        self.data = data.get_info(uniquenick=self._request.uniquenick,
                                  namespace_id=self._request.namespace_id,
                                  cdkey=self._request.cdkey,
                                  email=self._request.email)

    async def _result_construct(self) -> None:
        self._result = LoginProfileResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4])


class LoginPS3CertHandler(HandlerBase):
    _request: LoginPS3CertRequest

    async def _data_operate(self) -> None:
        raise NotImplementedError()


class LoginRemoteAuthHandler(HandlerBase):
    _request: LoginRemoteAuthRequest

    async def _data_operate(self) -> None:
        self.data = data.get_info(auth_token=self._request.auth_token)

    async def _result_construct(self) -> None:
        self._result = LoginProfileResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4])


class LoginUniqueNickHandler(HandlerBase):
    _request: LoginUniqueNickRequest

    async def _data_operate(self) -> None:
        self.data = data.get_info(uniquenick=self._request.uniquenick,
                                  namespace_id=self._request.namespace_id)

    async def _result_construct(self) -> None:
        self._result = LoginProfileResult(
            user_id=self.data[0],
            profile_id=self.data[1],
            profile_nick=self.data[2],
            unique_nick=self.data[3],
            cdkey_hash=self.data[4])
# region d2g


class GetPurchaceHistoryHandler(HandlerBase):
    _request: GetPurchaseHistoryRequest

    async def _result_construct(self) -> None:
        self._result = GetPurchaseHistoryResult()


class GetStoreAvailabilityHandler(HandlerBase):
    _request: GetStoreAvailabilityRequest

    async def _result_construct(self) -> None:
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

    async def _data_operate(self) -> None:
        raise NotImplementedError()


class GetMyRecordsHandler(HandlerBase):
    _request: GetMyRecordsRequest

    async def _data_operate(self):
        self.data = data.get_user_data(self._request.table_id)
        raise NotImplementedError()


class SearchForRecordsHandler(HandlerBase):
    _request: SearchForRecordsRequest

    async def _data_operate(self) -> None:
        return await super()._data_operate()
