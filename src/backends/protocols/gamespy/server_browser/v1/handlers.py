
# region V1

from backends.protocols.gamespy.server_browser.v1.requests import ServerListRequest
from backends.protocols.gamespy.server_browser.v1.responses import GroupListResponse, ServerInfoResponse, ServerListCompressResponse
from frontends.gamespy.protocols.server_browser.v1.contracts.results import GroupListResult, ServerInfoResult, ServerListCompressResult
from backends.library.abstractions.handler_base import HandlerBase
import backends.protocols.gamespy.query_report.data as data


# todo implement filter
class ServerInfoHandler(HandlerBase):
    _request: ServerListRequest
    _result: ServerInfoResult
    response: ServerInfoResponse

    def _data_operate(self) -> None:
        self._caches = data.get_server_info_list_with_game_name(
            self._request.game_name, self._session
        )

    def _result_construct(self) -> None:
        self._result = ServerInfoResult(
            servers=self._caches
        )


class ServerListCompressHandler(HandlerBase):
    _request: ServerListRequest
    _result: ServerListCompressResult
    response: ServerListCompressResponse

    def _data_operate(self) -> None:
        self._caches = data.get_server_info_list_with_game_name(
            self._request.game_name, self._session
        )

    def _result_construct(self) -> None:
        self._result = ServerListCompressResult(
            servers=self._caches
        )


class GroupListHandler(HandlerBase):
    _request: ServerListRequest
    _result: GroupListResult
    response: GroupListResponse

    def _data_operate(self) -> None:
        self._caches = data.get_peer_group_channel(
            self._request.game_name, self._session)

    def _result_construct(self) -> None:
        self._result = GroupListResult(
            peer_rooms=self._caches
        )
