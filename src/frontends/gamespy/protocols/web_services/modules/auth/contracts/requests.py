from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import (
    NAMESPACE,
    LoginRequestBase,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import CommandName
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import ParseException


class LoginProfileRequest(LoginRequestBase):
    email: str
    nick: str
    cdkey: str
    password: str

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_PROFILE

    def parse(self) -> None:
        super().parse()
        self.email = self._get_str("email")
        self.nick = self._get_str("profilenick")
        self.password = self._parse_password()


class LoginProfileWithGameIdRequest(LoginProfileRequest):
    game_id: int

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_PROFILE_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")


class LoginPs3CertRequest(LoginRequestBase):
    ps3_cert: str
    game_id: int
    npticket: str

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_PS3_CERT

    def parse(self) -> None:
        super().parse()
        self.ps3_cert = self._get_str("ps3sert")
        self.game_id = self._get_int("gameid")
        self.npticket = self._get_str("npticket")


class LoginPs3CertWithGameIdRequest(LoginPs3CertRequest):
    game_id: int

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_PS3_CERT_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")


class LoginRemoteAuthRequest(LoginRequestBase):
    auth_token: str
    challenge: str

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_REMOTE_AUTH

    def parse(self) -> None:
        super().parse()
        self.auth_token = self._get_str("authtoken")
        self.challenge = self._get_str("challenge")
        self.game_id = self._get_int("gameid")


class LoginRemoteAuthWithGameIdRequest(LoginRemoteAuthRequest):
    game_id: int

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_REMOTE_AUTH_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")


class LoginUniqueNickRequest(LoginRequestBase):
    uniquenick: str
    password: str

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_UNIQUENICK

    def parse(self) -> None:
        super().parse()
        self.uniquenick = self._get_str("uniquenick")
        self.password = self._parse_password()


class LoginUniqueNickWithGameIdRequest(LoginUniqueNickRequest):
    game_id: int

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.LOGIN_UNIQUENICK_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")


class CreateUserAccountRequest(LoginRequestBase):
    email: str
    nick: str
    uniquenick: str
    password: str
    game_id: int

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.command_name = CommandName.CREATE_USER_ACCOUNT

    def parse(self) -> None:
        super().parse()
        self.email = self._get_str("email")
        self.nick = self._get_str("profilenick")
        self.uniquenick = self._get_str("uniquenick")
        # self.game_id = self._parse_int("gameid")
        self.password = self._parse_password()
