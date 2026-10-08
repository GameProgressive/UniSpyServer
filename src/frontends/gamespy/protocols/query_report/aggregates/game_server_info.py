from datetime import datetime
from uuid import UUID

from pydantic import BaseModel

from frontends.gamespy.library.extentions.bytes_extentions import ip_to_4_bytes
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus

NESSESARY_KEYS: list[str] = ["gamename", "hostname", "hostport"]


class GameServerInfo(BaseModel):
    server_id: UUID
    host_ip_address: str
    instant_key: str
    game_name: str
    query_report_port: int

    update_time: datetime
    status: GameServerStatus
    data:dict[str,str]

    @property
    def query_report_port_bytes(self) -> bytes:
        return self.query_report_port.to_bytes(2)

    @property
    def host_ip_address_bytes(self) -> bytes:
        return ip_to_4_bytes(self.host_ip_address)
