from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.applications.client import Client
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import CreateRecordRequest, DeleteRecordRequest, GetMyRecordsRequest, GetRecordLimitRequest, GetSpecificRecordsRequest, RateRecordRequest, SearchForRecordsRequest
from frontends.gamespy.protocols.web_services.modules.sake.contracts.responses import CreateRecordResponse, GetMyRecordResponse, SearchForRecordsResponse
from frontends.gamespy.protocols.web_services.modules.sake.contracts.results import CreateRecordResult, GetMyRecordsResult, SearchForRecordsResult

# General


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


class GetSpecificRecordsHansler(CmdHandlerBase):
    _request: GetSpecificRecordsRequest


class DeleteRecordHandler(CmdHandlerBase):
    _request: DeleteRecordRequest


class RateRecordHandler(CmdHandlerBase):
    _request: RateRecordRequest


class GetRecordLimitHandler(CmdHandlerBase):
    _request: GetRecordLimitRequest

# region CloudFile


class FileUploadHandler(CmdHandlerBase):
    """
    headers{"Sake-File-Result":"0","Sake-File-Id":"int"}
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()


class FileDownloadHandler(CmdHandlerBase):
    """
    {"Sake-File-Result":"0"}
    body: file bytes
    """

    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        raise NotImplementedError()
