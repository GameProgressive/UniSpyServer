import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.web_services.modules.auth.contracts.requests as fnt_auth
import frontends.gamespy.protocols.web_services.modules.sake.contracts.requests as fnt_sake
import frontends.gamespy.protocols.web_services.modules.direct2game.contracts.requests as fnt_d2g

import frontends.gamespy.protocols.web_services.abstractions.contracts as fnt


class WebRequestBase(lib.RequestBase):
    data:fnt.RequestBase


# region Sake


class SakeRequestBase(WebRequestBase):
    data:fnt_sake.RequestBase


class CreateRecordRequest(SakeRequestBase):
    data:fnt_sake.CreateRecordRequest

class DeleteRecordRequest(SakeRequestBase):
    data:fnt_sake.DeleteRecordRequest


class GetMyRecordsRequest(SakeRequestBase):
    data:fnt_sake.GetMyRecordsRequest


class GetRandomRecordsRequest(SakeRequestBase):
    data:fnt_sake.GetRandomRecordsRequest


class GetRecordLimitRequest(SakeRequestBase):
    data:fnt_sake.GetRecordLimitRequest


class GetSpecificRecordsRequest(SakeRequestBase):
    data:fnt_sake.GetSpecificRecordsRequest


class RateRecordRequest(SakeRequestBase):
    data:fnt_sake.RateRecordRequest


class SearchForRecordsRequest(SakeRequestBase):
    data:fnt_sake.SearchForRecordsRequest



class UpdateRecordRequest(SakeRequestBase):
    data:fnt_sake.UpdateRecordRequest


class GetRecordCountRequest(SakeRequestBase):
    data:fnt_sake.GetRecordCountRequest


# region Auth


class AuthRequestBase(WebRequestBase):
    data:fnt.RequestBase


class LoginProfileRequest(AuthRequestBase):
    data:fnt_auth.LoginProfileRequest


class LoginPs3CertRequest(AuthRequestBase):
    data:fnt_auth.LoginPs3CertRequest


class LoginRemoteAuthRequest(AuthRequestBase):
    data:fnt_auth.LoginRemoteAuthRequest


class LoginUniqueNickRequest(AuthRequestBase):
    data:fnt_auth.LoginUniqueNickRequest


class CreateUserAccountRequest(AuthRequestBase):
    data:fnt_auth.CreateUserAccountRequest



# region D2G


class Direct2GameRequestBase(WebRequestBase):
    data:fnt.RequestBase


class GetPurchaseHistoryRequest(Direct2GameRequestBase):
    data:fnt_d2g.GetPurchaseHistoryRequest


class GetStoreAvailabilityRequest(Direct2GameRequestBase):
    data:fnt_d2g.GetStoreAvailabilityRequest

