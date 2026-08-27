from typing import TYPE_CHECKING, cast

from sqlalchemy import Column
from sqlalchemy.orm import Session

from backends.library.database.pg_orm import (
    Friendlist,
    Profiles,
    SubProfiles,
    Users,
)
from frontends.gamespy.protocols.presence_search_player.contracts.results import (
    SearchResultData,
)


def db_commit(session: Session) -> None:
    session.commit()


def verify_email(email: str, session: Session):
    assert isinstance(email, str)

    count = session.query(Users).where(Users.email == email).count()
    return count == 1


def verify_email_and_password(email: str, password: str, session: Session):
    assert isinstance(email, str)
    assert isinstance(password, str)

    result = (
        session.query(Users)
        .where(Users.email == email, Users.password == password)
        .count()
    )
    return result == 1


def get_profile_id(
    email: str, password: str, nick_name: str, partner_id: int, session: Session
) -> int | None:
    result = (
        session.query(Profiles.profileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(
            Users.email == email,
            Users.password == password,
            Profiles.nick == nick_name,
            SubProfiles.partnerid == partner_id,
        )
        .first()
    )
    if result is not None:
        result = result[0]
        assert isinstance(result, int)
    return result


def add_user(user: Users, session: Session):
    session.add(user)
    session.commit()


def add_profile(profile: Profiles, session: Session):
    session.add(profile)
    session.commit()


def add_sub_profile(subprofile: SubProfiles, session: Session):
    session.add(subprofile)
    session.commit()


def update_user(user: Users, session: Session):
    session.merge(user)
    session.commit()


def update_profile(profile: Profiles, session: Session):
    session.merge(profile)
    session.commit()


def update_subprofile(subprofile: SubProfiles, session: Session):
    session.merge(subprofile)
    session.commit()


def get_user(email: str, session: Session) -> Users | None:
    assert isinstance(email, str)

    result = session.query(Users).where(Users.email == email).first()
    return result


def get_profile(user_id: int, nick_name: str, session: Session) -> Profiles | None:
    assert isinstance(user_id, int)
    assert isinstance(nick_name, str)

    result = (
        session.query(Profiles)
        .where(Profiles.userid == user_id, Profiles.nick == nick_name)
        .first()
    )
    return result


def get_sub_profile(
    profile_id: int, namespace_id: int, product_id: int, session: Session
) -> SubProfiles | None:
    assert isinstance(profile_id, int)
    assert isinstance(namespace_id, int)
    assert isinstance(product_id, int)

    result = (
        session.query(SubProfiles)
        .where(
            SubProfiles.profileid == profile_id,
            SubProfiles.namespaceid == namespace_id,
            SubProfiles.productid == product_id,
        )
        .first()
    )
    return result


def get_nick_and_unique_nick_list(
    email: str, password: str, namespace_id: int, session: Session
) -> list[tuple[str, str]]:
    """
    return [(nick, uniquenick)]
    """

    result = (
        session.query(Profiles.nick, SubProfiles.uniquenick)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(
            Users.email == email,
            Users.password == password,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    assert isinstance(result, list)
    data = []
    for r in result:
        # convert to tuple
        data.append(tuple(r))
    return data


def get_friend_info_list(
    profile_id: int, namespace_id: int, game_name: str, session: Session
) -> list:
    """
    return [(profileid, nick, uniquenick, lastname, firstname, userid, email)]
    """

    result = (
        session.query(
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            Users.userid,
            Users.email,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .join(Friendlist, Profiles.profileid == Friendlist.profileid)
        # todo check whether friends table join is correct
        .where(
            Friendlist.profileid == profile_id,
            SubProfiles.namespaceid == namespace_id,
            SubProfiles.gamename == game_name,
        )
        .all()
    )
    return result


def get_matched_profile_info_list(
    profile_ids: list[int], namespace_id: int, session: Session
) -> list[tuple[int, str]]:
    """
    return [(profileid,uniquenick)]

    """
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)

    result = (
        session.query(SubProfiles.profileid, SubProfiles.uniquenick)
        .where(
            SubProfiles.profileid.in_(profile_ids),
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    data = []
    for r in result:
        data.append(tuple(r))
    return data


def get_matched_info_by_nick(nick_name: str, session: Session) -> list[dict]:
    result = (
        session.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
            Profiles.extra_info,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(Profiles.nick == nick_name)
        .all()
    )
    temp: list[dict] = []
    for email, profile_id, nick, uniquenick, namespace_id, extra_info in result:
        if TYPE_CHECKING:
            extra_info = cast(dict, extra_info)
        firstname = extra_info.get("firstname", "")
        lastname = extra_info.get("lastname", "")
        t = {
            "profile_id": profile_id,
            "nick": nick,
            "uniquenick": uniquenick,
            "email": email,
            "namespace_id": namespace_id,
            "firstname": firstname,
            "lastname": lastname,
        }
        temp.append(t)

    return temp


def get_matched_info_by_email(email: str, session: Session) -> list[dict]:
    result = (
        session.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
            Profiles.extra_info,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(Users.email == email)
        .all()
    )
    temp: list[dict] = []
    for email_r, profile_id, nick, uniquenick, namespace_id, extra_info in result:
        if TYPE_CHECKING:
            extra_info = cast(dict, extra_info)
        firstname = extra_info.get("firstname", "")
        lastname = extra_info.get("lastname", "")
        t = {
            "profile_id": profile_id,
            "nick": nick,
            "uniquenick": uniquenick,
            "email": email_r,
            "namespace_id": namespace_id,
            "firstname": firstname,
            "lastname": lastname,
        }
        temp.append(t)
    return temp


def get_matched_info_by_nick_and_email(
    nick_name: str, email: str, session: Session
) -> list[dict]:
    result = (
        session.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
            Profiles.extra_info,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    data: list[dict] = []
    for email_r, profile_id, nick, uniquenick, namespace_id, extra_info in result:
        if TYPE_CHECKING:
            extra_info = cast(dict, extra_info)
        firstname = extra_info.get("firstname", "")
        lastname = extra_info.get("lastname", "")
        t = {
            "profile_id": profile_id,
            "nick": nick,
            "uniquenick": uniquenick,
            "email": email_r,
            "namespace_id": namespace_id,
            "firstname": firstname,
            "lastname": lastname,
        }
        data.append(t)
    return data


def get_matched_info_by_uniquenick_and_namespaceid(
    unique_nick: str, namespace_id: int, session: Session
) -> list[dict]:
    result = (
        session.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
            Profiles.extra_info,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    data: list[dict] = []
    for email, profile_id, nick, uniquenick, namespace_id_r, extra_info in result:
        if TYPE_CHECKING:
            extra_info = cast(dict, extra_info)
        firstname = extra_info.get("firstname", "")
        lastname = extra_info.get("lastname", "")
        t = {
            "profile_id": profile_id,
            "nick": nick,
            "uniquenick": uniquenick,
            "email": email,
            "namespace_id": namespace_id_r,
            "firstname": firstname,
            "lastname": lastname,
        }
        data.append(t)

    return data


def get_matched_info_by_uniquenick_and_namespaceids(
    unique_nick: str, namespace_ids: list[int], session: Session
) -> list[SearchResultData]:
    result = (
        session.query(
            Users.email,
            Profiles.profileid,
            Profiles.nick,
            SubProfiles.uniquenick,
            SubProfiles.namespaceid,
            Profiles.extra_info,
        )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid.in_(namespace_ids),
        )
        .all()
    )
    data: list[SearchResultData] = []
    for email, profile_id, nick, uniquenick, namespace_id, extra_info in result:
        if TYPE_CHECKING:
            extra_info = cast(dict, extra_info)
        firstname = extra_info.get("firstname", "")
        lastname = extra_info.get("lastname", "")
        t = SearchResultData(
            profile_id=profile_id,
            nick=nick,
            uniquenick=uniquenick,
            email=email,
            namespace_id=namespace_id,
            firstname=firstname,
            lastname=lastname,
        )

        data.append(t)

    return data


def is_uniquenick_exist(
    unique_nick: str, namespace_id: int, game_name: str, session: Session
) -> bool:
    result = (
        session.query(Profiles)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .where(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.gamename == game_name,
            SubProfiles.namespaceid == namespace_id,
        )
        .count()
    )

    return result != 0


def is_email_exist(email: str, session: Session) -> bool:
    if TYPE_CHECKING:
        Users.userid = cast(Column, Users.userid)
        Users.email = cast(Column, Users.email)

    result = session.query(Users.userid).where(Users.email == email).count()
    # According to game <FSW> partnerid is not nessesary
    return result != 0
