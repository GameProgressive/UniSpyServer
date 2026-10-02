from typing import Any, final

from frontends.gamespy.library.abstractions.contracts import RequestBase
from frontends.gamespy.library.extentions.gamespy_utils import (
    convert_to_key_value,
    is_email_format_correct,
)
from frontends.gamespy.library.extentions.password_encoder import process_password
from frontends.gamespy.protocols.presence_connection_manager.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPException,
    GPParseException,
)

from pydantic import BaseModel, ConfigDict
import frontends.gamespy.protocols.presence_search_player.contracts.requests as psp


class ExtraInfo(BaseModel):
    location: str | None = None
    firstname: str | None = None
    lastname: str | None = None
    publicmask: int | None = None
    latitude: float | None = None
    longitude: float | None = None
    aim: str | None = None
    picture: int | None = None
    occupationid: int | None = None
    incomeid: int | None = None
    industryid: int | None = None
    marriedid: int | None = None
    childcount: int | None = None
    interests1: int | None = None
    ownership1: int | None = None
    connectiontype: int | None = None
    sex: int | None = None
    zipcode: str | None = None
    countrycode: str | None = None
    homepage: str | None = None
    birthday: int | None = None
    birthmonth: int | None = None
    birthyear: int | None = None
    icquin: int | None = None
    quietflags: int | None = None
    streetaddr: str | None = None
    streeaddr: str | None = None
    city: str | None = None
    cpubrandid: int | None = None
    cpushpeed: int | None = None
    memory: int | None = None
    videocard1string: str | None = None
    videocard1ram: int | None = None
    videocard2string: str | None = None
    videocard2ram: int | None = None
    subscription: int | None = None
    adminrights: int | None = None

    model_config = ConfigDict(extra="allow")


@final
class KeepAliveRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "KeepAliveRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data = KeepAliveRequest(**data_dict)
        return data


SDK_MAPPING: list[SdkRevisionType] = [
    SdkRevisionType.GPINEW_AUTH_NOTIFICATION,
    SdkRevisionType.GPINEW_REVOKE_NOTIFICATION,
    SdkRevisionType.GPINEW_STATUS_NOTIFICATION,
    SdkRevisionType.GPINEW_LIST_RETRIEVAL_ON_LOGIN,
    SdkRevisionType.GPIREMOTE_AUTH_IDS_NOTIFICATION,
    SdkRevisionType.GPINEW_CD_KEY_REGISTRATION,
]


@final
class LoginRequest(RequestBase):
    user_challenge: str
    response: str
    user_data: str
    unique_nick: str | None = None
    namespace_id: int | None = None
    auth_token: str | None = None
    nick: str | None = None
    email: str | None = None
    product_id: int
    type: LoginType
    sdk_revision: list[SdkRevisionType]
    game_port: int
    user_id: int
    profile_id: int
    partner_id: int
    """
    partner id default is 0
    """
    game_name: str
    quiet_mode_flags: int
    firewall: bool

    def model_post_init(self, context: Any) -> None:
        super().model_post_init(context)
        if self.type == LoginType.AUTH_TOKEN:
            if self.auth_token is None:
                raise ValueError("auth token is missing")
        if self.type == LoginType.NICK_EMAIL:
            if self.nick is None or self.email is None:
                raise ValueError("nick or email is missing")
        if self.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            if self.unique_nick is None or self.namespace_id is None:
                raise ValueError("uniquenick or namespace is missing")

    @staticmethod
    def gs_parse(raw_request: str) -> "LoginRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        other_data = LoginRequest.parse_other_data(data)
        data_dict["user_challenge"] = data.request_dict["challenge"]
        data_dict["response"] = data.request_dict["response"]
        if "uniquenick" in data.request_dict and "namespaceid" in data.request_dict:
            data_dict["namespace_id"] = data.request_dict["namespaceid"]
            data_dict["type"] = LoginType.UNIQUENICK_NAMESPACE_ID
            data_dict["unique_nick"] = data.request_dict["uniquenick"]
            data_dict["user_data"] = data_dict["unique_nick"]
            data = LoginRequest(**other_data, **data_dict)
            return data
        elif "authtoken" in data.request_dict:
            data_dict["type"] = LoginType.AUTH_TOKEN
            data_dict["auth_token"] = data.request_dict["authtoken"]
            data_dict["user_data"] = data_dict["auth_token"]
            data = LoginRequest(**other_data, **data_dict)
            return data
        elif "user" in data.request_dict:
            data_dict["type"] = LoginType.NICK_EMAIL
            data_dict["nick"], data_dict["email"] = LoginRequest.gs_process_nick_email(
                data.request_dict
            )
            data_dict["user_data"] = data.request_dict["user"]
            data_dict["namespace_id"] = data.request_dict["namespaceid"]
            data = LoginRequest(**other_data, **data_dict)
            return data
        else:
            raise GPParseException("Unknown login method detected.")

    @staticmethod
    def gs_process_nick_email(request_dict: dict) -> tuple[str, str]:
        user_data: str = request_dict["user"]
        pos = user_data.index("@")
        if pos == -1 or pos < 1 or (pos + 1) >= len(user_data):
            raise GPParseException("user format is incorrect")
        nick = user_data[:pos]
        email = user_data[pos + 1 :]
        return nick, email

    @staticmethod
    def parse_other_data(data: RequestBase):
        request_dict = data.request_dict
        other_data = {}

        userid = request_dict.get("userid")
        if userid is not None:
            other_data["user_id"] = userid

        profileid = request_dict.get("profileid")
        if profileid is not None:
            other_data["profile_id"] = profileid

        partnerid = request_dict.get("partnerid", 0)
        other_data["partner_id"] = partnerid

        sdkrevision = request_dict.get("sdkrevision")
        if sdkrevision is not None:
            sdk_revision_type = int(sdkrevision)
            sdk_revision_list = []

            for item in SDK_MAPPING:
                if item & sdk_revision_type:
                    sdk_revision_list.append(item)

            other_data["sdk_revision"] = sdk_revision_list

        gamename = request_dict.get("gamename")
        if gamename is not None:
            other_data["game_name"] = gamename

        port = request_dict.get("port")
        if port is not None:
            other_data["game_port"] = int(port)

        productid = request_dict.get("productid")
        if productid is not None:
            other_data["product_id"] = int(productid)

        firewall = request_dict.get("firewall")
        if firewall is not None:
            other_data["firewall"] = bool(firewall)

        quiet = request_dict.get("quiet")
        if quiet is not None:
            other_data["quiet_mode_flags"] = QuietModeType(int(quiet))

        return other_data


