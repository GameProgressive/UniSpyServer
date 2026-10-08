from enum import Enum


class GameServerStatus(Enum):
    NORMAL = 0
    UPDATE = 1
    SHUTDOWN = 2
    PLAYING = 3


class ProtocolVersion(Enum):
    V1 = 1
    V2 = 2
