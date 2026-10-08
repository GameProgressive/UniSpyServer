from datetime import datetime
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column

# region General
from sqlalchemy.orm import Session

import backends.protocols.gamespy.presence_search_player.data as psp
from backends.library.database.pg_orm import (
    Blacklist,
    Friendlist,
    FriendRequest,
    Profiles,
    SubProfiles,
    Users,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
    LoginStatus,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.requests import (
    ExtraInfo,
)
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import (
    BuddyMessageFriendAddResult,
    GetProfileData,
    LoginData,
)
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPAddBuddyException,
    GPDatabaseException,
    GPException,
    GPStatusException,
    GPUpdateProfileException,
)


def is_email_exist(email: str, session: Session):
    return psp.is_email_exist(email, session)


def update_online_time(ip: str, port: int, session: Session):
    result = session.query(Users).where(Users.lastip == ip).first()
    if result is None:
        return False
    result.lastonline = datetime.now()
    session.commit()


def del_buddy(target_profile_id: int, subprofile_id: int, session: Session):
    friend = (
        session.query(Friendlist)
        .where(
            Friendlist.targetid == target_profile_id,
            SubProfiles.id == subprofile_id,
        )
        .first()
    )
    if friend is None:
        raise GPDatabaseException(
            f"friend deletion have errors on profile id:{target_profile_id}"
        )
    else:
        session.delete(friend)
        session.commit()


