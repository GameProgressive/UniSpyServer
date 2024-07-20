from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.enums.general import (
    LoginType,
    QuietModeType,
    SdkRevisionType,
)
from servers.presence_search_player.src.exceptions.general import (
    GPParseException,
)


class KeepAliveRequest(RequestBase):
    pass


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
    game_name: int
    quiet_mode_flags: int
    firewall: bool

    def __init__(self, raw_request):
        super().__init__(raw_request)

    def parse(self):
        super().parse()

        if "challenge" not in self.request_key_values:
            raise GPParseException("challenge is missing")

        if "response" not in self.request_key_values:
            raise GPParseException("response is missing")

        self.user_challenge = self.request_key_values["challenge"]
        self.response = self.request_key_values["response"]

        if (
            "uniquenick" in self.request_key_values
            and "namespaceid" in self.request_key_values
        ):
            namespace_id = int(self.request_key_values["namespaceid"])
            self.type = LoginType.UNIQUENICK_NAMESPACE_ID
            self.unique_nick = self.request_key_values["uniquenick"]
            self.user_data = self.unique_nick
            self.namespace_id = namespace_id
        elif "authtoken" in self.request_key_values:
            self.type = LoginType.AUTH_TOKEN
            self.auth_token = self.request_key_values["authtoken"]
            self.user_data = self.auth_token
        elif "user" in self.request_key_values:
            self.type = LoginType.NICK_EMAIL
            self.user_data = self.request_key_values["user"]
            pos = self.user_data.index("@")
            if pos == -1 or pos < 1 or (pos + 1) >= len(self.user_data):
                raise GPParseException("user format is incorrect")
            self.nick = self.user_data[:pos]
            self.email = self.user_data[pos + 1 :]
            if "namespaceid" in self.request_key_values:
                namespace_id = int(self.request_key_values["namespaceid"])
                self.namespace_id = namespace_id
        else:
            raise GPParseException("Unknown login method detected.")

        self.parse_other_data()

    def parse_other_data(self):
        if "userid" in self.request_key_values:
            user_id = int(self.request_key_values["userid"])
            self.user_id = user_id

        if "profileid" in self.request_key_values:
            profile_id = int(self.request_key_values["profileid"])
            self.profile_id = profile_id

        if "partnerid" in self.request_key_values:
            partner_id = int(self.request_key_values["partnerid"])
            self.partner_id = partner_id

        if "sdkrevision" in self.request_key_values:
            sdk_revision_type = int(self.request_key_values["sdkrevision"])
            self.sdk_revision_type = SdkRevisionType(sdk_revision_type)

        if "gamename" in self.request_key_values:
            self.game_name = self.request_key_values["gamename"]

        if "port" in self.request_key_values:
            game_port = int(self.request_key_values["port"])
            self.game_port = game_port

        if "productid" in self.request_key_values:
            product_id = int(self.request_key_values["productid"])
            self.product_id = product_id

        if "firewall" in self.request_key_values:
            self.firewall = self.request_key_values["firewall"]

        if "quiet" in self.request_key_values:
            quiet = int(self.request_key_values["quiet"])
            self.quiet_mode_flags = QuietModeType(quiet)


class LogoutRequest(RequestBase):
    pass
