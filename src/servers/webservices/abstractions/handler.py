import library.abstractions.handler as lib
from servers.webservices.applications.client import Client
from servers.webservices.abstractions.contracts import RequestBase


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client

    def __init__(self, client: Client, request: RequestBase) -> None:
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
        super().__init__(client, request)
