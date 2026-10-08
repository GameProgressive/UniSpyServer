from enum import Enum


class CommandName(Enum):
    HEARTBEAT = "heartbeat"
    HEARTBEAT_ACK = "gamename"


class ServerStatus(Enum):
    START = 0
    CHANGED = 1
    SHUTDOWN = 2
