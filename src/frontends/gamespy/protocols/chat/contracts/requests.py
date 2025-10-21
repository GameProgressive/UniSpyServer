from frontends.gamespy.protocols.chat.abstractions.handler import (
    ChannelRequestBase,
    MessageRequestBase,
)
from frontends.gamespy.protocols.chat.aggregates.enums import (
    GetKeyRequestType,
    ModeName,
    ModeOperation,
    ModeRequestType,
    TopicRequestType,
)

import re
from frontends.gamespy.library.extentions.string_extentions import (
    convert_keystr_to_list,
    convert_kvstring_to_dictionary,
)
from frontends.gamespy.protocols.chat.abstractions.contract import RequestBase
from frontends.gamespy.protocols.chat.aggregates.enums import (
    LoginRequestType,
    WhoRequestType,
)
from frontends.gamespy.protocols.chat.aggregates.exceptions import ChatException
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    NickNameInUseException,
)

# General


class CdkeyRequest(RequestBase):
    cdkey: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) < 1:
            raise ChatException("The number of IRC cmdParams are incorrect.")

        self.cdkey = self._cmd_params[0]


class CryptRequest(RequestBase):
    version_id: str
    gamename: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) < 3:
            raise ChatException(
                "The number of IRC params in CRYPT request is incorrect."
            )
        self.version_id = self._cmd_params[1]
        self.gamename = self._cmd_params[2]


class InviteRequest(RequestBase):
    channel_name: str
    nick_name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) > 2:
            raise ChatException(
                "The number of IRC cmd params in Invite request is incorrect."
            )
        self.channel_name = self._cmd_params[0]
        self.nick_name = self._cmd_params[1]


class ListLimitRequest(RequestBase):
    max_number_of_channels: int
    filter: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 2:
            raise ChatException(
                "The number of IRC cmd params in ListLimit request is incorrect."
            )
        try:
            self.max_number_of_channels = int(self._cmd_params[0])
        except Exception:
            raise ChatException("The max number format is incorrect.")

        self.filter = self._cmd_params[1]


class ListRequest(RequestBase):
    is_searching_channel: bool = False
    is_searching_user: bool = False
    filter: str

    def parse(self):
        super().parse()
        if self._cmd_params is None or len(self._cmd_params) == 0:
            raise ChatException("The Search filter is missing.")

        self.is_searching_channel = True
        self.filter = self._cmd_params[0]


class LoginPreAuth(RequestBase):
    auth_token: str
    partner_challenge: str

    def parse(self):
        super().parse()
        self.auth_token = self._cmd_params[0]
        self.partner_challenge = self._cmd_params[1]


class LoginRequest(RequestBase):
    request_type: LoginRequestType
    namespace_id: int
    nick_name: str
    email: str
    unique_nick: str
    password_hash: str

    def parse(self):
        super().parse()
        try:
            self.namespace_id = int(self._cmd_params[0])
        except Exception:
            raise ChatException("The namespaceid format is incorrect.")

        if self._cmd_params[1] == "*":
            self.request_type = LoginRequestType.NICK_AND_EMAIL_LOGIN
            self.password_hash = self._cmd_params[2]
            assert isinstance(self._long_param, str)
            if self._long_param.count("@") != 2:
                raise ChatException("The profile nick format is incorrect.")

            profile_nick_index = self._long_param.index("@")
            self.nick_name = self._long_param[0:profile_nick_index]
            self.email = self._long_param[profile_nick_index + 1:]
            return

        self.request_type = LoginRequestType.UNIQUE_NICK_LOGIN
        self.unique_nick = self._cmd_params[1]
        self.password_hash = self._cmd_params[2]


class NickRequest(RequestBase):
    _invalid_chars = "#@$%^&()~"
    nick_name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) == 1:
            self.nick_name = self._cmd_params[0]
        elif self._long_param is None:
            assert isinstance(self._long_param, str)
            self.nick_name = self._long_param
        else:
            raise ChatException("NICK request is invalid.")

        for c in self._invalid_chars:
            if c in self.nick_name:
                raise NickNameInUseException(
                    self.nick_name,
                    self.nick_name,
                    f"The nick name: {self.nick_name} contains invalid character.",
                )


class PingRequest(RequestBase):
    pass


class PongRequest(RequestBase):
    echo_message: str

    def parse(self):
        super().parse()
        if self._long_param is None:
            raise ChatException("Echo message is missing.")
        self.echo_message = self._long_param


class QuitRequest(RequestBase):
    reason: str

    def parse(self):
        super().parse()
        if self._long_param is None:
            raise ChatException("Quit reason is missing.")

        self.reason = self._long_param


class RegisterNickRequest(RequestBase):
    namespace_id: int
    unique_nick: str
    cdkey: str

    def parse(self):
        super().parse()
        assert isinstance(self._cmd_params, list)
        self.namespace_id = int(self._cmd_params[0])
        self.unique_nick = self._cmd_params[1]
        self.cdkey = self._cmd_params[2]


