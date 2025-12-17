# region altas

# region auth

from typing import TYPE_CHECKING, cast
from backends.library.database.pg_orm import (
    Profiles,
    SubProfiles,
    Users,
    SakeStorage,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import (
    AuthException,
)
from frontends.gamespy.protocols.web_services.modules.sake.exceptions.general import (
    SakeException,
)
from sqlalchemy.orm import Session


def is_user_exist(
    uniquenick: str,
    cdkey: str,
    partner_id: int,
    namespace_id: int,
    email: str,
    password: str,
    session: Session,
) -> bool:
    result = (
        session.query(Profiles)
        .join(Users)
        .join(SubProfiles)
        .where(
            SubProfiles.uniquenick == uniquenick,
            SubProfiles.cdkeyenc == cdkey,
            SubProfiles.partnerid == partner_id,
            SubProfiles.namespaceid == namespace_id,
            Users.email == email,
            Users.password == password,
        )
        .first()
    )

    if result is None:
        return False
    else:
        return True

    if result is None:
        raise AuthException(
            "No account exists with the provided email address.")


def get_info_by_cdkey_email(
    uniquenick: str, namespace_id: int, cdkey: str, email: str, session: Session
) -> tuple[int, int, str, str, str] | None:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """
    assert isinstance(uniquenick, str)
    assert isinstance(namespace_id, int)
    assert isinstance(cdkey, str)
    assert isinstance(email, str)

    result = (
        session.query(Users, Profiles, SubProfiles)
        .join(Users)
        .join(
            Profiles,
        )
        .join(SubProfiles)
        .where(
            SubProfiles.uniquenick == uniquenick,
            SubProfiles.namespaceid == namespace_id,
            SubProfiles.cdkeyenc == cdkey,
            Users.email == email,
        )
        .first()
    )

    if result is None:
        return None

    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[2]
    assert isinstance(user.userid, int)
    assert isinstance(profile.profileid, int)
    assert isinstance(profile.nick, str)
    assert isinstance(subprofile.uniquenick, str)
    assert isinstance(subprofile.cdkeyenc, str)

    return (
        user.userid,
        profile.profileid,
        profile.nick,
        subprofile.uniquenick,
        subprofile.cdkeyenc,
    )


def get_info_by_authtoken(
    auth_token: str, session: Session
) -> tuple[int, int, str, str, str] | None:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """

    result = (
        session.query(Users.userid,
                      Profiles.profileid,
                      Profiles.nick,
                      SubProfiles.uniquenick,
                      SubProfiles.cdkeyenc)
        .join(Profiles, Profiles.userid == Users.userid)
        .join(SubProfiles, SubProfiles.profileid == Profiles.profileid)
        .where(SubProfiles.authtoken == auth_token)
        .first()
    )
    if result is None:
        return None

    userid, profileid, nick, uniquenick, cdkeyenc = result
    assert isinstance(userid, int)
    assert isinstance(profileid, int)
    assert isinstance(nick, str)
    assert isinstance(uniquenick, str)
    assert isinstance(cdkeyenc, str)
    return (
        userid, profileid, nick, uniquenick, cdkeyenc
    )


def get_info_by_uniquenick(
    uniquenick: str, namespace_id: int, session: Session
) -> tuple[int, int, str, str, str] | None:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """

    result = (
        session.query(Users, Profiles, SubProfiles)
        .join(Users, Users.userid == Profiles.userid)
        .join(SubProfiles, SubProfiles.profileid == Profiles.profileid)
        .where(
            SubProfiles.uniquenick == uniquenick,
            SubProfiles.namespaceid == namespace_id,
        )
        .first()
    )

    if result is None:
        return None
    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[2]
    assert isinstance(user.userid, int)
    assert isinstance(profile.profileid, int)
    assert isinstance(profile.nick, str)
    assert isinstance(subprofile.uniquenick, str)
    assert isinstance(subprofile.cdkeyenc, str)

    return (
        user.userid,
        profile.profileid,
        profile.nick,
        subprofile.uniquenick,
        subprofile.cdkeyenc,
    )


# region d2g

# region ingamead

# region patching and tracking

# region racing

# region sake


def get_user_data(table_id: int, session: Session) -> dict:
    result = (
        session.query(SakeStorage.data).where(
            SakeStorage.tableid == table_id).first()
    )

    if TYPE_CHECKING:
        result = cast(dict, result)
    return result


def update_user_data(table_id: int, data: dict, session: Session) -> None:
    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).first()
    if result is None:
        raise SakeException("user data not found")
    assert isinstance(result.data, dict)
    for key, value in result.data.items():
        if key in data:
            if data[key] is None or data[key] == "":
                raise SakeException(f"the value of {key} should not be None.")
            if value == data[key]:
                continue
            result.data[key] = data[key]


def create_records(table_id: int, data: dict, session: Session) -> None:
    assert isinstance(table_id, int)
    assert isinstance(data, dict)

    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).count()

    if result != 0:
        raise SakeException("Records already existed")

    sake = SakeStorage(table_id=table_id, data=data)

    session.add(sake)
    session.commit()
