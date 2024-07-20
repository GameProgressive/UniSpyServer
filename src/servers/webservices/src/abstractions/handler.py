import library.src.abstractions.handler as lib
from servers.webservices.src.applications.client import Client
from servers.webservices.src.abstractions.contracts import RequestBase


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client

    def __init__(self, client: Client, request: RequestBase) -> None:
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
        super().__init__(client, request)
