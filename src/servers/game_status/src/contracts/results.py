from typing import final
from servers.game_status.src.abstractions.contracts import ResultBase


@final
class AuthGameResult(ResultBase):
    session_key: str


@final
class AuthPlayerResult(ResultBase):
    profile_id: int


@final
class GetPlayerDataResult(ResultBase):
    keyvalues: dict[str, str]


@final
class GetProfileIdResult(ResultBase):
    profile_id: int
