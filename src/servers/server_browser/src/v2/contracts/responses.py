from library.src.extentions.encoding import get_bytes
from servers.server_browser.src.v2.abstractions.contracts import (
    AdHocResponseBase,
    ServerListUpdateOptionResponseBase,
)
from servers.server_browser.src.v2.aggregations.server_info_builder import (
    build_server_info_header,
)
from servers.server_browser.src.v2.aggregations.string_flags import *
from servers.server_browser.src.v2.contracts.requests import ServerListRequest
from servers.server_browser.src.v2.contracts.results import (
    P2PGroupRoomListResult,
    ServerInfoResult,
    ServerMainListResult,
)

from servers.server_browser.src.v2.aggregations.enums import GameServerFlags, ResponseType


class DeleteServerInfoResponse(AdHocResponseBase):
    _result: ServerInfoResult

    def __init__(self, result: ServerInfoResult) -> None:
        assert isinstance(result, ServerInfoResult)
        self._result = result

    def build(self):
        buffer = bytearray()
        buffer.append(ResponseType.DELETE_SERVER_MESSAGE)
        buffer.extend(self._result.game_server_info.host_ip_address_bytes)
        self.sending_buffer = bytes(buffer)


class UpdateServerInfoResponse(AdHocResponseBase):
    def __init__(self, result: ServerInfoResult) -> None:
        assert isinstance(result, ServerInfoResult)
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
            buffer.extend(STRING_SPLITER)
            buffer.extend(get_bytes(v))
            buffer.extend(STRING_SPLITER)
        return buffer


class P2PGroupRoomListResponse(ServerListUpdateOptionResponseBase):
    _request: ServerListRequest
    _result: P2PGroupRoomListResult

    def build(self) -> None:
        super().build()
        self.build_server_keys()
        self.build_unique_value()
        self._build_servers_full_info()
        self.sending_buffer = bytes(self._servers_info_buffers)

    def _build_servers_full_info(self):
        for room in self._result.peer_room_info:
            self._servers_info_buffers.append(GameServerFlags.HAS_KEYS_FLAG)
            group_id_bytes = room.group_id.to_bytes()
            self._servers_info_buffers.extend(group_id_bytes)
            # get gamespy format dict
            gamespy_dict = room.get_gamespy_dict()
            for key in self._request.keys:
                self._servers_info_buffers.extend(NTS_STRING_FLAG)
                value = (
                    gamespy_dict[key]
                    if key in gamespy_dict.keys()
                    else ""
                )
                self._servers_info_buffers.extend(get_bytes(value))
                self._servers_info_buffers.extend(STRING_SPLITER)
        end_flag = b"\x00"
        self._servers_info_buffers.extend(end_flag)


class ServerMainListResponse(ServerListUpdateOptionResponseBase):
    _request: ServerListRequest
    _result: ServerMainListResult

    def __build_servers_full_info(self):
        for info in self._result.servers_info:
            header = build_server_info_header(self._result.flag, info)
            self._servers_info_buffers.extend(header)
            for key in self._request.keys:
                self._servers_info_buffers.extend(NTS_STRING_FLAG)
                if key in info.server_data.keys():
                    self._servers_info_buffers.extend(
                        get_bytes(info.server_data[key]))
                self._servers_info_buffers.extend(STRING_SPLITER)

            self._servers_info_buffers.extend(ALL_SERVER_END_FLAG)

    def build(self) -> None:
        super().build()
        self.build_server_keys()
        self.build_unique_value()
        self.__build_servers_full_info()
        self.sending_buffer = bytes(self._servers_info_buffers)


class ServerNetworkInfoListResponse(ServerListUpdateOptionResponseBase):

    def build(self) -> None:
        super().build()
        self.sending_buffer = bytes(self._servers_info_buffers)
