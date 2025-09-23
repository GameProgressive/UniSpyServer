from enum import Enum, IntEnum


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
    BAD_HEART_BEAT = 1
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


class NatType(IntEnum):
    NO_NAT = 0
    FIRE_WALL_ONLY = 1
    """
    C发数据到210.21.12.140:8000,NAT会将数据包送到A(192.168.0.4:5000).因为NAT上已经有了192.168.0.4:5000到210.21.12.140:8000的映射
    """
    FULL_CONE = 2
    """
    C无法和A通信,因为A从来没有和C通信过,NAT将拒绝C试图与A连接的动作.但B可以通过210.21.12.140:8000与A的 192.168.0.4:5000通信,且这里B可以使用任何端口与A通信.如:210.15.27.166:2001 -> 210.21.12.140:8000,NAT会送到A的5000端口上
    """
    ADDRESS_RESTRICTED_CONE = 3
    """
    C无法与A通信,因为A从来没有和C通信过.而B也只能用它的210.15.27.166:2000与A的192.168.0.4:5000通信,因为A也从来没有和B的其他端口通信过.该类型NAT是端口受限的.
    """
    PORT_RESTRICTED_CONE = 4
    """
    上面3种类型,统称为Cone NAT,有一个共同点:只要是从同一个内部地址和端口出来的包,NAT都将它转换成同一个外部地址和端口.但是Symmetric有点不同,具体表现在: 只要是从同一个内部地址和端口出来,且到同一个外部目标地址和端口,则NAT也都将它转换成同一个外部地址和端口.但如果从同一个内部地址和端口出来,是 到另一个外部目标地址和端口,则NAT将使用不同的映射,转换成不同的端口(外部地址只有一个,故不变).而且和Port Restricted Cone一样,只有曾经收到过内部地址发来包的外部地址,才能通过NAT映射后的地址向该内部地址发包.
    """
    SYMMETRIC = 5
    """
    端口分配是随机的,无法确定下一次NAT映射端口.
    """
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
