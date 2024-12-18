from pydantic import BaseModel
from library.src.exceptions.general import UniSpyException
from servers.presence_connection_manager.src.aggregates.enums import PublicMasks
from typing import Optional, final
from library.src.extentions.gamespy_utils import convert_to_key_value
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus, UserStatusInfo
from servers.presence_connection_manager.src.aggregates.enums import GPStatusCode
from servers.presence_search_player.src.aggregates.exceptions import GPParseException
from typing import final
from library.src.extentions.gamespy_utils import is_email_format_correct
from library.src.extentions.password_encoder import process_password
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.aggregates.enums import (
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
from servers.presence_search_player.src.aggregates.exceptions import (
    GPParseException,
)

# region General
EXTRA_INFO_DICT: dict[str, type] = {
    "firstname": str,
    "lastname": str,
    "publicmask": int,
    "latitude": float,
    "longitude": float,
    "aim": str,
    "picture": int,
    "occupationid": int,
    "incomeid": int,
    "industryid": int,
    "marriedid": int,
    "childcount": int,
    "interests1": int,
    "ownership1": int,
    "connectiontype": int,
    "sex": int,
    "zipcode": str,
    "countrycode": str,
    "homepage": str,
    "birthday": int,
    "birthmonth": int,
    "birthyear": int,
    "icquin": int,
    "quietflags": int,
    "streetaddr": str,
    "streeaddr": str,
    "city": str,
    "cpubrandid": int,
    "cpuspeed": int,
    "memory": int,
    "videocard1string": str,
    "videocard1ram": int,
    "videocard2string": str,
    "videocard2ram": int,
    "subscription": int,
    "adminrights": int
}


def validate_extra_infos(info_dict: dict) -> dict:
    """
    validate and create a dict of extra info
    """
    assert isinstance(info_dict, dict)
    extra_infos = {}
    for key, value in info_dict.items():
        if key in EXTRA_INFO_DICT and key not in extra_infos:
            if not isinstance(value, EXTRA_INFO_DICT[key]):
                converted_value = value
            else:
                try:
                    value_type = EXTRA_INFO_DICT[key]
                    converted_value = value_type(value)
                except Exception as e:
                    raise UniSpyException(f"value conversion failed: {e}")
            extra_infos[key] = converted_value
    return extra_infos


@final
class KeepAliveRequest(RequestBase):
    client_ip: str
    client_port: int
    pass


@final
class LoginRequest(RequestBase):
    user_challenge: str
    response: str
    unique_nick: str
    user_data: str
    namespace_id: int
    auth_token: str
    nick: str
    email: str
    product_id: int
    type: LoginType
    sdk_revision_type: SdkRevisionType
    game_port: int
    user_id: int
    profile_id: int
    partner_id: int
    game_name: str
    quiet_mode_flags: int
    firewall: bool

    def __init__(self, raw_request):
        super().__init__(raw_request)

    def parse(self):
        super().parse()

        if "challenge" not in self.request_dict:
            raise GPParseException("challenge is missing")

        if "response" not in self.request_dict:
            raise GPParseException("response is missing")

        self.user_challenge = self.request_dict["challenge"]
        self.response = self.request_dict["response"]

        if (
            "uniquenick" in self.request_dict
            and "namespaceid" in self.request_dict
        ):
            namespace_id = int(self.request_dict["namespaceid"])
            self.type = LoginType.UNIQUENICK_NAMESPACE_ID
            self.unique_nick = self.request_dict["uniquenick"]
            self.user_data = self.unique_nick
            self.namespace_id = namespace_id
        elif "authtoken" in self.request_dict:
            self.type = LoginType.AUTH_TOKEN
            self.auth_token = self.request_dict["authtoken"]
            self.user_data = self.auth_token
        elif "user" in self.request_dict:
            self.type = LoginType.NICK_EMAIL
            self.user_data = self.request_dict["user"]
            pos = self.user_data.index("@")
            if pos == -1 or pos < 1 or (pos + 1) >= len(self.user_data):
                raise GPParseException("user format is incorrect")
            self.nick = self.user_data[:pos]
            self.email = self.user_data[pos + 1:]
            if "namespaceid" in self.request_dict:
                namespace_id = int(self.request_dict["namespaceid"])
                self.namespace_id = namespace_id
        else:
            raise GPParseException("Unknown login method detected.")

        self.parse_other_data()

    def parse_other_data(self):
        if "userid" in self.request_dict:
            user_id = int(self.request_dict["userid"])
            self.user_id = user_id

        if "profileid" in self.request_dict:
            profile_id = int(self.request_dict["profileid"])
            self.profile_id = profile_id

        if "partnerid" in self.request_dict:
            partner_id = int(self.request_dict["partnerid"])
            self.partner_id = partner_id
        else:
            self.partner_id = 0

        if "sdkrevision" in self.request_dict:
            sdk_revision_type = int(self.request_dict["sdkrevision"])
            self.sdk_revision_type = SdkRevisionType(sdk_revision_type)

        if "gamename" in self.request_dict:
            self.game_name = self.request_dict["gamename"]

        if "port" in self.request_dict:
            game_port = int(self.request_dict["port"])
            self.game_port = game_port

        if "productid" in self.request_dict:
            product_id = int(self.request_dict["productid"])
            self.product_id = product_id

        if "firewall" in self.request_dict:
            self.firewall = bool(self.request_dict["firewall"])

        if "quiet" in self.request_dict:
            quiet = int(self.request_dict["quiet"])
            self.quiet_mode_flags = QuietModeType(quiet)


@final
class LogoutRequest(RequestBase):
    pass


@final
class NewUserRequest(RequestBase):
    product_id: int
    game_port: int
    cd_key: str
    has_game_name: bool
    has_product_id: bool
    has_cdkey: bool
    has_partner_id: bool
    has_game_port: bool
    nick: str
    email: str
    password: str
    partner_id: int
    game_name: str
    uniquenick: str

    def parse(self):
        super().parse()
        self.password = process_password(self.request_dict)

        if "nick" not in self.request_dict:
            raise GPParseException("nickname is missing.")
        if "email" not in self.request_dict:
            raise GPParseException("email is missing.")
        if not is_email_format_correct(self.request_dict["email"]):
            raise GPParseException("email format is incorrect.")
        self.nick = self.request_dict["nick"]
        self.email = self.request_dict["email"]

        if "uniquenick" in self.request_dict and "namespaceid" in self.request_dict:
            if "namespaceid" in self.request_dict:
                try:
                    self.namespace_id = int(self.request_dict["namespaceid"])
                except ValueError:
                    raise GPParseException("namespaceid is incorrect.")

            self.uniquenick = self.request_dict["uniquenick"]
        self.parse_other_info()

    def parse_other_info(self):
        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
                self.has_partner_id_flag = True
            except ValueError:
                raise GPParseException("partnerid is incorrect.")
        else:
            self.partner_id = 0  # set default partner id to 0 means gamespy

        if "productid" in self.request_dict:
            try:
                self.product_id = int(self.request_dict["productid"])
                self.has_product_id_flag = True
            except ValueError:
                raise GPParseException("productid is incorrect.")

        if "gamename" in self.request_dict:
            self.has_game_name_flag = True
            self.game_name = self.request_dict["gamename"]

        if "port" in self.request_dict:
            try:
                self.game_port = int(self.request_dict["port"])
                self.has_game_port_flag = True
            except ValueError:
                raise GPParseException("port is incorrect.")

        if "cdkey" in self.request_dict:
            self.has_cd_key_enc_flag = True
            self.cd_key = self.request_dict["cdkey"]

# region Buddy


@final
class AddBuddyRequest(RequestBase):
    friend_profile_id: int
    reason: str

    def parse(self):
        super().parse()
        if (
            ("sesskey" not in self.request_dict)
            or ("newprofileid" not in self.request_dict)
            or ("reason" not in self.request_dict)
        ):
            raise GPParseException("addbuddy request is invalid.")
        try:
            self.friend_profile_id = int(self.request_dict["newprofileid"])
        except:
            raise GPParseException("newprofileid format is incorrect.")

        self.reason = self.request_dict["reason"]


@final
class DelBuddyRequest(RequestBase):
    friend_profile_id: int

    def parse(self):
        super().parse()
        if "delprofileid" not in self.request_dict:
            raise GPParseException("delprofileid is missing.")

        try:
            self.friend_profile_id = int(self.request_dict["delprofileid"])
        except:
            raise GPParseException("delprofileid format is incorrect.")


@final
class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    session_key: str
    """the invite target profile id"""

    def parse(self):
        super().parse()
        if "productid" not in self.request_dict:
            raise GPParseException("productid is missing.")

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing.")

        try:
            self.product_id = int(self.request_dict["productid"])
        except ValueError:
            raise GPParseException("productid format is incorrect.")

        try:
            self.profile_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect.")

        self.session_key = self.request_dict["sesskey"]


@final
class StatusInfoRequest(RequestBase):
    namespace_id: int
    status_info: UserStatusInfo
    profile_id: int

    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            self.raw_request = raw_request

    def parse(self):
        super().parse()

        if (
            "state" not in self.request_dict
            or "hostip" not in self.request_dict
            or "hprivip" not in self.request_dict
            or "qport" not in self.request_dict
            or "hport" not in self.request_dict
            or "sessflags" not in self.request_dict
            or "rechstatus" not in self.request_dict
            or "gametype" not in self.request_dict
            or "gamevariant" not in self.request_dict
            or "gamemapname" not in self.request_dict
        ):
            raise GPParseException("StatusInfo request is invalid.")

        self.status_info.status_state = self.request_dict["state"]
        self.status_info.host_ip = self.request_dict["hostip"]
        self.status_info.host_private_ip = self.request_dict["hprivip"]

        try:
            self.status_info.query_report_port = int(
                self.request_dict["qport"])
            self.status_info.host_port = int(self.request_dict["hport"])
            self.status_info.session_flags = self.request_dict["sessflags"]
        except ValueError:
            raise GPParseException(
                "qport, hport, or sessflags format is incorrect.")

        if "namespace_id" in self.request_dict:
            self.namespace_id = int(self.request_dict["namespaceid"])
        else:
            self.namespace_id = 0

        self.status_info.rich_status = self.request_dict["rechstatus"]
        self.status_info.game_type = self.request_dict["gametype"]
        self.status_info.game_variant = self.request_dict["gamevariant"]
        self.status_info.game_map_name = self.request_dict["gamemapname"]


@final
class StatusRequest(RequestBase):
    status: UserStatus
    is_get: bool

    def parse(self):
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self.request_dict.keys())[0]

        if "status" not in self.request_dict:
            raise GPParseException("status is missing.")

        if "statstring" not in self.request_dict:
            raise GPParseException("statstring is missing.")

        if "locstring" not in self.request_dict:
            raise GPParseException("locstring is missing.")

        try:
            status_code = int(self.request_dict["status"])
            self.status = UserStatus(
                location_string=self.request_dict["locstring"], status_string=self.request_dict['statstring'], current_status=GPStatusCode(status_code))
        except ValueError:
            raise GPParseException("status format is incorrect.")


