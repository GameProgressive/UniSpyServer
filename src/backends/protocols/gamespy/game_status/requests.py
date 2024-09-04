from typing import Optional
import backends.library.abstractions.contracts as lib
from servers.game_status.src.enums.general import AuthMethod, PersistStorageType


class RequestBase(lib.RequestBase):
    command_name: str
    raw_request: str
    local_id: Optional[int]
    request_dict: dict[str, str]


class AuthGameRequest(RequestBase):
    game_name: str


class AuthPlayerRequest(RequestBase):
    auth_type: AuthMethod
    profile_id: int

    auth_token: str
    response: str
    cdkey_hash: str
    nick: str


class GetPlayerDataRequest(RequestBase):
    profile_id: str
    storage_type: PersistStorageType
    data_index: int
    is_get_all_data: bool = False
    keys: list[str]


class GetProfileIdRequest(RequestBase):
    nick: str
    keyhash: str


class NewGameRequest(RequestBase):
    is_client_local_storage_available: bool
    challenge: str
    connection_id: int
    session_key: str


class SetPlayerDataRequest(RequestBase):
    profile_id: int
    storage_type: PersistStorageType
    data_index: int
    length: int
    report: str
    data: str


class UpdateGameRequest(RequestBase):
    connection_id: int
    is_done: bool
    is_client_local_storage_available: bool
    game_data: str
    game_data_dict: dict[str, str]
    session_key: str
