import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.chat.aggregates.enums import GetKeyRequestType, LoginRequestType, MessageType, ModeOperationType, ModeRequestType, TopicRequestType, WhoRequestType


class RequestBase(lib.RequestBase):
    raw_request: str
    command_name: str

# region General


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


class LoginPreAuthRequest(RequestBase):
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

# region Channel


class ChannelRequestBase(RequestBase):
    channel_name: str


class GetChannelKeyRequest(ChannelRequestBase):
    cookie: str
    keys: list


class GetCKeyRequest(ChannelRequestBase):
    nick_name: str
    cookie: str
    keys: list
    request_type: GetKeyRequestType


class JoinRequest(ChannelRequestBase):
    password: str
    game_name: str


class KickRequest(ChannelRequestBase):
    kickee_nick_name: str
    reason: str


class ModeRequest(ChannelRequestBase):
    request_type: ModeRequestType
    mode_operations: list[ModeOperationType]
    nick_name: str
    user_name: str
    limit_number: int
    mode_flag: str
    password: str


class NamesRequest(ChannelRequestBase):
    pass


class PartRequest(ChannelRequestBase):
    reason: str


class SetChannelKeyRequest(ChannelRequestBase):
    key_value_string: str
    key_values: dict[str, str]


class SetCKeyRequest(ChannelRequestBase):
    nick_name: str
    cookie: str
    is_broadcast: bool
    key_value_string: str
    key_values: dict[str, str]


class SetGroupRequest(ChannelRequestBase):
    group_name: str


class TopicRequest(ChannelRequestBase):
    channel_topic: str
    request_type: TopicRequestType


class MessageRequestBase(ChannelRequestBase):
    type: MessageType
    nick_name: str
    message: str

# region Message


class AtmRequest(MessageRequestBase):
    pass


class NoticeRequest(MessageRequestBase):
    pass


class PrivateRequest(MessageRequestBase):
    pass


class UtmRequest(MessageRequestBase):
    pass
