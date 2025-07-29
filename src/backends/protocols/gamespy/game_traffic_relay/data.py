from datetime import datetime

from uuid import UUID
from backends.library.database.pg_orm import RelayServerCaches
from sqlalchemy.orm import Session


def search_relay_server(
    server_id: UUID, server_ip: str, session: Session
) -> RelayServerCaches | None:
    result = (
        session.query(RelayServerCaches)
        .where(
            RelayServerCaches.server_id == server_id,
            RelayServerCaches.public_ip_address == server_ip,
        )
        .first()
    )
    return result


def get_available_relay_serves(session: Session) -> list[RelayServerCaches]:
    """
    Return
    ------
        list of ip:port
    """

    result: list[RelayServerCaches] = session.query(RelayServerCaches).all()
    return result


def update_relay_server(info: RelayServerCaches, session: Session):
    info.update_time = datetime.now()  # type: ignore

    session.commit()


def add_relay_server(info: RelayServerCaches, session: Session):
    session.add(info)
    session.commit()


def delete_relay_server(server_id: UUID, ip_address: str, port: int, session: Session):
    assert isinstance(server_id, UUID)
    assert isinstance(ip_address, str)
    assert isinstance(port, int)

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
