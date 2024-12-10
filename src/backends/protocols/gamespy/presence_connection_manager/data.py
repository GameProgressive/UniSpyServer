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
    PG_SESSION,
)
from servers.presence_connection_manager.src.aggregates.enums import GPStatusCode, LoginStatus
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from servers.presence_connection_manager.src.contracts.results import GetProfileData, LoginData
from servers.presence_search_player.src.aggregates.exceptions import GPAddBuddyException, GPDatabaseException, GPStatusException, GPException


def update_online_time(ip: str, port: int):
    if TYPE_CHECKING:
        assert isinstance(Users.lastip, Column)

    result = PG_SESSION.query(Users).where(Users.lastip == ip).first()
    if result is None:
        return False
    result.lastonline = datetime.now()
    PG_SESSION.commit()


def is_email_exist(email: str) -> bool:
    if TYPE_CHECKING:
        assert isinstance(Users.email, Column)
    if PG_SESSION.query(Users).filter(Users.email == email).count() == 1:
        return True
    else:
        return False


def delete_friend_by_profile_id(profile_id: int):
    friend = PG_SESSION.query(Friends).filter(
        Friends.friendid == profile_id).first()
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
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def get_friend_profile_id_list(profile_id: int, namespace_id: int) -> list[int]:
    result = (
        PG_SESSION.query(Friends.targetid)
        .filter(Friends.profileid == profile_id, Friends.namespaceid == namespace_id)
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


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

    namespace_id = PG_SESSION.query(SubProfiles.namespaceid).where(
        SubProfiles.session_key == session_key).first()
    if namespace_id is None:
        raise GPException("namespace not found")

    result = (
        PG_SESSION.query(Users, Profiles, SubProfiles)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .join(Users, Profiles.userid == Users.userid)
        .where(
            Profiles.profileid == profile_id, SubProfiles.namespaceid == namespace_id
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

    data = GetProfileData(nick=profile.nick, profile_id=profile.profileid,
                          unique_nick=subprofile.uniquenick, email=user.email, extra_infos=Profiles.extra_info)

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

    result = (
        PG_SESSION.query(Users.userid, Profiles.profileid,
                         SubProfiles.subprofileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[tuple[int, int, int]], result)
    return result


def get_user_info(unique_nick: str, namespace_id: int) -> tuple[int, int, int]:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.uniquenick, Column)
        assert isinstance(SubProfiles.namespaceid, Column)

    result = (
        PG_SESSION.query(Users.userid, Profiles.profileid,
                         SubProfiles.subprofileid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .first()
    )
    if TYPE_CHECKING:
        result = cast(tuple[int, int, int], result)
    return result


def get_user_infos_by_uniquenick_namespace_id(unique_nick: str, namespace_id: int) -> LoginData | None:
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

    result = (
        PG_SESSION.query(Users.userid,
                         Profiles.profileid,
                         SubProfiles.subprofileid,
                         Profiles.nick,
                         Users.email,
                         SubProfiles.uniquenick,
                         Users.password,
                         Users.emailverified,
                         Users.banned,
                         SubProfiles.namespaceid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
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

    result = (
        PG_SESSION.query(Users.userid,
                         Profiles.profileid,
                         SubProfiles.subprofileid,
                         Profiles.nick,
                         Users.email,
                         SubProfiles.uniquenick,
                         Users.password,
                         Users.emailverified,
                         Users.banned,
                         SubProfiles.namespaceid
                         )
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            Users.email == email,
            Profiles.nick == nick
        )
        .first()
    )
    if result is not None:
        return LoginData(*result)  # type: ignore
    else:
        return None


def update_online_status(user_id: int, status: LoginStatus):
    if TYPE_CHECKING:
        assert isinstance(Users.userid, Column)
    result = PG_SESSION.query(Users).where(Users.userid == user_id).first()
    raise NotImplementedError("implement sesskey")


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

    result = (
        PG_SESSION.query(Users.userid,
                         Profiles.profileid,
                         SubProfiles.subprofileid,
                         Profiles.nick,
                         Users.email,
                         SubProfiles.uniquenick,
                         Users.password,
                         Users.emailverified,
                         Users.banned,
                         SubProfiles.namespaceid)
        .join(Users, Profiles.userid == Users.userid)
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)
        .filter(
            SubProfiles.authtoken == auth_token
        )
        .first()
    )
    if result is not None:
        return LoginData(*result)  # type: ignore
    else:
        return None


def get_block_list(profile_id: int, namespace_id: int) -> list[int]:
    result = (
        PG_SESSION.query(Blocked.targetid)
        .filter(
            Blocked.namespaceid == namespace_id,
            Blocked.profileid == profile_id,
        ).all()
    )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def get_buddy_list(profile_id: int, namespace_id: int) -> list[int]:
    result = (
        PG_SESSION.query(Friends.targetid)
        .filter(
            Blocked.namespaceid == namespace_id,
            Blocked.profileid == profile_id,
        ).all()
    )
    # assert isinstance(result, list)
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def update_block(profile_id: int, target_id: int, session_key: str) -> None:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)
    namespace_id = PG_SESSION.query(SubProfiles).where(
        SubProfiles.session_key == session_key).first()
    result = (
        PG_SESSION.query(Blocked)
        .where(
            Blocked.targetid == target_id,
            Blocked.namespaceid == namespace_id,
            Blocked.profileid == profile_id,
        )
        .count()
    )
    if result == 0:
        b = Blocked(targetid=target_id, namespaceid=namespace_id,
                    profileid=profile_id)
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
    f = Friends(targetid=target_id, namespaceid=namespace_id,
                profileid=profile_id)

    if result == 0:
        PG_SESSION.add(f)
        PG_SESSION.commit()


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
    result = (
        PG_SESSION.query(Profiles)
        .filter(Profiles.profileid == profile_id, Profiles.nick == old_nick)
        .first()
    )

    if result is None:
        raise GPDatabaseException("No user infomation found in database.")

    result.nick = new_nick  # type:ignore
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
    result.uniquenick = unique_nick  # type:ignore
    PG_SESSION.commit()


def update_subprofile_info(subprofile: SubProfiles):
    PG_SESSION.add(subprofile)
    PG_SESSION.commit()


def add_friend_request(profileid: int, targetid: int, namespace_id: int, reason: str) -> None:
    data = PG_SESSION.query(FriendAddRequest).where(FriendAddRequest.profileid ==
                                                    profileid,
                                                    FriendAddRequest.targetid == targetid,
                                                    FriendAddRequest.namespaceid == namespace_id).first()
    if data is not None:
        raise GPAddBuddyException("Request is existed, add friend ignored")
    request = FriendAddRequest(profileid=profileid, targetid=targetid,
                               namespaceid=namespace_id, reason=reason)
    PG_SESSION.add(request)
    PG_SESSION.commit()


def get_status(session_key: str) -> UserStatus:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)

    result = PG_SESSION.query(Profiles).join(SubProfiles).where(
        SubProfiles.session_key == session_key).first()
    if result is None:
        raise GPStatusException(
            "No profile found with the provided session key")

    if TYPE_CHECKING:
        assert isinstance(result.statstring, str)
        assert isinstance(result.location, str)
        assert isinstance(result.status, GPStatusCode)

    data = UserStatus(status_string=result.statstring,
                      location_string=result.location, current_status=result.status)
    return data


def update_status(session_key: str, status: UserStatus):
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.session_key, Column)
    result = PG_SESSION.query(Profiles).join(SubProfiles).where(
        SubProfiles.session_key == session_key).first()
    if result is None:
        raise GPStatusException(
            "No profile found with the provided session key")

    result.statstring = status.status_string
    result.status = status.current_status
    result.location = status.location_string

    PG_SESSION.commit()


if __name__ == "__main__":
    result = get_block_list(1, 1)
