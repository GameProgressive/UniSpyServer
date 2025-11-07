from datetime import datetime
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
    data: str
    profile_id: int
    modified: datetime


@final
class GetProfileIdResult(ResultBase):
    profile_id: int


@final
class SetPlayerDataResult(ResultBase):
    profile_id: int
    modified: datetime
