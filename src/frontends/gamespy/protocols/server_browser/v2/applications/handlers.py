from concurrent.futures import ProcessPoolExecutor
from typing import TYPE_CHECKING, cast
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    GameServerInfo,
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
    ServerFullInfoListResponse,
    UpdateServerInfoResponse,
)
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    ServerFullInfoListResult,
    UpdateServerInfoResult,
    ServerMainListResult,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    # RequestType,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.abstractions.handlers import (
    CmdHandlerBase,
    ServerListUpdateOptionHandlerBase,
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
    _result: UpdateServerInfoResult

    def __init__(self, message: GameServerInfo) -> None:
        self._log_current_class()
        self._message = message

    def handle(self) -> None:
        result = UpdateServerInfoResult(game_server_info=self._message)
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
            client.log_info(
                f"Sending AdHoc message {self._message.status} to client")
            client.send(self.response)


class SendMessageHandler(CmdHandlerBase):
    _request: SendMessageRequest

    def __init__(self, client: Client, request: SendMessageRequest) -> None:
        assert isinstance(request, SendMessageRequest)
        super().__init__(client, request)
        # we just need send the message to backend, then backend will send to queryreport frontend, query report frontend will handle for us


class UpdateServerInfoHandler(CmdHandlerBase):
    _request: ServerInfoRequest
    _result: UpdateServerInfoResult
    _response: UpdateServerInfoResponse

    def __init__(self, client: Client, request: ServerInfoRequest) -> None:
        assert isinstance(request, ServerInfoRequest)
        super().__init__(client, request)


class ServerMainListHandler(ServerListUpdateOptionHandlerBase):
    _request: ServerListRequest
    _result: ServerMainListResult
    _response: ServerMainListResponse

    def __init__(self, client: Client, request: ServerListRequest) -> None:
        assert isinstance(request, ServerListRequest)
        super().__init__(client, request)


class P2PGroupRoomListHandler(ServerListUpdateOptionHandlerBase):
    _request: ServerListRequest
    _result: P2PGroupRoomListResult
    _response: P2PGroupRoomListResponse

    def __init__(self, client: Client, request: ServerListRequest) -> None:
        assert isinstance(request, ServerListRequest)
        super().__init__(client, request)


class ServerFullInfoListHandler(ServerListUpdateOptionHandlerBase):
    """
    In sbctest.c 
    line 392 ServerBrowserAuxUpdateServer(sb, server, async, fullUpdate);
    will get the full info of a server such as: player data, server data, team data
    """
    """
    !! below is the source code of sb v2, adhocdata is directly follow the mainlist response
    todo check if adhoc data is append after mainlist response
    if (slist->state == sl_mainlist)
		err = ProcessMainListData(slist);
	if (err != sbe_noerror)
		return err;
	//always need to check this after mainlistdata, in case some extra data has some in (e.g. key list for push)
	if (slist->state == sl_connected && slist->inbufferlen > 0)	
		return ProcessAdHocData(slist);
    """
    _request: ServerListRequest
    _result: ServerFullInfoListResult
    _response: ServerFullInfoListResponse

    def __init__(self, client: Client, request: ServerListRequest) -> None:
        assert isinstance(request, ServerListRequest)
        super().__init__(client, request)
