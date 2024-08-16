from datetime import datetime
import socket
from uuid import UUID

from library.src.extentions.bytes_extentions import ip_to_4_bytes
from servers.query_report.src.v2.enums.general import GameServerStatus


class GameServerInfo:
    server_id: UUID
    host_ip_address: str
    instant_key: int
    game_name: str
    query_report_port: int

    last_heart_beart_received_time: datetime
    status: GameServerStatus
    server_data: dict[str, str]
    player_data: list[dict[str, str]]
    team_data: list[dict[str, str]]

    @property
    def query_report_port_bytes(self) -> bytes:
        return self.query_report_port.to_bytes(2, "big")

    @property
    def host_ip_address_bytes(self) -> bytes:
        return ip_to_4_bytes(self.host_ip_address)