@final
class LogoutRequest(RequestBase):
    sub_profile_id: int
    user_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "LogoutRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["user_id"] = data.request_dict["user_id"]
        data = LogoutRequest(**data_dict)
        return data


@final
class NewUserRequest(psp.NewUserRequest, RequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "NewUserRequest":
        data = psp.NewUserRequest.gs_parse(raw_request)
        data = NewUserRequest(**data.model_dump())
        return data


# region Buddy


class BuddyRequestBase(RequestBase):
    profile_id: int
    namespace_id: int
    operation_id: int


@final
class BuddyListRetriveRequest(BuddyRequestBase):
    pass


@final
class BlockListRetriveRequest(BuddyRequestBase):
    pass


@final
class BuddyMessageFriendAddRequest(BuddyRequestBase):
    """
    retrive add buddy request message
    """

    pass


@final
class BuddyMessageRequest(BuddyRequestBase):
    """
    retrive friend message
    """

    pass


@final
class AuthAddBuddyRequest(RequestBase):
    """
    verify the friend add request after the client accept the friend invitation
    """

    sub_profile_id: int
    receiver_profile_id: int
    sig: str
    auto_sync: bool

    @staticmethod
    def gs_parse(raw_request: str) -> "AuthAddBuddyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["receiver_profile_id"] = int(data.request_dict["fromprofileid"])
        data_dict["sig"] = data.request_dict["sig"]
        data_dict["auto_sync"] = bool(data.request_dict.get("autoSync"))
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = AuthAddBuddyRequest(**data_dict)
        return data


@final
class AddBuddyRequest(RequestBase):
    target_profile_id: int
    sub_profile_id: int
    reason: str
    signature: str | None
    """
    Added a sync requested value that tells the backend to automatically add me and my friend
    """

    @staticmethod
    def gs_parse(raw_request: str) -> "AddBuddyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["target_profile_id"] = data.request_dict["newprofileid"]
        data_dict["reason"] = data.request_dict["reason"]
        data_dict["signature"] = data.request_dict.get("syncrequested")
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = AddBuddyRequest(**data_dict)
        return data


@final
class DelBuddyRequest(RequestBase):
    target_profile_id: int
    sub_profile_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "DelBuddyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["target_profile_id"] = data.request_dict["delprofileid"]
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = DelBuddyRequest(**data_dict)
        return data


@final
class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    sub_profile_id: int
    """the invite target profile id"""

    @staticmethod
    def gs_parse(raw_request: str) -> "InviteToRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["product_id"] = data.request_dict["productid"]
        data_dict["profile_id"] = data.request_dict["profileid"]
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = InviteToRequest(**data_dict)
        return data


@final
class StatusInfoRequest(RequestBase):
    namespace_id: int
    profile_id: int
    status_state: str
    buddy_ip: str
    host_ip: str
    host_private_ip: str
    query_report_port: int
    host_port: int
    session_flags: str
    rich_status: str
    game_type: str
    game_variant: str
    game_map_name: str
    quiet_mode_flags: str
    raw_request: str | None

    @staticmethod
    def gs_parse(raw_request: str) -> "StatusInfoRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["status_state"] = data.request_dict["state"]
        data_dict["host_ip"] = data.request_dict["hostip"]
        data_dict["host_private_ip"] = data.request_dict["hprivip"]
        data_dict["query_report_port"] = data.request_dict["qport"]
        data_dict["host_port"] = data.request_dict["hport"]
        data_dict["session_flags"] = data.request_dict["sessflags"]

        data_dict["namespace_id"] = data.request_dict.get("namespaceid", 0)
        data_dict["rich_status"] = data.request_dict["rechstatus"]
        data_dict["game_type"] = data.request_dict["gametype"]
        data_dict["game_variant"] = data.request_dict["gamevariant"]
        data_dict["game_map_name"] = data.request_dict["gamemapname"]

        data = StatusInfoRequest(**data_dict)
        return data


@final
class BuddyStatusRetrieveRequest(RequestBase):
    profile_id: int
    namespace_id: int


@final
class StatusRequest(RequestBase):
    current_status: GPStatusCode
    location_string: str
    status_string: str
    sub_profile_id: int
    operation_id: int | None = None

    @staticmethod
    def gs_parse(raw_request: str) -> "StatusRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["location_string"] = data.request_dict["locstring"]
        data_dict["status_string"] = data.request_dict["statstring"]
        data_dict["current_status"] = data.request_dict["status"]
        data = StatusRequest(**data_dict)
        return data


# region Profile


@final
class AddBlockRequest(RequestBase):
    target_profile_id: int
    sub_profile_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "AddBlockRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["target_profile_id"] = data.request_dict["profileid"]
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = AddBlockRequest(**data_dict)
        return data


@final
class RemoveBlockRequest(RequestBase):
    sub_profile_id: int
    target_profile_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "RemoveBlockRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["target_profile_id"] = data.request_dict["profileid"]
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]

        data = RemoveBlockRequest(**data_dict)
        return data


