import abc
from servers.natneg.src.applications.client import Client
import library.src.abstractions.handler
from servers.natneg.src.abstractions.contracts import RequestBase


class CmdHandlerBase(library.src.abstractions.handler.CmdHandlerBase, abc.ABC):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(request, RequestBase)


if __name__ == "__main__":
    cmd = CmdHandlerBase(None, None)
    pass
