from pydantic import Field, model_validator
import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.game_status.aggregations.enums import (
    AuthMethod,
    PersistStorageType,
)
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException


class RequestBase(lib.RequestBase):
    raw_request: str
    local_id: int
    _request_dict: dict[str, str]


class AuthGameRequest(RequestBase):
    game_name: str


class AuthPlayerRequest(RequestBase):
    auth_type: AuthMethod
    profile_id: int | None = None
    auth_token: str | None = None
    cdkey_hash: str | None = None
    response: str | None = None
    nick: str | None = None


class GetPlayerDataRequest(RequestBase):
    profile_id: int
    storage_type: PersistStorageType
    data_index: int
    is_get_all_data: bool = False
    keys: list[str]


class GetProfileIdRequest(RequestBase):
    nick: str
    key_hash: str


class NewGameRequest(RequestBase):
    is_client_local_storage_available: bool
    challenge: str | None = None
    connection_id: int = Field(
        description="The session key that backend send to client."
    )
    session_key: str = Field(description="The game session key")


class SetPlayerDataRequest(RequestBase):
    profile_id: int
    storage_type: PersistStorageType
    data_index: int
    length: int
    report: str | None = None
    data: str
    is_key_value: bool


class UpdateGameRequest(RequestBase):
    connection_id: int
    is_done: bool
    is_client_local_storage_available: bool
    game_data: str
    game_data_dict: dict[str, str]
    session_key: str
