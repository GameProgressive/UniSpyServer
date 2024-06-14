from typing import List, Tuple
from servers.chat.abstractions.contract import ResultBase


class CryptResult(ResultBase):
    pass


class GetKeyResult(ResultBase):
    nick_name: str
    values: list = []


class ListResult(ResultBase):
    user_irc_prefix: str
    channel_info_list: List[Tuple[str, int, str]] = []
    """(channel_name:str,total_channel_user:int,channel_topic:str)"""


class LoginResult(ResultBase):
    profile_id: int
    user_id: int


class NickResult(ResultBase):
    nick_name: str


class PingResult(ResultBase):
    requester_irc_prefix: str


class QuitResult(ResultBase):
    quiter_prefix: str
    channel_infos: List[Tuple[str, bool, bool, str, str]]
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
    infos: list[tuple[str, str, str, str, str]]
    """
        public string ChannelName { get; set; }
        public string UserName { get; set; }
        public string PublicIPAddress { get; set; }
        public string NickName { get; set; }
        public string Modes { get; set; }
    """
