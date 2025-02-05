from enum import Enum


class RequestType(Enum):
    INIT = 0
    ERT_ACK = 3
    CONNECT = 5
    CONNECT_ACK = 6
    """
    only used in client, not used by server
    """
    PING = 7
    ADDRESS_CHECK = 10
    NATIFY_REQUEST = 12
    REPORT = 13
    PRE_INIT = 15


class NatPortType(Enum):
    GP = 0
    NN1 = 1
    NN2 = 2
    NN3 = 3


class ResponseType(Enum):
    INIT_ACK = 1
    ERT_TEST = 2
    ERT_ACK = 3
    CONNECT = 5
    ADDRESS_REPLY = 11
    REPORT_ACK = 14
    PRE_INIT_ACK = 16


class ConnectPacketStatus(Enum):
    NO_ERROR = 0
    BEAD_HEART_BEAT = 1
    INIT_PACKET_TIMEOUT = 2


class NatifyPacketType(Enum):
    PACKET_MAP_1A = 0
    PACKET_MAP_2 = 1
    PACKET_MAP_3 = 2
    PACKET_MAP_1B = 3
    NUM_PACKETS = 4


class PreInitState(Enum):
    WAITING_FOR_CLIENT = 0
    WAITING_FOR_MATCH_UP = 1
    READY = 2


class NatType(Enum):
    NO_NAT = 0
    FIREWALL_ONLY = 1
    FULL_CONE = 2
    ADDRESS_RESTRICTED_CONE = 3
    PORT_RESTRICTED_CONE = 4
    SYMMETRIC = 5
    UNKNOWN = 6


class NatPortMappingScheme(Enum):
    UNRECOGNIZED = 0
    PRIVATE_AS_PUBLIC = 1
    CONSISTENT_PORT = 2
    INCREMENTAL = 3
    MIXED = 4


class NatClientIndex(Enum):
    GAME_CLIENT = 0
    GAME_SERVER = 1
