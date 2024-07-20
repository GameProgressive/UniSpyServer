from servers.webservices.src.modules.auth.abstractions.general import (
    NAMESPACE,
    LoginRequestBase,
)
from servers.webservices.src.modules.auth.exceptions.general import AuthException


class LoginProfileRequest(LoginRequestBase):
    email: str
    uniquenick: str
    cdkey: str
    password: str

    def parse(self) -> None:
        super().parse()
        self.email = self._content_element.find(f".//{{{NAMESPACE}}}email")
        if self.email is None:
            raise AuthException("email is missing from the request.")

        self.uniquenick = self._content_element.find(
            f".//{{{NAMESPACE}}}uniquenick")
        if self.uniquenick is None:
            raise AuthException("uniquenick is missing from the request.")

        self.cdkey = self._content_element.find(f".//{{{NAMESPACE}}}cdkey")
        if self.cdkey is None:
            raise AuthException("cdkey is missing from the request.")

        self.password = self._content_element.find(
            f".//{{{NAMESPACE}}}password")
        if self.password is None:
            raise AuthException("password is missing from the request.")


class LoginProfileWithGameIdRequest(LoginProfileRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id)


class LoginPs3CertRequest(LoginRequestBase):
    ps3_cert: str

    def parse(self) -> None:
        super().parse()
        self.ps3_cert = self._content_element.find(
            f".//{{{NAMESPACE}}}npticket")
        if self.ps3_cert is None:
            raise AuthException("ps3cert is missing from the request")


class LoginPs3CertWithGameIdRequest(LoginPs3CertRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id)


class LoginRemoteAuthRequest(LoginRequestBase):
    auth_token: str
    challenge: str

    def parse(self) -> None:
        super().parse()
        self.auth_token = self._content_element.find(
            f".//{{{NAMESPACE}}}authtoken")
        if self.auth_token is None:
            raise AuthException("authtoken is missing from the request.")

        self.challenge = self._content_element.find(
            f".//{{{NAMESPACE}}}challenge")
        if self.challenge is None:
            raise AuthException("challenge is missing from the request.")


class LoginRemoteAuthWithGameIdRequest(LoginRemoteAuthRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id)


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

        password_node = self._content_element.find(
            f".//{{{NAMESPACE}}}password")
        if password_node is None:
            raise AuthException("password is missing from the request.")
        password_value_node = password_node.find(f".//{{{NAMESPACE}}}Value")
        if password_value_node is None or password_value_node.text is None:
            raise AuthException("No password value found")
        self.password = password_value_node.text


class LoginUniqueNickWithGameIdRequest(LoginUniqueNickRequest):
    game_id: int

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise AuthException("game id is missing from the request.")

        self.game_id = int(game_id)
