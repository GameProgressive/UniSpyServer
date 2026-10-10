import re
from typing import final

from pydantic import field_validator

from frontends.gamespy.library.extentions.string_extentions import (
    convert_keystr_to_list,
    convert_kvstring_to_dictionary,
)
from frontends.gamespy.protocols.chat.abstractions.contract import RequestBase
from frontends.gamespy.protocols.chat.abstractions.handler import (
    ChannelRequestBase,
    MessageRequestBase,
)
from frontends.gamespy.protocols.chat.aggregates.enums import (
    GetKeyRequestType,
    LoginRequestType,
    ModeName,
    ModeOperation,
    ModeRequestType,
    SearchOption,
    TopicRequestType,
    WhoRequestType,
)
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    ChatException,
    NickNameInUseException,
)

# General


@final
class CdkeyRequest(RequestBase):
    cdkey: str

    @staticmethod
    def gs_parse(raw_request: str) -> "CdkeyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["cdkey"] = data.irc.params[0]
        data = CdkeyRequest(**data_dict)
        return data


@final
class CryptRequest(RequestBase):
    version_id: str
    gamename: str

    @staticmethod
    def gs_parse(raw_request: str) -> "CryptRequest":
        data = RequestBase.gs_parse(raw_request)
        if len(data.irc.params) < 3:
            raise ChatException(
                "The number of IRC params in CRYPT request is incorrect."
            )
        data_dict = data.model_dump()
        data_dict["version_id"] = data.irc.params[1]
        data_dict["gamename"] = data.irc.params[2]

        data = CryptRequest(**data_dict)
        return data


@final
class InviteRequest(RequestBase):
    channel_name: str
    nick_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "InviteRequest":
        data = RequestBase.gs_parse(raw_request)
        if len(data.irc.params) > 2:
            raise ChatException(
                "The number of IRC cmd params in Invite request is incorrect."
            )
        data_dict = data.model_dump()
        data_dict["channel_name"] = data.irc.params[0]
        data_dict["nick_name"] = data.irc.params[1]

        data = InviteRequest(**data_dict)
        return data


@final
class ListLimitRequest(RequestBase):
    max_number_of_channels: int
    filter: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ListLimitRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["max_number_of_channels"] = data.irc.params[0]
        data_dict["filter"] = data.irc.params[1]

        data = ListLimitRequest(**data_dict)
        return data


@final
class ListRequest(RequestBase):
    option: SearchOption
    filter: str

    @staticmethod
    def gs_parse(raw_request: str) -> "ListRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["filter"] = data.irc.params[0]
        if "#" in data_dict["filter"]:
            data_dict["option"] = SearchOption.Channel
        else:
            data_dict["option"] = SearchOption.User
        data = ListRequest(**data_dict)
        return data


@final
class LoginPreAuthRequest(RequestBase):
    auth_token: str
    partner_challenge: str

    @staticmethod
    def gs_parse(raw_request: str) -> RequestBase:
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["auth_token"] = data.irc.params[0]
        data_dict["partner_challenge"] = data.irc.params[1]
        data = LoginPreAuthRequest(**data_dict)
        return data


@final
class LoginRequest(RequestBase):
    request_type: LoginRequestType
    namespace_id: int
    nick_name: str | None = None
    email: str | None = None
    unique_nick: str | None = None
    password_hash: str

    @staticmethod
    def gs_parse(raw_request: str) -> "LoginRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["namespace_id"] = int(data.irc.params[0])
        if data.irc.params[1] == "*":
            if data.irc.trailing.count("@") != 2:
                raise ChatException("The profile nick format is incorrect.")
            profile_nick_index = data.irc.trailing.index("@")
            data_dict["request_type"] = LoginRequestType.NICK_AND_EMAIL_LOGIN
            data_dict["password_hash"] = data.irc.params[2]
            data_dict["nick_name"] = data.irc.trailing[0:profile_nick_index]
            data_dict["email"] = data.irc.trailing[profile_nick_index + 1 :]
            data = LoginRequest(**data_dict)
            return data
        else:
            data_dict["request_type"] = LoginRequestType.UNIQUE_NICK_LOGIN
            data_dict["unique_nick"] = data.irc.params[1]
            data_dict["password_hash"] = data.irc.params[2]
            data = LoginRequest(**data_dict)
            return data


