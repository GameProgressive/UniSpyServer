from uuid import UUID

from pydantic import BaseModel, Field

from types import MappingProxyType


class PeerRoomInfo(BaseModel):
    server_id: UUID
    game_name: str
    group_id: int = Field(..., alias='groupid')
    room_name: str = Field(alias="hostname")
    number_of_waiting: int = Field(default=0, alias="numwaiting")
    max_waiting: int = Field(default=200, alias='maxwaiting')
    number_of_servers: int = Field(default=0, alias="numservers")
    number_of_players: int = Field(default=0, alias="numplayers")
    max_players: int = Field(200, alias="maxplayers")
    password: str = Field(default="", alias="password")
    number_of_games: int = Field(default=0, alias="numgames")
    number_of_playing: int = Field(default=0, alias="numplaying")

    # def __init__(self, game_name, group_id, room_name) -> None:
    #     self.game_name = game_name
    #     self.group_id = group_id
    #     self.room_name = room_name

    def get_gamespy_dict(self) -> MappingProxyType:
        """
        return a immutable dict
        """
        data = self.model_dump()
        del data["server_id"]
        del data["game_name"]
        return MappingProxyType(data)
