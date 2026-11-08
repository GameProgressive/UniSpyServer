from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    ClientMessage,
    ServerFullInfoListResult,
    ServerMainListResult,
    ServerInfoResult,
)

# region v1


# region v2


class ServerFullInfoListResponse(DataResponse):
    result: ServerFullInfoListResult


class P2PGroupRoomListResponse(DataResponse):
    result: P2PGroupRoomListResult


class ServerInfoResponse(DataResponse):
    result: ServerInfoResult


class ServerMainListResponse(DataResponse):
    result: ServerMainListResult
