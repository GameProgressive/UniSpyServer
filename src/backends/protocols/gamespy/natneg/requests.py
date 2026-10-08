import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.natneg.contracts.requests as fnt
# region Requests


class ConnectRequest(lib.RequestBase):
    """
    Server will send this request to client to let them connect to each other
    """

    data: fnt.ConnectRequest


class InitRequest(lib.RequestBase):
    data: fnt.InitRequest


class ReportRequest(lib.RequestBase):
    data: fnt.ReportRequest
