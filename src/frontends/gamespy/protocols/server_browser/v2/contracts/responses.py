from frontends.gamespy.library.extentions.encoding import get_bytes
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import GameServerInfo
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import PeerRoomInfo
from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
    AdHocResponseBase,
    ServerListUpdateOptionResponseBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import SBException
from frontends.gamespy.protocols.server_browser.v2.aggregations.server_info_builder import (
    build_server_info_header,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.string_flags import ALL_SERVER_END_FLAG, NTS_STRING_FLAG, STRING_SPLITER, SINGLE_SERVER_END_FLAG
from frontends.gamespy.protocols.server_browser.v2.contracts.requests import ServerListRequest
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    ServerFullInfoListResult,
    UpdateServerInfoResult,
    ServerMainListResult,
)

from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import GameServerFlags, ResponseType


class DeleteServerInfoResponse(AdHocResponseBase):
    _result: UpdateServerInfoResult

    def __init__(self, result: UpdateServerInfoResult) -> None:
        assert isinstance(result, UpdateServerInfoResult)
        self._result = result

    def build(self):
        buffer = bytearray()
        buffer.append(ResponseType.DELETE_SERVER_MESSAGE)
        buffer.extend(self._result.game_server_info.host_ip_address_bytes)
        self.sending_buffer = bytes(buffer)


class UpdateServerInfoResponse(AdHocResponseBase):
    def __init__(self, result: UpdateServerInfoResult) -> None:
        assert isinstance(result, UpdateServerInfoResult)
        self._result = result
        self._buffer = bytearray()

    def build(self) -> None:
        self._buffer.append(ResponseType.PUSH_SERVER_MESSAGE)
        self.__build_single_server_full_info()
        msg_leng = bytearray(len(self._buffer).to_bytes(2, "big"))
        # make sure the msg_leng is at first 2 bytes
        self._buffer = msg_leng+self._buffer
        self.sending_buffer = bytes(self._buffer)

    def __build_single_server_full_info(self):
        header = build_server_info_header(
            GameServerFlags.HAS_FULL_RULES_FLAG, self._result.game_server_info
        )
        self._buffer.extend(header)
        if self._result.game_server_info.server_data is not None:
            server_data = UpdateServerInfoResponse._build_kv(
                self._result.game_server_info.server_data)
            self._buffer.extend(server_data)
        if self._result.game_server_info.player_data is not None:
            for pd in self._result.game_server_info.player_data:
                player_data = UpdateServerInfoResponse._build_kv(pd)
                self._buffer.extend(player_data)
        if self._result.game_server_info.team_data is not None:
            for td in self._result.game_server_info.team_data:
                team_data = UpdateServerInfoResponse._build_kv(td)
                self._buffer.extend(team_data)

    @staticmethod
    def _build_kv(data: dict) -> bytearray:
        buffer = bytearray()
        for k, v in data.items():
            buffer.extend(get_bytes(k))
            buffer.append(STRING_SPLITER)
            buffer.extend(get_bytes(v))
            buffer.append(STRING_SPLITER)
        return buffer


class P2PGroupRoomListResponse(ServerListUpdateOptionResponseBase):
    _result: P2PGroupRoomListResult

    def build(self) -> None:
        super().build()
        self.build_server_keys()
        self.build_unique_value()
        self._build_servers_full_info()
        self.sending_buffer = bytes(self._buffer)

    def _build_servers_full_info(self):
        # we have to add a server with group ip 0 to make peer sdk finish the sb process
        empty_info = PeerRoomInfo(game_name="", groupid=0, hostname="")
        self._result.peer_room_info.append(empty_info)
        for room in self._result.peer_room_info:
            self._buffer.append(GameServerFlags.HAS_KEYS_FLAG)
            group_id_bytes = room.group_id.to_bytes(length=4)
            self._buffer.extend(group_id_bytes)
            # get gamespy format dict
            gamespy_dict = room.get_gamespy_dict()
            for key in self._result.keys:
                self._buffer.append(NTS_STRING_FLAG)
                value = (
                    gamespy_dict[key]
                    if key in gamespy_dict.keys()
                    else ""
                )
                self._buffer.extend(get_bytes(value))
                self._buffer.append(STRING_SPLITER)
        # apped end flag
        end_flag = b"\x00\x00\x00\x00"
        self._buffer.extend(end_flag)


class ServerMainListResponse(ServerListUpdateOptionResponseBase):
    _result: ServerMainListResult

    def __add_key_value_to_buffer(self, value):
        self._buffer.append(NTS_STRING_FLAG)
        self._buffer.extend(get_bytes(value))
        self._buffer.append(STRING_SPLITER)

    def __check_key_existance(self):
        for info in self._result.servers_info:
            for key in self._result.keys:
                if key not in info.server_data:
                    raise SBException(
                        f"key:{key} is not in server info, please check database")

    def __build_servers_full_info(self):
        for info in self._result.servers_info:
            last_header = build_server_info_header(self._result.flag, info)
            self._buffer.extend(last_header)
            for key in self._result.keys:
                value = info.server_data[key]
                self.__add_key_value_to_buffer(value)

    def __build_tail(self):
        # after all server is added we add the tail
        self._buffer.extend(ALL_SERVER_END_FLAG)

    def build(self) -> None:
        super().build()
        self.build_server_keys()
        self.build_unique_value()
        self.__check_key_existance()
        self.__build_servers_full_info()
        self.__build_tail()
        self.sending_buffer = bytes(self._buffer)


class ServerFullInfoListResponse(ServerListUpdateOptionResponseBase):
    """
    currently we send all server info to game to make it pass HAS_FULL_RULES_FLAG
    todo check how this cmd is handled
    """
    _result: ServerFullInfoListResult

    def build(self) -> None:
        """
        adhoc data can contains multiple message,
        gamespy will read the response to the end
        """
        # cryptheader is nessesary for make the connection with server browser
        super().build()
        push_buffer = bytearray()
        for info in self._result.servers_info:
            result = UpdateServerInfoResult(game_server_info=info)
            response = UpdateServerInfoResponse(result)
            response.build()
            push_buffer.extend(response.sending_buffer)
        # we append the push server full info response (Adhoc response) to the crypt header
        self._buffer = self._buffer+push_buffer
        self.sending_buffer = bytes(self._buffer)
