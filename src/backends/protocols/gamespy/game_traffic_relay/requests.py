from uuid import UUID
from pydantic import BaseModel

from backends.library.abstractions.contracts import RequestBase
import frontends.gamespy.protocols.game_traffic_relay.contracts.general as fnt

"""
There are 2 UpdateGTRServiceRequest class
The other one is in frontends/gamespy/protocols/game_traffic_relay/contracts/general.py
"""


class GtrHeartBeatRequest(RequestBase):
    info: None = None
    """we do not need client info here"""
    data: fnt.GtrHeartbeatData
