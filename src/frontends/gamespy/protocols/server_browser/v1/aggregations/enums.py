from enum import Enum


class RequestType(Enum):
    SERVER_LIST = "gamename"


class Modifier(Enum):
    GROUPS = "group"
    INFO = "info2"
    COMPRESS = "cmp"