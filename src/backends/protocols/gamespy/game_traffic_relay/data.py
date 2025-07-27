from datetime import datetime

from uuid import UUID
from backends.library.database.pg_orm import ENGINE, RelayServerCaches
from sqlalchemy.orm import Session

def search_relay_server(server_id: UUID, server_ip: str) -> RelayServerCaches | None:
    with Session(ENGINE) as session:
        result = (
            session.query(RelayServerCaches)
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
    with Session(ENGINE) as session:
        result: list[RelayServerCaches] = session.query(RelayServerCaches).all()
    return result


def update_relay_server(info: RelayServerCaches):
    info.update_time = datetime.now()  # type: ignore
    with Session(ENGINE) as session:
        session.commit()


def add_relay_server(info: RelayServerCaches):
    with Session(ENGINE) as session:
        session.add(info)
        session.commit()


def delete_relay_server(server_id: UUID, ip_address: str, port: int):
    assert isinstance(server_id, UUID)
    assert isinstance(ip_address, str)
    assert isinstance(port, int)
    with Session(ENGINE) as session:
        info = (
            session.query(RelayServerCaches)
            .where(
                RelayServerCaches.server_id == server_id,
                RelayServerCaches.public_ip_address == ip_address,
                RelayServerCaches.public_port == port,
            )
            .first()
        )
        session.delete(info)
        session.commit()
