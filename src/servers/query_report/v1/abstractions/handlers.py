import library.abstractions.handler
from servers.query_report.v1.abstractions.contracts import RequestBase
from servers.query_report.applications.client import Client


class CmdHandlerBase(library.abstractions.handler.CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)
