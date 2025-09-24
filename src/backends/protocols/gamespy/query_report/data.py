from typing import TYPE_CHECKING,  cast
from uuid import UUID
from backends.library.database.pg_orm import (
    ENGINE,
    ChatChannelCaches,
    GroupList,
    Games,
    GameServerCaches,
)
from frontends.gamespy.protocols.chat.aggregates.peer_room import PeerRoom
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import (
    PeerRoomInfo,
)
from sqlalchemy.orm import Session

from datetime import datetime

from frontends.gamespy.protocols.query_report.v2.aggregates.enums import GameServerStatus


def get_all_groups() -> dict:
    with Session(ENGINE) as session:
        result = (
            session.query(Games, GroupList)
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


PEER_GROUP_LIST = get_all_groups()


def get_peer_staging_channels(
    game_name: str, group_id: int, session: Session
) -> list[GameServerInfo]:
    """
    todo check where use this function
    """
    assert isinstance(game_name, str)
    assert isinstance(group_id, int)
    staging_name = f"{PeerRoom.StagingRoomPrefix}!{game_name}!*"
    result = (
        session.query(ChatChannelCaches)
        .where(ChatChannelCaches.channel_name == staging_name)
        .all()
    )
    data = []
    for s in result:
        t = {k: v for k, v in s.__dict__.items() if k != "_sa_instance_state"}
        data.append(GameServerInfo(**t))
    return data


def get_group_data_list_by_gamename(game_name: str) -> list[dict]:
    assert isinstance(game_name, str)
    if game_name not in PEER_GROUP_LIST:
        raise ValueError(f"game name: {game_name} not in PEER_GROUP_LIST")
    # the group id list length can not be 0
    result = PEER_GROUP_LIST[game_name]
    assert len(result) != 0
    return result


def get_peer_group_channel(
    group_data: list[dict], session: Session
) -> list[PeerRoomInfo]:
    assert isinstance(group_data, list) and all(
        isinstance(id, dict) for id in group_data
    )
    # Construct the group names based on the provided group_ids
    group_names = [
        f"{PeerRoom.GroupRoomPrefix}!{item['group_id']}" for item in group_data
    ]

    # Query the database for channels matching the constructed group names
    result = (
        session.query(ChatChannelCaches)
        .filter(ChatChannelCaches.channel_name.in_(group_names))
        .all()
    )

    # Convert the result to a list of PeerRoomInfo objects
    data = [PeerRoomInfo(**s.__dict__) for s in result]

    return data


def get_server_info_with_instant_key(
    instant_key: str, session: Session
) -> GameServerCaches | None:
    assert isinstance(instant_key, str)
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.instant_key == instant_key)
        .first()
    )
    return result


def get_server_info_with_game_name(
    game_name: str, session: Session
) -> GameServerCaches | None:
    assert isinstance(game_name, str)
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.game_name == game_name)
        .first()
    )
    return result


def get_server_info_list_with_game_name(
    game_name: str, session: Session
) -> list[GameServerInfo]:
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.game_name == game_name)
        .all()
    )
    data = []
    for s in result:
        data.append(GameServerInfo(**s.__dict__))
    return data


def get_server_info_with_ip_and_port(ip: str, port: int, session: Session) -> GameServerInfo | None:
    assert isinstance(ip, str)
    assert isinstance(port, int)
    result = (
        session.query(GameServerCaches)
        .where(
            GameServerCaches.host_ip_address == ip,
            GameServerCaches.query_report_port == port,
        )
        .first()
    )
    if result is None:
        return None
    data = GameServerInfo(**result.__dict__)
    return data


def remove_server_info(info: GameServerCaches, session: Session) -> None:
    session.delete(info)
    session.commit()


# todo finish the GameServerCaches creation


def create_game_server(info: GameServerCaches, session: Session) -> None:
    session.add(info)
    session.commit()


def update_game_server(
    cache: GameServerCaches,
    instant_key: str,
    server_id: UUID,
    host_ip_address: str,
    game_name: str,
    query_report_port: int,
    server_status: GameServerStatus,
    player_data: list[dict[str, object]],
    server_data: dict[str, object],
    team_data: list[dict[str, object]],
    session: Session,
) -> None:
    cache.instant_key = instant_key  # type: ignore
    cache.server_id = server_id  # type: ignore
    cache.host_ip_address = host_ip_address  # type: ignore
    cache.game_name = game_name  # type: ignore
    cache.query_report_port = query_report_port  # type: ignore
    cache.update_time = datetime.now()  # type: ignore
    cache.status = server_status  # type: ignore
    cache.player_data = player_data  # type: ignore
    cache.server_data = server_data  # type: ignore
    cache.team_data = team_data  # type: ignore
    session.commit()


def refresh_game_server_cache(instant_key: str, session: Session):
    cache = get_server_info_with_instant_key(instant_key, session)
    if cache is None:
        raise QRException(
            "no game server cache found, please check the database")
    cache.update_time = datetime.now()  # type: ignore
    session.commit()


if __name__ == "__main__":
    get_all_groups()
