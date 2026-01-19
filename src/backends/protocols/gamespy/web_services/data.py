# region altas

# region auth

from typing import TYPE_CHECKING, cast

from sqlalchemy import ColumnExpressionArgument, Integer, and_
from backends.library.database.pg_orm import (
    Profiles,
    SubProfiles,
    Users,
    SakeStorage,
)
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.exceptions import (
    AuthException,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import CommandName
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import (
    SakeException,
)
from sqlalchemy.orm import Session

from frontends.gamespy.protocols.web_services.modules.sake.aggregates.utils import RecordConverter


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
) -> tuple[int, int, str, str, str | None] | None:
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
    assert isinstance(subprofile.cdkeyenc, str) or subprofile.cdkeyenc is None
    return (
        user.userid,
        profile.profileid,
        profile.nick,
        subprofile.uniquenick,
        subprofile.cdkeyenc
    )


# region d2g

# region ingamead

# region patching and tracking

# region racing

# region sake
def _get_specific_condition(con: str):
    if ">=" in con:
        name,  value = con.split(" >= ")
        return SakeStorage.record[name]['value'].cast(
            Integer) >= int(value)
    if "<=" in con:
        name,  value = con.split(" <= ")
        return SakeStorage.record[name]['value'].cast(
            Integer) <= int(value)
    if ">" in con:
        name,  value = con.split(" > ")
        return SakeStorage.record[name]['value'].cast(Integer) > int(value)
    if "<" in con:
        name,  value = con.split(" < ")
        return SakeStorage.record[name]['value'].cast(Integer) < int(value)
    if "=" in con:
        name,  value = con.split(" = ")
        return SakeStorage.record[name]['value'] == value


def _filter_to_sql_con(filter: str) -> ColumnExpressionArgument[bool]:
    # todo build or queries
    # currently we only support AND condition
    sql_cons = []
    and_conditions = filter.split(" AND ")
    for and_c in and_conditions:
        qq = _get_specific_condition(and_c)
        sql_cons.append(qq)

    # or_queries = []
    # or_condition = filter.split(" OR ")
    # for or_c in or_condition:
    #     and_conditions = filter.split(" AND ")
    #     and_queries = []
    #     for and_c in and_conditions:
    #         qq = _get_specific_condition(and_c)
    #         and_queries.append(qq)

    #     or_queries.append(qq)

    if len(sql_cons) == 1:
        return sql_cons[0]
    return and_(*sql_cons)


def count_for_record(table_id: str, command_name: CommandName, session: Session) -> int:
    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).count()
    return result

# def _check_record_integrety(sake:SakeStorage)->bool:


def _get_filtered_record(sake: SakeStorage, fields: list, command_name: CommandName) -> dict:
    """
    get filterd record, return with gamespy format
    """
    assert isinstance(sake.record, dict)

    filtered_record = dict(sake.record)  # type: ignore
    filtered_key_value = {}
    for f in fields:
        if f not in filtered_record:
            raise SakeException(f"{f} not in record", command_name)
        filtered_key_value[f] = filtered_record[f]
    return filtered_key_value


def search_for_record(table_id: str, max_num: int, filter: str, fields: list[str], command_name: CommandName, session: Session) -> list[dict]:
    """
    max_num default to 100
    search and get the value that key in fields
    """
    queries = _filter_to_sql_con(filter)
    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id,
        queries).limit(max_num).all()
    records = []
    for item in result:
        record = _get_filtered_record(item, fields, command_name)
        records.append(record)
    return records


def get_my_records(table_id: str, fields: list[str], command_name: CommandName, session: Session) -> dict:
    """
    search and filtered the record by fields
    """
    result = (
        session.query(SakeStorage).where(
            SakeStorage.tableid == table_id).first()
    )
    if result is None:
        return {}

    record = _get_filtered_record(result, fields, command_name)
    return record


def create_records(table_id: str, records: dict, command_name: CommandName, session: Session) -> int:
    assert isinstance(table_id, str)
    assert isinstance(records, dict)

    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).count()

    if result != 0:
        raise SakeException("Records already existed", command_name)

    sake = SakeStorage(tableid=table_id,
                       record=records)
    session.add(sake)
    session.commit()
    assert isinstance(sake.id, int)
    return sake.id


def update_record(table_id: str, records: dict, command_name: CommandName, session: Session) -> int:
    """
    update record with new data and returns record id
    """
    assert isinstance(records, dict)
    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).first()

    if result is None:
        raise SakeException("Records do not existed", command_name)

    result.record = records  # type: ignore
    session.commit()
    record_id: int = cast(int, result.id)
    return record_id


def delete_record(table_id: str, command_name: CommandName, session: Session) -> None:
    result = session.query(SakeStorage).where(
        SakeStorage.tableid == table_id).first()

    if result is None:
        raise SakeException("Records not existed", command_name)

    session.delete(result)
    session.commit()


if __name__ == "__main__":
    filter = "collectionID=5 AND version=11 AND attackerID=0 AND attackRating &gt; 30 AND attackRating &lt; 120"
    con = _filter_to_sql_con(filter)
    pass
