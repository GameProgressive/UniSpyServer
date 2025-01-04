from datetime import datetime, timedelta
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatNickCaches, ChatUserCaches, Users, Profiles, SubProfiles
from servers.chat.src.aggregates.exceptions import ChatException


def is_cdkey_valid(cdkey: str) -> bool:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.cdkeyenc, Column)
    result = PG_SESSION.query(SubProfiles).where(
        SubProfiles.cdkeyenc == cdkey).count()
    if result == 0:
        return False

    else:
        return True


def is_nick_exist(nick_name: str) -> bool:
    c = PG_SESSION.query(ChatNickCaches.nick_name).count()
    if c == 1:
        return True
    else:
        return False


def add_nick_cache(cache: ChatNickCaches):
    PG_SESSION.add(cache)
    PG_SESSION.commit()


def nick_and_email_login(nick_name: str, email: str, password_hash: str) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Users.emailverified, Column)
        assert isinstance(Users.banned, Column)
        assert isinstance(Users.password, Column)

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
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Users.emailverified, Column)
        assert isinstance(Users.banned, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(SubProfiles.uniquenick ,Column)
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
        .where(ChatChannelCaches.channel_name == channel_name,
                ChatChannelCaches.game_name == game_name,
                ChatChannelCaches.update_time >= datetime.now()-timedelta(minutes=10))\
                .count()
    if channel_count == 1:
        return True
    else:
        return False
def add_channel(channel:ChatChannelCaches):
    PG_SESSION.add(channel)
    PG_SESSION.commit()

def get_channel_cache(channel_name:str,game_name:str)->ChatChannelCaches:
    channel = PG_SESSION.query(ChatChannelCaches)\
        .where(ChatChannelCaches.channel_name == channel_name,
                ChatChannelCaches.game_name == game_name)\
                .first()
    return channel

def update_channel(channel:ChatChannelCaches):
    channel.update_time = datetime.now() # type: ignore
    PG_SESSION.commit()


def get_user_cache_by_nick_name(nick_name:str)->ChatUserCaches:
    result = PG_SESSION.query(ChatUserCaches).where(ChatUserCaches.nick_name == nick_name).first()
    return result

def remove_channel(cache:ChatChannelCaches)->None:
    assert isinstance(cache,ChatChannelCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def remove_user(cache:ChatUserCaches):
    assert isinstance(cache,ChatUserCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def is_user_exist(ip:str,port:int)->bool:
    user_count= PG_SESSION.query(ChatUserCaches).where(ChatUserCaches.remote_ip_address==ip,
                                                        ChatUserCaches.remote_port==port).count()
    if user_count ==1:
        return True
    else:
        return False

def update_client(cache:ChatUserCaches):
    assert isinstance(cache,ChatUserCaches)
    PG_SESSION.commit()


