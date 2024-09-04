# from servers.natneg.contracts.requests import InitRequest
from backends.gamespy.protocols.natneg.requests import InitRequest
from backends.gamespy.protocols.natneg.init_packet_info import InitPacketInfo
from mongoengine import QuerySet

def store_init_packet(request: InitRequest) -> None:
    info = InitPacketInfo(request.model_dump())
    # InitPacketInfo.objects(server_id=info.server_id, cookie=info.cookie,        version=info.version, port_type=info.port_type,
    #                        client_index=info.client_index, game_name=info.game_name, use_game_port=info.use_game_port,
    #                        public_ip=info.public_ip, public_port=info.public_port, private_ip=info.private_ip, private_port=info.private_port)
    info.update(upsert=True)


def count_inif_info(cookie: int, version: int) -> int:
    result = InitPacketInfo.objects(cookie=cookie, version=version).count()
    return result
