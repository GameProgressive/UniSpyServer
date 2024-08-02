from enum import IntEnum


class RequestType(IntEnum):
    CHALLENGE = 0x01
    HEARTBEAT = 0x03
    CLIENT_MESSAGE = 0x06
    CLIENT_MESSAGE_ACK = 0x07
    ADD_ERROR = 0x04
    ECHO = 0x02
    KEEP_ALIVE = 0x08
    AVALIABLE_CHECK = 0x09


class ResponseType(IntEnum):
    QUERY = 0x00
    ECHO = 0x02
    ADD_ERROR = 0x04
    CLIENT_MESSAGE = 0x06
    REQUIRE_IP_VERIFY = 0x09
    CLIENT_REGISTERED = 0x0A


class PacketType(IntEnum):
    QUERY = 0x00
    CHALLENGE = 0x01
    ECHO = 0x02
    ADD_ERROR = 0x04
    CLIENT_MESSAGE = 0x06
    REQUIRE_IP_VERIFY = 0x09
    CLIENT_REGISTERED = 0x0A
    HEARTBEAT = 0x03
    ECHO_RESPONSE = 0x05
    CLIENT_MESSAGE_ACK = 0x07
    KEEP_ALIVE = 0x08
    AVALIABLE_CHECK = 0x09


class QRStateChange(IntEnum):
    NORMAL_HEARTBEAT = 0
    GAME_MODE_CHANGE = 1
    SERVER_SHUTDOWN = 2
    CANNOT_RECIEVE_CHALLENGE = 3


class HeartBeatReportType(IntEnum):
    SERVER_TEAM_PLAYER_DATA = 1
    SERVER_PLAYER_DATA = 2
    SERVER_DATA = 3


class GameServerStatus(IntEnum):
    NORMAL = 0
    UPDATE = 1
    SHUTDOWN = 2
    PLAYING = 3


class ServerAvailability(IntEnum):
    AVAILABLE = 0
    WAITING = 1
    PERMANENT_UNAVAILABLE = 2
    TEMPORARILY_UNAVAILABLE = 3
