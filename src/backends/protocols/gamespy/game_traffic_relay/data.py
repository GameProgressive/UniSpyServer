from datetime import datetime
from typing import Optional
from uuid import UUID
from backends.library.database.pg_orm import PG_SESSION, RelayServerCaches


def search_relay_server(server_id: UUID, server_ip: str) -> Optional[RelayServerCaches]:
    result = (
        PG_SESSION.query(RelayServerCaches)
        .where(
            RelayServerCaches.server_id == server_id,
            RelayServerCaches.public_ip_address == server_ip,
        )
        .first()
    )
    return result


def get_available_relay_serves() -> list[RelayServerCaches]:
    """
    Return
    ------
        list of ip:port
    """
    result: list[RelayServerCaches] = PG_SESSION.query(RelayServerCaches).all()
    return result


def update_relay_server(info: RelayServerCaches):
    info.update_time = datetime.now()  # type: ignore
    PG_SESSION.commit()


def add_relay_server(info: RelayServerCaches):
    PG_SESSION.add(info)
    PG_SESSION.commit()


def delete_relay_server(server_id: UUID, ip_address: str, port: int):
    assert isinstance(server_id, UUID)
    assert isinstance(ip_address, str)
    assert isinstance(port, int)
    info = (
        PG_SESSION.query(RelayServerCaches)
        .where(
            RelayServerCaches.server_id == server_id,
            RelayServerCaches.public_ip_address == ip_address,
            RelayServerCaches.public_port == port,
        )
        .first()
    )
    PG_SESSION.delete(info)
    PG_SESSION.commit()
