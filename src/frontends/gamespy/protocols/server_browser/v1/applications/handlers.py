from frontends.gamespy.protocols.server_browser.v1.abstractions.handlers import HandlerBase
from frontends.gamespy.protocols.server_browser.v1.contracts.requests import ServerListRequest
from frontends.gamespy.protocols.server_browser.v1.contracts.responses import GroupListResponse, ServerInfoResponse, ServerListCompressResponse
from frontends.gamespy.protocols.server_browser.v1.contracts.results import ServerInfoResult, ServerListCompressResult, GroupListResult


class ServerInfoHandler(HandlerBase):
    _request: ServerListRequest
    _result: ServerInfoResult
    _response: ServerInfoResponse


class ServerListCompressHandler(HandlerBase):
    _request: ServerListRequest
    _result: ServerListCompressResult
    _response: ServerListCompressResponse


class GroupListHandler(HandlerBase):
    _request: ServerListRequest
    _result: GroupListResult
    _response: GroupListResponse
