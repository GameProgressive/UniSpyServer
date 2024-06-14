from library.abstractions.client import ClientBase
from servers.chat.abstractions.contract import RequestBase
from servers.chat.exceptions.general import IRCException
import library.abstractions.handler


class CmdHandlerBase(library.abstractions.handler.CmdHandlerBase):
    _request: RequestBase

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
