from library.src.abstractions.client import ClientBase
from servers.chat.src.abstractions.contract import RequestBase, ResultBase
from servers.chat.src.applications.client import Client
from servers.chat.src.exceptions.general import IRCException
import library.src.abstractions.handler
from typing import cast


class CmdHandlerBase(library.src.abstractions.handler.CmdHandlerBase):
    _request: RequestBase
    _client: Client
    _result: ResultBase

    def __init__(self, client: ClientBase, request: RequestBase):
        super().__init__(client, request)
        assert issubclass(type(request), RequestBase)

    def _handle_exception(self, ex: Exception) -> None:
        t_ex = type(ex)
        if t_ex is IRCException:
            ex = cast(IRCException, ex)
            self._client.connection.send(ex.message.encode())
        super()._handle_exception(ex)


class PostLoginHandlerBase(CmdHandlerBase):
    pass
