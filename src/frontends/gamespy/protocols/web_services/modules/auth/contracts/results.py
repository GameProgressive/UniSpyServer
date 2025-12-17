from frontends.gamespy.protocols.web_services.modules.auth.abstractions.general import LoginResultBase


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
