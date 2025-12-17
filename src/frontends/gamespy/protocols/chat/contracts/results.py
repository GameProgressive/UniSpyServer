from pydantic import BaseModel
from frontends.gamespy.protocols.chat.abstractions.contract import ResultBase
from frontends.gamespy.protocols.chat.abstractions.handler import MessageResultBase
from frontends.gamespy.protocols.chat.aggregates.enums import IRCErrorCode, WhoRequestType

# region General


class CryptResult(ResultBase):
    secret_key: str
    pass


class GetKeyResult(ResultBase):
    nick_name: str
    values: list
    cookie: str


class ListResult(ResultBase):
    class ListInfo(BaseModel):
        channel_name: str
        total_channel_user: int
        channel_topic: str
    invoker_nick_name: str
    invoker_user_name: str
    channel_info_list: list[ListInfo] = []
    """(channel_name:str,total_channel_user:int,channel_topic:str)"""


class LoginResult(ResultBase):
    profile_id: int
    user_id: int


class NickResult(ResultBase):
    nick_name: str


class PingResult(ResultBase):
    nick_name: str
    user_name: str


class QuitResult(ResultBase):
    class QuitInfo(BaseModel):
        channel_name: str
        is_peer_server: bool
        is_channel_operator: bool
        leave_reply_sending_buffer: str
        kick_replay_sending_buffer: str

    quiter_prefix: str
    channel_infos: list[QuitInfo]
    # (channel_name: str, is_peer_server: bool, is_channel_operator: bool,leave_reply_sending_buffer: str,kick_replay_sending_buffer: str)
    message: str


class UserIPResult(ResultBase):
    remote_ip: str


class WhoIsResult(ResultBase):
    nick_name: str
    user_name: str
    public_ip_address: str
    joined_channels: list[str]


class WhoResult(ResultBase):
    class WhoInfo(BaseModel):
        user_name: str
        nick_name: str
        channel_name: str
        public_ip_addr: str
        modes: str

    infos: list[WhoInfo]
    request_type: WhoRequestType
    channel_name: str
    nick_name: str
# region Channel


class GetChannelKeyResult(ResultBase):
    nick_name: str
    user_name: str
    channel_name: str
    key_values: dict
    cookie: str


class GetCKeyResult(ResultBase):
    class GetCKeyInfos(BaseModel):
        nick_name: str
        key_values: dict
    infos: list[GetCKeyInfos]
    """ nick_name:str, user_values:str"""
    channel_name: str
    cookie: str
    keys: list[str]


class ModeResult(ResultBase):
    channel_name: str
    channel_modes: list[str]
    joiner_nick_name: str


class NamesResultData(BaseModel):
    nick_name: str
    is_channel_creator: bool = False
    is_channel_operator: bool = False


class NamesResult(ResultBase):
    channel_nicks: list[NamesResultData]
    channel_name: str
    requester_nick_name: str


class JoinResult(ResultBase):
    joiner_nick_name: str
    joiner_user_name: str
    channel_name: str


class KickResult(ResultBase):
    channel_name: str
    kicker_nick_name: str
    kicker_user_name: str
    kickee_nick_name: str
    reason: str


class PartResult(ResultBase):
    leaver_nick_name: str
    leaver_user_name: str
    is_channel_creator: bool
    channel_name: str
    reason: str


class TopicResult(ResultBase):
    channel_name: str
    channel_topic: str


class SetChannelKeyResult(ResultBase):
    setter_nick_name: str
    setter_user_name: str
    channel_name: str
    key_value: dict[str, str]


class SetCKeyResult(ResultBase):
    setter_nick_name: str
    setter_user_name: str
    channel_name: str
    cookie: str
    key_value: dict


if __name__ == "__main__":
    dd = {
        "infos": [
            {"nick_name": "John", "user_values": "12345"},
            {"nick_name": "Alice", "user_values": "67890"},
        ],
        "channel_name": "example_channel",
    }
    result = GetCKeyResult(**dd)
    pass

# region Message


class AtmResult(MessageResultBase):
    pass


class NoticeResult(MessageResultBase):
    pass


class PrivateResult(MessageResultBase):
    pass


class UtmResult(MessageResultBase):
    pass

# # region Exception


# class ExceptionResult(ResultBase):
#     error_code: IRCErrorCode


# class ChannelExceptionResult(ExceptionResult):
#     channel_name: str


# class NickNameInUseExceptionResult(ExceptionResult):
#     new_nick: str
#     old_nick: str
