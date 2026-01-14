from frontends.gamespy.library.abstractions.handler import CmdHandlerBase as CHB
from frontends.gamespy.protocols.presence_search_player.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.presence_search_player.applications.client import Client
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import EXCEPTIONS, GPException


class CmdHandlerBase(CHB):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        self._exceptions_mapping = EXCEPTIONS
        super().__init__(client, request)

    def _handle_exception(self, ex) -> None:
        if ex is GPException:
            self._client.send(ex)
        super()._handle_exception(ex)
