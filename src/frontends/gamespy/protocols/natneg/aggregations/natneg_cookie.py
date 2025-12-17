

from pydantic import BaseModel


class NatNegCookie(BaseModel):
    host_ip: str
    host_port: int
    heartbeat_ip: str
    heartbeat_port: int
    game_name: str
    natneg_message: list
    instant_key: int
