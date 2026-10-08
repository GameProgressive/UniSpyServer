from typing import final
from frontends.gamespy.library.extentions.gamespy_utils import is_email_format_correct
from frontends.gamespy.library.extentions.password_encoder import process_password
from frontends.gamespy.protocols.presence_search_player.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import (
    SearchType,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPParseException,
)


@final
class CheckRequest(RequestBase):
    # \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\

    nick: str
    password: str
    email: str
    partner_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "CheckRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["password"] = process_password(data.request_dict)
        data_dict["nick"] = data.request_dict["nick"]
        data_dict["email"] = data.request_dict["email"]
        data_dict["partner_id"] = data.request_dict.get("partner_id", 1)
        data = CheckRequest(**data_dict)
        return data


class NewUserRequest(RequestBase):
    product_id: int
    game_port: int | None
    cd_key: str | None
    nick: str
    email: str
    password: str
    partner_id: int
    game_name: str
    uniquenick: str | None

    @staticmethod
    def gs_parse(raw_request: str) -> "NewUserRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["password"] = process_password(data.request_dict)
        data_dict["nick"] = data.request_dict["nick"]
        data_dict["email"] = data.request_dict["email"]
        data_dict["uniquenick"] = data.request_dict.get("uniquenick")
        data_dict["other_data"] = NewUserRequest.parse_other_info(data)
        # ! fixme
        other_data = NewUserRequest.parse_other_info(data)
        data = NewUserRequest(**data_dict)
        return data

    @staticmethod
    def parse_other_info(data: RequestBase) -> dict:
        other_data = {}
        other_data["partner_id"] = int(data.request_dict.get("partnerid", 0))
        other_data["product_id"] = int(data.request_dict.get("productid", 0))
        other_data["game_name"] = data.request_dict.get("gamename")
        other_data["game_port"] = data.request_dict.get("port")
        other_data["cd_key"] = data.request_dict.get("cdkey")
        return other_data


@final
class NicksRequest(RequestBase):
    password: str
    email: str
    is_require_uniquenicks: bool

    @staticmethod
    def gs_parse(raw_request: str) -> "NicksRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["password"] = process_password(data.request_dict)
        data_dict["email"] = data.request_dict["email"]
        if "pass" in data.request_dict.keys():
            data_dict["is_require_uniquenicks"] = False
        else:
            data_dict["is_require_uniquenicks"] = True
        data = NicksRequest(**data_dict)
        return data


@final
class OthersListRequest(RequestBase):
    profile_ids: list[int]

    @staticmethod
    def gs_parse(raw_request: str) -> "OthersListRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["profile_ids"] = [
            int(opid) for opid in data.request_dict["opids"].strip("|").split("|")
        ]
        data = OthersListRequest(**data_dict)
        return data


@final
class OthersRequest(RequestBase):
    profile_id: int
    game_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "OthersRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["profile_id"] = int(data.request_dict["profileid"])

        data = OthersRequest(**data_dict)
        return data


@final
class SearchRequest(RequestBase):
    skip_num: int
    request_type: SearchType
    game_name: str
    profile_id: int
    partner_id: int
    email: str | None
    nick: str | None
    uniquenick: str | None
    session_key: str
    firstname: str
    lastname: str
    icquin: str

    @staticmethod
    def gs_parse(raw_request: str) -> "SearchRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["skip_num"] = 0
        data_dict["session_key"] = data.request_dict["sesskey"]
        data_dict["firstname"] = data.request_dict["firstname"]
        data_dict["lastname"] = data.request_dict["lastname"]
        data_dict["icquin"] = data.request_dict["icquin"]
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["profile_id"] = int(data.request_dict["profileid"])
        data_dict["partner_id"] = data.request_dict.get("partnerid", 0)
        data_dict["skip_num"] = int(data.request_dict["skip"])
        data_dict["uniquenick"] = data.request_dict.get("uniquenick")

        data_dict["nick"] = data.request_dict.get("nick")
        data_dict["email"] = data.request_dict.get("email")

        if data_dict["uniquenick"] and data.namespace_id:
            data_dict["request_type"] = SearchType.UNIQUENICK_NAMESPACEID_SEARCH
        elif data_dict["nick"] and data_dict["email"]:
            data_dict["request_type"] = SearchType.NICK_EMAIL_SEARCH
        elif data_dict["nick"]:
            data_dict["request_type"] = SearchType.NICK_SEARCH
        elif data_dict["email"]:
            data_dict["request_type"] = SearchType.EMAIL_SEARCH

        data = SearchRequest(**data_dict)
        return data


@final
class SearchUniqueRequest(RequestBase):
    uniquenick: str
    namespace_ids: list[int]

    @staticmethod
    def gs_parse(raw_request: str) -> "SearchUniqueRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["uniquenick"] = data.request_dict["uniquenick"]
        data_dict["namespace_ids"] = [
            int(namespace_id)
            for namespace_id in data.request_dict["namespaces"].lstrip(",").split(",")
        ]

        data = SearchUniqueRequest(**data_dict)
        return data


@final
class UniqueSearchRequest(RequestBase):
    preferred_nick: str
    game_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "UniqueSearchRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["preferred_nick"] = data.request_dict["preferrednick"]
        data_dict["game_name"] = data.request_dict["gamename"]
        data = UniqueSearchRequest(**data_dict)
        return data


from pydantic import EmailStr


@final
class ValidRequest(RequestBase):
    email: EmailStr

    @staticmethod
    def gs_parse(raw_request: str) -> "ValidRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["email"] = data.request_dict["email"]
        data = ValidRequest(**data_dict)
        return data
