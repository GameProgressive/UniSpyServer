from enum import IntEnum


class AuthMethod(IntEnum):
    UNKNOWN = 0
    PROFILE_ID_AUTH = 0
    PARTNER_ID_AUTH = 1
    CDKEY_AUTH = 2


class PersistStorageType(IntEnum):
    PRIVATE_READ_ONLY = 0
    PRIVATE_READ_WRITE = 1
    PUBLIC_READ_ONLY = 2
    PUBLIC_READ_WRITE = 3


class GSErrorCode(IntEnum):
    GENERAL = 0
    PARSE = 1
    DATABASE = 2
    NOERROR = 3
