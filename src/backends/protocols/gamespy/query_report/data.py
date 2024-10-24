from typing import TYPE_CHECKING, Optional, cast
from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, GroupList, Games, GameServerCaches
from servers.chat.src.aggregates.peer_room import PeerRoom


def get_all_groups() -> dict:
    result = (
        PG_SESSION.query(Games, GroupList)
        .join(GroupList, (Games.gameid == GroupList.gameid))
        .all()
    )
    if TYPE_CHECKING:
        result = cast(list[tuple[Games, GroupList]], result)

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


def get_peer_staging_channels(game_name: str, group_id: int) -> list[ChatChannelCaches]:
    assert isinstance(game_name, str)
    assert isinstance(group_id, int)
    staging_name = f"{PeerRoom.StagingRoomPrefix}!{game_name}!*"
    result = PG_SESSION.query(ChatChannelCaches).filter(
        ChatChannelCaches.channel_name == staging_name).all()
    return result


def get_peer_group_channel(group_id: int) -> list[ChatChannelCaches]:
    assert isinstance(group_id, int)
    group_name = f"{PeerRoom.GroupRoomPrefix}!{group_id}"

    result = PG_SESSION.query(ChatChannelCaches).filter(
        ChatChannelCaches.channel_name == group_name).all()
    return result


def get_server_info_with_instant_key(instant_key: int) -> Optional[GameServerCaches]:
    assert isinstance(instant_key, int)
    result = PG_SESSION.query(GameServerCaches).filter(
        GameServerCaches.instant_key == instant_key).first()
    return result


def get_server_info_with_game_name(game_name: str) -> Optional[GameServerCaches]:
    assert isinstance(game_name, str)
    result = PG_SESSION.query(GameServerCaches).filter(
        GameServerCaches.game_name == game_name).first()
    return result


def get_server_info_with_ip_and_port(ip: str, port: int) -> GameServerCaches:
    assert isinstance(ip, str)
    assert isinstance(port, int)
    result = PG_SESSION.query(GameServerCaches).filter(
        GameServerCaches.host_ip_address == ip, GameServerCaches.query_report_port == port).first()

    return result


def remove_server_info(info: GameServerCaches) -> None:
    PG_SESSION.delete(info)
    PG_SESSION.commit()

# todo finish the GameServerCaches creation


def update_game_server(info: GameServerCaches) -> None:
    from datetime import datetime
    info.update_time = datetime.now()  # type:ignore
    PG_SESSION.add(info)
    PG_SESSION.commit()


if __name__ == "__main__":
    get_all_groups()
