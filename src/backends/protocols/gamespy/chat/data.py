from datetime import datetime, timedelta
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column, func
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatUserCaches, ChatChannelUserCaches, Users, Profiles, SubProfiles
from frontends.gamespy.protocols.chat.aggregates.exceptions import ChatException, NoSuchNickException


def is_nick_exist(nick_name: str) -> bool:
    c = PG_SESSION.query(ChatUserCaches.nick_name).count()
    if c == 1:
        return True
    else:
        return False


def add_nick_cache(cache: ChatUserCaches):
    PG_SESSION.add(cache)
    PG_SESSION.commit()


def nick_and_email_login(nick_name: str, email: str, password_hash: str) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    assert isinstance(nick_name, str)
    assert isinstance(email, str)
    assert isinstance(password_hash, str)

    result = PG_SESSION.query(Users.userid, Profiles.profileid,
                              Users.emailverified, Users.banned)\
                            .join(Profiles, (Users.userid == Profiles.userid))\
                            .where(
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
    assert isinstance(uniquenick, str)
    assert isinstance(namespace_id, int)
    result = PG_SESSION.query(Users.userid, Profiles.profileid,Users.emailverified, Users.banned).join(Profiles,(Users.userid == Profiles.userid)).join(Profiles,(Profiles.profileid == SubProfiles.profileid)).where(SubProfiles.namespaceid == namespace_id,SubProfiles.uniquenick == uniquenick).first()
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with uniquenick:{uniquenick} in database.")
        # fmt on
    if TYPE_CHECKING:
        result = cast(tuple[int, int, bool, bool],result)
    return result


# region User


def is_cdkey_valid(cdkey: str) -> bool:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.cdkeyenc, Column)
    result = PG_SESSION.query(SubProfiles).where(
        SubProfiles.cdkeyenc == cdkey).count()
    if result == 0:
        return False

    else:
        return True

# region Channel

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


def get_channel_by_name_and_game(channel_name:str,game_name:str)->ChatChannelCaches|None:
    channel = PG_SESSION.query(ChatChannelCaches)\
        .where(ChatChannelCaches.channel_name == channel_name,
                ChatChannelCaches.game_name == game_name)\
                .first()
    return channel

def get_channel_by_name(channel_name:str)->ChatChannelCaches|None:
    channel = PG_SESSION.query(ChatChannelCaches)\
        .where(ChatChannelCaches.channel_name == channel_name)\
                .first()
    return channel

def get_channel_by_name_and_ip_port(channel_name:str,ip:str,port:int)->ChatChannelCaches|None:
    assert isinstance(channel_name,str)
    assert isinstance(ip,str)
    assert isinstance(port,int)
    result = PG_SESSION.query(ChatChannelCaches).join(ChatChannelUserCaches).where(
            ChatChannelUserCaches.channel_name == channel_name, 
            ChatChannelUserCaches.remote_ip_address == ip, 
            ChatChannelUserCaches.remote_port == port).first()
    return result

def get_channel_user_cache_by_name(channel_name:str,nick_name:str)->ChatChannelUserCaches|None:
    assert isinstance(channel_name,str)
    assert isinstance(nick_name,str)
    result = PG_SESSION.query(ChatChannelUserCaches).where(ChatChannelUserCaches.channel_name == channel_name,ChatChannelUserCaches.nick_name == nick_name).first()
    return result

def get_channel_user_cache_by_name_and_ip_port(channel_name:str,ip:str,port:int)->ChatChannelUserCaches|None:
    result = PG_SESSION.query(ChatChannelUserCaches).where(ChatChannelUserCaches.channel_name == channel_name,
    ChatChannelUserCaches.remote_ip_address == ip,
    ChatChannelUserCaches.remote_port == port).first()
    return result

def get_channel_user_caches_by_name(channel_name:str)->list:
    assert isinstance(channel_name,str)
    result = PG_SESSION.query(ChatChannelUserCaches.key_values).where(ChatChannelUserCaches.channel_name == channel_name).all()
    return result

def update_channel_time(channel:ChatChannelCaches):
    channel.update_time = datetime.now() # type: ignore
    PG_SESSION.commit()

def db_commit():
    PG_SESSION.commit()

def get_user_cache_by_nick_name(nick_name:str)->ChatUserCaches|None:
    result = PG_SESSION.query(ChatUserCaches).where(ChatUserCaches.nick_name == nick_name).first()
    return result

def get_user_cache_by_ip_port(ip:str,port:int)->ChatUserCaches:
    result = PG_SESSION.query(ChatUserCaches).where(ChatUserCaches.remote_ip_address == ip, ChatUserCaches.remote_port == port).first()
    assert isinstance(result,ChatUserCaches)
    return result

