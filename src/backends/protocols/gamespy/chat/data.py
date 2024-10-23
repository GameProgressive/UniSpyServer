from backends.protocols.gamespy.chat.storage_infos import ChannelInfo, ChannelUser
from library.src.database.pg_orm import PG_SESSION, Users, Profiles, SubProfiles
from servers.chat.src.aggregates.channel import Channel
from servers.chat.src.exceptions.general import ChatException


def nick_and_email_login(nick_name: str, email: str, password_hash: str) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    result = PG_SESSION.query(Users.userid, Profiles.profileid,
                              Users.emailverified, Users.banned).join(Profiles, (Users.userid, Profiles.userid)).where(
        Users.email == email,
        Profiles.nick == nick_name,
        Users.password == password_hash
    ).first()
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
    result = PG_SESSION.query(Users.userid, Profiles.profileid,Users.emailverified, Users.banned).join(Profiles,(Users.userid,Profiles.userid)).join(Profiles,(Profiles.profileid,SubProfiles.profileid)).where(SubProfiles.namespaceid == namespace_id,SubProfiles.uniquenick == uniquenick).first()
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with uniquenick:{uniquenick} in database.")
        # fmt on
    return result


def is_channel_exist(channel_name:str,game_name:str)->bool:
    channel_count = ChannelInfo.objects.filter(channel_name = channel_name,game_name = game_name).count()
    if channel_count == 1:
        return True
    else:
        return False

def update_channel(channel:Channel):
    info = ChannelInfo(
        channel_name=channel.name, game_name=channel.game_name, key_values =channel.kv_manager.data, max_num_user=channel.max_num_user, room_name=channel.room_name, topic=channel.topic, password=channel.password, group_id=channel.group_id, create_time=channel.create_time, previously_joined_channel=channel.previously_join_channel
    )
    info.save()

def remove_channel(channel_name:str)->None:
    info = ChannelInfo.objects(channel_name=channel_name).first()
    info.delete()

def remove_user(nick_name:str):
    user = ChannelUser.objects(nick_name == nick_name).first()
    user.delete()

def is_user_exist(nick_name:str)->bool:
    user_count = ChannelUser.objects.filter(nick_name == nick_name).count()
    if user_count ==1:
        return True
    else:
        return False


def update_client(user:ChannelUser):
    user.save()


def remove_user(nick_name:str):
    user = ChannelUser.objects(nick_name == nick_name).first()
    user.delete()

def is_user_exist(nick_name:str)->bool:
    user_count = ChannelUser.objects.filter(nick_name == nick_name).count()
    if user_count ==1:
        return True
    else:
        return False

