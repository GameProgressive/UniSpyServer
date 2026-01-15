import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.server_browser.v1.aggregations.enums import Modifier

class RequestBase(lib.RequestBase):
    raw_request: str


class ServerListRequest(RequestBase):
    modifier: Modifier
    filter: str | None
    game_name: str

