from uuid import UUID
from library.src.extentions.encoding import get_string
from library.src.log.log_manager import LogWriter
from servers.query_report.src.exceptions.exceptions import QRException
from servers.query_report.src.v2.abstractions.contracts import RequestBase
from servers.query_report.src.v2.enums.general import GameServerStatus


PREFIX = bytes([0x09, 0x00, 0x00, 0x00, 0x00])
POSTFIX = bytes([0x00])


class AvaliableRequest(RequestBase):

    def parse(self):
        super().parse()
        for i in range(len(PREFIX)):
            if self.raw_request[i] != PREFIX[i]:
                raise QRException("Avaliable request prefix is invalid.")

        # postfix check
        if self.raw_request[len(self.raw_request) - 1] != POSTFIX:
            raise QRException("Avaliable request postfix is invalid.")


class ChallengeRequest(RequestBase):
    pass


class ClientMessageAckRequest(RequestBase):
    pass


class ClientMessageRequest(RequestBase):
    server_browser_sender_id: UUID
    natneg_message: list[int]
    target_ip_address: str
    target_port: str
    message_key: int

    @property
    def cookie(self):
        return int.from_bytes(self.natneg_message[6:10], "little")


class HeartBeatRequest(RequestBase):
    server_data: dict[str, str]
    player_data: list[dict[str, str]]
    team_data: list[dict[str, str]]
    server_status: GameServerStatus
    group_id: int

    def parse(self):
        super().parse()
        player_pos, team_pos = 0, 0
        player_length, team_length = 0, 0
        self.data_partition = get_string(self.raw_request[5:])

        player_pos = self.data_partition.index("player_\0", 0)
        team_pos = self.data_partition.index("team_t\0", 0)

        if player_pos != -1 and team_pos != -1:
            player_length = team_pos - player_pos
            team_length = len(self.data_partition) - team_pos

            server_data_str = self.data_partition[: player_pos - 4]
            self.parse_server_data(server_data_str)

            player_data_str = self.data_partition[
                player_pos - 1 : player_pos - 1 + player_length - 2
            ]
            self.parse_player_data(player_data_str)

            team_data_str = self.data_partition[
                team_pos - 1 : team_pos - 1 + team_length
            ]
            self.parse_team_data(team_data_str)

        elif player_pos != -1:
            player_length = len(self.data_partition) - player_pos

            server_data_str = self.data_partition[: player_pos - 4]
            self.parse_server_data(server_data_str)

            player_data_str = self.data_partition[player_pos - 1 :]
            self.parse_player_data(player_data_str)

        elif player_pos == -1 and team_pos == -1:
            server_data_str = self.data_partition
            self.parse_server_data(server_data_str)

        else:
            raise QRException("HeartBeat request is invalid.")

        if "groupid" in self.server_data:
            group_id = 0
            if not int(self.server_data["groupid"], group_id):
                raise QRException("GroupId is invalid.")
            self.group_id = group_id

    def parse_server_data(self, server_data_str: str):
        self.server_data = {}
        key_value_array = server_data_str.split("\0")

        for i in range(0, len(key_value_array), 2):
            if i + 2 > len(key_value_array):
                break

            temp_key = key_value_array[i]
            temp_value = key_value_array[i + 1]

            if temp_key == "":
                LogWriter.debug("Skipping empty key value")
                continue

            if temp_key in self.server_data:
                self.server_data[temp_key] = temp_value
            else:
                self.server_data[temp_key] = temp_value

        if "statechanged" not in self.server_data:
            self.server_status = GameServerStatus.NORMAL
        else:
            self.server_status = GameServerStatus[self.server_data["statechanged"]]

    def parse_player_data(self, player_data_str: str):
        self.player_data = []
        player_count = int(player_data_str[0])
        player_data_str = player_data_str[1:]

        index_of_key = player_data_str.index("\0\0", 0)
        key_str = player_data_str[:index_of_key]
        keys = key_str.split("\0")

        values_str = player_data_str[index_of_key + 2 :]
        values = values_str.split("\0")

        for player_index in range(player_count):
            key_value = {}

            for key_index in range(len(keys)):
                temp_key = keys[key_index] + str(player_index)
                temp_value = values[player_index * len(keys) + key_index]

                if temp_key in key_value:
                    key_value[temp_key] = temp_value
                else:
                    key_value[temp_key] = temp_value

            self.player_data.append(key_value)

    def parse_team_data(self, team_data_str: str):
        self.team_data = []
        team_count = int(team_data_str[0])
        team_data_str = team_data_str[1:]

        end_key_index = team_data_str.index("\0\0", 0)
        key_str = team_data_str[:end_key_index]
        keys = key_str.split("\0")

        value_str = team_data_str[end_key_index + 2 :]
        values = value_str.split("\0")

        for team_index in range(team_count):
            key_value = {}

            for key_index in range(len(keys)):
                temp_key = keys[key_index] + str(team_index)
                temp_value = values[team_index * len(keys) + key_index]

                if temp_key in key_value:
                    key_value[temp_key] = temp_value
                else:
                    key_value[temp_key] = temp_value

            self.team_data.append(key_value)


class EchoRequest(RequestBase):
    pass


class KeepAliveRequest(RequestBase):
    pass