class SetKeyRequest(RequestBase):
    key_values: dict[str, str]

    def parse(self):
        super().parse()
        if self._long_param is None:
            raise ChatException("The keys and values are missing.")

        self.key_values = convert_kvstring_to_dictionary(self._long_param)


class UserIPRequest(RequestBase):
    remote_ip: str


class UserRequest(RequestBase):
    user_name: str
    local_ip_address: str
    server_name: str
    name: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)

    def parse(self):
        super().parse()
        if len(self._cmd_params) == 3:
            self.user_name = self._cmd_params[0]
            self.local_ip_address = self._cmd_params[1]
            self.server_name = self._cmd_params[2]
        else:
            self.local_ip_address = self._cmd_params[0]
            self.server_name = self._cmd_params[1]

        assert isinstance(self._long_param, str)
        self.name = self._long_param


class WhoIsRequest(RequestBase):
    nick_name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 1:
            raise ChatException(
                "The number of IRC cmd params in WHOIS request is incorrect."
            )

        self.nick_name = self._cmd_params[0]


class WhoRequest(RequestBase):
    request_type: WhoRequestType
    channel_name: str
    nick_name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 1:
            raise ChatException(
                "The number of IRC cmd params in WHO request is incorrect."
            )

        if "#" in self._cmd_params[0]:
            self.request_type = WhoRequestType.GET_CHANNEL_USER_INFO
            self.channel_name = self._cmd_params[0]
            return

        self.request_type = WhoRequestType.GET_USER_INFO
        self.nick_name = self._cmd_params[0]


class GetKeyRequest(RequestBase):
    is_get_all_user: bool = False
    nick_name: str
    cookie: str
    unknown_cmd_param: str
    keys: list[str]

    def parse(self) -> None:
        super().parse()
        if len(self._cmd_params) < 2:
            raise ChatException(
                "The number of IRC cmd params in GETKEY request is incorrect."
            )

        if self._long_param is None:
            raise ChatException(
                "The number of IRC cmd params in GETKEY request is incorrect."
            )

        self.nick_name = self._cmd_params[0]
        self.cookie = self._cmd_params[1]
        self.unknown_cmd_param = self._cmd_params[2]

        self._long_param = self._long_param[: len(self._long_param)]
        if self.nick_name == "*":
            self.is_get_all_user = True

        self.keys = convert_keystr_to_list(self._long_param)


# region Channel


class GetChannelKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "GETCHANKEY %s %s 0 :", channel, cookie);
    """

    cookie: str
    keys: list

    def parse(self):
        super().parse()

        if len(self._cmd_params) != 3:
            raise ChatException("The cmdParams number is invalid.")

        if self._long_param is None or self._long_param[-1] != "\0":
            raise ChatException("The longParam number is invalid.")
        self.cookie = self._cmd_params[1]
        self.keys = self._long_param.strip("\\").rstrip("\0").split("\\")


class GetCKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "GETCKEY %s %s %s 0 :", channel, nick, cookie);
    """

    nick_name: str | None
    cookie: str
    keys: list
    request_type: GetKeyRequestType

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 4:
            raise ChatException("The number of IRC parameters are incorrect.")
        if self._long_param is None:
            raise ChatException("The IRC long parameter is incorrect.")

        self.nick_name = self._cmd_params[1]
        if self.nick_name == "*":
            self.request_type = GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE
        else:
            self.request_type = GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE

        self.cookie = self._cmd_params[2]

        if "\0" not in self._long_param and "\\" not in self._long_param:
            raise ChatException("The key provide is incorrect.")

        self.keys = self._long_param.strip("\\").rstrip("\0").split("\\")


class GetUdpRelayRequest(ChannelRequestBase):
    pass


class JoinRequest(ChannelRequestBase):
    password: str | None

    def __init__(self, raw_request):
        super().__init__(raw_request)
        self.password = None

    def parse(self):
        super().parse()
        if len(self._cmd_params) > 2:
            raise ChatException("The number of IRC parameters are incorrect.")

        if len(self._cmd_params) == 2:
            self.password = self._cmd_params[1]


