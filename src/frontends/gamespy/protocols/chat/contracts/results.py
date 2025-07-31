from pydantic import BaseModel
from frontends.gamespy.protocols.chat.abstractions.contract import ResultBase
from frontends.gamespy.protocols.chat.abstractions.handler import MessageResultBase

# region General


class CryptResult(ResultBase):
    secret_key: str
    pass


class GetKeyResult(ResultBase):
    nick_name: str
    values: list


class ListResult(ResultBase):
    class ListInfo(BaseModel):
        channel_name: str
        total_channel_user: int
        channel_topic: str

    user_irc_prefix: str
    channel_info_list: list[ListInfo] = []
    """(channel_name:str,total_channel_user:int,channel_topic:str)"""


class LoginResult(ResultBase):
    profile_id: int
    user_id: int


class NickResult(ResultBase):
    nick_name: str


class PingResult(ResultBase):
    requester_irc_prefix: str


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
    name: str
    public_ip_address: str
    joined_channel_name: list[str] = []


class WhoResult(ResultBase):
    class WhoInfo(BaseModel):
        channel_name: str
        user_name: str
        public_ip_addr: str
        nick_name: str
        modes: str

    infos: list[WhoInfo]


# region Channel


class GetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str
    values: str


class GetCKeyResult(ResultBase):
    class GetCKeyInfos(BaseModel):
        nick_name: str
        user_values: list

    infos: list[GetCKeyInfos]
    """ nick_name:str, user_values:str"""
    channel_name: str


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
    joiner_user_name: str
    joiner_nick_name: str
    # channel_nicks_data: list[NamesResultData]
    # channel_modes: list[str]


class KickResult(ResultBase):
    channel_name: str
    kicker_nick_name: str
    kicker_user_name: str
    kickee_nick_name: str


class PartResult(ResultBase):
    leaver_irc_prefix: str
    is_channel_creator: bool
    channel_name: str


class TopicResult(ResultBase):
    channel_name: str
    channel_topic: str


class SetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str


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


class ATMResult(MessageResultBase):
    pass


class NoticeResult(MessageResultBase):
    pass


class PrivateResult(MessageResultBase):
    is_broadcast_message: bool = False


class UTMResult(MessageResultBase):
    pass
