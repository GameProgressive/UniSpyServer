from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.natneg.contracts.requests import PingRequest


class PingHandler(CmdHandlerBase):
    _request: PingRequest
    _client: Client

    def __init__(self, client: Client, request: PingRequest) -> None:
        assert isinstance(request, PingRequest)
        super().__init__(client, request)
        self._is_fetching = False
        self._is_uploading = False

    def _data_operate(self) -> None:
        if not self._client.listener.is_client_exist(
            self._request.cookie, self._client
        ):
            Client.listener.add_client(self._request.cookie, self._client)
            self._client.info.cookie = self._request.cookie
            self._client.log_info(
                f"Add client to listener cookie:{self._request.cookie}"
            )
