from servers.presence_connection_manager.src.abstractions.contracts import ResultBase


class LoginDataModel:
    user_id: int
    profile_id: int
    nick: str
    email: str
    unique_nick: str
    password_hash: str
    email_verified_flag: bool
    banned_flag: bool
    namespace_id: int
    sub_profile_id: int


class LoginResult(ResultBase):
    response_proof: str
    data: LoginDataModel


class NewUserResult(ResultBase):
    user_id: int
    profile_id: int
