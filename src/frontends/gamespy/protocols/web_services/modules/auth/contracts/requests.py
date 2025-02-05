from frontends.gamespy.protocols.web_services.modules.auth.abstractions.general import (
    NAMESPACE,
    LoginRequestBase,
)
from frontends.gamespy.protocols.web_services.modules.auth.exceptions.general import AuthException


class LoginProfileRequest(LoginRequestBase):
    email: str
    uniquenick: str
    cdkey: str
    password: str

    def parse(self) -> None:
        super().parse()
        email = self._content_element.find(f".//{{{NAMESPACE}}}email")
        if email is None or email.text is None:
            raise AuthException("email is missing from the request.")
        self.email = email.text

        uniquenick = self._content_element.find(
            f".//{{{NAMESPACE}}}uniquenick")
        if uniquenick is None or uniquenick.text is None:
            raise AuthException("uniquenick is missing from the request.")
        self.uniquenick = uniquenick.text

        cdkey = self._content_element.find(f".//{{{NAMESPACE}}}cdkey")
        if cdkey is None or cdkey.text is None:
            raise AuthException("cdkey is missing from the request.")
        self.cdkey = cdkey.text

        password = self._content_element.find(
            f".//{{{NAMESPACE}}}password//{{{NAMESPACE}}}Value")
        if password is None or password.text is None:
            raise AuthException("password is missing from the request.")
        self.password = password.text


class LoginProfileWithGameIdRequest(LoginProfileRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id.text)


class LoginPs3CertRequest(LoginRequestBase):
    ps3_cert: str
    game_id: int
    npticket: str

    def parse(self) -> None:
        super().parse()
        ps3_cert = self._content_element.find(
            f".//{{{NAMESPACE}}}ps3sert")
        if ps3_cert is None or ps3_cert.text is None:
            raise AuthException("ps3cert is missing from the request")
        self.ps3_cert = ps3_cert.text

        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")
        self.game_id = int(game_id.text)

        npticket = self._content_element.find(f".//{{{NAMESPACE}}}npticket")
        if npticket is None or npticket.text is None:
            raise AuthException("npticket is missing from the request.")
        self.npticket = npticket.text


class LoginPs3CertWithGameIdRequest(LoginPs3CertRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")
        self.game_id = int(game_id.text)


class LoginRemoteAuthRequest(LoginRequestBase):
    auth_token: str
    challenge: str

    def parse(self) -> None:
        super().parse()
        auth_token = self._content_element.find(
            f".//{{{NAMESPACE}}}authtoken")
        if auth_token is None or auth_token.text is None:
            raise AuthException("authtoken is missing from the request.")
        self.auth_token = auth_token.text

        challenge = self._content_element.find(
            f".//{{{NAMESPACE}}}challenge")
        if challenge is None or challenge.text is None:
            raise AuthException("challenge is missing from the request.")
        self.challenge = challenge.text

        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")
        self.game_id = int(game_id.text)


class LoginRemoteAuthWithGameIdRequest(LoginRemoteAuthRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id.text)


class LoginUniqueNickRequest(LoginRequestBase):
    uniquenick: str
    password: str

    def parse(self) -> None:
        super().parse()
        unique_nick_node = self._content_element.find(
            f".//{{{NAMESPACE}}}uniquenick")
        if unique_nick_node is None or unique_nick_node.text is None:
            raise AuthException("uniquenick is missing from the request.")
        self.uniquenick = unique_nick_node.text

        password = self._content_element.find(
            f".//{{{NAMESPACE}}}password//{{{NAMESPACE}}}Value")
        if password is None or password.text is None:
            raise AuthException("password is missing from the request.")
        self.password = password.text


class LoginUniqueNickWithGameIdRequest(LoginUniqueNickRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id.text)
