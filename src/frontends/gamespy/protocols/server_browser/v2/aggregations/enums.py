from enum import IntEnum


class PlayerSearchOptions(IntEnum):
    SEARCH_ALL_GAMES = 1
    SEARCH_LEFT_SUBSTRING = 2
    SEARCH_RIGHT_SUBSTRING = 4
    SEARCH_ANY_SUBSTRING = 8


class QueryType(IntEnum):
    BASIC = 1
    FULL = 2
    ICMP = 3


class DataKeyType(IntEnum):
    STRING = 1
    BYTE = 2
    SHORT = 3


class RequestType(IntEnum):
    SERVER_LIST_REQUEST = 0x00
    SERVER_INFO_REQUEST = 0x01
    SEND_MESSAGE_REQUEST = 0x02
    KEEP_ALIVE_REPLY = 0x03
    MAP_LOOP_REQUEST = 0x04
    PLAYER_SEARCH_REQUEST = 0x05

class ResponseType(IntEnum):
    PUSH_KEYS_MESSAGE = 1
    PUSH_SERVER_MESSAGE = 2
    KEEP_ALIVE_MESSAGE = 3
    DELETE_SERVER_MESSAGE = 4
    MAP_LOOP_MESSAGE = 5
    PLAYER_SEARCH_MESSAGE = 6


class ProtocolVersion(IntEnum):
    LIST_PROTOCOL_VERSION_1 = 0
    LIST_ENCODING_VERSION = 3


class ServerListUpdateOption(IntEnum):
    SERVER_MAIN_LIST = 0
    SEND_FIELD_FOR_ALL = 1
    SERVER_FULL_MAIN_LIST = 2
    P2P_SERVER_MAIN_LIST = 4
    ALTERNATE_SOURCE_IP = 8
    P2P_GROUP_ROOM_LIST = 32
    NO_LIST_CACHE = 64
    LIMIT_RESULT_COUNT = 128


class GameServerFlags(IntEnum):
    UNSOLICITED_UDP_FLAG = 1
    PRIVATE_IP_FLAG = 2
    CONNECT_NEGOTIATE_FLAG = 4
    ICMP_IP_FLAG = 8
    NON_STANDARD_PORT_FLAG = 16
    NON_STANDARD_PRIVATE_PORT_FLAG = 32
    HAS_KEYS_FLAG = 64
    HAS_FULL_RULES_FLAG = 128


if __name__ == "__main__":
    GameServerFlags.PRIVATE_IP_FLAG.value
    pass
