from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, GetMyRecordsRequest, SearchForRecordsRequest
from frontends.gamespy.protocols.web_services.modules.sake.contracts.responses import CreateRecordResponse, GetMyRecordResponse, SearchForRecordsResponse
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import CreateRecordResult, GetMyRecordsResult, SearchForRecordsResult


class CreateRecordHandler(CmdHandlerBase):
    _request: CreateRecordRequest
    _result: CreateRecordResult
    _response: CreateRecordResponse

    def __init__(self, client: Client, request: CreateRecordRequest) -> None:
        assert isinstance(request, CreateRecordRequest)
        super().__init__(client, request)


class GetMyRecordsHandler(CmdHandlerBase):
    _request: GetMyRecordsRequest
    _result: GetMyRecordsResult
    _response: GetMyRecordResponse

    def __init__(self, client: Client, request: GetMyRecordsRequest) -> None:
        assert isinstance(request, GetMyRecordsRequest)
        super().__init__(client, request)


class SearchForRecordsHandler(CmdHandlerBase):
    _request: SearchForRecordsRequest
    _result: SearchForRecordsResult
    _response: SearchForRecordsResponse

    def __init__(self, client: Client, request: SearchForRecordsRequest) -> None:
        assert isinstance(request, SearchForRecordsRequest)
        super().__init__(client, request)
