from datetime import datetime, timedelta

from backends.library.database.pg_orm import (
    InitPacketCaches,
    NatFailCaches,
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


def count_init_info(cookie: int, version: int, session: Session) -> int:
    time = datetime.now() - timedelta(seconds=30)

    result = (
        session.query(InitPacketCaches)
        .where(
            InitPacketCaches.cookie == cookie,
            InitPacketCaches.version == version,
            InitPacketCaches.update_time >= time,
        )
        .count()
    )
    return result


def get_init_info(
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


def get_init_infos(
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


def store_nat_fail_info(info: NatFailCaches, session: Session) -> None:
    assert isinstance(info, NatFailCaches)

    session.add(info)
    session.commit()


def update_nat_fail_info(info: NatFailCaches, session: Session) -> None:
    assert isinstance(info, NatFailCaches)

    result = get_nat_fail_info(info, session)
    if result is not None:
        session.delete(result)
    store_nat_fail_info(info, session)


def remove_nat_fail_info(info: NatFailCaches, session: Session) -> None:
    assert isinstance(info, NatFailCaches)

    session.delete(info)
    session.commit()


def get_nat_fail_info(info: NatFailCaches, session: Session):
    result = get_nat_fail_info_by_ip(
        str(info.public_ip_address1), str(info.public_ip_address2), session
    )
    return result


def get_nat_fail_info_by_ip(
    public_ip1: str, public_ip2: str, session: Session
) -> list[NatFailCaches]:
    result = (
        session.query(NatFailCaches)
        .where(
            NatFailCaches.public_ip_address1 == public_ip1,
            NatFailCaches.public_ip_address2 == public_ip2,
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
