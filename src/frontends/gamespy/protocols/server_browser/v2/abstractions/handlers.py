from frontends.gamespy.library.abstractions.handler import CmdHandlerBase as CMB

from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
    ServerListUpdateOptionRequestBase,
    ServerListUpdateOptionResponseBase,
    ServerListUpdateOptionResultBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.encryption import EnctypeX
from frontends.gamespy.protocols.server_browser.v2.applications.client import Client
from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
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
        # query game secret key
        super()._data_operate()
        self._client.info.client_challenge = self._request.client_challenge
        self._client.info.game_secret_key = self._result.game_secret_key
        # use secret key to construct _client.crypto
        self._client.crypto = EnctypeX(
            self._client.info.game_secret_key, self._client.info.client_challenge
        )

