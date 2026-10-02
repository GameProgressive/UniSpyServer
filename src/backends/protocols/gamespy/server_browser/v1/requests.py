import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.server_browser.v1.contracts.requests as fnt


class ServerListRequest(lib.RequestBase):
    data: fnt.ServerListRequest
