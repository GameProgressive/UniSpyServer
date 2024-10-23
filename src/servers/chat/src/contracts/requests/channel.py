import re
from typing import List, Optional
from servers.chat.src.abstractions.channel import ChannelRequestBase
from servers.chat.src.enums.general import (
    GetKeyRequestType,
    ModeOperationType,
    ModeRequestType,
    TopicRequestType,
)
from servers.chat.src.exceptions.general import ChatException
from library.src.extentions.string_extentions import convert_kvstring_to_dictionary


class GetChannelKeyRequest(ChannelRequestBase):
    cookie: str
    keys: List

    def parse(self):
        super().parse()

        if len(self._cmd_params) != 3:
            raise ChatException("The cmdParams number is invalid.")

        if self._longParam is None or self._longParam[-1] != "\0":
            raise ChatException("The longParam number is invalid.")
        self.cookie = self._cmd_params[0]
        self.keys = self._longParam.strip("\\").rstrip("\0").split("\\")


class GetCKeyRequest(ChannelRequestBase):
    nick_name: str
    cookie: str
    keys: List
    request_type: GetKeyRequestType

    def parse(self):
        super().parse()
        if len(self._cmd_params) != 4:
            raise ChatException("The number of IRC parameters are incorrect.")
        if self._longParam is None:
            raise ChatException("The IRC long parameter is incorrect.")

        self.nick_name = self._cmd_params[1]
        if self.nick_name == "*":
            self.request_type = GetKeyRequestType.GET_CHANNEL_ALL_USER_KEY_VALUE
        else:
            self.request_type = GetKeyRequestType.GET_CHANNEL_SPECIFIC_USER_KEY_VALUE

        self.cookie = self._cmd_params[2]

        if "\0" not in self._longParam and "\\" not in self._longParam:

            raise ChatException("The key provide is incorrect.")

        self.keys = self._longParam.strip("\\").rstrip("\0").split("\\")


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

        if self._longParam is None:
            raise ChatException("The IRC long parameters is missing.")

        self.reason = self._longParam


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
    mode_operations: list = []
    nick_name: str
    user_name: str
    limit_number: int
    mode_flag: str
    password: str

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
    channel_name: str

    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            super().__init__(raw_request)


class PartRequest(ChannelRequestBase):
    channel_name: str
    reason: str = "Unknown reason"

    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            super().__init__(raw_request)

    def parse(self):
        super().parse()

        if self._longParam is None:
            raise ChatException("The reason of living channel is missing.")
        self.reason = self._longParam


class SetChannelKeyRequest(ChannelRequestBase):
    key_value_string: str
    key_values: dict[str, str]

    def parse(self):
        super().parse()
        if self._longParam is None:
            raise ChatException("Channel keys and values are missing.")
        self._longParam = self._longParam[1:]
        self.key_value_string = self._longParam
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

        if self._longParam is None:
            raise ChatException(
                "The longParam from SETCKEY request is missing.")

        self.channel_name = self._cmd_params[0]
        self.nick_name = self._cmd_params[1]
        self.key_value_string = self._longParam[1:]
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
        if self._longParam is None:
            self.request_type = TopicRequestType.GET_CHANNEL_TOPIC
        else:
            self.request_type = TopicRequestType.SET_CHANNEL_TOPIC
            self.channel_topic = self._longParam
