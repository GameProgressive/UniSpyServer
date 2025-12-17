from datetime import datetime
from typing import TYPE_CHECKING, cast

from sqlalchemy import Column
from backends.library.database.pg_orm import ENGINE, PStorage, Profiles, SubProfiles, Users
from frontends.gamespy.protocols.game_status.aggregations.enums import PersistStorageType
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException
from sqlalchemy.orm import Session


def get_player_data(profile_id: int, storage_type: PersistStorageType, data_index: int, session: Session) -> PStorage | None:
    result = (session.query(PStorage)
              .where(PStorage.ptype == storage_type.value,
                     PStorage.dindex == data_index,
                     PStorage.profileid == profile_id).first())
    return result


def create_new_game_data():
    raise NotImplementedError()


def create_player_data(profile_id: int, ptype, dindex, data: str, session: Session) -> None:
    pd = PStorage(
        profileid=profile_id,
        ptype=ptype,
        dindex=dindex,
        data=data
    )
    session.add(pd)
    session.commit()


def update_player_data(pd: PStorage, data: str, session: Session) -> None:
    pd.data = data  # type: ignore
    pd.update_time = datetime.now()  # type: ignore
    session.commit()


def get_profile_id_by_token(token: str, session: Session) -> int:
    assert isinstance(token, str)
    result = session.query(SubProfiles.profileid).where(
        SubProfiles.authtoken == token).first()
    if result is None:
        raise GSException("No records found in database")
    assert isinstance(result, int)
    return result


def get_profile_id_by_profile_id(profile_id: int, session: Session) -> int:
    assert isinstance(profile_id, int)
    result = session.query(SubProfiles.profileid).where(
        SubProfiles.profileid == profile_id).count()
    if result != 1:
        raise GSException(f"There is no profile_id {profile_id} existed")
    assert isinstance(result, int)
    return result


def get_profile_id_by_cdkey(cdkey: str, nick_name: str, session: Session) -> int:
    if TYPE_CHECKING:
        assert isinstance(Profiles.profileid, Column)
        assert isinstance(Profiles.userid, Column)
        assert isinstance(Users.userid, Column)
        assert isinstance(Users.email, Column)
        assert isinstance(Profiles.nick, Column)
        assert isinstance(SubProfiles.profileid, Column)
        assert isinstance(SubProfiles.cdkeyenc, Column)

    result = (session.query(Profiles.profileid)
              .join(SubProfiles)
              .where(SubProfiles.cdkeyenc == cdkey,
                     Profiles.nick == nick_name)
              .first())
    if result is None:
        raise GSException("No record found in database")
    pid: int = result[0]
    return pid
