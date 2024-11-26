from typing import TYPE_CHECKING, Optional, cast
from sqlalchemy import Column
from backends.library.database.pg_orm import (
    Friends,
    Profiles,
    SubProfiles,
    Users,
    PG_SESSION,
)
from servers.presence_search_player.src.contracts.results import *


def verify_email(email: str):
    if TYPE_CHECKING:
        Users.email = cast(Column, Users.email)
        Users.password = cast(Column, Users.password)
    if PG_SESSION.query(Users).filter(Users.email == email).count() == 1:
        return True
    else:
        return False


def verify_email_and_password(email: str, password: str):
    if TYPE_CHECKING:
        assert isinstance(Users.email, Column)
        assert isinstance(Users.password, Column)
    result = (
        PG_SESSION.query(Users)
        .filter(Users.email == email, Users.password == password)
        .count()
    )
    if result == 1:
        return True
    return False


def get_profile_id(email: str, password: str, nick_name: str, partner_id: int) -> int:
    if TYPE_CHECKING:
        assert isinstance(Users.email, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.partnerid, Column)

    pid = (
        PG_SESSION.query(Profiles.profileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            Users.email == email,
            Users.password == password,
            Profiles.nick == nick_name,
            SubProfiles.partnerid == partner_id,
        )
        .first()
    )
    if TYPE_CHECKING:
        pid = cast(int, pid)
    return pid


def add_user(user: Users):
    PG_SESSION.add(user)
    PG_SESSION.commit()


def add_profile(profile: Profiles):
    PG_SESSION.add(profile)
    PG_SESSION.commit()


def add_sub_profile(subprofile: SubProfiles):
    PG_SESSION.add(subprofile)
    PG_SESSION.commit()


def update_user(user: Users):
    PG_SESSION.merge(user)
    PG_SESSION.commit()


def update_profile(profile: Profiles):
    PG_SESSION.merge(profile)
    PG_SESSION.commit()


def update_subprofile(subprofile: SubProfiles):
    PG_SESSION.merge(subprofile)
    PG_SESSION.commit()


def get_user(email: str):
    if TYPE_CHECKING:
        Users.email = cast(Column, Users.email)
    result = PG_SESSION.query(Users).filter(Users.email == email).first()
    return result


def get_profile(user_id: int, nick_name: str) -> Profiles:
    if TYPE_CHECKING:
        Profiles.userid = cast(Column, Profiles.userid)
        Profiles.nick = cast(Column, Profiles.nick)
    result = PG_SESSION.query(Profiles).filter(
        Profiles.userid == user_id, Profiles.nick == nick_name
    ).first()
    return result


def get_sub_profile(profile_id: int, namespace_id: int, product_id: int) -> SubProfiles:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
    result = PG_SESSION.query(SubProfiles).filter(
        SubProfiles.profileid == profile_id,
        SubProfiles.namespaceid == namespace_id,
        SubProfiles.namespaceid == product_id,
    ).first()
    return result


def get_nick_and_unique_nick_list(email: str, password: str, namespace_id: int) -> list[tuple[str, str]]:
    """
    return [(nick, uniquenick)]
    """
    if TYPE_CHECKING:
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
    result = (
        PG_SESSION.query(Profiles.nick, SubProfiles.uniquenick)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            Users.email == email,
            Users.password == password,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list, result)
    return result


def get_friend_info_list(profile_id: int, namespace_id: int, game_name: str) -> list:
    """
    return [(profileid, nick, uniquenick, lastname, firstname, userid, email)]
    """
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(SubProfiles.gamename, Column)
        assert isinstance(Friends.profileid, Column)
    result = (
        PG_SESSION.query(
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            Profiles.lastname,
            Profiles.firstname,
            Users.userid,
            Users.email,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            Profiles.profileid.in_(PG_SESSION.query(
                Friends.profileid == profile_id)),
            SubProfiles.namespaceid == namespace_id,
            SubProfiles.gamename == game_name,
        )
        .all()
    )
    return result


