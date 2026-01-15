from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.server_browser.v1.contracts.results import GroupListResult, ServerInfoResult, ServerListCompressResult


class ServerInfoResponse(DataResponse):
    result: ServerInfoResult


class ServerListCompressResponse(DataResponse):
    result: ServerListCompressResult


class GroupListResponse(DataResponse):
    result: GroupListResult
