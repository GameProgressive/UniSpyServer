from typing import TYPE_CHECKING, cast

from sqlalchemy import Column
from backends.library.database.pg_orm import PG_SESSION, PStorage, Profiles, SubProfiles, Users
from servers.game_status.src.aggregations.enums import PersistStorageType
from servers.game_status.src.aggregations.exceptions import GSException


def create_new_game_data():
    raise NotImplementedError()


def create_new_player_data():
    raise NotImplementedError()


def update_player_data():
    raise NotImplementedError()


def get_profile_id_by_token(token: str) -> int:
    if TYPE_CHECKING:
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.authtoken, Column)

    result = PG_SESSION.query(SubProfiles.profileid).filter(
        SubProfiles.authtoken == token).first()
    if result is None:
        raise GSException("No records found in database")
    if TYPE_CHECKING:
        result = cast(int, result)
    return result


def get_profile_id(cdkey: str, nick_name: str) -> int:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.cdkeyenc, Column)
    result = PG_SESSION.query(SubProfiles.profileid).join(
        SubProfiles, Profiles.profileid == SubProfiles.profileid)\
        .filter(SubProfiles.cdkeyenc == cdkey,
                Profiles.nick == nick_name)\
        .first()
    if result is None:
        raise GSException("No record found in database")
    if TYPE_CHECKING:
        result = cast(int, result)
    return result


def get_player_data(profile_id: int, storage_type: PersistStorageType, data_index: int) -> dict:
    result = PG_SESSION.query(PStorage.data).filter(PStorage.ptype == storage_type.value,
                                                    PStorage.dindex == data_index,
                                                    PStorage.profileid == profile_id).first()
    if result is None:
        raise GSException("No records found in database")
    if TYPE_CHECKING:
        result = cast(dict, result)
    return result
