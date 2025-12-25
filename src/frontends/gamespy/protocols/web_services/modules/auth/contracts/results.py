
from frontends.gamespy.protocols.web_services.abstractions.contracts import ResultBase
from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import LoginResultBase
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import AuthCode


class LoginProfileResult(LoginResultBase):
    pass


class LoginPs3CertResult(LoginResultBase):
    auth_token: str
    partner_challenge: str



class LoginRemoteAuthResult(LoginResultBase):
    pass


class LoginUniqueNickResult(LoginResultBase):
    pass


class CreateUserAccountResult(LoginResultBase):
    pass


# region Exception
