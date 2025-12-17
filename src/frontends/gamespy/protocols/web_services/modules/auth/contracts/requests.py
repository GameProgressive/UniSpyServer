from frontends.gamespy.protocols.web_services.modules.auth.abstractions.contracts import (
    NAMESPACE,
    LoginRequestBase,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import ResponseName
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import ParseException


class LoginProfileRequest(LoginRequestBase):
    email: str
    nick: str
    cdkey: str
    password: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_PROFILE

    def parse(self) -> None:
        super().parse()
        email = self._content_element.find(f".//{{{NAMESPACE}}}email")
        if email is None or email.text is None:
            raise ParseException("email is missing",
                                 self.response_name)
        self.email = email.text

        nick = self._content_element.find(
            f".//{{{NAMESPACE}}}profilenick")
        if nick is None or nick.text is None:
            raise ParseException("uniquenick is missing",
                                 self.response_name)
        self.nick = nick.text

        password = self._content_element.find(
            f".//{{{NAMESPACE}}}password//{{{NAMESPACE}}}Value")
        if password is None or password.text is None:
            raise ParseException("password is missing",
                                 self.response_name)
        self.password = password.text


class LoginProfileWithGameIdRequest(LoginProfileRequest):
    game_id: int

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_PROFILE_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing",
                                 self.response_name)

        self.game_id = int(game_id.text)


class LoginPs3CertRequest(LoginRequestBase):
    ps3_cert: str
    game_id: int
    npticket: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_PS3_CERT

    def parse(self) -> None:
        super().parse()
        ps3_cert = self._content_element.find(
            f".//{{{NAMESPACE}}}ps3sert")
        if ps3_cert is None or ps3_cert.text is None:
            raise ParseException(
                "ps3cert is missing from the request", self.response_name)
        self.ps3_cert = ps3_cert.text

        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing",
                                 self.response_name)
        self.game_id = int(game_id.text)

        npticket = self._content_element.find(f".//{{{NAMESPACE}}}npticket")
        if npticket is None or npticket.text is None:
            raise ParseException("npticket is missing",
                                 self.response_name)
        self.npticket = npticket.text


class LoginPs3CertWithGameIdRequest(LoginPs3CertRequest):
    game_id: int

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_PS3_CERT_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing",
                                 self.response_name)
        self.game_id = int(game_id.text)


class LoginRemoteAuthRequest(LoginRequestBase):
    auth_token: str
    challenge: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_REMOTE_AUTH

    def parse(self) -> None:
        super().parse()
        auth_token = self._content_element.find(
            f".//{{{NAMESPACE}}}authtoken")
        if auth_token is None or auth_token.text is None:
            raise ParseException("authtoken is missing",
                                 self.response_name)
        self.auth_token = auth_token.text

        challenge = self._content_element.find(
            f".//{{{NAMESPACE}}}challenge")
        if challenge is None or challenge.text is None:
            raise ParseException("challenge is missing",
                                 self.response_name)
        self.challenge = challenge.text

        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing",
                                 self.response_name)
        self.game_id = int(game_id.text)


class LoginRemoteAuthWithGameIdRequest(LoginRemoteAuthRequest):
    game_id: int

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_REMOTE_AUTH_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing", self.response_name)

        self.game_id = int(game_id.text)


class LoginUniqueNickRequest(LoginRequestBase):
    uniquenick: str
    password: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_UNIQUENICK

    def parse(self) -> None:
        super().parse()
        unique_nick_node = self._content_element.find(
            f".//{{{NAMESPACE}}}uniquenick")
        if unique_nick_node is None or unique_nick_node.text is None:
            raise ParseException("uniquenick is missing",
                                 self.response_name)
        self.uniquenick = unique_nick_node.text

        password = self._content_element.find(
            f".//{{{NAMESPACE}}}password//{{{NAMESPACE}}}Value")
        if password is None or password.text is None:
            raise ParseException("password is missing",
                                 self.response_name)
        self.password = password.text


class LoginUniqueNickWithGameIdRequest(LoginUniqueNickRequest):
    game_id: int

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.LOGIN_UNIQUENICK_WITH_GAME_ID

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise ParseException("game id is missing",
                                 self.response_name)

        self.game_id = int(game_id.text)


class CreateUserAccountRequest(LoginRequestBase):
    email: str
    nick: str
    uniquenick: str
    password: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.response_name = ResponseName.CREATE_USER_ACCOUNT

    def parse(self) -> None:
        super().parse()
        email = self._content_element.find(f".//{{{NAMESPACE}}}email")
        if email is None or email.text is None:
            raise ParseException("email is missing",
                                 self.response_name)
        self.email = email.text

        nick = self._content_element.find(
            f".//{{{NAMESPACE}}}profilenick")
        if nick is None or nick.text is None:
            raise ParseException("password is missing",
                                 self.response_name)
        self.nick = nick.text

        uniquenick = self._content_element.find(
            f".//{{{NAMESPACE}}}uniquenick")
        if uniquenick is None or uniquenick.text is None:
            raise ParseException("password is missing",
                                 self.response_name)
        self.uniquenick = uniquenick.text

        password = self._content_element.find(
            f".//{{{NAMESPACE}}}password//{{{NAMESPACE}}}Value")
        if password is None or password.text is None:
            raise ParseException("password is missing",
                                 self.response_name)
        self.password = password.text