class KickRequest(ChannelRequestBase):
    kickee_nick_name: str
    reason: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 2:
            raise ChatException("The number of IRC parameters are incorrect.")
        self.kickee_nick_name = self._cmd_params[1]

        if self._long_param is None:
            raise ChatException("The IRC long parameters is missing.")

        self.reason = self._long_param


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
    mode_operations: dict[str, str]
    """
    <mode_name>,<operation>
    o,+
    i,+
    """
    nick_name: str
    user_name: str
    limit_number: int
    mode_flag: str
    password: str | None

    def parse(self):
        super().parse()
        self.mode_operations = {}
        if len(self._cmd_params) == 1:
            self.request_type = ModeRequestType.GET_CHANNEL_MODES
        elif len(self._cmd_params) == 2 or len(self._cmd_params) == 3:
            self.request_type = ModeRequestType.SET_CHANNEL_MODES
            self.mode_flag = self._cmd_params[1]
            mode_flags = [
                s for s in re.split(r"(?=\+|\-)", self.mode_flag) if s.strip()
            ]
            mode_flags = list(filter(None, mode_flags))
            self.process_mode_flags(mode_flags)
        else:
            raise ChatException("The number of IRC parameters are incorrect.")

    def process_mode_flags(self, mode_flags: list[str]):
        for flag in mode_flags:
            try:
                operation = ModeOperation(flag[0])
                flag_name = ModeName(flag[1:])
            except Exception:
                continue
            match flag_name:
                case (
                    ModeName.OPERATOR_ABEY_CHANNEL_LIMITS
                    | ModeName.TOPIC_CHANGE_BY_OPERATOR_FLAG
                    | ModeName.EXTERNAL_MESSAGES_FLAG
                    | ModeName.MODERATED_CHANNEL_FLAG
                    | ModeName.SECRET_CHANNEL_FLAG
                    | ModeName.INVITED_ONLY
                    | ModeName.PRIVATE_CHANNEL_FLAG
                    | ModeName.USER_QUIET_FLAG
                    | ModeName.CHANNEL_PASSWORD
                ):
                    self.mode_operations[flag_name.value] = operation.value
                case ModeName.CHANNEL_USER_LIMITS:
                    if operation == ModeOperation.SET:
                        self.channel_name = self._cmd_params[0]
                        self.limit_number = int(self._cmd_params[2])
                    else:
                        self.channel_name = self._cmd_params[0]
                    self.mode_operations[flag_name.value] = operation.value
                case ModeName.BAN_ON_USER:
                    self.channel_name = self._cmd_params[0]
                    if len(self._cmd_params) == 3:
                        self.nick_name = self._cmd_params[2]
                    self.mode_operations[flag_name.value] = operation.value
                case ModeName.CHANNEL_OPERATOR | ModeName.USER_VOICE_PERMISSION:
                    self.channel_name = self._cmd_params[0]
                    self.user_name = self._cmd_params[2]
                    self.mode_operations[flag_name.value] = operation.value

    @staticmethod
    def build(channel_name: str):
        """
        build the irc request for get the channel modes
        """
        raw = f"MODE {channel_name}"
        return raw


class NamesRequest(ChannelRequestBase):
    @staticmethod
    def build(channel_name: str):
        """
        build raw request to get channel mode
        """
        raw = f"NAMES {channel_name}"
        return raw


class PartRequest(ChannelRequestBase):
    reason: str = "Unknown reason"

    def __init__(self, raw_request: str | None = None) -> None:
        if raw_request is not None:
            super().__init__(raw_request)

    def parse(self):
        super().parse()

        if self._long_param is None:
            raise ChatException("The reason of living channel is missing.")
        self.reason = self._long_param


class SetChannelKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "SETCHANKEY %s :", channel);
    """

    key_value_string: str
    key_values: dict[str, str]

    def parse(self):
        super().parse()
        if self._long_param is None:
            raise ChatException("Channel keys and values are missing.")
        self._long_param = self._long_param[1:]
        self.key_value_string = self._long_param
        self.key_values = convert_kvstring_to_dictionary(self.key_value_string)


class SetCKeyRequest(ChannelRequestBase):
    """
    sprintf(buffer, "SETCKEY %s %s :", channel, user);
    """

    nick_name: str
    cookie: str
    is_broadcast: bool
    key_value_string: str
    key_values: dict[str, str]

    def parse(self) -> None:
        super().parse()
        if self._cmd_params is None:
            raise ChatException(
                "The cmdParams from SETCKEY request are missing.")

        if self._long_param is None:
            raise ChatException(
                "The longParam from SETCKEY request is missing.")

        self.channel_name = self._cmd_params[0]
        self.nick_name = self._cmd_params[1]
        self.key_value_string = self._long_param[1:]
        self.key_values = convert_kvstring_to_dictionary(self.key_value_string)
        is_broadcast = False
        for key in self.key_values:
            if "b_" in key:
                is_broadcast = True
                break
        if is_broadcast:
            self.cookie = "BCAST"
            self.is_broadcast = True


class SetGroupRequest(ChannelRequestBase):
    group_name: str

    def parse(self) -> None:
        super().parse()
        if len(self._cmd_params) != 1:
            raise ChatException(
                "the number of IRC cmd params in GETKEY request is incorrect."
            )
        self.group_name = self._cmd_params[0]


class TopicRequest(ChannelRequestBase):
    channel_topic: str
    request_type: TopicRequestType

    def parse(self) -> None:
        super().parse()
        if self._long_param is None:
            self.request_type = TopicRequestType.GET_CHANNEL_TOPIC
        else:
            self.request_type = TopicRequestType.SET_CHANNEL_TOPIC
            self.channel_topic = self._long_param


#  region Message


class AtmRequest(MessageRequestBase):
    pass


class NoticeRequest(MessageRequestBase):
    pass


class PrivateRequest(MessageRequestBase):
    pass


class UtmRequest(MessageRequestBase):
    pass


# region publish message

class PublishMessageRequest(RequestBase):
    """
    this class is use to send broadcast message to backends
    """

    def parse(self) -> None:
        """
        we do not need to parse any irc request, we just send it to backend
        """
        pass
        # return super().parse()
