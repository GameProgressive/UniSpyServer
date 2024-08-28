from servers.game_status.src.abstractions.contracts import ResultBase


class AuthGameResult(ResultBase):
    session_key: str


class AuthPlayerResult(ResultBase):
    profile_id: int


class GetPlayerDataResult(ResultBase):
    keyvalues: dict[str, str]


class GetProfileIdResult(ResultBase):
    profile_id: int
