# type:ignore
from datetime import datetime
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column
from backends.library.database.pg_orm import (
    Blocked,
    FriendAddRequest,
    Friends,
    Profiles,
    SubProfiles,
    Users,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
    LoginStatus,
)

from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    GetProfileData,
    LoginData,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPAddBuddyException,
    GPDatabaseException,
    GPStatusException,
    GPException,
)
import backends.protocols.gamespy.presence_search_player.data as psp

# region General
from backends.library.database.pg_orm import ENGINE
from sqlalchemy.orm import Session


def is_email_exist(email: str):
    return psp.is_email_exist(email)


def update_online_time(ip: str, port: int):
    if TYPE_CHECKING:
        assert isinstance(Users.lastip, Column)
    with Session(ENGINE) as session:
        result = session.query(Users).where(Users.lastip == ip).first()
        if result is None:
            return False
        result.lastonline = datetime.now()
        session.commit()


def delete_friend_by_profile_id(profile_id: int):
    with Session(ENGINE) as session:
        friend = session.query(Friends).where(Friends.friendid == profile_id).first()
        if friend is None:
            raise GPDatabaseException(
                f"friend deletion have errors on profile id:{profile_id}"
            )
        else:
            session.delete(friend)
            session.commit()


def get_blocked_profile_id_list(profile_id: int, namespace_id: int) -> list[int]:
    with Session(ENGINE) as session:
        result = (
            session.query(Blocked.targetid)
            .where(Blocked.profileid == profile_id, Blocked.namespaceid == namespace_id)
            .all()
        )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def get_friend_profile_id_list(profile_id: int, namespace_id: int) -> list[int]:
    with Session(ENGINE) as session:
        result = (
            session.query(Friends.targetid)
            .where(Friends.profileid == profile_id, Friends.namespaceid == namespace_id)
            .all()
        )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


# region Profile


def get_profile_infos(profile_id: int, session_key: str) -> GetProfileData:
    """
    Retrieve profile information based on profile_id and namespace_id.

    Args:
        profile_id (int): The ID of the profile to retrieve information for.
        namespace_id (int): The ID of the namespace to filter the sub-profiles.

    Returns:
        tuple: A tuple containing the profile information, sub-profile information, and user information.
    """
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(SubProfiles.session_key, Column)

    with Session(ENGINE) as session:
        namespace_id = (
            session.query(SubProfiles.namespaceid)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
    if namespace_id is None:
        raise GPException("namespace not found")

    with Session(ENGINE) as session:
        result = (
            session.query(Users, Profiles, SubProfiles)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .join(Users, Profiles.userid == Users.userid)
            .where(
                Profiles.profileid == profile_id,
                SubProfiles.namespaceid == namespace_id,
            )
            .first()
        )
    if result is None:
        raise GPException("no profile found")

    if TYPE_CHECKING:
        result = cast(tuple, result)

    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[3]
    if TYPE_CHECKING:
        assert isinstance(profile.nick, str)
        assert isinstance(profile.profileid, int)
        assert isinstance(subprofile.uniquenick, str)
        assert isinstance(user.email, str)
        assert isinstance(Profiles.extra_info, dict)

    data = GetProfileData(
        nick=profile.nick,
        profile_id=profile.profileid,
        unique_nick=subprofile.uniquenick,
        email=user.email,
        extra_infos=Profiles.extra_info,
    )

    return data


def get_user_info_list(email: str, nick_name: str) -> list[tuple[int, int, int]]:
    """
    Retrieve the user information list based on the provided email and nickname.

    Args:
        email (str): The email address of the user to search for.
        nick_name (str): The nickname of the user to search for.

    Returns:
        List[Tuple[int, int, int]]: A list of tuples containing the userid, profileid, and subprofileid
        of users that match the provided email and nickname in the database.
    """
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)

    with Session(ENGINE) as session:
        result = (
            session.query(Users.userid, Profiles.profileid, SubProfiles.subprofileid)
            .join(Users, Profiles.userid == Users.userid)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .where(Users.email == email, Profiles.nick == nick_name)
            .all()
        )
    if TYPE_CHECKING:
        result = cast(list[tuple[int, int, int]], result)
    return result


def get_user_info(unique_nick: str, namespace_id: int) -> tuple[int, int, int]:
    # if TYPE_CHECKING:
    #     assert isinstance(Profiles.profileid, Column)
    #     assert isinstance(Profiles.userid, Column)
    #     assert isinstance(Users.userid, Column)
    #     assert isinstance(Users.email, Column)
    #     assert isinstance(Profiles.nick, Column)
    #     assert isinstance(SubProfiles.uniquenick, Column)
    #     assert isinstance(SubProfiles.namespaceid, Column)

    with Session(ENGINE) as session:
        result = (
            session.query(Users.userid, Profiles.profileid, SubProfiles.subprofileid)
            .join(Users, Profiles.userid == Users.userid)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .where(
                SubProfiles.uniquenick == unique_nick,
                SubProfiles.namespaceid == namespace_id,
            )
            .first()
        )
    if TYPE_CHECKING:
        result = cast(tuple[int, int, int], result)
    return result