def get_matched_profile_info_list(
    profile_ids: list[int], namespace_id: int
) -> list[tuple[int, str]]:
    """
    return [(profileid,uniquenick)]

    """
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
    result = (
        PG_SESSION.query(SubProfiles.profileid, SubProfiles.uniquenick)
        .filter(
            SubProfiles.profileid.in_(profile_ids),
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    if result is None:
        result = []
    if TYPE_CHECKING:
        result = cast(list[tuple[int, str]], result)
    return result


def get_matched_info_by_nick(
    nick_name: str,
) -> list[SearchResultData]:
    if TYPE_CHECKING:
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Users.userid, Column)
    result = (
        PG_SESSION.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            Profiles.firstname,
            Profiles.lastname,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(Profiles.nick == nick_name)
        .all()
    )
    temp: list[SearchResultData] = []
    for email, profile_id, nick, first_name, last_name, uniquenick, namespace_id in result:
        temp.append(SearchResultData(profile_id=profile_id, nick=nick, uniquenick=uniquenick,
                    email=email, namespace_id=namespace_id, firstname=first_name, lastname=last_name))

    return temp


def get_matched_info_by_email(
    email: str,
) -> list[SearchResultData]:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.userid, Column)
    result = (
        PG_SESSION.query(
            Profiles.profileid,
            Profiles.nick,
            Profiles.firstname,
            Profiles.lastname,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(Users.email == email)
        .all()
    )
    temp: list[SearchResultData] = []
    for email, profile_id, nick, first_name, last_name, uniquenick, namespace_id in result:
        temp.append(SearchResultData(profile_id=profile_id, nick=nick, uniquenick=uniquenick,
                    email=email, namespace_id=namespace_id, firstname=first_name, lastname=last_name))
    return temp


def get_matched_info_by_nick_and_email(nick_name: str, email: str):
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.email, Column)
    result = (
        PG_SESSION.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            Profiles.firstname,
            Profiles.lastname,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    temp: list[SearchResultData] = []
    for email, profile_id, nick, first_name, last_name, uniquenick, namespace_id in result:
        temp.append(SearchResultData(profile_id=profile_id, nick=nick, uniquenick=uniquenick,
                    email=email, namespace_id=namespace_id, firstname=first_name, lastname=last_name))
    return temp


def get_matched_info_by_uniquenick_and_namespaceid(
    unique_nick: str, namespace_id: int
) -> list[SearchResultData]:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.email, Column)
    result = (
        PG_SESSION.query(
            Profiles.profileid,
            Profiles.nick,
            Profiles.firstname,
            Profiles.lastname,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    temp: list[SearchResultData] = []
    for email, profile_id, nick, first_name, last_name, uniquenick, namespace_id in result:
        temp.append(SearchResultData(profile_id=profile_id, nick=nick, uniquenick=uniquenick,
                    email=email, namespace_id=namespace_id, firstname=first_name, lastname=last_name))

    return temp


def get_matched_info_by_uniquenick_and_namespaceids(
    unique_nick: str, namespace_ids: list[int]
) -> list[SearchResultData]:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(Profiles.firstname, Column)
        assert isinstance(Profiles.lastname, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.email, Column)

    result = (
        PG_SESSION.query(
            Profiles.profileid,
            Profiles.nick,
            Profiles.firstname,
            Profiles.lastname,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid.in_(namespace_ids)
        )
        .all()
    )
    data: list[SearchResultData] = []
    for email, profile_id, nick, first_name, last_name, uniquenick, namespace_id in result:
        data.append(SearchResultData(profile_id=profile_id, nick=nick, uniquenick=uniquenick,
                    email=email, namespace_id=namespace_id, firstname=first_name, lastname=last_name))

    return data


def is_uniquenick_exist(unique_nick: str, namespace_id: int, game_name: str) -> bool:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.gamename, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
    result = (
        PG_SESSION.query(Profiles)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.gamename == game_name,
            SubProfiles.namespaceid == namespace_id,
        )
        .count()
    )

    if result == 0:
        return False
    else:
        return True


def is_email_exist(email: str) -> bool:
    if TYPE_CHECKING:
        Users.userid = cast(Column, Users.userid)
        Users.email = cast(Column, Users.email)
    result = PG_SESSION.query(Users.userid).filter(
        Users.email == email).count()
    # According to game <FSW> partnerid is not nessesary
    if result == 0:
        return False
    return True
