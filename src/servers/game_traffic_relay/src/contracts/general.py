from pydantic import BaseModel, UUID4

from servers.natneg.src.aggregations.enums import NatClientIndex, NatPortType


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
