from frontends.gamespy.library.abstractions.handler import CmdHandlerBase as CHB
from frontends.gamespy.protocols.query_report.v2.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.query_report.applications.client import Client


class CmdHandlerBase(CHB):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)
