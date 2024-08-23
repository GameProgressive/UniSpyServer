from library.src.abstractions.handler import CmdHandlerBase as CMB
import abc

from servers.server_browser.src.v2.abstractions.contracts import (
    ServerListUpdateOptionRequestBase,
    ServerListUpdateOptionResponseBase,
    ServerListUpdateOptionResultBase,
)
from servers.server_browser.src.v2.aggregations.encryption import EnctypeX
from servers.server_browser.src.v2.applications.client import Client
from servers.server_browser.src.v2.abstractions.contracts import (
    RequestBase,
    ResultBase,
    ResponseBase,
)


class CmdHandlerBase(CMB):
    _client: Client
    _request: RequestBase
    _result: ResultBase
    _response: ResponseBase

    def __init__(self, client: Client, request: RequestBase) -> None:
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
        super().__init__(client, request)


class ServerListUpdateOptionHandlerBase(CmdHandlerBase):
    _request: ServerListUpdateOptionRequestBase
    _result: ServerListUpdateOptionResultBase
    _response: ServerListUpdateOptionResponseBase

    def _data_operate(self) -> None:
        # send to backend to query the game secret key
        secretkey = None
        if secretkey is None:
            raise NotImplementedError("not implemented")
        self._client.info.game_secret_key = secretkey
        # use secret key to construct _client.crypto
        self._client.crypto = EnctypeX(
            self._client.info.game_secret_key, self._client.info.client_challenge
        )
        pass

    def _response_send(self) -> None:
        self._response.build()
        head_buffer = self._response.sending_buffer[:14]
        body_buffer = self._response.sending_buffer[14:]
        encrypted_body_buffer = self._client.crypto.encrypt(body_buffer)
        buffer = head_buffer + encrypted_body_buffer
        self._client.log_network_sending(buffer)
        self._client.send(buffer)
