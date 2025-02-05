from pydantic import BaseModel

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import GPStatusCode


class UserStatus(BaseModel):
    status_string: str
    location_string: str
    current_status: GPStatusCode = GPStatusCode.OFFLINE


class UserStatusInfo(BaseModel):
    status_state: str
    buddy_ip: str
    host_ip: str
    host_private_ip: str
    query_report_port: int
    host_port: int
    session_flags: str
    rich_status: str
    game_type: str
    game_variant: str
    game_map_name: str
    quiet_mode_flags: str
