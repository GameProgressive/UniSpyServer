from concurrent.futures import ProcessPoolExecutor
from typing import TYPE_CHECKING, cast
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
)
from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
    ClientMessageRequest,
)
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import (
    GameServerStatus,
    RequestType,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.requests import (
    SendMessageRequest,
    ServerInfoRequest,
    ServerListRequest,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.responses import (
    DeleteServerInfoResponse,
    P2PGroupRoomListResponse,
    ServerMainListResponse,
    ServerNetworkInfoListResponse,
    UpdateServerInfoResponse,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    ServerNetworkInfoListResult,
    ServerInfoResult,
    ServerMainListResult,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    # RequestType,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.handlers import (
    CmdHandlerBase,
)

from frontends.gamespy.protocols.server_browser.v2.applications.client import Client


def get_clients(game_name: str):
    client_list = []
    assert isinstance(game_name, str)
    for ip, client in Client.pool.items():
        if TYPE_CHECKING:
            client = cast(Client, client)
        if client.info.game_name == game_name:
            client_list.append(client)

    return client_list


class AdHocHandler(CmdHandlerBase):
    _message: GameServerInfo
    # !fix this
    _result: ServerInfoResult

    def __init__(self, message: GameServerInfo) -> None:
        self._log_current_class()
        self._message = message

    def handle(self) -> None:
        result = ServerInfoResult(game_server_info=self._message)
        match self._message.status:
            case status if (
                status == GameServerStatus.NORMAL
                or status == GameServerStatus.UPDATE
                or status == GameServerStatus.PLAYING
            ):
                self.response = UpdateServerInfoResponse(result)
            case GameServerStatus.SHUTDOWN:
                self.response = DeleteServerInfoResponse(result)
            case _:
                pass

        clients = get_clients(self._message.game_name)

        with ProcessPoolExecutor() as executor:
            executor.map(self.send_message, clients)

    def send_message(self, client: Client):
        if (
            client.info.game_name == self._message.game_name
            and client.crypto is not None
            and (
                client.info.search_type == ServerListUpdateOption.SERVER_MAIN_LIST
                or client.info.search_type
                == ServerListUpdateOption.P2P_SERVER_MAIN_LIST
            )
        ):
            client.log_info(f"Sending AdHoc message {self._message.status} to client")
            client.send(self.response)


class SendMessageHandler(CmdHandlerBase):
    _request: SendMessageRequest
    _result: GameServerInfo

    def __init__(self, client: Client, request: SendMessageRequest) -> None:
        assert isinstance(request, SendMessageRequest)
        # super().__init__(client, request)
        assert isinstance(client, Client)

    def _response_construct(self) -> None:
        message = ClientMessageRequest()
        message.server_browser_sender_id = self._client.server_config.server_id
        message.natneg_message = self._request.client_message
        message.instant_key = self._result.instant_key
        message.target_ip_address = self._result.host_ip_address
        message.target_port = self._result.query_report_port
        message.command_name = RequestType.CLIENT_MESSAGE

    def _response_send(self) -> None:
        """
        QueryReport.V2.Application.StorageOperation.Persistance.PublishClientMessage(message);
            _client.LogInfo($"Send client message to QueryReport Server: {gameServer.ServerID} [{StringExtensions.ConvertByteToHexString(message.NatNegMessage)}]");
        """
        raise NotImplementedError()


class ServerInfoHandler(CmdHandlerBase):
    _request: ServerInfoRequest
    _result: ServerInfoResult

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = ServerInfoResult

    def _response_construct(self) -> None:
        if self._result.game_server_info is not None:
            self._response = UpdateServerInfoResponse(self._result)


class ServerMainListHandler(CmdHandlerBase):
    _request: ServerListRequest
    _result: ServerMainListResult
    _result_cls: type[ServerMainListResult]

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = ServerMainListResult

    def _response_construct(self) -> None:
        self._response = ServerMainListResponse(self._result)


class P2PGroupRoomListHandler(CmdHandlerBase):
    _request: ServerListRequest
    _result: P2PGroupRoomListResult
    _result_cls: type[P2PGroupRoomListResult]

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = P2PGroupRoomListResult

    def _response_construct(self) -> None:
        self._response = P2PGroupRoomListResponse(self._result)


class ServerNetworkInfoListHandler(CmdHandlerBase):
    _request: ServerListRequest
    _result: ServerNetworkInfoListResult
    _result_cls: type[ServerNetworkInfoListResult]

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        self._result_cls = ServerNetworkInfoListResult

    def _response_construct(self) -> None:
        self._response = ServerNetworkInfoListResponse(self._result)


# class ServerListHandler(CmdHandlerBase):
#     _request: ServerListRequest
#     _result: ServerMainListResult
#     _result_cls: (
#         type[ServerMainListResult]
#         | type[P2PGroupRoomListResult]
#         | type[ServerInfoResult]
#     )

#     def _request_check(self) -> None:
#         super()._request_check()
#         match self._request.update_option:
#             case option if option in [
#                 ServerListUpdateOption.SERVER_MAIN_LIST,
#                 ServerListUpdateOption.P2P_SERVER_MAIN_LIST,
#                 ServerListUpdateOption.LIMIT_RESULT_COUNT,
#                 ServerListUpdateOption.SERVER_FULL_MAIN_LIST,
#             ]:
#                 self._result_cls = ServerMainListResult
#             case ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
#                 self._result_cls = P2PGroupRoomListResult
#             case _:
#                 raise ServerBrowserException("unknown serverlist update option type")

#     def _response_construct(self) -> None:
#         match self._request.update_option:
#             case option if option in [
#                 ServerListUpdateOption.SERVER_MAIN_LIST,
#                 ServerListUpdateOption.P2P_SERVER_MAIN_LIST,
#                 ServerListUpdateOption.LIMIT_RESULT_COUNT,
#                 ServerListUpdateOption.SERVER_FULL_MAIN_LIST,
#             ]:
#                 self._response = ServerMainListResponse(self._request, self._result)
#             case ServerListUpdateOption.P2P_GROUP_ROOM_LIST:
#                 self._response = P2PGroupRoomListResponse(self._request, self._result)
#             case ServerListUpdateOption.SERVER_FULL_MAIN_LIST:
#                 self._response = ServerNetworkInfoListResponse(
#                     self._request, self._result
#                 )
#             case _:
#                 raise ServerBrowserException("unknown serverlist update option type")
