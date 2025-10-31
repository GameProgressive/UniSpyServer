# region Response
from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.natneg.contracts.results import ConnectResult


class ConnectResponse(DataResponse):
    result: ConnectResult
