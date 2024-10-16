
from uuid import UUID
from backends.protocols.gamespy.game_traffic_relay.relay_server_info import RelayServerInfo


def get_available_relay_serves() -> list[RelayServerInfo]:
    """
    Return
    ------
        list of ip:port
    """
    result: list[RelayServerInfo] = list(RelayServerInfo.objects)
    return result


def update_relay_server(info: RelayServerInfo):
    info.save()


def delete_relay_server(server_id: UUID, ip_address: str, port: int):
    info = RelayServerInfo.object(
        server_id=server_id, public_ip_address=ip_address, public_port=port)
    info.delete()
