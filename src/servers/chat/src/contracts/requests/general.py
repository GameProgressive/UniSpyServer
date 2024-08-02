from typing import Dict
from library.src.extentions.string_extentions import (
    convert_keystr_to_list,
    convert_kvstring_to_dictionary,
)
from servers.chat.src.abstractions.contract import RequestBase
from servers.chat.src.enums.general import LoginRequestType, WhoRequestType
from servers.chat.src.exceptions.general import ChatException
from servers.chat.src.exceptions.general import NickNameInUseException


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


class GetUdpRelayRequest(RequestBase):
    pass


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
        if self._cmd_params is None or self._cmd_params.count() == 0:
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
            if self._longParam.count("@") != 2:
                raise ChatException("The profile nick format is incorrect.")

            profile_nick_index = self._longParam.index("@")
            self.nick_name = self._longParam[0:profile_nick_index]
            self.email = self._longParam[profile_nick_index + 1 :]
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
        elif self._longParam is None:
            self.nick_name = self._longParam
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
        if self._longParam is None:
            raise ChatException("Echo message is missing.")
        self.echo_message = self._longParam


class QuitRequest(RequestBase):
    reason: str

    def parse(self):
        super().parse()
        if self._longParam is None:
            raise ChatException("Quit reason is missing.")

        self.reason = self._longParam


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
    key_values: Dict[str, str]

    def parse(self):
        super().parse()
        if self._longParam is None:
            raise ChatException("The keys and values are missing.")

        self.key_values = convert_kvstring_to_dictionary(self._longParam)


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

        self.name = self._longParam


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

        if self._longParam is None:
            raise ChatException(
                "The number of IRC cmd params in GETKEY request is incorrect."
            )

        self.nick_name = self._cmd_params[0]
        self.cookie = self._cmd_params[1]
        self.unknown_cmd_param = self._cmd_params[2]

        self._longParam = self._longParam[: len(self._longParam)]
        if self.nick_name == "*":
            self.is_get_all_user = True

        self.keys = convert_keystr_to_list(self._longParam)