def get_user_infos_by_uniquenick_namespace_id(
    unique_nick: str, namespace_id: int
) -> LoginData | None:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(Users.emailverified, Column)
        assert isinstance(Users.banned, Column)

    with Session(ENGINE) as session:
        result = (
            session.query(
                Users.userid,
                Profiles.profileid,
                SubProfiles.subprofileid,
                Profiles.nick,
                Users.email,
                SubProfiles.uniquenick,
                Users.password,
                Users.emailverified,
                Users.banned,
                SubProfiles.namespaceid,
            )
            .join(Users, Profiles.userid == Users.userid)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .where(
                SubProfiles.uniquenick == unique_nick,
                SubProfiles.namespaceid == namespace_id,
            )
            .first()
        )

    return result


def get_user_infos_by_nick_email(nick: str, email: str) -> LoginData | None:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(Users.emailverified, Column)
        assert isinstance(Users.banned, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)

    with Session(ENGINE) as session:
        result = (
            session.query(
                Users.userid,
                Profiles.profileid,
                SubProfiles.subprofileid,
                Profiles.nick,
                Users.email,
                SubProfiles.uniquenick,
                Users.password,
                Users.emailverified,
                Users.banned,
                SubProfiles.namespaceid,
            )
            .join(Users, Profiles.userid == Users.userid)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .where(Users.email == email, Profiles.nick == nick)
            .first()
        )
    data = {
        "user_id": result[0],
        "profile_id": result[1],
        "sub_profile_id": result[2],
        "nick": result[3],
        "email": result[4],
        "unique_nick": result[5],
        "password_hash": result[6],
        "email_verified_flag": result[7],
        "banned_flag": result[8],
        "namespace_id": result[9],
    }
    if result is not None:
        return LoginData(**data)  # type: ignore
    else:
        return None


def update_online_status(user_id: int, status: LoginStatus):
    if TYPE_CHECKING:
        assert isinstance(Users.userid, Column)
    raise NotImplementedError("implement sesskey")
    with Session(ENGINE) as session:
        result = session.query(Users).where(Users.userid == user_id).first()


def get_user_infos_by_authtoken(auth_token: str) -> LoginData | None:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Users.password, Column)
        assert isinstance(Users.emailverified, Column)
        assert isinstance(Users.banned, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)
        assert isinstance(SubProfiles.authtoken, Column)

    with Session(ENGINE) as session:
        result = (
            session.query(
                Users.userid,
                Profiles.profileid,
                SubProfiles.subprofileid,
                Profiles.nick,
                Users.email,
                SubProfiles.uniquenick,
                Users.password,
                Users.emailverified,
                Users.banned,
                SubProfiles.namespaceid,
            )
            .join(Users, Profiles.userid == Users.userid)
            .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
            .where(SubProfiles.authtoken == auth_token)
            .first()
        )

    if result is not None:
        keys = [
            "user_id",
            "profile_id",
            "sub_profile_id",
            "nick",
            "email",
            "unique_nick",
            "password_hash",
            "email_verified_flag",
            "banned_flag",
            "namespace_id",
        ]
        data = {key: result[i] for i, key in enumerate(keys)}
        return LoginData(**data)  # type: ignore
    else:
        return None


def get_block_list(profile_id: int, namespace_id: int) -> list[int]:
    with Session(ENGINE) as session:
        result = (
            session.query(Blocked.targetid)
            .where(
                Blocked.namespaceid == namespace_id,
                Blocked.profileid == profile_id,
            )
            .all()
        )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


# region Buddy


