from typing import final
from library.src.extentions.gamespy_utils import is_email_format_correct
from library.src.extentions.password_encoder import process_password
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.enums.general import (
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
from servers.presence_search_player.src.exceptions.general import (
    GPParseException,
)


@final
class KeepAliveRequest(RequestBase):
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
