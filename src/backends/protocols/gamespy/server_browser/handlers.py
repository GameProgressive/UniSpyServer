from typing import TYPE_CHECKING, cast
from uuid import UUID
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.query_report.broker import BROCKER, MANAGER
import backends.protocols.gamespy.query_report.data as data
from backends.protocols.gamespy.query_report.handlers import ClientMessageHandler
from backends.protocols.gamespy.query_report.requests import ClientMessageRequest
from backends.protocols.gamespy.server_browser.requests import (
    AdHocRequestBase,
    SendMessageRequest,
    ServerInfoRequest,
    ServerListRequest,
)
from backends.protocols.gamespy.server_browser.responses import P2PGroupRoomListResponse, SendMessageResponse, ServerFullInfoListResponse, ServerInfoResponse
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.aggregates.peer_room_info import (
    PeerRoomInfo,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import RequestType
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    GameServerFlags,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import SBException
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    SendMessageResult,
    UpdateServerInfoResult,
    ServerMainListResult,
    ServerFullInfoListResult,
)
from backends.protocols.gamespy.server_browser.responses import ServerInfoResponse, ServerMainListResponse


# region Server list
class P2PGroupRoomListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[PeerRoomInfo]
    response: P2PGroupRoomListResponse

    def __init__(self, request: ServerListRequest) -> None:
        assert isinstance(request, ServerListRequest)
        super().__init__(request)

    def _data_operate(self):
        self._secret_key = data.get_secret_key(
            self._request.game_name, self._session)
        self._caches = data.get_peer_group_channel(
            self._request.game_name, self._session)

    def _result_construct(self) -> None:
        assert isinstance(self._caches, list) and all(
            isinstance(item, PeerRoomInfo) for item in self._caches
        )

        self._result = P2PGroupRoomListResult(
            client_remote_ip=self._request.client_ip,
            flag=GameServerFlags.HAS_KEYS_FLAG,
            game_secret_key=self._secret_key,
            peer_room_info=self._caches,
            keys=self._request.keys
        )


class ServerMainListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[GameServerInfo]
    _secret_key: str
    response: ServerMainListResponse

    def _data_operate(self):
        self._secret_key = data.get_secret_key(
            self._request.game_name, self._session)
        self._caches = data.get_server_info_list_with_game_name(
            self._request.game_name, self._session
        )
        # we just need server data
        for cache in self._caches:
            cache.player_data = []
            cache.team_data = []

    def _result_construct(self):
        assert isinstance(self._caches, list) and all(
            isinstance(item, GameServerInfo) for item in self._caches
        )

        if TYPE_CHECKING:
            self._caches = cast(list[GameServerInfo], self._caches)
        self._result = ServerMainListResult(
            client_remote_ip=self._request.client_ip,
            game_secret_key=self._secret_key,
            servers_info=self._caches,
            keys=self._request.keys
        )


class ServerFullInfoListHandler(HandlerBase):
    _request: ServerListRequest
    _caches: list[GameServerInfo]
    response: ServerFullInfoListResponse

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
        all_keys = list(self._caches[0].server_data.keys())
        self._result = ServerFullInfoListResult(
            client_remote_ip=self._request.client_ip,
            game_secret_key=self._secret_key,
            servers_info=self._caches,
            keys=all_keys
        )

# region Adhoc


class AdHocHandler(HandlerBase):
    _request: AdHocRequestBase

    def _data_operate(self):
        raise NotImplementedError()


class SendMessageHandler(HandlerBase):
    """
    client -> server browser -> backend -> sb-SendMessageHandler -> qr-ClientMessageHandler -> websocket -> query report -> client
    """
    _request: SendMessageRequest
    response: SendMessageResponse

    def _data_operate(self):
        # construct client message and invoke frontends qr server client message
        cache = data.get_game_server_cache_by_ip_port(
            self._request.game_server_public_ip, self._request.game_server_public_port, self._session)
        if cache is None:
            raise SBException(
                f"could not find game server: {self._request.game_server_public_ip}:{self._request.game_server_public_port}")
        assert isinstance(cache.instant_key, str)
        assert isinstance(cache.server_id, UUID)
        request = ClientMessageRequest(
            server_id=self._request.server_id,
            server_browser_sender_id=self._request.server_id,
            target_query_report_id=cache.server_id,
            raw_request=self._request.raw_request,
            client_ip=self._request.client_ip,
            client_port=self._request.client_port,
            target_ip_address=self._request.game_server_public_ip,
            target_port=self._request.game_server_public_port,
            natneg_message=self._request.client_message,
            instant_key=cache.instant_key,
        )
        BROCKER.publish_message(request.model_dump_json())


class ServerInfoHandler(HandlerBase):
    _request: ServerInfoRequest
    response: ServerInfoResponse

    def __init__(self, request: ServerInfoRequest) -> None:
        assert isinstance(request, ServerInfoRequest)
        super().__init__(request)

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
        self._result = UpdateServerInfoResult(game_server_info=self._data)
