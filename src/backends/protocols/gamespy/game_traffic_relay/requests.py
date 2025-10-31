from uuid import UUID
from pydantic import BaseModel

from backends.library.abstractions.contracts import RequestBase

"""
There are 2 UpdateGTRServiceRequest class
The other one is in frontends/gamespy/protocols/game_traffic_relay/contracts/general.py
"""


class GtrHeartBeatRequest(RequestBase):
    server_id: UUID
    public_ip_address: str
    public_port: int
    client_count: int
    raw_request: None = None
    client_ip: None = None
    client_port: None = None
