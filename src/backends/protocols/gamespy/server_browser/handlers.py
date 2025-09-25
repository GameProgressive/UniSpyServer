from typing import TYPE_CHECKING, cast
from backends.library.abstractions.handler_base import HandlerBase
import backends.protocols.gamespy.query_report.data as data
from backends.protocols.gamespy.server_browser.requests import (
    AdHocRequestBase,
    SendMessageRequest,
    ServerInfoRequest,
    ServerListRequest,
)
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import (
    PeerRoomInfo,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    GameServerFlags,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    SendMessageResult,
    ServerInfoResult,
    ServerMainListResult,
    ServerNetworkInfoListResult,
)


# region Server list
class ServerNetworkInfoListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[GameServerInfo]

    def _data_operate(self):
        self._caches = data.get_server_info_list_with_game_name(
            self._request.game_name, self._session
        )

    def _result_construct(self):
        assert isinstance(self._caches, list) and all(
            isinstance(item, GameServerInfo) for item in self._caches
        )
        if TYPE_CHECKING:
            self._caches = cast(list[GameServerInfo], self._caches)
        self._result = ServerNetworkInfoListResult(
            client_remote_ip=self._request.client_ip,
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key="",
            servers_info=self._caches,
            keys=self._request.keys
        )


class P2PGroupRoomListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[PeerRoomInfo]

    def _data_operate(self):
        group_data = data.get_group_data_list_by_gamename(
            self._request.game_name)
        self._caches = data.get_peer_group_channel(group_data, self._session)

    def _result_construct(self) -> None:
        assert isinstance(self._caches, list) and all(
            isinstance(item, PeerRoomInfo) for item in self._caches
        )
        if TYPE_CHECKING:
            self._caches = cast(list[PeerRoomInfo], self._caches)
        self._result = P2PGroupRoomListResult(
            client_remote_ip=self._request.client_ip,
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key="",
            peer_room_info=self._caches,
            keys=self._request.keys
        )


class ServerMainListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[GameServerInfo]
    _secret_key: str

    def _data_operate(self):
        self._secret_key = data.get_secret_key(
            self._request.game_name, self._session)
        self._caches = data.get_server_info_list_with_game_name(
            self._request.game_name, self._session
        )

    def _result_construct(self):
        assert isinstance(self._caches, list) and all(
            isinstance(item, GameServerInfo) for item in self._caches
        )
        if TYPE_CHECKING:
            self._caches = cast(list[GameServerInfo], self._caches)
        self._result = ServerMainListResult(
            client_remote_ip=self._request.client_ip,
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key=self._secret_key,
            servers_info=self._caches,
            keys=self._request.keys
        )


# region Adhoc


class AdHocHandler(HandlerBase):
    _request: AdHocRequestBase

    def _data_operate(self):
        raise NotImplementedError()


class SendMessageHandler(HandlerBase):
    _request: SendMessageRequest

    def _data_operate(self):
        self._data = data.get_server_info_with_ip_and_port(
            self._request.game_server_public_ip,
            self._request.game_server_public_port,
            self._session,
        )

    def _result_construct(self):
        if self._data is None:
            return
        self._result = SendMessageResult(
            sb_sender_id=self._request.server_id,
            natneg_message=self._request.client_message,
            server_info=self._data,
        )


class ServerInfoHandler(HandlerBase):
    _request: ServerInfoRequest

    def _data_operate(self) -> None:
        self._data = data.get_server_info_with_ip_and_port(
            self._request.game_server_public_ip,
            self._request.game_server_public_port,
            self._session,
        )

    def _result_construct(self) -> None:
        # TODO: check whether we need respond when gameserver not exist
        if self._data is None:
            return
        self._result = ServerInfoResult(game_server_info=self._data)
