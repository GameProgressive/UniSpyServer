from datetime import datetime, timedelta

from backends.library.database.pg_orm import (
    InitPacketCaches,
    NatResultCaches,
    RelayServerCaches,
)
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortType,
)

from sqlalchemy.orm import Session


def add_init_packet(info: InitPacketCaches, session: Session) -> None:
    assert isinstance(info, InitPacketCaches)

    session.add(info)
    session.commit()


def clean_expired_init_cache(session: Session) -> None:
    session.query(InitPacketCaches).where(
        InitPacketCaches.update_time < datetime.now() - timedelta(minutes=5)
    ).delete()
    session.commit()


def count_init_info(cookie: int, version: int, session: Session) -> int:
    time = datetime.now() - timedelta(seconds=30)

    result = (
        session.query(InitPacketCaches)
        .where(
            InitPacketCaches.cookie == cookie,
            InitPacketCaches.version == version,
            InitPacketCaches.update_time <= time,
        )
        .count()
    )
    return result


def get_init_cache(
    cookie: int, client_index: NatClientIndex, port_type: NatPortType, session: Session
) -> InitPacketCaches | None:
    result = (
        session.query(InitPacketCaches)
        .where(
            InitPacketCaches.cookie == cookie,
            InitPacketCaches.client_index == client_index,
            InitPacketCaches.port_type == port_type,
        )
        .first()
    )
    return result


def get_init_caches(
    cookie: int, client_index: NatClientIndex, session: Session
) -> list[InitPacketCaches]:
    # query the latest init info with in 30 seconds
    time = datetime.now() - timedelta(seconds=30)

    result = (
        session.query(InitPacketCaches)
        .where(
            InitPacketCaches.cookie == cookie,
            InitPacketCaches.client_index == client_index,
            InitPacketCaches.update_time >= time,
        )
        .all()
    )
    return result


def update_init_info(info: InitPacketCaches, session: Session) -> None:
    assert isinstance(info, InitPacketCaches)

    session.commit()


def remove_init_info(info: InitPacketCaches, session: Session) -> None:
    assert isinstance(info, InitPacketCaches)

    session.delete(info)
    session.commit()


def store_nat_result_info(info: NatResultCaches, session: Session) -> None:
    assert isinstance(info, NatResultCaches)

    session.add(info)
    session.commit()


def update_nat_result_info(info: NatResultCaches, session: Session) -> None:
    assert isinstance(info, NatResultCaches)

    result = get_nat_result_info(info, session)
    if len(result) != 0:
        session.delete(result)
    store_nat_result_info(info, session)


def remove_nat_result_info(info: NatResultCaches, session: Session) -> None:
    assert isinstance(info, NatResultCaches)

    session.delete(info)
    session.commit()


def get_nat_result_info(info: NatResultCaches, session: Session) -> list:
    assert isinstance(info.cookie, int)
    assert isinstance(info.public_ip, str)
    result = get_nat_result_info_by_cookie_ip(
        info.cookie, info.public_ip, session
    )
    return result


def get_nat_result_info_by_ip(
        public_ip: str, session: Session
) -> list[NatResultCaches]:
    result = (
        session.query(NatResultCaches)
        .where(
            NatResultCaches.public_ip == public_ip,
        )
        .all()
    )
    return result


def get_nat_result_info_by_cookie_ip(
    cookie: int, public_ip: str, session: Session
) -> list[NatResultCaches]:
    result = (
        session.query(NatResultCaches)
        .where(
            NatResultCaches.cookie == cookie,
            NatResultCaches.public_ip == public_ip,
        )
        .all()
    )
    return result


def get_game_traffic_relay_servers(
    session: Session, number: int | None = None
) -> list[RelayServerCaches]:
    if number is None:
        result = (
            session.query(RelayServerCaches)
            .order_by(RelayServerCaches.client_count.desc())
            .all()
        )
    else:
        assert isinstance(number, int)
        result = (
            session.query(RelayServerCaches)
            .order_by(RelayServerCaches.client_count.desc())
            .limit(number)
            .all()
        )
    return result
