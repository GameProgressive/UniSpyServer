from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.web_services.modules.auth.contracts.results import CreateUserAccountResult, LoginProfileResult, LoginPs3CertResult, LoginRemoteAuthResult, LoginUniqueNickResult
from frontends.gamespy.protocols.web_services.modules.direct2game.contracts.results import GetPurchaseHistoryResult
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import CreateRecordResult, DeleteRecordResult, GetMyRecordsResult, SearchForRecordsResult, UpdateRecordResult

# region Auth


class LoginProfileResponse(DataResponse):
    result: LoginProfileResult


class LoginPS3CertRepsonse(DataResponse):
    result: LoginPs3CertResult


class LoginRemoteAuthRepsonse(DataResponse):
    result: LoginRemoteAuthResult


class LoginUniqueNickResponse(DataResponse):
    result: LoginUniqueNickResult


class GetPurchaceHistoryResponse(DataResponse):
    result: GetPurchaseHistoryResult


class CreateUserAccountResponse(DataResponse):
    result: CreateUserAccountResult


# class GetTargettedAdResponse(DataResponse):
#     result: GetTargettedAdResult

# region Sake


class CreateRecordResponse(DataResponse):
    result: CreateRecordResult


class UpdateRecordResponse(DataResponse):
    result: UpdateRecordResult


class GetMyRecordsResponse(DataResponse):
    result: GetMyRecordsResult


class SearchForRecordsResponse(DataResponse):
    result: SearchForRecordsResult


class DeleteRecordResponse(DataResponse):
    result: DeleteRecordResult
