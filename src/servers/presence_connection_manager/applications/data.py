from sqlalchemy import insert
from library.database.pg_orm import (
    Blocked,
    Friends,
    Profiles,
    SubProfiles,
    Users,
    PG_SESSION,
)
from servers.presence_search_player.exceptions.general import GPDatabaseException


def is_email_exist(email: str) -> bool:
    if PG_SESSION.query(Users).filter(Users.email == email).count() == 1:
        return True
    else:
        return False


def delete_friend_by_profile_id(profile_id: int, target_id: int, namespace_id: int):
    friend = PG_SESSION.query(Friends).filter(Friends.ProfileId == profile_id, Friends.TargetId == target_id, Friends.NamespaceId == namespace_id).first()
    if friend is None:
        raise GPDatabaseException(
            f"friend deletion have errors on profile id:{profile_id}"
        )
    else:
        PG_SESSION.delete(friend)
        PG_SESSION.commit()


def get_blocked_profile_id_list(profile_id: int, namespace_id: int) -> list[int]:
    result = (
        PG_SESSION.query(Blocked.targetid)
        .filter(Blocked.profileid == profile_id, Blocked.namespaceid == namespace_id)
        .all()
    )
    return result


def get_friend_profile_id_list(profile_id: int, namespace_id: int) -> list[int]:
    result = (
        PG_SESSION.query(Friends.targetid)
        .filter(Friends.profileid == profile_id, Friends.namespaceid == namespace_id)
        .all()
    )
    return result


def get_profile_info_list(profile_id: int, namespace_id: int):
    result = (
        PG_SESSION.query(Profiles, SubProfiles, Users)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .join(Users, Profiles.userid == Users.userid)
        .filter(
            Profiles.profileid == profile_id, SubProfiles.namespaceid == namespace_id
        )
        .first()
    )
    return result


def get_user_info_list(email: str, nick_name: str) -> list[tuple[int, int, int]]:
    """
    return (userid, profileid, subprofileid)
    """
    result = (
        PG_SESSION.query(Users.userid, Profiles.profileid, SubProfiles.subprofileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    return result


def get_user_info(unique_nick: str, namespace_id: int) -> tuple[int, int, int]:
    result = (
        PG_SESSION.query(Users.userid, Profiles.profileid, SubProfiles.subprofileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .first()
    )
    return result


def get_user_infos(unique_nick: str, namespace_id: int) -> list[tuple[int, int, int]]:
    result = (
        PG_SESSION.query(Users.userid, Profiles.profileid, SubProfiles.subprofileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    return result


def update_block_info_list(target_id: int, profile_id: int, namespace_id: int) -> None:
    result = (
        PG_SESSION.query(Blocked)
        .filter(
            Blocked.targetid == target_id,
            Blocked.namespaceid == namespace_id,
            Blocked.profileid == profile_id,
        )
        .count()
    )
    if result == 0:
        b = Blocked(targetid=target_id, namespaceid=namespace_id, profileid=profile_id)
        PG_SESSION.add(b)
        PG_SESSION.commit()


def update_friend_info(target_id: int, profile_id: int, namespace_id: int):
    result = (
        PG_SESSION.query(Friends)
        .filter(
            Friends.targetid == target_id,
            Friends.namespaceid == namespace_id,
            Friends.profileid == profile_id,
        )
        .count()
    )
    f = Friends(targetid=target_id, namespaceid=namespace_id, profileid=profile_id)

    if result == 0:
        PG_SESSION.add(f)
        PG_SESSION.commit()


def add_nick_name(profile_id: int, old_nick: str, new_nick: str):

    result = (
        PG_SESSION.query(Profiles)
        .filter(Profiles.profileid == profile_id, Profiles.nick == old_nick)
        .first()
    )

    if result is None:
        raise GPDatabaseException("No user infomation found in database.")

    result.nick = new_nick
    PG_SESSION.commit()


def update_profile_info(profile: Profiles):
    PG_SESSION.add(profile)
    PG_SESSION.commit()

def update_unique_nick(subprofile_id: int, unique_nick: str):
    result = (
        PG_SESSION.query(SubProfiles)
        .filter(SubProfiles.subprofileid == subprofile_id)
        .first()
    )
    result.uniquenick = unique_nick
    PG_SESSION.commit()


def update_subprofile_info(subprofile: SubProfiles):
    PG_SESSION.add(subprofile)
    PG_SESSION.commit()
