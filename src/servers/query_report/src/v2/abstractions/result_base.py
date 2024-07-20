import abc
import library.src.abstractions.contracts
from servers.query_report.src.v2.enums.general import PacketType


class ResultBase(library.src.abstractions.contracts.ResultBase, abc.ABC):
    packet_type: PacketType = None
