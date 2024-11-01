from library.src.extentions.string_extentions import convert_kvstring_to_dictionary
from servers.chat.src.abstractions.handler import ChannelRequestBase, MessageRequestBase
from servers.chat.src.aggregates.enums import (
    GetKeyRequestType,
    ModeOperationType,
    ModeRequestType,
    TopicRequestType,
)
from typing import Optional
import re
from library.src.extentions.string_extentions import (
    convert_keystr_to_list,
    convert_kvstring_to_dictionary,
)
from servers.chat.src.abstractions.contract import RequestBase
from servers.chat.src.aggregates.enums import LoginRequestType, WhoRequestType
from servers.chat.src.aggregates.exceptions import ChatException
from servers.chat.src.aggregates.exceptions import NickNameInUseException

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

        except Exception as e:
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

        except Exception as e:
            raise ChatException("The namespaceid format is incorrect.")

        if self._cmd_params[1] == "*":
            self.request_type = LoginRequestType.NICK_AND_EMAIL_LOGIN
            self.password_hash = self._cmd_params[2]
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
    _invalid_chars = "#@$%^&*()~"
    nick_name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) == 1:
            self.nick_name = self._cmd_params[0]
        elif self._long_param is None:
            self.nick_name = self._long_param
        else:
            raise ChatException("NICK request is invalid.")

        for c in self._invalid_chars:
            if c in self.nick_name:
                raise NickNameInUseException(
                    self.nick_name,
                    self.nick_name,
                    f"The nick name: {
                        self.nick_name} contains invalid character.",
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
        self.namespace_id = self._cmd_params[0]
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
    remote_ip_address: str


