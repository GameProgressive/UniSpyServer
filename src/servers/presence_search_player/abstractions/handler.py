from library.abstractions.handler import CmdHandlerBase as CHB
from servers.presence_search_player.abstractions.contracts import RequestBase
from servers.presence_search_player.applications.client import Client
from servers.presence_search_player.exceptions.general import GPException


class CmdHandlerBase(CHB):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)

    def _handle_exception(self, ex) -> None:
        if ex is GPException:
            self._client.send(ex)
        super()._handle_exception(ex)
