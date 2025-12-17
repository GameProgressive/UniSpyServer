import frontends.gamespy.library.abstractions.handler as lib
from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.query_report.applications.client import Client


class CmdHandlerBase(lib.CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)