def get_whois_result(nick:str)->tuple:
    """
    nick is unique in chat
    """
    info = PG_SESSION.query(ChatUserCaches).first()

    if info is None:
        raise NoSuchNickException(f"User not find by nick name:{nick}.")
    channels = PG_SESSION.query(ChatChannelUserCaches.channel_name).join(ChatUserCaches,ChatChannelUserCaches.nick_name == ChatUserCaches.nick_name).where(ChatChannelUserCaches.nick_name == info.nick_name).all()
    return info.nick_name,info.user_name,info.nick_name,info.remote_ip_address,channels # type:ignore



def remove_user_caches_by_ip_port(ip:str,port:int):
    assert isinstance(ip,str)
    assert isinstance(port,int)
    PG_SESSION.query(ChatChannelUserCaches).where(ChatChannelUserCaches.remote_ip_address==ip,ChatChannelUserCaches.remote_port == port).delete()


def remove_channel(cache:ChatChannelCaches)->None:
    assert isinstance(cache,ChatChannelCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def remove_user(cache:ChatChannelUserCaches):
    assert isinstance(cache,ChatChannelUserCaches)
    PG_SESSION.delete(cache)
    PG_SESSION.commit()

def is_user_exist(ip:str,port:int)->bool:
    user_count= PG_SESSION.query(ChatChannelUserCaches).where(ChatChannelUserCaches.remote_ip_address==ip,
                                                        ChatChannelUserCaches.remote_port==port).count()
    if user_count ==1:
        return True
    else:
        return False

def update_client(cache:ChatChannelUserCaches):
    assert isinstance(cache,ChatChannelUserCaches)
    PG_SESSION.commit()


def add_invited(channel_name:str,client_ip:str,client_port:int):
    pass



def find_channel_by_substring(channel_name:str)->list[dict]:
    assert isinstance(channel_name,str)

    names,topics = PG_SESSION.query(ChatChannelCaches.channel_name,ChatChannelCaches.topic)\
                   .where(ChatChannelCaches.channel_name.like(f"%{channel_name}%")).all()
    users = PG_SESSION.query(ChatChannelUserCaches)\
            .where(ChatChannelUserCaches.channel_name.like(f"%{channel_name}%")).all()
    data: list[dict] =[]
    for name,topic,count in zip(names,topics,users):
        d = {
            "channel_name":name,
            "total_channel_user":count,
            "channel_topic":topic
        }
        data.append(d)
    return data

def find_user_by_substring(user_name:str)->list[dict]:
    assert isinstance(user_name,str)
    names,topics,users = PG_SESSION.query(
        ChatChannelCaches.channel_name,
        ChatChannelCaches.topic,func.count(ChatChannelUserCaches.channel_name))\
        .join(ChatUserCaches,ChatUserCaches.nick_name==ChatChannelUserCaches.nick_name)\
        .join(ChatChannelCaches,ChatChannelCaches.channel_name==ChatChannelUserCaches.channel_name)\
        .where(ChatUserCaches.user_name.like(f"%{user_name}%")).all()
    data: list[dict] =[]

    for name,topic,count in zip(names,topics,users):
        d = {
            "channel_name":name,
            "total_channel_user":count,
            "channel_topic":topic
        }
        data.append(d)
    return data

def create_channel_user_caches(chan_user:ChatChannelUserCaches):
    PG_SESSION.add(chan_user)
    PG_SESSION.commit()

def get_channel_user_caches(channel_name:str)->list[dict]:
    result:list[ChatChannelUserCaches] = PG_SESSION.query(ChatChannelUserCaches).join(ChatChannelCaches,ChatChannelCaches.channel_name == ChatChannelUserCaches.channel_name)\
        .join(ChatUserCaches,ChatUserCaches.user_name == ChatChannelUserCaches.user_name)\
        .where(ChatChannelUserCaches.channel_name == channel_name).all()
    data = []
    for r in result:
        temp = {}
        temp["channel_name"] = r.channel_name
        temp["user_name"] = r.user_name
        temp["public_ip_addr"] = r.remote_ip_address
        temp["nick_name"] = r.nick_name
        data.append(temp)
    return data

def get_channel_user_cache_by_ip(ip:str,port:int)->list[dict]:

    result:list[ChatChannelUserCaches] = PG_SESSION.query(ChatChannelUserCaches).join(ChatChannelCaches,ChatChannelCaches.channel_name == ChatChannelUserCaches.channel_name).join(ChatUserCaches,ChatUserCaches.user_name == ChatChannelUserCaches.user_name).where(ChatUserCaches.remote_ip_address==ip,ChatUserCaches.remote_port == port).all()
    data = []
    for r in result:
        temp = {}
        temp["channel_name"] = r.channel_name
        temp["user_name"] = r.user_name
        temp["public_ip_addr"] = r.remote_ip_address
        temp["nick_name"] = r.nick_name
        data.append(temp)
    return data