class UserRequest(RequestBase):
    user_name: str
    host_name: str
    server_name: str
    nick_name: str
    name: str

    def parse(self):
        super().parse()
        if len(self._cmd_params) == 3:
            self.user_name = self._cmd_params[0]
            self.host_name = self._cmd_params[1]
            self.server_name = self._cmd_params[2]
        else:
            self.host_name = self._cmd_params[0]
            self.server_name = self._cmd_params[1]

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
    nick_name: Optional[str]
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
    password: str

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

    # "MODE <channel name> +b" actually we do not care about this request
    # "MODE <channel name> +/-b <nick name>"

    # "MODE <channel name> +/-co <user name>"
    # "MODE <channel name> +/-cv <user name>"

    # "MODE <channel name> <mode flags>"
    # "MODE <channel name> <mode flags> <limit number>"
    request_type: ModeRequestType
    mode_operations: list[ModeOperationType]
    nick_name: str
    user_name: str
    limit_number: int
    mode_flag: str
    password: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.mode_operations = []

    def parse(self):
        if self.raw_request is None:
            return
        super().parse()
        if len(self._cmd_params) == 1:
            self.request_type = ModeRequestType.GET_CHANNEL_MODES
        elif len(self._cmd_params) == 2 or len(self._cmd_params) == 3:
            self.request_type = ModeRequestType.SET_CHANNEL_MODES
            self.mode_flag = self._cmd_params[1]
            modeFlags = [s for s in re.split(
                r"(?=\+|\-)", self.mode_flag) if s.strip()]
            modeFlags = list(filter(None, modeFlags))
            for flag in modeFlags:
                match flag:
                    case "+e":
                        self.mode_operations.append(
                            ModeOperationType.SET_OPERATOR_ABEY_CHANNEL_LIMITS
                        )
                    case "-e":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_OPERATOR_ABEY_CHANNEL_LIMITS
                        )
                    case "+t":
                        self.mode_operations.append(
                            ModeOperationType.SET_TOPIC_CHANGE_BY_OPERATOR_FLAG
                        )
                    case "-t":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_TOPIC_CHANGE_BY_OPERATOR_FLAG
                        )
                    case "+n":
                        self.mode_operations.append(
                            ModeOperationType.ENABLE_EXTERNAL_MESSAGES_FLAG
                        )
                    case "-n":
                        self.mode_operations.append(
                            ModeOperationType.DISABLE_EXTERNAL_MESSAGES_FLAG
                        )
                    case "+m":
                        self.mode_operations.append(
                            ModeOperationType.SET_MODERATED_CHANNEL_FLAG
                        )
                    case "-m":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_MODERATED_CHANNEL_FLAG
                        )
                    case "+s":
                        self.mode_operations.append(
                            ModeOperationType.SET_SECRET_CHANNEL_FLAG
                        )

                    case "-s":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_SECRET_CHANNEL_FLAG
                        )
                    case "+i":
                        self.mode_operations.append(
                            ModeOperationType.SET_INVITED_ONLY)
                    case "-i":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_INVITED_ONLY
                        )
                    case "-p":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_PRIVATE_CHANNEL_FLAG
                        )
                    case "-p":
                        self.mode_operations.append(
                            ModeOperationType.SET_PRIVATE_CHANNEL_FLAG
                        )
                    case "+q":
                        self.mode_operations.append(
                            ModeOperationType.ENABLE_USER_QUIET_FLAG
                        )
                    case "-q":
                        self.mode_operations.append(
                            ModeOperationType.DISABLE_USER_QUIET_FLAG
                        )
                    case "+k":
                        self.mode_operations.append(
                            ModeOperationType.ADD_CHANNEL_PASSWORD
                        )
                    case "-k":
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_CHANNEL_PASSWORD
                        )
                    case "+l":
                        self.channel_name = self._cmd_params[0]
                        self.limit_number = int(self._cmd_params[2])
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_CHANNEL_PASSWORD
                        )

                    case "+l":
                        self.channel_name = self._cmd_params[0]
                        self.limit_number = int(self._cmd_params[2])
                        self.mode_operations.append(
                            ModeOperationType.ADD_CHANNEL_USER_LIMITS
                        )
                    case "-l":
                        self.channel_name = self._cmd_params[0]
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_CHANNEL_USER_LIMITS
                        )
                    case "+b":
                        self.channel_name = self._cmd_params[0]
                        if len(self._cmd_params) == 3:
                            self.nick_name = self._cmd_params[2]
                            self.mode_operations.append(
                                ModeOperationType.ADD_BAN_ON_USER
                            )
                        else:
                            self.mode_operations.append(
                                ModeOperationType.GET_BANNED_USERS
                            )
                    case "-b":
                        self.channel_name = self._cmd_params[0]
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_BAN_ON_USER
                        )
                    case "+co":
                        self.channel_name = self._cmd_params[0]
                        self.user_name = self._cmd_params[2]
                        self.mode_operations.append(
                            ModeOperationType.ADD_CHANNEL_OPERATOR
                        )
                    case "-co":
                        self.channel_name = self._cmd_params[0]
                        self.user_name = self._cmd_params[2]
                        self.mode_operations.append(
                            ModeOperationType.REMOVE_CHANNEL_OPERATOR
                        )
                    case "+cv":
                        self.channel_name = self._cmd_params[0]
                        self.user_name = self._cmd_params[2]
                        self.mode_operations.append(
                            ModeOperationType.ENABLE_USER_VOICE_PERMISSION
                        )
                    case "-cv":
                        self.channel_name = self._cmd_params[0]
                        self.user_name = self._cmd_params[2]
                        self.mode_operations.append(
                            ModeOperationType.DISABLE_USER_VOICE_PERMISSION
                        )
                    # Add more cases for other flags following the same pattern
                    case _:
                        raise ChatException("Unknown mode request type.")
        else:
            raise ChatException("The number of IRC parameters are incorrect.")


class NamesRequest(ChannelRequestBase):
    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            super().__init__(raw_request)


class PartRequest(ChannelRequestBase):
    reason: str = "Unknown reason"

    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            super().__init__(raw_request)

    def parse(self):
        super().parse()

        if self._long_param is None:
            raise ChatException("The reason of living channel is missing.")
        self.reason = self._long_param


class SetChannelKeyRequest(ChannelRequestBase):
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

        if "b_" in self.key_values:
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
        if not hasattr(self, "_long_param"):
            self.request_type = TopicRequestType.GET_CHANNEL_TOPIC
        else:
            self.request_type = TopicRequestType.SET_CHANNEL_TOPIC
            self.channel_topic = self._long_param

#  region Message


class ATMRequest(MessageRequestBase):
    pass


class NoticeRequest(MessageRequestBase):
    pass


class PrivateRequest(MessageRequestBase):
    pass


class UTMRequest(MessageRequestBase):
    pass
