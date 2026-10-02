from typing import Any, final

from pydantic import BaseModel

from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.game_status.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.game_status.aggregations.enums import (
    AuthMethod,
    PersistStorageType,
)
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException


@final
class AuthGameRequest(RequestBase):
    game_name: str
    response: str | None
    port: int | None
    local_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "AuthGameRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict.update(port=None)
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["response"] = data.request_dict.get("response")
        data_dict["port"] = data.request_dict.get("port")
        data = AuthGameRequest(**data_dict)
        return data


class AuthPlayerRequest(RequestBase):
    auth_type: AuthMethod
    profile_id: int | None
    auth_token: str | None
    response: str | None
    cdkey_hash: str | None
    nick: str | None
    local_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "AuthPlayerRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict.update(
            profile_id=None,
            auth_token=None,
            cdkey_hash=None,
            nick=None,
        )
        if "pid" in data.request_dict and "resp" in data.request_dict:
            data_dict["profile_id"] = data.request_dict["pid"]
            data_dict["response"] = data.request_dict["resp"]
            data_dict["auth_type"] = AuthMethod.PROFILE_ID_AUTH

        elif "authtoken" in data.request_dict and "response" in data.request_dict:
            data_dict["auth_token"] = data.request_dict["authtoken"]
            data_dict["response"] = data.request_dict["response"]
            data_dict["auth_type"] = AuthMethod.PARTNER_ID_AUTH

        elif "keyhash" in data.request_dict and "nick" in data.request_dict:
            data_dict["cdkey_hash"] = data.request_dict["keyhash"]
            data_dict["nick"] = data.request_dict["nick"]
            data_dict["response"] = data.request_dict["keyhash"]
            data_dict["auth_type"] = AuthMethod.CDKEY_AUTH
        else:
            raise GSException("unknown authp request type")
        data = AuthPlayerRequest(**data_dict)
        return data


@final
class GetPlayerDataRequest(RequestBase):
    profile_id: int
    storage_type: PersistStorageType
    data_index: int
    is_get_all_data: bool
    keys: list[str]
    local_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "GetPlayerDataRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()

        data_dict["profile_id"] = data.request_dict["pid"]
        data_dict["storage_type"] = data.request_dict["ptype"]
        data_dict["data_index"] = data.request_dict["dindex"]
        key_str = data.request_dict.get("keys")
        data_dict.update(keys=[])
        if not key_str:
            data_dict["is_get_all_data"] = True
        else:
            data_dict["keys"] = key_str.split("\x01")
            data_dict["is_get_all_data"] = False
        data = GetPlayerDataRequest(**data_dict)
        return data


@final
class GetProfileIdRequest(RequestBase):
    nick: str
    key_hash: str
    local_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "GetProfileIdRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick"] = data.request_dict["nick"]
        data_dict["key_hash"] = data.request_dict["keyhash"]
        data = GetProfileIdRequest(**data_dict)
        return data


@final
class NewGameRequest(RequestBase):
    is_client_local_storage_available: bool
    challenge: str | None
    connection_id: int
    """
    client session key
    """
    session_key: str | None
    """
    game session key
    """

    def model_post_init(self, context: Any) -> None:
        super().model_post_init(context)
        if self.challenge is None and self.session_key is None:
            raise GSException("Authentication info missing")

    @staticmethod
    def gs_parse(raw_request: str) -> "NewGameRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict.update(is_client_local_storage_available=True)
        data_dict["session_key"] = data.request_dict.get("sesskey")
        data_dict["challenge"] = data.request_dict.get("challenge")
        data_dict["connection_id"] = int(data.request_dict["connid"])
        data = NewGameRequest(**data_dict)
        return data


@final
class SetPlayerDataRequest(RequestBase):
    """
    request have 2 different data type
    SetPersistData() data type is bytes
    SetPersistDataValues() data type is \\key\\value
    """

    profile_id: int
    data_index: int
    length: int
    local_id: int
    game_data: str
    is_key_value: bool
    storage_type: PersistStorageType

    @staticmethod
    def gs_parse(raw_request: str) -> "SetPlayerDataRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["is_key_value"] = data.request_dict["kv"]
        data_dict["profile_id"] = data.request_dict["pid"]
        data_dict["storage_type"] = data.request_dict["ptype"]
        data_dict["data_index"] = data.request_dict["dindex"]
        data_dict["length"] = int(data.request_dict["length"])
        data_dict["game_data"] = data.request_dict["data"]
        data = SetPlayerDataRequest(**data_dict)
        return data


@final
class UpdateGameRequest(RequestBase):
    session_key: str
    connection_id: int | None
    is_done: bool
    is_local_storage: bool = False
    game_data: str

    @staticmethod
    def gs_parse(raw_request: str) -> "UpdateGameRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_data"] = data.request_dict["gamedata"]
        data_dict["is_done"] = data.request_dict["done"]
        data_dict["session_key"] = data.request_dict["sesskey"]
        data_dict["connection_id"] = data.request_dict.get("connid")
        data_dict["is_local_straoge"] = data.request_dict.get("dl")
        data = UpdateGameRequest(**data_dict)
        return data
