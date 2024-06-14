from servers.chat.abstractions.contract import ResultBase


class GetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str
    values: str


class GetCKeyResult(ResultBase):
    infos: list[tuple]
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


class GetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str
    values: str


class TopicResult(ResultBase):
    channel_name: str
    channel_topic: str


class SetChannelKeyResult(ResultBase):
    channel_user_irc_prefix: str
    channel_name: str
