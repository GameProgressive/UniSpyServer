from enum import Enum


class RequestType(Enum):
    HEARTBEAT = "heartbeat"
    HEARTBEAT_ACK = "gamename"


class ServerStatus(Enum):
    START = 0
    CHANGED = 1
    SHUTDOWN = 2
