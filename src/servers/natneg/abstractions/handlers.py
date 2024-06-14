import abc
from servers.natneg.applications.client import Client
import library.abstractions.handler
from servers.natneg.abstractions.contracts import RequestBase


class CmdHandlerBase(library.abstractions.handler.CmdHandlerBase, abc.ABC):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(request, RequestBase)


if __name__ == "__main__":
    cmd = CmdHandlerBase(None, None)
    pass
