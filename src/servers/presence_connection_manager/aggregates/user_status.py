from dataclasses import dataclass
from typing import Union
from servers.presence_connection_manager.enums.general import GPStatusCode


@dataclass
class UserStatus:
    status_string: str
    location_string: str
    current_status: Union[GPStatusCode, int] = GPStatusCode.OFFLINE