def get_buddy_list(profile_id: int, namespace_id: int) -> list[int]:
    with Session(ENGINE) as session:
        result = (
            session.query(Friends.targetid)
            .where(
                Blocked.namespaceid == namespace_id,
                Blocked.profileid == profile_id,
            )
            .all()
        )
    # assert isinstance(result, list)
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def update_block(profile_id: int, target_id: int, session_key: str) -> None:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)
    with Session(ENGINE) as session:
        namespace_id = (
            session.query(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        result = (
            session.query(Blocked)
            .where(
                Blocked.targetid == target_id,
                Blocked.namespaceid == namespace_id,
                Blocked.profileid == profile_id,
            )
            .count()
        )
        if result == 0:
            b = Blocked(
                targetid=target_id, namespaceid=namespace_id, profileid=profile_id
            )
            session.add(b)
            session.commit()


def update_friend_info(target_id: int, profile_id: int, namespace_id: int):
    with Session(ENGINE) as session:
        result = (
            session.query(Friends)
            .where(
                Friends.targetid == target_id,
                Friends.namespaceid == namespace_id,
                Friends.profileid == profile_id,
            )
            .count()
        )
        f = Friends(targetid=target_id, namespaceid=namespace_id, profileid=profile_id)

        if result == 0:
            session.add(f)
            session.commit()


def add_nick_name(profile_id: int, old_nick: str, new_nick: str):
    assert isinstance(profile_id, int)
    assert isinstance(old_nick, str)
    assert isinstance(new_nick, str)
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
    with Session(ENGINE) as session:
        result = (
            session.query(Profiles)
            .where(Profiles.profileid == profile_id, Profiles.nick == old_nick)
            .first()
        )

        if result is None:
            raise GPDatabaseException("No user infomation found in database.")

        result.nick = new_nick  # type:ignore
        session.commit()


# def update_profile_info(profile: Profiles):
#     session.add(profile)
#     session.commit()


def update_unique_nick(subprofile_id: int, unique_nick: str):
    with Session(ENGINE) as session:
        result = (
            session.query(SubProfiles)
            .where(SubProfiles.subprofileid == subprofile_id)
            .first()
        )
        result.uniquenick = unique_nick  # type:ignore
        session.commit()


def update_subprofile_info(subprofile: SubProfiles):
    with Session(ENGINE) as session:
        session.add(subprofile)
        session.commit()


def add_friend_request(
    profileid: int, targetid: int, namespace_id: int, reason: str
) -> None:
    with Session(ENGINE) as session:
        data = (
            session.query(FriendAddRequest)
            .where(
                FriendAddRequest.profileid == profileid,
                FriendAddRequest.targetid == targetid,
                FriendAddRequest.namespaceid == namespace_id,
            )
            .first()
        )
        if data is not None:
            raise GPAddBuddyException("Request is existed, add friend ignored")
        request = FriendAddRequest(
            profileid=profileid,
            targetid=targetid,
            namespaceid=namespace_id,
            reason=reason,
        )
        session.add(request)
        session.commit()


def get_status(session_key: str) -> dict:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)
    with Session(ENGINE) as session:
        result = (
            session.query(Profiles)
            .join(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
    if result is None:
        raise GPStatusException("No profile found with the provided session key")

    if TYPE_CHECKING:
        assert isinstance(result.statstring, str)
        assert isinstance(result.status, GPStatusCode)
    if "location" not in result.extra_info:
        result.extra_info["location"] = ""
    data = {
        "status_string": result.statstring,
        "location_string": result.extra_info["location"],
        "current_status": result.status,
    }
    return data


def update_status(
    session_key: str,
    current_status: GPStatusCode,
    location_string: str,
    status_string: str,
):
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)
    with Session(ENGINE) as session:
        result = (
            session.query(Profiles)
            .join(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        if result is None:
            raise GPStatusException("No profile found with the provided session key")

        result.statstring = status_string
        result.status = current_status
        assert isinstance(result.extra_info, list)
        result.extra_info.append(location_string)

        session.commit()


def update_new_nick(session_key: str, old_nick: str, new_nick: str):
    with Session(ENGINE) as session:
        result = (
            session.query(Profiles)
            .join(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        if result.nick == old_nick and result.nick != new_nick:
            result.nick = new_nick
        session.commit()


def update_cdkey(session_key: str, cdkey: str):
    with Session(ENGINE) as session:
        subprofile = (
            session.query(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        if subprofile is None:
            raise GPDatabaseException(
                f"no subprofile found with session key:{session_key}"
            )

        subprofile.cdkeyenc = cdkey

        session.commit()


def update_uniquenick(session_key: str, uniquenick: str):
    with Session(ENGINE) as session:
        subprofile = (
            session.query(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        if subprofile is None:
            raise GPDatabaseException(
                f"no subprofile found with session key:{session_key}"
            )

        subprofile.uniquenick = uniquenick
        session.commit()


def update_profiles(session_key: str, extra_info: dict):
    with Session(ENGINE) as session:
        profile = (
            session.query(Profiles)
            .join(SubProfiles)
            .where(SubProfiles.session_key == session_key)
            .first()
        )
        if profile is None:
            raise GPDatabaseException(
                f"no profile found with session key:{session_key}"
            )
        for key, value in extra_info.items():
            profile.extra_info[key] = value

        session.commit()


def update_user(session_key):
    raise NotImplementedError()


if __name__ == "__main__":
    result = get_block_list(1, 1)
