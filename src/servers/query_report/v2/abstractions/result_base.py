import abc
import library.abstractions.contracts
from servers.query_report.v2.enums.general import PacketType


class ResultBase(library.abstractions.contracts.ResultBase, abc.ABC):
    packet_type: PacketType = None