# region Profile

@final
class AddBlockRequest(RequestBase):
    taget_id: int

    def parse(self):
        super().parse()

        if "profileid" not in self.request_dict:
            raise GPParseException("profileid is missing")

        try:
            self.taget_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")


@final
class GetProfileRequest(RequestBase):
    profile_id: int
    session_key: str

    def parse(self):
        super().parse()

        if "profileid" not in self.request_dict:
            raise GPParseException("profileid is missing")

        try:
            self.profile_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]


@final
class NewProfileRequest(RequestBase):
    is_replace_nick_name: bool
    session_key: str
    new_nick: str
    old_nick: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "replace" in self.request_dict:
            if (
                "oldnick" not in self.request_dict
                and "nick" not in self.request_dict
            ):
                raise GPParseException("oldnick or nick is missing.")

            if "oldnick" in self.request_dict:
                self.old_nick = self.request_dict["oldnick"]
            if "nick" in self.request_dict:
                self.new_nick = self.request_dict["nick"]

            self.is_replace_nick_name = True
        else:
            if "nick" not in self.request_dict:
                raise GPParseException("nick is missing.")

            self.new_nick = self.request_dict["nick"]
            self.is_replace_nick_name = False


@final
class RegisterCDKeyRequest(RequestBase):
    session_key: str
    cdkey_enc: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "cdkeyenc" not in self.request_dict:
            raise GPParseException("cdkeyenc is missing")

        self.cdkey_enc = self.request_dict["cdkeyenc"]


@final
class RegisterNickRequest(RequestBase):
    unique_nick: str
    session_key: str
    partner_id: int

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "uniquenick" not in self.request_dict:
            raise GPParseException("uniquenick is missing")

        self.unique_nick = self.request_dict["uniquenick"]

        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is missing")


@final
class UpdateProfileRequest(RequestBase):
    session_key: str
    partner_id: int
    nick: str
    uniquenick: str
    extra_infos: dict

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")
        self.session_key = self.request_dict["sesskey"]

        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is incorrect")

        if "nick" in self.request_dict:
            self.nick = self.request_dict["nick"]

        if "uniquenick" in self.request_dict:
            self.uniquenick = self.request_dict["uniquenick"]

        self.extra_info = validate_extra_infos(self.request_dict)


@final
class UpdateUserInfoRequest(RequestBase):
    extra_infos: dict[str, str]

    def parse(self):
        super().parse()
        self.extra_infos = validate_extra_infos(self.request_dict)
