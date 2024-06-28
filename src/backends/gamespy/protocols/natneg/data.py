from servers.natneg.contracts.requests import InitRequest
from servers.natneg.aggregations.init_packet_info import InitPacketInfo


def store_init_packet(request: InitRequest):
    json_dict = request.to_serializable_dict()
    info = InitPacketInfo(json_dict)