INVALID_CHARS = "#@$%^&()~"


@final
class NickRequest(RequestBase):
    nick_name: str

    @field_validator("nick_name")
    def validate_nick_name(cls, value: str) -> str:
        invalid = set(value) & set(INVALID_CHARS)
        if invalid:
            raise ValueError(f"Invalid characters: {', '.join(sorted(invalid))}")
        return value

    @staticmethod
    def gs_parse(raw_request: str) -> "NickRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        if len(data.irc.params) == 1:
            data_dict["nick_name"] = data.irc.params[0]
        elif data.irc.trailing is not None:
            assert isinstance(data.irc.trailing, str)
            data_dict["nick_name"] = data.irc.trailing
        else:
            raise ChatException("NICK request is invalid.")
        return NickRequest(**data_dict)


@final
class PingRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "PingRequest":
        data = RequestBase.gs_parse(raw_request)
        data = PingRequest(**data.model_dump())
        return data


@final
class PongRequest(RequestBase):
    echo_message: str

    @staticmethod
    def gs_parse(raw_request: str) -> "PongRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["echo_msg"] = data.irc.trailing
        data = PongRequest(**data_dict)
        return data


@final
class QuitRequest(RequestBase):
    reason: str

    @staticmethod
    def gs_parse(raw_request: str) -> "QuitRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["reason"] = data.irc.trailing
        data = QuitRequest(**data_dict)
        return data


@final
class RegisterNickRequest(RequestBase):
    namespace_id: int
    unique_nick: str
    cdkey: str

    @staticmethod
    def gs_parse(raw_request: str) -> "RegisterNickRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["namespace_id"] = int(data.irc.params[0])
        data_dict["unique_nick"] = data.irc.params[1]
        data_dict["cdkey"] = data.irc.params[2]

        data = RegisterNickRequest(**data_dict)
        return data


@final
class SetKeyRequest(RequestBase):
    key_values: dict[str, str]

    @staticmethod
    def gs_parse(raw_request: str) -> "SetKeyRequest":
        data = RequestBase.gs_parse(raw_request)
        kvs = convert_kvstring_to_dictionary(data.irc.trailing)
        data_dict = data.model_dump()
        data_dict["key_values"] = kvs
        data = SetKeyRequest(**data_dict)
        return data


@final
class UserIPRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "UserIPRequest":
        data = RequestBase.gs_parse(raw_request)
        data = UserIPRequest(**data.model_dump())
        return data


class UserRequest(RequestBase):
    user_name: str | None
    local_ip_address: str
    server_name: str
    nick_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "UserRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick_name"] = data.irc.trailing
        if len(data.irc.params) == 3:
            data_dict["user_name"] = data.irc.params[0]
            data_dict["local_ip_address"] = data.irc.params[1]
            data_dict["server_name"] = data.irc.params[2]
            data = UserRequest(**data_dict)
        else:
            data_dict["local_ip_address"] = data.irc.params[0]
            data_dict["server_name"] = data.irc.params[1]
            data = UserRequest(**data_dict)
        return data


class WhoIsRequest(RequestBase):
    nick_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "WhoIsRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick_name"] = data.irc.params[0]
        data = WhoIsRequest(**data_dict)
        return data


class WhoRequest(RequestBase):
    request_type: WhoRequestType
    channel_name: str | None
    nick_name: str | None

    @staticmethod
    def gs_parse(raw_request: str) -> "WhoRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        if "#" in data.irc.params[0]:
            data_dict["request_type"] = WhoRequestType.GET_CHANNEL_USER_INFO
            data_dict["channel_name"] = data.irc.params[0]
            data_dict["nick_name"] = None
            data = WhoRequest(**data_dict)
        else:
            data_dict["request_type"] = WhoRequestType.GET_USER_INFO
            data_dict["channel_name"] = None
            data_dict["nick_name"] = data.irc.params[0]
            data = WhoRequest(**data_dict)
        return data


class GetKeyRequest(RequestBase):
    is_get_all_user: bool = False
    nick_name: str
    cookie: str
    unknown_cmd_param: str
    keys: list[str]

    @staticmethod
    def gs_parse(raw_request: str) -> "GetKeyRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick_name"] = data.irc.params[0]
        data_dict["cookie"] = data.irc.params[1]
        data_dict["unknown_cmd_param"] = data.irc.params[2]
        data_dict["is_get_all_user"] = data_dict["nick_name"] == "*"
        data_dict["keys"] = convert_keystr_to_list(data.irc.trailing)
        data = GetKeyRequest(**data_dict)
        return data


