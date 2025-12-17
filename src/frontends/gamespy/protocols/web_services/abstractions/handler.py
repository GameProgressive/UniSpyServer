import frontends.gamespy.library.abstractions.handler as lib
from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client

    def __init__(self, client: Client, request: RequestBase) -> None:
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
        super().__init__(client, request)
