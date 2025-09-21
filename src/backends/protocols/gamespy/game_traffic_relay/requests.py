from uuid import UUID
from pydantic import BaseModel

"""
There are 2 UpdateGTRServiceRequest class
The other one is in frontends/gamespy/protocols/game_traffic_relay/contracts/general.py
"""
class GTRHeartBeat(BaseModel):
    server_id: UUID
    public_ip_address: str
    public_port: int
    client_count: int