# region Channel


class GetChannelKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "GETCHANKEY %s %s 0 :", channel, cookie);
    """

    cookie: str
    keys: list

    @staticmethod
    def gs_parse(raw_request: str) -> "GetChannelKeyRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["cookie"] = data.irc.params[1]
        data_dict["keys"] = data.irc.trailing.strip("\\").rstrip("\0").split("\\")
        data = GetChannelKeyRequest(**data_dict)
        return data


class GetCKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "GETCKEY %s %s %s 0 :", channel, nick, cookie);
    """

    nick_name: str | None
    cookie: str
    keys: list
    request_type: GetKeyRequestType

    @staticmethod
    def gs_parse(raw_request: str) -> "GetCKeyRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["nick_name"] = data.irc.params[1]
        if data_dict["nick_name"] == "*":
            data_dict["request_type"] = GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE
        else:
            data_dict["request_type"] = (
                GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE
            )

        data_dict["cookie"] = data.irc.params[2]
        data_dict["keys"] = data.irc.trailing.strip("\\").rstrip("\0").split("\\")

        data = GetCKeyRequest(**data_dict)
        return data


class GetUdpRelayRequest(ChannelRequestBase):
    pass


class JoinRequest(ChannelRequestBase):
    password: str | None = None

    @staticmethod
    def gs_parse(raw_request: str) -> "JoinRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        if len(data.irc.params) >= 2:
            data_dict["password"] = data.irc.params[1]
        data = JoinRequest(**data_dict)
        return data


class KickRequest(ChannelRequestBase):
    kickee_nick_name: str
    reason: str

    @staticmethod
    def gs_parse(raw_request: str) -> "KickRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        if len(data.irc.params) != 2:
            raise ChatException("The number of IRC parameters are incorrect.")
        data_dict = data.model_dump()
        data_dict["kickee_nick_name"] = data.irc.params[1]

        if data.irc.trailing is None:
            raise ChatException("The IRC long parameters is missing.")

        data_dict["reason"] = data.irc.trailing
        data = KickRequest(**data_dict)
        return data


class ModeRequest(ChannelRequestBase):
    # request:
    # "MODE <nick> +/-q"

    # "MODE <channel name> +/-k <password>"

    # "MODE <channel name> +l <limit number>"
    # "MODE <channel name> -l"

    # "MODE <channel name> +b *!*@<host name>" actually we do not care about this request
    # "MODE <channel name> +/-b <nick name>"

    # "MODE <channel name> +/-co <user name>"
    # "MODE <channel name> +/-cv <user name>"

    # "MODE <channel name> <mode flags>"
    # "MODE <channel name> <mode flags> <limit number>"
    request_type: ModeRequestType
    mode_flags: list[tuple[ModeOperation, ModeName]]
    """
    <mode_name>,<operation>
    o,+
    i,+
    """
    nick_name: str | None = None
    user_name: str | None = None
    limit_number: int | None = None
    password: str | None = None

    @staticmethod
    def gs_parse(raw_request: str) -> "ModeRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        mode_flags = []
        if len(data.irc.params) == 1:
            data_dict["request_type"] = ModeRequestType.GET_CHANNEL_MODES
            data = ModeRequest(**data_dict, mode_flags=[])
        elif len(data.irc.params) == 2 or len(data.irc.params) == 3:
            params = data.irc.params
            mode_flag_str = params[1]
            data_dict.update(
                request_type=ModeRequestType.SET_CHANNEL_MODES,
            )
            mode_flags_raw = [
                (flag[0], flag[1:])
                for flag in re.split(r"(?=[+-])", mode_flag_str)
                if flag.strip()
            ]

            for operation, flag_name in mode_flags_raw:
                mode_flags.append((ModeOperation(operation), ModeName(flag_name)))

            for operation, flag_name in mode_flags:
                match flag_name:
                    case ModeName.CHANNEL_USER_LIMITS if operation == ModeOperation.SET:
                        data_dict["limit_number"] = params[2]
                    case ModeName.BAN_ON_USER if len(params) == 3:
                        data_dict["nick_name"] = params[2]
                    case ModeName.CHANNEL_OPERATOR | ModeName.USER_VOICE_PERMISSION:
                        data_dict["user_name"] = params[2]
            data_dict["mode_flags"] = mode_flags
            data = ModeRequest(**data_dict)
        else:
            raise ChatException("The number of IRC parameters are incorrect.")
        return data


