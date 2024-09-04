from typing import Optional
from library.src.database.pg_orm import PG_SESSION, PStorage, Profiles, SubProfiles
from servers.game_status.src.enums.general import PersistStorageType
from servers.game_status.src.exceptions.general import GSException


def create_new_game_data():
    raise NotImplementedError()


def create_new_player_data():
    raise NotImplementedError()


def update_player_data():
    raise NotImplementedError()


def get_profile_id(token: str) -> int:
    result = PG_SESSION.query(SubProfiles.profileid).filter(
        SubProfiles.authtoken == token).one()
    if result is None:
        raise GSException("No records found in database")
    return result


def get_profile_id(cdkey: str, nick_name: str) -> Optional[int]:
    result = PG_SESSION.query(SubProfiles.profileid).join(
        SubProfiles, Profiles.profileid == SubProfiles.profileid).filter(SubProfiles.cdkeyenc == cdkey, Profiles.nick == nick_name).one()
    if result is None:
        raise GSException("No record found in database")
    return result


def get_player_data(profile_id: int, storage_type: PersistStorageType, data_index: int) -> dict[str, str]:
    result = PG_SESSION.query(PStorage.data).filter(PStorage.ptype == storage_type.value,
                                                    PStorage.dindex == data_index,
                                                    PStorage.profileid == profile_id)
    if result is None:
        raise GSException("No records found in database")
    return result
