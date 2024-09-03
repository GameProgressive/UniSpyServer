from library.src.abstractions.client import ClientBase
from servers.chat.src.abstractions.contract import RequestBase
from servers.chat.src.applications.client import Client
from servers.chat.src.exceptions.general import IRCException
import library.src.abstractions.handler


class CmdHandlerBase(library.src.abstractions.handler.CmdHandlerBase):
    _request: RequestBase
    _client: Client

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        assert issubclass(request, RequestBase)

    def _handle_exception(self, ex) -> None:
        t_ex = type(ex)
        if t_ex is IRCException:
            self._client.send()

        super()._handle_exception(ex)


class PostLoginHandlerBase(CmdHandlerBase):
    pass