def get_blocked_profile_id_list(
    profile_id: int, namespace_id: int, session: Session
) -> list[int]:
    result = (
        session.query(Blacklist.targetid)
        .where(Blacklist.profileid == profile_id, Blacklist.namespaceid == namespace_id)
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


def get_friend_profile_id_list(
    profile_id: int, namespace_id: int, session: Session
) -> list[int]:
    result = (
        session.query(Friendlist.targetid)
        .where(
            Friendlist.profileid == profile_id, Friendlist.namespaceid == namespace_id
        )
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


# region Profile


def get_subprofile_by_id(subprofile_id: int, session: Session) -> SubProfiles:
    result = (
        session.query(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if not result:
        raise GPDatabaseException("Subprofile not found by subprofile id")
    return result


def get_profile_infos(
    profile_id: int, subprofile_id: int, session: Session
) -> GetProfileData:
    """
    Retrieve profile information based on profile_id and namespace_id.

    Args:
        profile_id (int): The ID of the profile to retrieve information for.
        namespace_id (int): The ID of the namespace to filter the sub-profiles.

    Returns:
        tuple: A tuple containing the profile information, sub-profile information, and user information.
    """
    result = (
        session.query(Users, Profiles, SubProfiles)
        .join(SubProfiles)
        .join(Users)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if result is None:
        raise GPException("no profile found")

    if TYPE_CHECKING:
        result = cast(tuple[Users, Profiles, SubProfiles], result)
    user, profile, subprofile = result

    assert subprofile.uniquenick is not None
    data = GetProfileData(
        nick=profile.nick,
        profile_id=profile.id,
        unique_nick=subprofile.uniquenick,
        email=user.email,
        extra_infos=profile.extra_info,
    )

    return data


def get_user_info_list(
    email: str, nick_name: str, session: Session
) -> list[tuple[int, int, int]]:
    """
    Retrieve the user information list based on the provided email and nickname.

    Args:
        email (str): The email address of the user to search for.
        nick_name (str): The nickname of the user to search for.

    Returns:
        List[Tuple[int, int, int]]: A list of tuples containing the userid, profileid, and subprofileid
        of users that match the provided email and nickname in the database.
    """
    result = (
        session.query(Users.id, Profiles.id, SubProfiles.id)
        .join(Users, Profiles.userid == Users.id)
        .join(SubProfiles, Profiles.id == SubProfiles.profileid)
        .where(Users.email == email, Profiles.nick == nick_name)
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[tuple[int, int, int]], result)
    return result


def get_user_info(
    unique_nick: str, namespace_id: int, session: Session
) -> tuple[int, int, int]:
    result = (
        session.query(Users.id, Profiles.id, SubProfiles.id)
        .join(Users, Profiles.userid == Users.id)
        .join(SubProfiles, Profiles.id == SubProfiles.profileid)
        .where(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .first()
    )
    if TYPE_CHECKING:
        result = cast(tuple[int, int, int], result)
    return result


_login_infos = [
    Users.id,
    Profiles.id,
    SubProfiles.id,
    Profiles.nick,
    Users.email,
    SubProfiles.uniquenick,
    Users.password,
    Users.emailverified,
    Users.banned,
    SubProfiles.namespaceid,
]


def get_user_infos_by_uniquenick_namespace_id(
    unique_nick: str, namespace_id: int, session: Session
) -> LoginData | None:
    result = (
        session.query(*_login_infos)
        .join(Users, Profiles.userid == Users.id)
        .join(SubProfiles, Profiles.id == SubProfiles.profileid)
        .where(
            SubProfiles.uniquenick == unique_nick,
            SubProfiles.namespaceid == namespace_id,
        )
        .first()
    )

    if result is None:
        return None

    login = LoginData(
        user_id=result[0],
        profile_id=result[1],
        subprofile_id=result[2],
        nick=result[3],
        email=result[4],
        unique_nick=result[5],
        password_hash=result[6],
        email_verified_flag=result[7],
        banned_flag=result[8],
        namespace_id=result[9],
    )
    return login


def get_user_infos_by_nick_email(
    nick: str, email: str, session: Session
) -> LoginData | None:
    result = (
        session.query(*_login_infos)
        .join(Users)
        .join(SubProfiles)
        .where(Users.email == email, Profiles.nick == nick)
        .first()
    )
    if result is None:
        return None

    login = LoginData(
        user_id=result[0],
        profile_id=result[1],
        subprofile_id=result[2],
        nick=result[3],
        email=result[4],
        unique_nick=result[5],
        password_hash=result[6],
        email_verified_flag=result[7],
        banned_flag=result[8],
        namespace_id=result[9],
    )
    return login


def update_online_status(user_id: int, status: LoginStatus, session: Session):
    result = session.query(Users).where(Users.id == user_id).first()
    assert result
    result.lastonline = datetime.now()
    session.commit()


def get_user_infos_by_authtoken(auth_token: str, session: Session) -> LoginData | None:

    result = (
        session.query(*_login_infos)
        .join(Users, Profiles.userid == Users.id)
        .join(SubProfiles, Profiles.id == SubProfiles.profileid)
        .where(SubProfiles.authtoken == auth_token)
        .first()
    )

    if result is None:
        return None

    login = LoginData(
        user_id=result[0],
        profile_id=result[1],
        subprofile_id=result[2],
        nick=result[3],
        email=result[4],
        unique_nick=result[5],
        password_hash=result[6],
        email_verified_flag=result[7],
        banned_flag=result[8],
        namespace_id=result[9],
    )
    return login


def get_block_list(profile_id: int, namespace_id: int, session: Session) -> list[int]:
    result = (
        session.query(Blacklist.targetid)
        .where(
            Blacklist.namespaceid == namespace_id,
            Blacklist.profileid == profile_id,
        )
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[int], result)
    return result


# region Buddy


def get_buddy_list(profile_id: int, namespace_id: int, session: Session) -> list[int]:
    pass
    result = (
        session.query(Friendlist.targetid)
        .where(
            Friendlist.profileid == profile_id, Friendlist.namespaceid == namespace_id
        )
        .all()
    )

    data = [r[0] for r in result]
    return data


def add_block(subprofile_id: int, target_id: int, session: Session) -> None:
    subprofile = get_subprofile_by_id(subprofile_id, session)
    result = (
        session.query(Blacklist)
        .join(SubProfiles, SubProfiles.namespaceid == Blacklist.namespaceid)
        .where(
            Blacklist.targetid == target_id,
            Blacklist.profileid == subprofile.profileid,
            SubProfiles.namespaceid == subprofile.namespaceid,
        )
        .first()
    )
    if result is None:
        b = Blacklist(
            targetid=target_id,
            namespaceid=subprofile.namespaceid,
            profileid=subprofile.profileid,
        )
        session.add(b)
        session.commit()


def remove_block(subprofile_id: int, target_profile_id: int, session: Session):
    subprofile = get_subprofile_by_id(subprofile_id, session)
    result = (
        session.query(Blacklist)
        .join(SubProfiles, SubProfiles.namespaceid == Blacklist.namespaceid)
        .where(
            Blacklist.targetid == target_profile_id,
            Blacklist.profileid == subprofile.profileid,
            SubProfiles.namespaceid == subprofile.namespaceid,
        )
        .first()
    )
    if result is None:
        return
    session.delete(result)
    session.commit()


def add_buddy(
    sender_subprofile_id: int,
    receiver_profile_id: int,
    reason: str,
    session: Session,
) -> None:
    sender_subprofile = get_subprofile_by_id(sender_subprofile_id, session)
    is_buddy_exist = (
        session.query(Friendlist)
        .where(
            FriendRequest.sender_profileid == sender_subprofile.profileid,
            FriendRequest.receiver_profileid == receiver_profile_id,
            FriendRequest.namespaceid == sender_subprofile.namespaceid,
        )
        .count()
    )

    if is_buddy_exist != 0:
        raise GPAddBuddyException("buddy is already added, ignore the request")

    data = (
        session.query(FriendRequest)
        .join(SubProfiles, SubProfiles.namespaceid == FriendRequest.namespaceid)
        .where(
            FriendRequest.sender_profileid == sender_subprofile.profileid,
            FriendRequest.receiver_profileid == receiver_profile_id,
            SubProfiles.namespaceid == sender_subprofile.namespaceid,
        )
        .first()
    )
    if data is not None:
        data.update_time = datetime.now()
        return

    request = FriendRequest(
        sender_profileid=sender_subprofile.profileid,
        receiver_profileid=receiver_profile_id,
        namespaceid=sender_subprofile.namespaceid,
        reason=reason,
        update_time=datetime.now(),
    )
    session.add(request)
    session.commit()


def get_friend_add_info(
    profile_id: int, namespace_id: int, session: Session
) -> list[BuddyMessageFriendAddResult.BuddyMessageFriendAddData]:
    result = (
        session.query(FriendRequest)
        .join(SubProfiles, SubProfiles.namespaceid == FriendRequest.namespaceid)
        .where(
            FriendRequest.sender_profileid == profile_id,
            SubProfiles.namespaceid == namespace_id,
        )
        .all()
    )
    data = []
    for r in result:
        d = BuddyMessageFriendAddResult.BuddyMessageFriendAddData(
            from_profile_id=r.sender_profileid,
            date=r.update_time,
            message=r.reason,
            signature="fake_signature",
        )
        data.append(d)
    return data


def set_friendship_bidirectional(
    sender_subprofile_id: int,
    receiver_profile_id: int,
    session: Session,
):
    sender_subprofile = get_subprofile_by_id(sender_subprofile_id, session)

    result = (
        session.query(FriendRequest)
        .where(
            FriendRequest.sender_profileid == sender_subprofile.profileid,
            FriendRequest.receiver_profileid == receiver_profile_id,
            FriendRequest.namespaceid == sender_subprofile.namespaceid,
        )
        .first()
    )
    if result is None:
        raise GPAddBuddyException("no friend add request found")

    from_to = (
        session.query(Friendlist)
        .where(
            Friendlist.profileid == sender_subprofile.profileid,
            Friendlist.targetid == receiver_profile_id,
            Friendlist.namespaceid == sender_subprofile.namespaceid,
        )
        .count()
    )
    to_from = (
        session.query(Friendlist)
        .where(
            Friendlist.profileid == receiver_profile_id,
            Friendlist.targetid == sender_subprofile.profileid,
            Friendlist.namespaceid == sender_subprofile.namespaceid,
        )
        .count()
    )

    if from_to == 0:
        from_to_relation = Friendlist(
            profileid=sender_subprofile.profileid,
            targetid=receiver_profile_id,
            namespaceid=sender_subprofile.namespaceid,
        )
        session.add(from_to_relation)

    if to_from == 0:
        to_from_relation = Friendlist(
            profileid=receiver_profile_id,
            targetid=sender_subprofile.profileid,
            namespaceid=sender_subprofile.namespaceid,
        )
        session.add(to_from_relation)

    session.delete(result)
    session.commit()


def update_friend_info(
    target_id: int, profile_id: int, namespace_id: int, session: Session
):
    result = (
        session.query(Friendlist)
        .where(
            Friendlist.targetid == target_id,
            Friendlist.namespaceid == namespace_id,
            Friendlist.profileid == profile_id,
        )
        .count()
    )
    f = Friendlist(targetid=target_id, namespaceid=namespace_id, profileid=profile_id)

    if result == 0:
        session.add(f)
        session.commit()


def add_nick_name(profile_id: int, old_nick: str, new_nick: str, session: Session):
    assert isinstance(profile_id, int)
    assert isinstance(old_nick, str)
    assert isinstance(new_nick, str)

    result = (
        session.query(Profiles)
        .where(Profiles.id == profile_id, Profiles.nick == old_nick)
        .first()
    )

    if result is None:
        raise GPDatabaseException("No user infomation found in database.")

    result.nick = new_nick
    session.commit()


# def update_profile_info(profile: Profiles):
#     session.add(profile)
#     session.commit()


def update_unique_nick(subprofile_id: int, unique_nick: str, session: Session):
    result = (
        session.query(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if result is None:
        raise GPDatabaseException("user not existed")
    result.uniquenick = unique_nick
    session.commit()


def update_subprofile_info(subprofile: SubProfiles, session: Session):
    session.add(subprofile)
    session.commit()


def get_status(subprofile_id: int, session: Session) -> dict:
    result = (
        session.query(Profiles)
        .join(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if result is None:
        raise GPStatusException("No profile found with the provided subprofile id")

    if TYPE_CHECKING:
        assert isinstance(result.statstring, str)
        assert isinstance(result.status, GPStatusCode)
    if "location" not in result.extra_info:
        result.extra_info["location"] = ""
    data = {
        "status_string": result.statstring,
        "location_string": result.extra_info["locstring"],
        "current_status": result.status,
    }
    return data


def update_status(
    subprofile_id: int,
    current_status: GPStatusCode,
    location_string: str,
    status_string: str,
    session: Session,
):

    result = (
        session.query(Profiles)
        .join(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if result is None:
        raise GPStatusException("No profile found with the provided subprofile id")

    result.statstring = status_string
    result.status = current_status
    assert isinstance(result.extra_info, dict)
    result.extra_info["locstring"] = location_string

    session.commit()


def update_new_nick(subprofile_id: int, old_nick: str, new_nick: str, session: Session):
    result = (
        session.query(Profiles)
        .join(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if result is None:
        raise GPUpdateProfileException()

    if result.nick == old_nick and result.nick != new_nick:
        result.nick = new_nick
    session.commit()


def update_cdkey(subprofile_id: int, cdkey: str, session: Session):
    subprofile =get_subprofile_by_id(subprofile_id,session)
    if subprofile is None:
        raise GPDatabaseException(f"no subprofile found with subprofile id")

    subprofile.cdkeyenc = cdkey

    session.commit()


def update_uniquenick(subprofile_id: int, uniquenick: str, session: Session):
    subprofile = get_subprofile_by_id(subprofile_id, session)
    subprofile.uniquenick = uniquenick
    session.commit()


def update_profiles(subprofile_id: int, extra_info: ExtraInfo, session: Session):
    profile = (
        session.query(Profiles)
        .join(SubProfiles)
        .where(SubProfiles.id == subprofile_id)
        .first()
    )
    if profile is None:
        raise GPDatabaseException(
            f"no profile found with subprofile id:{subprofile_id}"
        )
    for key, value in extra_info.model_dump(mode="json").items():
        profile.extra_info[key] = value

    session.commit()


def update_user(session_key):
    raise NotImplementedError()


# if __name__ == "__main__":
#     result = get_block_list(1, 1)
