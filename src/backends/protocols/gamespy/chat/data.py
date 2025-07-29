from datetime import datetime, timedelta
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column, func
from backends.library.database.pg_orm import (
    ENGINE,
    ChatChannelCaches,
    ChatUserCaches,
    ChatChannelUserCaches,
    Games,
    Users,
    Profiles,
    SubProfiles,
)
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    ChatException,
    NoSuchNickException,
)
from sqlalchemy.orm import Session


def is_nick_exist(nick_name: str, session: Session) -> bool:
    c = session.query(ChatUserCaches.nick_name).count()
    if c == 1:
        return True
    else:
        return False


def get_secret_key_by_game_name(game_name: str, session: Session) -> str | None:
    result = session.query(Games).where(Games.gamename == game_name).first()
    if result is None:
        return None
    else:
        assert isinstance(result.secretkey, str)
        return result.secretkey


def add_user_cache(cache: ChatUserCaches, session: Session) -> Session:
    session.add(cache)
    session.commit()
    return session


def update_user_cache(cache: ChatUserCaches, session: Session):
    session.commit()


def nick_and_email_login(
    nick_name: str, email: str, password_hash: str, session: Session
) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    assert isinstance(nick_name, str)
    assert isinstance(email, str)
    assert isinstance(password_hash, str)

    result = (
        session.query(
            Users.userid, Profiles.profileid, Users.emailverified, Users.banned
        )
        .join(Profiles, (Users.userid == Profiles.userid))
        .where(
            Users.email == email,
            Profiles.nick == nick_name,
            Users.password == password_hash,
        )
        .first()
    )
    if TYPE_CHECKING:
        result = cast(tuple[int, int, bool, bool], result)
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with nickname:{nick_name} in database.")
        # fmt on

    return result


def uniquenick_login(
    uniquenick: str, namespace_id: int, session: Session
) -> tuple[int, int, bool, bool]:
    """
    return
        userid, profileid, emailverified, banned
    """
    assert isinstance(uniquenick, str)
    assert isinstance(namespace_id, int)

    result = (
        session.query(
            Users.userid, Profiles.profileid, Users.emailverified, Users.banned
        )
        .join(Profiles, (Users.userid == Profiles.userid))
        .join(Profiles, (Profiles.profileid == SubProfiles.profileid))
        .where(
            SubProfiles.namespaceid == namespace_id,
            SubProfiles.uniquenick == uniquenick,
        )
        .first()
    )
    if result is None:
        # fmt: off
        raise ChatException(f"Can not find user with uniquenick:{uniquenick} in database.")
        # fmt on
    if TYPE_CHECKING:
        result = cast(tuple[int, int, bool, bool], result)
    return result


# region User


def is_cdkey_valid(cdkey: str, session: Session) -> bool:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.cdkeyenc, Column)

        result = session.query(SubProfiles).where(SubProfiles.cdkeyenc == cdkey).count()
    if result == 0:
        return False

    else:
        return True


# region Channel


def is_channel_exist(channel_name: str, game_name: str, session: Session) -> bool:
    channel_count = (
        session.query(ChatChannelCaches)
        .where(
            ChatChannelCaches.channel_name == channel_name,
            ChatChannelCaches.game_name == game_name,
            ChatChannelCaches.update_time >= datetime.now() - timedelta(minutes=10),
        )
        .count()
    )
    if channel_count == 1:
        return True
    else:
        return False


def add_channel(channel: ChatChannelCaches, session: Session):
    session.add(channel)
    session.commit()


def get_channel_by_name_and_game(
    channel_name: str, game_name: str, session: Session
) -> ChatChannelCaches | None:
    channel = (
        session.query(ChatChannelCaches)
        .where(
            ChatChannelCaches.channel_name == channel_name,
            ChatChannelCaches.game_name == game_name,
        )
        .first()
    )
    return channel


def get_channel_by_name(
    channel_name: str, session: Session
) -> ChatChannelCaches | None:
    channel = (
        session.query(ChatChannelCaches)
        .where(ChatChannelCaches.channel_name == channel_name)
        .first()
    )
    return channel


