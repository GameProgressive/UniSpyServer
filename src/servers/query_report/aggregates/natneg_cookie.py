
from attr import dataclass


@dataclass
class NatNegCookie:
    host_ip:str
    host_port:int
    heartbeat_ip:str
    heartbeat_port:int
    game_name:str
    natneg_message:bytes
    instant_key:int