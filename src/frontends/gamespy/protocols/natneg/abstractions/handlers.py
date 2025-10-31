from frontends.gamespy.protocols.natneg.applications.client import Client
from frontends.gamespy.protocols.natneg.abstractions.contracts import RequestBase, ResponseBase, ResultBase
import frontends.gamespy.library.abstractions.handler as lib


class CmdHandlerBase(lib.CmdHandlerBase):
    _request: RequestBase
    _result: ResultBase
    _response: ResponseBase

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)


if __name__ == "__main__":
    # cmd = CmdHandlerBase(None, None)
    pass