def get_channel_by_name_and_ip_port(
    channel_name: str, ip: str, port: int, session: Session
) -> ChatChannelCaches | None:
    assert isinstance(channel_name, str)
    assert isinstance(ip, str)
    assert isinstance(port, int)

    result = (
        session.query(ChatChannelCaches)
        .join(ChatChannelUserCaches)
        .where(
            ChatChannelUserCaches.channel_name == channel_name,
            ChatChannelUserCaches.remote_ip_address == ip,
            ChatChannelUserCaches.remote_port == port,
        )
        .first()
    )
    return result


def get_channel_user_cache_by_name(
    channel_name: str, nick_name: str, session: Session
) -> ChatChannelUserCaches | None:
    assert isinstance(channel_name, str)
    assert isinstance(nick_name, str)

    result = (
        session.query(ChatChannelUserCaches)
        .where(
            ChatChannelUserCaches.channel_name == channel_name,
            ChatChannelUserCaches.nick_name == nick_name,
        )
        .first()
    )
    return result


def get_channel_user_cache_by_name_and_ip_port(
    channel_name: str, ip: str, port: int, session: Session
) -> ChatChannelUserCaches | None:
    result = (
        session.query(ChatChannelUserCaches)
        .where(
            ChatChannelUserCaches.channel_name == channel_name,
            ChatChannelUserCaches.remote_ip_address == ip,
            ChatChannelUserCaches.remote_port == port,
        )
        .first()
    )
    return result


def get_channel_user_caches_by_name(
    channel_name: str, session: Session
) -> list[ChatChannelUserCaches]:
    assert isinstance(channel_name, str)

    result: list[ChatChannelUserCaches] = (
        session.query(ChatChannelUserCaches.key_values)
        .where(ChatChannelUserCaches.channel_name == channel_name)
        .all()
    )  # type:ignore
    return result


def update_channel_time(channel: ChatChannelCaches, session: Session):
    channel.update_time = datetime.now()  # type: ignore

    session.commit()


def db_commit(session: Session):
    session.commit()


def get_user_cache_by_nick_name(
    nick_name: str, session: Session
) -> ChatUserCaches | None:
    result = (
        session.query(ChatUserCaches)
        .where(ChatUserCaches.nick_name == nick_name)
        .first()
    )
    return result


def get_user_cache_by_ip_port(
    ip: str, port: int, session: Session
) -> ChatUserCaches | None:
    result = (
        session.query(ChatUserCaches)
        .where(
            ChatUserCaches.remote_ip_address == ip,
            ChatUserCaches.remote_port == port,
        )
        .first()
    )
    assert isinstance(result, ChatUserCaches | None)
    return result


def get_whois_result(nick: str, session: Session) -> tuple:
    """
    nick is unique in chat
    """

    info = session.query(ChatUserCaches).first()

    if info is None:
        raise NoSuchNickException(f"User not find by nick name:{nick}.")
    channels = (
        session.query(ChatChannelUserCaches.channel_name)
        .join(
            ChatUserCaches,
            ChatChannelUserCaches.nick_name == ChatUserCaches.nick_name,
        )
        .where(ChatChannelUserCaches.nick_name == info.nick_name)
        .all()
    )
    return (
        info.nick_name,
        info.user_name,
        info.nick_name,
        info.remote_ip_address,
        channels,
    )  # type:ignore


def remove_user_caches_by_ip_port(ip: str, port: int, session: Session):
    assert isinstance(ip, str)
    assert isinstance(port, int)

    session.query(ChatChannelUserCaches).where(
        ChatChannelUserCaches.remote_ip_address == ip,
        ChatChannelUserCaches.remote_port == port,
    ).delete()


def remove_user_cache(cache: ChatUserCaches, session: Session):
    session.delete(cache)
    session.commit()


def remove_channel(cache: ChatChannelCaches, session: Session) -> None:
    assert isinstance(cache, ChatChannelCaches)

    session.delete(cache)
    session.commit()


def remove_user(cache: ChatChannelUserCaches, session: Session):
    assert isinstance(cache, ChatChannelUserCaches)

    session.delete(cache)
    session.commit()


