import library.src.abstractions.handler
from servers.query_report.src.v1.abstractions.contracts import RequestBase
from servers.query_report.src.applications.client import Client


class CmdHandlerBase(library.src.abstractions.handler.CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)
