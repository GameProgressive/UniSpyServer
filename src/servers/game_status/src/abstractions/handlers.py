from library.src.abstractions.contracts import ResponseBase
import library.src.abstractions.handler
from servers.game_status.src.abstractions.contracts import RequestBase, ResultBase
from servers.game_status.src.applications.client import Client


class CmdHandlerBase(library.src.abstractions.handler.CmdHandlerBase):
    _client: Client
    _request: RequestBase
    _result: ResultBase
    _response: ResponseBase

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
