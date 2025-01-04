from datetime import datetime, timedelta
from uuid import UUID
from backends.library.database.pg_orm import PG_SESSION, InitPacketCaches, NatFailCaches
from servers.natneg.src.aggregations.enums import NatClientIndex


def store_init_packet(info: InitPacketCaches) -> None:
    assert isinstance(info, InitPacketCaches)
    PG_SESSION.add(info)
    PG_SESSION.commit()


def count_init_info(cookie: int, version: int) -> int:
    time = datetime.now()-timedelta(seconds=30)
    result = PG_SESSION.query(InitPacketCaches).where(
        InitPacketCaches.cookie == cookie,
        InitPacketCaches.version == version,
        InitPacketCaches.update_time >= time).count()
    return result


def get_init_infos(server_id: UUID, cookie: int, client_index: NatClientIndex) -> list[InitPacketCaches]:
    # query the latest init info with in 30 seconds
    time = datetime.now()-timedelta(seconds=30)
    result = PG_SESSION.query(InitPacketCaches).where(
        InitPacketCaches.server_id == server_id,
        InitPacketCaches.cookie == cookie,
        InitPacketCaches.client_index == client_index,
        InitPacketCaches.update_time >= time).all()
    return result


def update_init_info(info: InitPacketCaches) -> None:
    assert isinstance(info, InitPacketCaches)
    remove_init_info(info)
    store_init_packet(info)


def remove_init_info(info: InitPacketCaches) -> None:
    assert isinstance(info, InitPacketCaches)
    PG_SESSION.delete(info)
    PG_SESSION.commit()


def store_nat_fail_info(info: NatFailCaches) -> None:
    assert isinstance(info, NatFailCaches)
    PG_SESSION.add(info)
    PG_SESSION.commit()


def update_nat_fail_info(info: NatFailCaches) -> None:
    assert isinstance(info, NatFailCaches)
    result = get_nat_fail_info(info)
    if result is not None:
        PG_SESSION.delete(result)
    store_nat_fail_info(info)


def remove_nat_fail_info(info: NatFailCaches) -> None:
    assert isinstance(info, NatFailCaches)
    PG_SESSION.delete(info)
    PG_SESSION.commit()


def get_nat_fail_info(info: NatFailCaches):
    result = get_nat_fail_info_by_ip(
        str(info.public_ip_address1), str(info.public_ip_address2))
    return result


def get_nat_fail_info_by_ip(public_ip1: str, public_ip2: str) -> list[NatFailCaches]:
    result = PG_SESSION.query(NatFailCaches).where(NatFailCaches.public_ip_address1 == public_ip1,
                                                    NatFailCaches.public_ip_address2 == public_ip2).all()
    return result
