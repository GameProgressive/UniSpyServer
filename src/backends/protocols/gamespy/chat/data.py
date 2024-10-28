from typing import TYPE_CHECKING, cast
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatUserCaches, Users, Profiles, SubProfiles
from servers.chat.src.aggregates.channel import Channel
from servers.chat.src.aggregates.exceptions import ChatException


def nick_and_email_login(nick_name: str, email: str, password_hash: str) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    result = PG_SESSION.query(Users.userid, Profiles.profileid,
                              Users.emailverified, Users.banned).join(Profiles, (Users.userid == Profiles.userid)).where(
        Users.email == email,
        Profiles.nick == nick_name,
        Users.password == password_hash
    ).first()
    if TYPE_CHECKING:
        result = cast(tuple[int, int, bool, bool], result)
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with nickname:{nick_name} in database.")
        # fmt on

    return result

def uniquenick_login(uniquenick:str,namespace_id:int)-> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    result = PG_SESSION.query(Users.userid, Profiles.profileid,Users.emailverified, Users.banned).join(Profiles,(Users.userid == Profiles.userid)).join(Profiles,(Profiles.profileid == SubProfiles.profileid)).where(SubProfiles.namespaceid == namespace_id,SubProfiles.uniquenick == uniquenick).first()
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with uniquenick:{uniquenick} in database.")
        # fmt on
    if TYPE_CHECKING:
        result = cast(tuple[int, int, bool, bool],result)
    return result


def is_channel_exist(channel_name:str,game_name:str)->bool:
    channel_count = PG_SESSION.query(ChatChannelCaches)\
        .filter(ChatChannelCaches.channel_name == channel_name,
                ChatChannelCaches.game_name == game_name)\
                .count()
    if channel_count == 1:
        return True
    else:
        return False
def add_channel(channel:Channel):
    info = ChatChannelCaches(
        channel_name=channel.name, game_name=channel.game_name, key_values =channel.kv_manager.data, max_num_user=channel.max_num_user, room_name=channel.room_name, topic=channel.topic, password=channel.password, group_id=channel.group_id, create_time=channel.create_time, previously_joined_channel=channel.previously_join_channel
    )
    PG_SESSION.add(info)
    PG_SESSION.commit()

def get_channel_cache(channel_name:str,game_name:str)->ChatChannelCaches:
    channel = PG_SESSION.query(ChatChannelCaches)\
        .filter(ChatChannelCaches.channel_name == channel_name,
                ChatChannelCaches.game_name == game_name)\
                .first()
    return channel

def update_channel(channel:Channel):

    info = ChatChannelCaches(
        channel_name=channel.name, game_name=channel.game_name, key_values =channel.kv_manager.data, max_num_user=channel.max_num_user, room_name=channel.room_name, topic=channel.topic, password=channel.password, group_id=channel.group_id, create_time=channel.create_time, previously_joined_channel=channel.previously_join_channel
    )
    PG_SESSION.add(info)


def get_user_cache_by_nick_name(nick_name:str)->ChatUserCaches:
    result = PG_SESSION.query(ChatUserCaches).filter(ChatUserCaches.nick_name == nick_name).first()
    return result

def remove_channel(cache:ChatChannelCaches)->None:
    assert isinstance(cache,ChatChannelCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def remove_user(cache:ChatUserCaches):
    assert isinstance(cache,ChatUserCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def is_user_exist(nick_name:str)->bool:
    user_count= PG_SESSION.query(ChatUserCaches).filter(ChatUserCaches.nick_name ==nick_name).count()
    if user_count ==1:
        return True
    else:
        return False

def update_client(cache:ChatUserCaches):
    assert isinstance(cache,ChatUserCaches)
    PG_SESSION.commit()


