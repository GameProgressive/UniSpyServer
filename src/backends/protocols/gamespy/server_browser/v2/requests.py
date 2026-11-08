import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.server_browser.v2.contracts.requests as fnt


class RequestBase(lib.RequestBase):
    pass


class ServerListRequest(RequestBase):
    data: fnt.ServerListRequest


class AdHocRequestBase(RequestBase):
    data: fnt.AdHocRequestBase


class ClientMessageRequest(AdHocRequestBase):
    data: fnt.ClientAdHocMessageRequest


class ServerInfoRequest(AdHocRequestBase):
    data: fnt.ServerInfoRequest
