from pydantic import BaseModel, UUID4

from frontends.gamespy.library.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.natneg.aggregations.enums import NatClientIndex, NatPortType


class InitPacketInfo(BaseModel):
    server_id: UUID4
    cookie: int
    version: int
    port_type: NatPortType
    client_index: NatClientIndex
    game_name: str
    use_game_port: bool
    public_ip: str
    public_port: int
    private_ip: str
    private_port: int


class GtrHeartbeat(BaseModel):
    server_id: UUID4
    public_ip_address: str
    public_port: int
    client_count: int


class MessageRelayRequest(RequestBase):
    raw_request: bytes
    pass
