from uuid import UUID

from pydantic import BaseModel

from frontends.gamespy.library.extentions.encoding import get_string
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.v2.abstractions.contracts import (
    RequestBase,
)

PREFIX = bytes([0x09, 0x00, 0x00, 0x00, 0x00])
POSTFIX = bytes([0x00])


class AvaliableRequest(RequestBase):

    @staticmethod
    def gs_parse(raw_request: bytes) -> "AvaliableRequest":
        data = RequestBase.gs_parse(raw_request)
        if data.raw_request[: len(PREFIX)] != PREFIX:
            raise QRException("Avaliable request prefix is invalid.")
        # postfix check
        if data.raw_request[len(data.raw_request) - 1].to_bytes() != POSTFIX:
            raise QRException("Avaliable request postfix is invalid.")

        data = AvaliableRequest(**data.model_dump())
        return data


class ChallengeRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "ChallengeRequest":
        data = RequestBase.gs_parse(raw_request)
        data = ChallengeRequest(**data.model_dump())
        return data


class ClientMessageAckRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "ClientMessageAckRequest":
        data = RequestBase.gs_parse(raw_request)
        data = ClientMessageAckRequest(**data.model_dump())
        return data


class ClientMessageRequest(RequestBase):
    server_browser_sender_id: UUID
    natneg_message: bytes
    target_ip_address: str
    target_port: int
    message_key: int

    @staticmethod
    def gs_parse(raw_request: bytes) -> "ClientMessageRequest":
        data = RequestBase.gs_parse(raw_request)
        data = ClientMessageRequest(**data.model_dump())
        return data

    @property
    def cookie(self) -> int:
        return int.from_bytes(self.natneg_message[6:10], "little")

    @property
    def target_ip_endpoint(self) -> str:
        return f"{self.target_ip_address}:{self.target_port}"


class HeartbeatRequest(RequestBase):
    game_data: dict[str, str]
    status: GameServerStatus
    group_id: int | None
    game_name: str

    @staticmethod
    def gs_parse(raw_request: bytes) -> "HeartbeatRequest":
        data = RequestBase.gs_parse(raw_request)
        game_data = {}
        player_pos, team_pos = 0, 0
        player_length, team_length = 0, 0
        data_partition = get_string(raw_request[5:])
        data_dict = data.model_dump()

        game_key_val = data_partition.split("\x00")
        index_gn = game_key_val.index("gamename")
        data_dict["game_name"] = game_key_val[index_gn + 1]
        try:
            player_pos = data_partition.index("player_\0", 0)
        except ValueError:
            player_pos = -1

        try:
            team_pos = data_partition.index("team_t\0", 0)
        except ValueError:
            team_pos = -1

        if player_pos != -1 and team_pos != -1:
            player_length = team_pos - player_pos
            team_length = len(data_partition) - team_pos

            server_data_str = data_partition[: player_pos - 4]
            HeartbeatRequest.parse_server_data(game_data, server_data_str)

            player_data_str = data_partition[
                player_pos - 1 : player_pos - 1 + player_length - 2
            ]
            HeartbeatRequest.parse_player_data(game_data, player_data_str)

            team_data_str = data_partition[team_pos - 1 : team_pos - 1 + team_length]
            HeartbeatRequest.parse_team_data(game_data, team_data_str)

        elif player_pos != -1:
            player_length = len(data_partition) - player_pos

            server_data_str = data_partition[: player_pos - 4]
            HeartbeatRequest.parse_server_data(game_data, server_data_str)

            player_data_str = data_partition[player_pos - 1 :]
            HeartbeatRequest.parse_player_data(game_data, player_data_str)

        elif player_pos == -1 and team_pos == -1:
            server_data_str = data_partition
            HeartbeatRequest.parse_server_data(game_data, server_data_str)

        else:
            raise QRException("HeartBeat request is invalid.")

        data_dict["game_data"] = game_data
        data_dict["status"] = int(game_data.get("statechanged", 0))
        data_dict["group_id"] = game_data.get("group_id", 0)
        data = HeartbeatRequest(**data_dict)
        return data

    @staticmethod
    def parse_server_data(request_dict, server_data_str: str):
        key_value_array = server_data_str.split("\0")

        for i in range(0, len(key_value_array), 2):
            if i + 2 > len(key_value_array):
                break

            temp_key = key_value_array[i]
            temp_value = key_value_array[i + 1]

            if temp_key in request_dict:
                request_dict[temp_key] = temp_value
            else:
                request_dict[temp_key] = temp_value

    @staticmethod
    def parse_player_data(request_dict: dict, player_data_str: str):
        player_count = int.from_bytes(player_data_str[0].encode())
        player_data_str = player_data_str[1:]

        index_of_key = player_data_str.index("\0\0", 0)
        key_str = player_data_str[:index_of_key]
        keys = key_str.split("\0")

        values_str = player_data_str[index_of_key + 2 :]
        values = values_str.split("\0")

        key_value = {}
        for player_index in range(player_count):

            for key_index in range(len(keys)):
                temp_key = keys[key_index] + str(player_index)
                temp_value = values[player_index * len(keys) + key_index]

                if temp_key in key_value:
                    key_value[temp_key] = temp_value
                else:
                    key_value[temp_key] = temp_value

        for key, value in key_value.items():
            request_dict[key] = value

    @staticmethod
    def parse_team_data(request_dict, team_data_str: str):
        team_count = int.from_bytes(team_data_str[0].encode())
        team_data_str = team_data_str[1:]

        end_key_index = team_data_str.index("\0\0", 0)
        key_str = team_data_str[:end_key_index]
        keys = key_str.split("\0")

        value_str = team_data_str[end_key_index + 2 :]
        values = value_str.split("\0")

        key_value = {}
        for team_index in range(team_count):

            for key_index in range(len(keys)):
                temp_key = keys[key_index] + str(team_index)
                temp_value = values[team_index * len(keys) + key_index]

                if temp_key in key_value:
                    key_value[temp_key] = temp_value
                else:
                    key_value[temp_key] = temp_value

        for key, value in key_value.items():
            request_dict[key] = value


class EchoRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "EchoRequest":
        data = RequestBase.gs_parse(raw_request)
        data = EchoRequest(**data.model_dump())
        return data


class KeepAliveRequest(RequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "KeepAliveRequest":
        data = RequestBase.gs_parse(raw_request)
        data = KeepAliveRequest(**data.model_dump())
        return data
