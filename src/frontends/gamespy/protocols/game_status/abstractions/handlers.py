
from frontends.gamespy.library.abstractions.contracts import ResponseBase
import frontends.gamespy.library.abstractions.handler as lib
from frontends.gamespy.protocols.game_status.abstractions.contracts import RequestBase, ResultBase
from frontends.gamespy.protocols.game_status.applications.client import Client


class CmdHandlerBase(lib.CmdHandlerBase):
    _client: Client
    _request: RequestBase
    _result: ResultBase
    _response: ResponseBase | None

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)
