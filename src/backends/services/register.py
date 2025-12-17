

from datetime import datetime, timedelta
from backends.library.database.pg_orm import ENGINE, FrontendInfo
from frontends.gamespy.library.configs import ServerConfig
from sqlalchemy.orm import Session

from frontends.gamespy.library.exceptions.general import UniSpyException


def register_services(config: ServerConfig, external_ip: str) -> dict:
    expire_time = datetime.now() - timedelta(minutes=5)
    with Session(ENGINE) as session:
        result = session.query(
            FrontendInfo.server_id == config.server_id,
            FrontendInfo.update_time >= expire_time).first()
        if result is not None:
            info = FrontendInfo(
                server_id=config.server_id,
                server_name=config.server_name,
                external_ip="",
                listening_ip=config.listening_address,
                listening_port=config.listening_port,
                update_time=datetime.now()
            )

            session.add(info)
            session.commit()
            return {"status": "online"}
        else:
            raise UniSpyException("server is already registered")
