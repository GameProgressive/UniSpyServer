from uuid import UUID
from backends.protocols.gamespy.game_traffic_relay.relay_server_info import RelayServerInfo
from backends.protocols.gamespy.natneg.requests import InitRequest
from backends.protocols.gamespy.natneg.init_packet_info import InitPacketInfo, NatFailInfo


def store_init_packet(request: InitRequest) -> None:
    info = InitPacketInfo(request.model_dump())
    info.update(upsert=True)


def count_init_info(cookie: int, version: int) -> int:
    result = InitPacketInfo.objects(cookie=cookie, version=version).count()
    return result


def get_init_infos(server_id: UUID, cookie: int) -> list[InitPacketInfo]:
    result: list[InitPacketInfo] = InitPacketInfo.objects(
        server_id=server_id, cookie=cookie)
    return result


def update_init_info(info: InitPacketInfo) -> None:
    assert isinstance(info, InitPacketInfo)
    info.save()


def remove_init_info(info: InitPacketInfo) -> None:
    info.delete()


def update_nat_fail_info(info: NatFailInfo) -> None:
    info.save()


def get_nat_fail_info(public_ip1: str, public_ip2: str) -> list[NatFailInfo]:
    result = NatFailInfo.objects(public_ip_address1=public_ip1,
                                 public_ip_address2=public_ip2)
    return result
