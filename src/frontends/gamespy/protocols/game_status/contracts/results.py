from typing import final
from frontends.gamespy.protocols.game_status.abstractions.contracts import ResultBase


@final
class AuthGameResult(ResultBase):
    session_key: str
    game_name: str


@final
class AuthPlayerResult(ResultBase):
    profile_id: int


@final
class GetPlayerDataResult(ResultBase):
    keyvalues: dict[str, str]
    profile_id: int


@final
class GetProfileIdResult(ResultBase):
    profile_id: int
