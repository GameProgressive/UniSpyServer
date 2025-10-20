from datetime import datetime
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.contracts import RequestBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import ConnectStatus, ConnectionListener
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import MessageRelayRequest
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
        match self._client.info.status:
            case ConnectStatus.WAITING_FOR_ANOTHER:
                self.__waiting_for_another()
            case ConnectStatus.CONNECTING:
                if self._client.info.ping_recv_times >= 7:
                    self._client.log_info(
                        "Negotiation is finished, ignore ping packet")
                    return
                self.__connecting()

    def __waiting_for_another(self):
        is_exist = ConnectionListener.is_client_exist(
            self._request.cookie, self._client
        )
        if not is_exist:
            ConnectionListener.add_client(
                self._request.cookie, self._client)
            self._client.info.cookie = self._request.cookie
            self._client.log_info(
                f"Add client to listener cookie:{self._request.cookie}"
            )
        else:
            assert self._client.info.cookie is not None
            is_both_client_ready = ConnectionListener.is_both_client_ready(
                self._client.info.cookie)
            if is_both_client_ready:
                self._client.info.status = ConnectStatus.CONNECTING

    def __connecting(self):
        assert self._client.info.cookie is not None
        handler = MessageRelayHandler(
            self._client, MessageRelayRequest(self._request.raw_request))
        handler.handle()
        self._client.info.ping_recv_times += 1
        
        if ConnectionListener.is_both_client_ready(self._client.info.cookie):
            if self._client.info.ping_recv_times >= 7:
                self._client.info.status = ConnectStatus.FINISHED


class MessageRelayHandler(CmdHandlerBase):
    _request: MessageRelayRequest
    _client: Client

    def __init__(self, client: ClientBase, request: RequestBase) -> None:
        super().__init__(client, request)
        self._is_fetching = False
        self._is_uploading = False

    def _data_operate(self) -> None:
        """
        when we receive udp message, we check whether the client pair is ready.
        if client is ready we send the following data to the another client
        """
        self._client.info.last_receive_time = datetime.now()
        assert self._client.info.cookie is not None
        another_client = ConnectionListener.get_another_client(
            self._client.info.cookie, self._client)
        another_client.connection.send(self._request.raw_request)
        self._client.log_network_sending(
            f"=> [{another_client.connection.ip_endpoint}] {self._request.raw_request}"
        )
