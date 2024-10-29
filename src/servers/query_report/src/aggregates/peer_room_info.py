from uuid import UUID

from pydantic import BaseModel


dd = {
    "groupid": "groupid",
    "hostname": "hostname",
    "number_of_waiting_player": "numwaiting",
    "max_number_of_waiting_players": "maxwaiting",
    "number_of_servers": "numservers",
    "number_of_players": "numplayers",
    "max_number_of_players": "maxplayers",
    "password": "password",
    "number_of_games": "numgames",
    "number_of_playing_players": "numplaying",
}


class PeerRoomInfo(BaseModel):
    server_id: UUID
    game_name: str
    group_id: int
    room_name: str
    # todo change to dict[str, object]
    raw_key_values: dict

    def __init__(self, game_name, group_id, room_name) -> None:
        self.game_name = game_name
        self.group_id = group_id
        self.room_name = room_name
        self.raw_key_values = {
            "groupid": group_id,
            "hostname": room_name,
            "numwaiting": 0,
            "maxwaiting": 200,
            "numservers": 0,
            "numplayers": 0,
            "maxplayers": 200,
            "password": "",
            "numgames": 0,
            "numplaying": 0,
        }

    @property
    def number_of_servers(self) -> int:
        return int(self.raw_key_values["numservers"])

    @number_of_servers.setter
    def number_of_servers(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["numservers"] = value

    @property
    def number_of_players(self) -> int:
        return int(self.raw_key_values["numplayers"])

    @number_of_players.setter
    def number_of_players(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["numplayers"] = value

    @property
    def max_number_of_waiting_players(self) -> int:
        return int(self.raw_key_values["maxwaiting"])

    @max_number_of_waiting_players.setter
    def max_number_of_waiting_players(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["maxwaiting"] = value

    @property
    def max_number_of_players(self) -> int:
        return int(self.raw_key_values["maxplayers"])

    @max_number_of_players.setter
    def max_number_of_players(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["maxplayers"] = value

    @property
    def number_of_games(self) -> int:
        return int(self.raw_key_values["numgames"])

    @number_of_games.setter
    def number_of_games(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["numgames"] = value

    @property
    def number_of_playing_players(self) -> int:
        return self.raw_key_values["numplaying"]

    @number_of_playing_players.setter
    def number_of_playing_players(self, value: int):
        assert isinstance(value, int)
        self.raw_key_values["numplaying"] = value

    def get_gamespy_format_data(self):
        """
        convert everything to string
        """
        return {key: str(value) for key, value in self.raw_key_values.items()}
