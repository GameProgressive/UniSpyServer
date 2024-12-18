from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.server_browser.requests import *
import backends.protocols.gamespy.query_report.data as data
from servers.query_report.src.aggregates.game_server_info import GameServerInfo
from servers.query_report.src.aggregates.peer_room_info import PeerRoomInfo
from servers.server_browser.src.aggregates.exceptions import ServerBrowserException
from servers.server_browser.src.v2.aggregations.enums import GameServerFlags, ServerListUpdateOption
from servers.server_browser.src.v2.contracts.results import P2PGroupRoomListResult, SendMessageResult, ServerInfoResult, ServerMainListResult

# region Server list


class ServerListHandler(HandlerBase):
    _request: ServerListRequest

    async def _data_operate(self):
        if self._request.update_option in\
            [ServerListUpdateOption.SERVER_MAIN_LIST,
             ServerListUpdateOption.P2P_SERVER_MAIN_LIST,
             ServerListUpdateOption.LIMIT_RESULT_COUNT,
             ServerListUpdateOption.SERVER_FULL_INFO_LIST,]:

            self.data = data.get_server_info_list_with_game_name(
                self._request.game_name)

        elif self._request.update_option == ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
            self.data = data.get_peer_group_channel
        else:
            raise ServerBrowserException(
                "invalid server browser update option")

    async def _result_construct(self):
        if self._request.update_option in\
            [ServerListUpdateOption.SERVER_MAIN_LIST,
             ServerListUpdateOption.P2P_SERVER_MAIN_LIST,
             ServerListUpdateOption.LIMIT_RESULT_COUNT,
             ServerListUpdateOption.SERVER_FULL_INFO_LIST,]:
            assert isinstance(self.data, GameServerInfo)
            self._result = ServerMainListResult(client_remote_ip=self._request.client_ip,
                                                flag=GameServerFlags.HAS_KEYS_FLAG, game_secret_key="", servers_info=self.data)
        elif self._request.update_option == ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
            assert isinstance(self.data, PeerRoomInfo)
            self._result = P2PGroupRoomListResult(
                client_remote_ip=self._request.client_ip, flag=GameServerFlags.HAS_KEYS_FLAG, game_secret_key="", peer_room_info=self.data)
        else:
            raise ServerBrowserException(
                "invalid server browser update option")

# region Adhoc


class AdHocHandler(HandlerBase):
    _request: AdHocRequestBase

    async def _data_operate(self):
        raise NotImplementedError()


class SendMessageHandler(HandlerBase):
    _request: SendMessageRequest

    async def _data_operate(self):
        self.data = data.get_server_info_with_ip_and_port(
            self._request.game_server_public_ip, self._request.game_server_public_port)

    async def _result_construct(self):
        self._result = SendMessageResult(sb_sender_id=self._request.server_id,
                                         natneg_message=self._request.client_message, server_info=self.data)


class ServerInfoHandler(HandlerBase):
    _request: ServerInfoRequest

    async def _data_operate(self) -> None:
        self.data = data.get_server_info_with_ip_and_port(
            self._request.game_server_public_ip, self._request.game_server_public_port)

    async def _result_construct(self) -> None:
        self._result = ServerInfoResult(game_server_info=self.data)
