from sqlalchemy import insert
from library.database.pg_orm import (
    Friends,
    Profiles,
    SubProfiles,
    Users,
    PG_SESSION,
)


def verify_email(email: str):
    if PG_SESSION.query(Users).filter(Users.email == email).count() == 1:
        return True
    else:
        return False


def verify_email_and_password(email: str, password: str):
    result = (
        PG_SESSION.query(Users)
        .filter(Users.email == email, Users.password == password)
        .count()
    )
    if result == 1:
        return True
    return False


def get_profile_id(email: str, password: str, nick_name: str, partner_id: int):
    result = (
        PG_SESSION.query(Profiles, SubProfiles, Users)
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

    return result


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
    result = PG_SESSION.query(Users).filter(Users.email == email).first()
    return result


def get_profile(user_id: int, nick_name: str) -> Profiles:
    result = PG_SESSION.query(Profiles).filter(
        Profiles.userid == user_id, Profiles.nick == nick_name
    )
    return result


def get_sub_profile(profile_id: int, namespace_id: int, product_id: int) -> SubProfiles:
    PG_SESSION.query(SubProfiles).filter(
        SubProfiles.profileid == profile_id,
        SubProfiles.namespaceid == namespace_id,
        SubProfiles.namespaceid == product_id,
    )


def get_nick_and_unique_nick_list(email: str, password: str, namespace_id: int):
    """
    return [(nick, uniquenick)]
    """
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
    return result


def get_friend_info_list(profile_id: int, namespace_id: int, game_name: str):
    """
    return [(profileid, nick, uniquenick, lastname, firstname, userid, email)]
    """

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
            Profiles.profileid.in_(PG_SESSION.query(Friends.profileid == profile_id)),
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
    result = (
        PG_SESSION.query(SubProfiles.profileid, SubProfiles.uniquenick)
        .filter(
            SubProfiles.profileid.in_(profile_ids),
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    return result


def get_matched_info_by_nick(
    nick_name: str,
) -> list[tuple[int, str, str, str, str, int]]:
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
        .filter(Profiles.nick == nick_name)
        .all()
    )
    return result


def get_matched_info_by_email(
    email: str,
) -> list[tuple[int, str, str, str, str, int]]:
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
    return result


def get_matched_info_by_nick_and_email(nick_name: str, email: str):
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
        .filter(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    return result


def get_matched_info_by_uniquenick_and_namespaceid(
    unique_nick: str, namespace_id: int
) -> list[tuple[int, str, str, str, str, int]]:
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
    return result


def is_uniquenick_exist(unique_nick: str, namespace_id: int, game_name: str):
    result = (
        PG_SESSION.query(Profiles)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.gamename == game_name,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )

    return result


def is_email_exist(email: str):
    result = PG_SESSION.query(Users.userid).filter(Users.email == email).count()
    #! According to FSW partnerid is not nessesary
    if result == 0:
        return False
    return True