class NamesRequest(ChannelRequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "NamesRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data = NamesRequest(**data.model_dump())
        return data


class PartRequest(ChannelRequestBase):
    reason: str

    @staticmethod
    def gs_parse(raw_request: str) -> "PartRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["reason"] = data.irc.trailing
        data = PartRequest(**data_dict)
        return data


class SetChannelKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "SETCHANKEY %s :", channel);
    BCAST is broadcast flag
    """

    key_values: dict[str, str]
    is_broadcast: bool
    cookie: str | None

    @staticmethod
    def gs_parse(raw_request: str) -> "SetChannelKeyRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        if data.irc.trailing is None:
            raise ChatException("Channel keys and values are missing.")
        data_dict = data.model_dump()
        # skip first char \\
        data_dict["key_values"] = convert_kvstring_to_dictionary(data.irc.trailing[1:])
        data_dict.update(is_broadcast=False, cookie=None)
        for key in data_dict["key_values"]:
            if "b_" in key:
                data_dict["is_broadcast"] = True
                data_dict["cookie"] = "BCAST"
                break

        data = SetChannelKeyRequest(**data_dict)
        return data


class SetCKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "SETCKEY %s %s :", channel, user);
    BCAST is broadcast flag
    """

    nick_name: str
    cookie: str | None
    is_broadcast: bool
    key_values: dict[str, str]

    @staticmethod
    def gs_parse(raw_request: str) -> "SetCKeyRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        if data.irc.params is None:
            raise ChatException("The cmdParams from SETCKEY request are missing.")

        if data.irc.trailing is None:
            raise ChatException("The longParam from SETCKEY request is missing.")
        data_dict = data.model_dump()

        data_dict["nick_name"] = data.irc.params[1]
        data_dict["key_values"] = convert_kvstring_to_dictionary(data.irc.trailing[1:])
        data_dict.update(is_broadcast=False, cookie=None)

        for key in data_dict["key_values"]:
            if "b_" in key:
                data_dict["is_broadcast"] = True
                data_dict["cookie"] = "BCAST"
                break
        data = SetCKeyRequest(**data_dict)
        return data


class SetGroupRequest(ChannelRequestBase):
    group_name: str

    @staticmethod
    def gs_parse(raw_request: str) -> "SetGroupRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["group_name"] = data.irc.params[0]
        data = SetGroupRequest(**data_dict)
        return data


class TopicRequest(ChannelRequestBase):
    channel_topic: str | None
    request_type: TopicRequestType

    @staticmethod
    def gs_parse(raw_request: str) -> "TopicRequest":
        data = ChannelRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        if data.irc.trailing is None:
            data_dict["request_type"] = TopicRequestType.GET_CHANNEL_TOPIC
            data_dict["channel_topic"] = None
        else:
            data_dict["request_type"] = TopicRequestType.SET_CHANNEL_TOPIC
            data_dict["channel_topic"] = data.irc.trailing
        data = TopicRequest(**data_dict)
        return data


#  region Message


class AtmRequest(MessageRequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "AtmRequest":
        data = MessageRequestBase.gs_parse(raw_request)
        data = AtmRequest(**data.model_dump())
        return data


class NoticeRequest(MessageRequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "NoticeRequest":
        data = MessageRequestBase.gs_parse(raw_request)
        data = NoticeRequest(**data.model_dump())
        return data


class PrivateRequest(MessageRequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "PrivateRequest":
        data = MessageRequestBase.gs_parse(raw_request)
        data = PrivateRequest(**data.model_dump())
        return data


class UtmRequest(MessageRequestBase):
    @staticmethod
    def gs_parse(raw_request: str) -> "UtmRequest":
        data = MessageRequestBase.gs_parse(raw_request)
        data = UtmRequest(**data.model_dump())
        return data


# region publish message


class PublishMessageRequest(MessageRequestBase):
    """
    this class is use to send broadcast message to backends
    """