@final
class GetProfileRequest(RequestBase):
    profile_id: int
    sub_profile_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "GetProfileRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["profile_id"] = data.request_dict["profileid"]
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data = GetProfileRequest(**data_dict)
        return data


@final
class NewProfileRequest(RequestBase):
    is_replace: bool
    sub_profile_id: int
    new_nick: str
    old_nick: str | None

    @staticmethod
    def gs_parse(raw_request: str) -> "NewProfileRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["old_nick"] = data.request_dict.get("oldnick")
        is_replace = False
        if data_dict["old_nick"] is not None:
            is_replace = True
        data_dict["new_nick"] = data.request_dict["nick"]
        data = NewProfileRequest(
            **data_dict,
            is_replace=is_replace,
        )
        return data


@final
class RegisterCDKeyRequest(RequestBase):
    sub_profile_id: int
    cdkey_enc: str

    @staticmethod
    def gs_parse(raw_request: str) -> "RegisterCDKeyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["cdkey_enc"] = data.request_dict["cdkeyenc"]
        data = RegisterCDKeyRequest(**data_dict)
        return data


@final
class RegisterNickRequest(RequestBase):
    unique_nick: str
    sub_profile_id: int
    partner_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> "RegisterNickRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["unique_nick"] = data.request_dict["uniquenick"]
        data_dict["partner_id"] = data.request_dict["partnerid"]
        data = RegisterNickRequest(**data_dict)
        return data


@final
class UpdateProfileRequest(RequestBase):
    sub_profile_id: int
    partner_id: int
    extra_infos: ExtraInfo

    @staticmethod
    def gs_parse(raw_request: str) -> "UpdateProfileRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        data_dict["partner_id"] = data.request_dict["partnerid"]
        extra_info = ExtraInfo.model_validate(data.request_dict)
        data = UpdateProfileRequest(
            **data_dict,
            extra_infos=extra_info,
        )
        return data


@final
class UpdateUserInfoRequest(RequestBase):
    extra_infos: ExtraInfo
    sub_profile_id: int

    @staticmethod
    def gs_parse(raw_request: str) -> RequestBase:
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["sub_profile_id"] = data.request_dict["sesskey"]
        extra_info = ExtraInfo.model_validate(data.request_dict)
        data = UpdateUserInfoRequest(**data_dict, extra_infos=extra_info)
        return data


# region Peer
@final
class AuthRequest(RequestBase):
    nick: str
    sig: str

    @staticmethod
    def gs_parse(raw_request: str) -> "AuthRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick"] = data.request_dict["nick"]
        data_dict["sig"] = data.request_dict["sig"]
        data = AuthRequest(**data_dict)
        return data
