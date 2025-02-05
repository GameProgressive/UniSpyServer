# region altas

# region auth

from typing import TYPE_CHECKING, cast, overload
from backends.library.database.pg_orm import PG_SESSION, Profiles, SubProfiles, Users, SakeStorage
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.modules.auth.exceptions.general import AuthException
from frontends.gamespy.protocols.web_services.modules.sake.exceptions.general import SakeException


def is_user_exist(uniquenick: str, cdkey: str, partner_id: int, namespace_id: int, email: str, password: str) -> None:
    result = PG_SESSION.query(Profiles)\
        .join(Users)\
        .join(SubProfiles)\
        .where(SubProfiles.uniquenick == uniquenick,
               SubProfiles.cdkeyenc == cdkey,
               SubProfiles.partnerid == partner_id,
               SubProfiles.namespaceid == namespace_id,
               Users.email == email,
               Users.password == password).first()
    if result is None:
        raise AuthException(
            "No account exists with the provided email address.")


def get_info_by_cdkey_email(uniquenick: str, namespace_id: int, cdkey: str, email: str) -> tuple[int, int, str, str, str]:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """
    assert isinstance(uniquenick, str)
    assert isinstance(namespace_id, int)
    assert isinstance(cdkey, str)
    assert isinstance(email, str)

    result = PG_SESSION.query(Users, Profiles, SubProfiles)\
        .join(Users)\
        .join(Profiles,)\
        .join(SubProfiles)\
        .where(SubProfiles.uniquenick == uniquenick,
               SubProfiles.namespaceid == namespace_id,
               SubProfiles.cdkeyenc == cdkey,
               Users.email == email).first()

    if result is None:
        raise AuthException(
            "No account exists with the provided uniquenick and namespace id.")
    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[2]
    assert isinstance(user.userid, int)
    assert isinstance(profile.profileid, int)
    assert isinstance(profile.nick, str)
    assert isinstance(subprofile.uniquenick, str)
    assert isinstance(subprofile.cdkeyenc, str)

    return user.userid, profile.profileid, profile.nick, subprofile.uniquenick, subprofile.cdkeyenc


def get_info_by_authtoken(auth_token: str) -> tuple[int, int, str, str, str]:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """
    result = PG_SESSION.query(Users, Profiles, SubProfiles)\
        .join(Users, Users.userid == Profiles.userid).join(
        Profiles, Profiles.profileid == SubProfiles.profileid)\
        .where(SubProfiles.authtoken == auth_token).first()
    if result is None:
        raise AuthException(
            "No account exists with the provided authtoken.")
    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[2]
    assert isinstance(user.userid, int)
    assert isinstance(profile.profileid, int)
    assert isinstance(profile.nick, str)
    assert isinstance(subprofile.uniquenick, str)
    assert isinstance(subprofile.cdkeyenc, str)
    return user.userid, profile.profileid, profile.nick, subprofile.uniquenick, subprofile.cdkeyenc


def get_info_by_uniquenick(uniquenick: str, namespace_id: int) -> tuple[int, int, str, str, str]:
    """
    return [user_id,profile_id,profile_nick,unique_nick,cdkey_hash]
    """
    result = PG_SESSION.query(Users, Profiles, SubProfiles)\
        .join(Users, Users.userid == Profiles.userid)\
        .join(Profiles, Profiles.profileid == SubProfiles.profileid)\
        .where(SubProfiles.uniquenick == uniquenick, SubProfiles.namespaceid == namespace_id).first()

    if result is None:
        raise AuthException(
            "No account exists with the provided uniquenick and namespace id.")
    user: Users = result[0]
    profile: Profiles = result[1]
    subprofile: SubProfiles = result[2]
    assert isinstance(user.userid, int)
    assert isinstance(profile.profileid, int)
    assert isinstance(profile.nick, str)
    assert isinstance(subprofile.uniquenick, str)
    assert isinstance(subprofile.cdkeyenc, str)

    return user.userid, profile.profileid, profile.nick, subprofile.uniquenick, subprofile.cdkeyenc

# region d2g

# region ingamead

# region patching and tracking

# region racing

# region sake


def get_user_data(table_id: int,) -> dict:
    result = PG_SESSION.query(SakeStorage.data).where(
        SakeStorage.tableid == table_id).first()

    if TYPE_CHECKING:
        result = cast(dict, result)
    return result


def update_user_data(table_id: int, data: dict) -> None:
    result = PG_SESSION.query(SakeStorage).where(
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


def create_records(table_id: int, data: dict) -> None:
    assert isinstance(table_id, int)
    assert isinstance(data, dict)
    result = PG_SESSION.query(SakeStorage).where(
        SakeStorage.tableid == table_id).count()

    if result != 0:
        raise SakeException("Records already existed")

    sake = SakeStorage(table_id=table_id, data=data)

    PG_SESSION.add(sake)
    PG_SESSION.commit()


if __name__ == "__main__":
    result = PG_SESSION.query(Users, Profiles, SubProfiles)\
        .join(Profiles, Users.userid == Profiles.userid)\
        .join(SubProfiles, Profiles.profileid == SubProfiles.profileid)\
        .where(Users.userid == 1).first()
