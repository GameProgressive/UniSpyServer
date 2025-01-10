from datetime import datetime
from uuid import UUID

from pydantic import BaseModel

from library.src.extentions.bytes_extentions import ip_to_4_bytes
from servers.query_report.src.v2.aggregates.enums import GameServerStatus


class GameServerInfo(BaseModel):
    server_id: UUID
    host_ip_address: str
    instant_key: str
    game_name: str
    query_report_port: int

    last_heart_beat_received_time: datetime
    status: GameServerStatus
    server_data: dict[str, str]
    player_data: list[dict[str, str]]
    team_data: list[dict[str, str]]

    @property
    def query_report_port_bytes(self) -> bytes:
        return self.query_report_port.to_bytes(2)

    @property
    def host_ip_address_bytes(self) -> bytes:
        return ip_to_4_bytes(self.host_ip_address)
