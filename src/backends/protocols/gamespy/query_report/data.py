from typing import TYPE_CHECKING,  cast
from uuid import UUID
from backends.library.database.pg_orm import (
    ENGINE,
    ChatChannelCaches,
    ChatChannelUserCaches,
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

from datetime import datetime, timedelta

from frontends.gamespy.protocols.query_report.v2.aggregates.enums import GameServerStatus
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import SBException


def __expire_time():
    return datetime.now() - timedelta(5)


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


def get_peer_group_channel(
    game_name: str, session: Session
) -> list[PeerRoomInfo]:
    # Query the database for channels matching the constructed group names
    group_data = (
        session.query(GroupList)
        .select_from(Games)
        .join(GroupList, (Games.gameid == GroupList.gameid))
        .where(Games.gamename == game_name)
        .all()
    )
    group_info: list[PeerRoomInfo] = []
    for gd in group_data:
        # get the group room info from GroupList

        # Construct the group names based on the provided group_ids
        group_room_channel_name = f"{PeerRoom.GroupRoomPrefix}!{gd.groupid}"
        channel_name_prefix_regex = f"{group_room_channel_name}!%"
        # Query the database for channels matching the constructed group names
        result = (
            session.query(ChatChannelCaches)
            .where(ChatChannelCaches.channel_name == group_room_channel_name
                   ).first())
        assert isinstance(gd.groupid, int)
        assert isinstance(gd.roomname, str)

        if result is None:
            info = PeerRoomInfo(groupid=gd.groupid,
                                game_name=game_name,
                                hostname=gd.roomname)
        else:
            assert isinstance(result.channel_name, str)

            assert isinstance(result.max_num_user, int)
            # todo get the peer room extra info
            waiting_player_count = session.query(ChatChannelUserCaches).where(
                ChatChannelUserCaches.channel_name == result.channel_name).count()
            playing_player_count = session.query(ChatChannelUserCaches).where(
                ChatChannelUserCaches.channel_name.like(channel_name_prefix_regex)).count()
            password = result.password if result.password is not None else ""
            assert isinstance(password, str)
            info = PeerRoomInfo(groupid=gd.groupid,
                                game_name=game_name,
                                hostname=result.channel_name,
                                password=password,
                                maxplayers=result.max_num_user,
                                numplayers=waiting_player_count+playing_player_count,
                                numwaiting=waiting_player_count,
                                numplaying=playing_player_count
                                )

        group_info.append(info)
    return group_info


def get_game_server_cache_by_ip_port(ip: str, port: int, session: Session) -> GameServerCaches | None:
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.host_ip_address == ip,
               GameServerCaches.query_report_port == port,
               GameServerCaches.update_time >= __expire_time())
        .first()
    )
    return result


def get_game_server_cache_with_instant_key(
    instant_key: str, session: Session
) -> GameServerCaches | None:
    assert isinstance(instant_key, str)
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.instant_key == instant_key,
               GameServerCaches.update_time >= __expire_time())
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


def get_secret_key(game_name: str, session: Session) -> str:
    result = session.query(Games.secretkey).where(
        Games.gamename == game_name).first()
    if result is None:
        raise SBException(f"{game_name} secret key not found in database")
    return result[0]


def get_server_info_list_with_game_name(
    game_name: str, session: Session
) -> list[GameServerInfo]:
    result = (
        session.query(GameServerCaches)
        .where(GameServerCaches.game_name == game_name,
               GameServerCaches.update_time >= __expire_time(),
               GameServerCaches.avaliable == True)
        .all()
    )
    data = []
    for s in result:
        assert isinstance(s.server_id, UUID)
        assert isinstance(s.host_ip_address, str)
        assert isinstance(s.instant_key, str)
        assert isinstance(s.game_name, str)
        assert isinstance(s.query_report_port, int)
        assert isinstance(s.update_time, datetime)
        assert isinstance(s.status, GameServerStatus)
        assert isinstance(s.server_data, dict)
        assert isinstance(s.player_data, list)
        assert isinstance(s.team_data, list)

        data.append(GameServerInfo(
            server_id=s.server_id,
            host_ip_address=s.host_ip_address,
            instant_key=s.instant_key,
            game_name=s.game_name,
            query_report_port=s.query_report_port,
            last_heart_beat_received_time=s.update_time,
            status=s.status,
            server_data=s.server_data,
            player_data=s.player_data,
            team_data=s.team_data
        ))
    return data


def check_game_server_cache_conflict(ip: str, port: int, instant_key: str, session: Session):
    cache = get_server_info_with_ip_and_port(
        ip, port, session)
    if cache is not None:
        if cache.instant_key != instant_key:
            session.delete(cache)
            session.commit()


def get_server_info_with_ip_and_port(ip: str, port: int, session: Session) -> GameServerInfo | None:
    assert isinstance(ip, str)
    assert isinstance(port, int)
    result = (
        session.query(GameServerCaches)
        .where(
            GameServerCaches.host_ip_address == ip,
            GameServerCaches.query_report_port == port,
            GameServerCaches.update_time >= __expire_time())
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


def create_game_server_cache(info: GameServerCaches, session: Session) -> None:
    session.add(info)
    session.commit()


def update_game_server_cache(
    cache: GameServerCaches,
    instant_key: str,
    server_id: UUID,
    host_ip_address: str,
    game_name: str,
    query_report_port: int,
    server_status: GameServerStatus,
    player_data: list[dict[str, object]] | None,
    server_data: dict[str, object] | None,
    team_data: list[dict[str, object]] | None,
    session: Session,
) -> None:
    cache.instant_key = instant_key  # type: ignore
    cache.server_id = server_id  # type: ignore
    cache.host_ip_address = host_ip_address  # type: ignore
    cache.game_name = game_name  # type: ignore
    cache.query_report_port = query_report_port  # type: ignore
    cache.update_time = datetime.now()  # type: ignore
    cache.status = server_status  # type: ignore
    if player_data is not None:
        cache.player_data = player_data  # type: ignore
    if server_data is not None:
        cache.server_data = server_data  # type: ignore
    if team_data is not None:
        cache.team_data = team_data  # type: ignore
    session.commit()


def refresh_game_server_cache(instant_key: str, session: Session):
    cache = get_game_server_cache_with_instant_key(instant_key, session)
    if cache is None:
        raise QRException(
            "no game server cache found, please check the database")
    cache.update_time = datetime.now()  # type: ignore
    session.commit()


def clean_expired_game_server_cache(session: Session):
    session.query(GameServerCaches).where(
        GameServerCaches.update_time < __expire_time()
    ).delete()
    session.commit()


if __name__ == "__main__":
    pass
