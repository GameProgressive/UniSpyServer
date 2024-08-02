from typing import List, Tuple

from pydantic import BaseModel
from servers.chat.src.abstractions.contract import ResultBase


class CryptResult(ResultBase):
    pass


class GetKeyResult(ResultBase):
    nick_name: str
    values: list = []


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
    remote_ip_address: str


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
