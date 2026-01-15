from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.server_browser.v2.contracts.results import ServerFullInfoListResult
from frontends.gamespy.protocols.server_browser.v2.contracts.results import (
    P2PGroupRoomListResult,
    SendMessageResult,
    UpdateServerInfoResult,
    ServerMainListResult,
    ServerFullInfoListResult,
)

# region v1




# region v2


class ServerFullInfoListResponse(DataResponse):
    result: ServerFullInfoListResult


class P2PGroupRoomListResponse(DataResponse):
    result: P2PGroupRoomListResult


class SendMessageResponse(DataResponse):
    result: SendMessageResult


class ServerInfoResponse(DataResponse):
    result: UpdateServerInfoResult


class ServerMainListResponse(DataResponse):
    result: ServerMainListResult
