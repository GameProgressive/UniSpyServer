from pydantic import BaseModel
from servers.chat.src.abstractions.contract import ResultBase


class GetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str
    values: str


class GetCKeyResult(ResultBase):
    class GetCKeyInfos(BaseModel):
        nick_name: str
        user_values: str

    infos: list[GetCKeyInfos]
    """ nick_name:str, user_values:str"""
    channel_name: str


class JoinResult(ResultBase):
    joiner_prefix: str
    joiner_nick_name: str
    all_channel_user_nicks: str
    channel_modes: str


class KickResult(ResultBase):
    channel_name: str
    kicker_irc_prefix: str
    kicker_nick_name: str
    kickee_nick_name: str


class ModeResult(ResultBase):
    channel_name: str
    channel_modes: str
    joiner_nick_name: str


class NamesResult(ResultBase):
    all_channel_user_nicks: str
    channel_name: str
    requester_nick_name: str


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
            {
                "nick_name": "John",
                "user_values": "12345"
            },
            {
                "nick_name": "Alice",
                "user_values": "67890"
            }
        ],
        "channel_name": "example_channel"
    }
    result = GetCKeyResult(**dd)
    pass