def is_user_exist(ip: str, port: int, session: Session) -> bool:
    user_count = (
        session.query(ChatChannelUserCaches)
        .where(
            ChatChannelUserCaches.remote_ip_address == ip,
            ChatChannelUserCaches.remote_port == port,
        )
        .count()
    )
    if user_count == 1:
        return True
    else:
        return False


def update_client(cache: ChatChannelUserCaches, session: Session):
    assert isinstance(cache, ChatChannelUserCaches)

    session.commit()


def add_invited(channel_name: str, client_ip: str, client_port: int, session: Session):
    pass


def find_channel_by_substring(channel_name: str, session: Session) -> list[dict]:
    assert isinstance(channel_name, str)

    names, topics = (
        session.query(ChatChannelCaches.channel_name, ChatChannelCaches.topic)
        .where(ChatChannelCaches.channel_name.like(f"%{channel_name}%"))
        .all()
    )
    users = (
        session.query(ChatChannelUserCaches)
        .where(ChatChannelUserCaches.channel_name.like(f"%{channel_name}%"))
        .all()
    )
    data: list[dict] = []
    assert isinstance(names, list)
    assert isinstance(topics, list)
    assert isinstance(users, list)
    for name, topic, count in zip(names, topics, users):
        d = {"channel_name": name, "total_channel_user": count, "channel_topic": topic}
        data.append(d)
    return data


def find_user_by_substring(user_name: str, session: Session) -> list[dict]:
    assert isinstance(user_name, str)

    names, topics, users = (
        session.query(
            ChatChannelCaches.channel_name,
            ChatChannelCaches.topic,
            func.count(ChatChannelUserCaches.channel_name),
        )
        .join(
            ChatUserCaches,
            ChatUserCaches.nick_name == ChatChannelUserCaches.nick_name,
        )
        .join(
            ChatChannelCaches,
            ChatChannelCaches.channel_name == ChatChannelUserCaches.channel_name,
        )
        .where(ChatUserCaches.user_name.like(f"%{user_name}%"))
        .all()
    )
    data: list[dict] = []

    for name, topic, count in zip(names, topics, users):
        d = {"channel_name": name, "total_channel_user": count, "channel_topic": topic}
        data.append(d)
    return data


def create_channel_user_caches(chan_user: ChatChannelUserCaches, session: Session):
    session.add(chan_user)
    session.commit()


def get_channel_user_caches(channel_name: str, session: Session) -> list[dict]:
    result: list[ChatChannelUserCaches] = (
        session.query(ChatChannelUserCaches)
        .join(
            ChatChannelCaches,
            ChatChannelCaches.channel_name == ChatChannelUserCaches.channel_name,
        )
        .join(
            ChatUserCaches,
            ChatUserCaches.user_name == ChatChannelUserCaches.user_name,
        )
        .where(ChatChannelUserCaches.channel_name == channel_name)
        .all()
    )
    data = []
    for r in result:
        temp = {}
        temp["channel_name"] = r.channel_name
        temp["user_name"] = r.user_name
        temp["public_ip_addr"] = r.remote_ip_address
        temp["nick_name"] = r.nick_name
        data.append(temp)
    return data


def get_channel_user_cache_by_ip(ip: str, port: int, session: Session) -> list[dict]:
    result: list[ChatChannelUserCaches] = (
        session.query(ChatChannelUserCaches)
        .join(
            ChatChannelCaches,
            ChatChannelCaches.channel_name == ChatChannelUserCaches.channel_name,
        )
        .join(
            ChatUserCaches,
            ChatUserCaches.user_name == ChatChannelUserCaches.user_name,
        )
        .where(
            ChatUserCaches.remote_ip_address == ip,
            ChatUserCaches.remote_port == port,
        )
        .all()
    )
    data = []
    for r in result:
        temp = {}
        temp["channel_name"] = r.channel_name
        temp["user_name"] = r.user_name
        temp["public_ip_addr"] = r.remote_ip_address
        temp["nick_name"] = r.nick_name
        data.append(temp)
    return data


if __name__ == "__main__":
    pass
    #
    #     result = (
    #         session.query(ChatUserCaches)
    #         .where(ChatUserCaches.nick_name == "172.19.0.5:52986")
    #         .first()
    #     )
    #     result.nick_name = "changed"
    #     session.commit()
