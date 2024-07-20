from library.src.database.pg_orm import PG_SESSION, GroupList, Games


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
            grouped_result[game.gamename] = []
        grouped_result[game.gamename].append(
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


if __name__ == "__main__":
    get_all_groups()
