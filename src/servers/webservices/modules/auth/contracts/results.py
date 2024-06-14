from servers.webservices.modules.auth.abstractions.contracts import LoginResultBase


class LoginProfileResult(LoginResultBase):
    pass


class LoginPs3CertResult(LoginResultBase):
    auth_token: str
    partner_challenge: str


class LoginRemoteAuthResult(LoginResultBase):
    pass


class LoginUniqueNickResult(LoginResultBase):
    pass
