from pydantic import BaseModel, Field


class PeerRoomInfo(BaseModel):
    game_name: str
    group_id: int = Field(..., alias='groupid')
    room_name: str = Field(alias="hostname")
    number_of_waiting: int = Field(default=0, alias="numwaiting")
    max_waiting: int = Field(default=200, alias='maxwaiting')
    number_of_servers: int = Field(default=0, alias="numservers")
    number_of_players: int = Field(default=0, alias="numplayers")
    max_players: int = Field(default=200, alias="maxplayers")
    password: str = Field(default="", alias="password")
    number_of_games: int = Field(default=0, alias="numgames")
    number_of_playing: int = Field(default=0, alias="numplaying")

    def get_gamespy_dict(self) -> dict:
        """
        return a immutable dict
        """
        data = self.model_dump(mode="json")
        del data["game_name"]
        return data
