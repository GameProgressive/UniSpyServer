
from dataclasses import dataclass
from servers.presence_connection_manager.src.aggregates.enums import GPStatusCode


@dataclass
class UserStatus:
    status_string: str
    location_string: str
    current_status: GPStatusCode = GPStatusCode.OFFLINE
