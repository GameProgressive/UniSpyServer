import backends.library.abstractions.contracts as lib
from servers.chat.src.enums.general import LoginRequestType, WhoRequestType


class RequestBase(lib.RequestBase):
    raw_request: str
    command_name: str
    _prefix: str
    _cmd_params: list
    _longParam: str


class CdkeyRequest(RequestBase):
    cdkey: str


class CryptRequest(RequestBase):
    version_id: str
    gamename: str


class GetUdpRelayRequest(RequestBase):
    pass


class InviteRequest(RequestBase):
    channel_name: str
    nick_name: str


class ListLimitRequest(RequestBase):
    max_number_of_channels: int
    filter: str


class ListRequest(RequestBase):
    is_searching_channel: bool
    is_searching_user: bool
    filter: str


class LoginPreAuth(RequestBase):
    auth_token: str
    partner_challenge: str


class LoginRequest(RequestBase):
    request_type: LoginRequestType
    namespace_id: int
    nick_name: str
    email: str
    unique_nick: str
    password_hash: str


class NickRequest(RequestBase):
    nick_name: str


class PingRequest(RequestBase):
    pass


class PongRequest(RequestBase):
    echo_message: str


class QuitRequest(RequestBase):
    reason: str


class RegisterNickRequest(RequestBase):
    namespace_id: int
    unique_nick: str
    cdkey: str


class SetKeyRequest(RequestBase):
    key_values: dict[str, str]


class UserIPRequest(RequestBase):
    remote_ip_address: str


class UserRequest(RequestBase):
    user_name: str
    host_name: str
    server_name: str
    nick_name: str
    name: str


class WhoIsRequest(RequestBase):
    nick_name: str


class WhoRequest(RequestBase):
    request_type: WhoRequestType
    channel_name: str
    nick_name: str


class GetKeyRequest(RequestBase):
    is_get_all_user: bool
    nick_name: str
    cookie: str
    unknown_cmd_param: str
    keys: list[str]
