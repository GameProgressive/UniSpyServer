from backends.protocols.gamespy.chat.storage_infos import ChannelInfo
from library.src.database.pg_orm import PG_SESSION, GroupList, Games
from servers.chat.src.aggregates.peer_room import PeerRoom
from servers.query_report.src.v2.aggregates.game_server_info_v2 import GameServerInfoV2


def get_all_groups():
    result: list[tuple[Games, GroupList]] = (
        PG_SESSION.query(Games, GroupList)
        .join(GroupList, Games.gameid == GroupList.gameid)
        .all()
    )

    # Group the results by Game name
    grouped_result = {}
    for game, group in result:
        if game.gamename not in grouped_result:
            temp_list = []
            grouped_result[game.gamename] = temp_list
        temp_list.append(
            {
                "game_id": group.gameid,
                "game_name": game.gamename,
                "group_id": group.groupid,
                "room_name": group.roomname,
                "secret_key": game.secretkey,
            }
        )

    # Convert the grouped result to the desired format
    return grouped_result


def get_peer_staging_channels(game_name: str, group_id: int):
    assert isinstance(game_name, str)
    assert isinstance(group_id, int)
    staging_name = f"{PeerRoom.StagingRoomPrefix}!{game_name}!*"

    result: list[ChannelInfo] = ChannelInfo.objects(channel_name=staging_name)
    return result


def get_peer_group_channel(group_id: int):
    assert isinstance(group_id, int)
    group_name = f"{PeerRoom.GroupRoomPrefix}!{group_id}"
    result: list[ChannelInfo] = ChannelInfo.objects(channel_name=group_name)
    return result


def get_server_info_with_instant_key(instant_key: int) -> GameServerInfoV2:
    assert isinstance(instant_key, int)

    result = GameServerInfoV2.objects(instant_key=instant_key)

    return result


def get_server_info_with_game_name(game_name: str) -> GameServerInfoV2:
    assert isinstance(game_name, str)

    result = GameServerInfoV2.objects(game_name=game_name).one()

    return result


def get_server_info_with_ip_and_port(ip: str, port: int) -> GameServerInfoV2:
    assert isinstance(ip, str)
    assert isinstance(port, int)

    result = GameServerInfoV2.objects(
        host_ip_address=ip, query_report_port=port).one()

    return result

def remove_server_info(info: GameServerInfoV2) -> None:
    info.delete()


def update_game_server(info: GameServerInfoV2) -> None:
    info.update()


if __name__ == "__main__":
    get_all_groups()
