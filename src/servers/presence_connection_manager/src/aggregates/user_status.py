from typing import Union

from pydantic import BaseModel
from servers.presence_connection_manager.src.enums.general import GPStatusCode


class UserStatus(BaseModel):
    status_string: str = None
    location_string: str = None
    current_status: Union[GPStatusCode, int] = GPStatusCode.OFFLINE
