
from servers.presence_search_player.src.contracts.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest

from servers.presence_search_player.src.contracts.responses import CheckResponse, NewUserResponse, NicksResponse, OthersListResponse, OthersResponse, SearchResponse, SearchUniqueResponse, UniqueSearchResponse, ValidResponse
from servers.presence_search_player.src.contracts.results import CheckResult, NewUserResult, NicksResult, OthersListResult, OthersResult, SearchResult, SearchUniqueResult, UniqueSearchResult, ValidResult

from servers.presence_search_player.src.abstractions.handler import CmdHandlerBase
from servers.presence_search_player.src.applications.client import Client


class CheckHandler(CmdHandlerBase):
    _result_cls: type[CheckResult]
    _request: CheckRequest
    _result: CheckResult

    def __init__(self, client: Client, request: CheckRequest) -> None:
        assert isinstance(request, CheckRequest)
        self._result_cls = CheckResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = CheckResponse(self._request, self._result)


class NewUserHandler(CmdHandlerBase):
    _result_cls: type[NewUserResult]
    _request: NewUserRequest
    _result: NewUserResult

    def __init__(self, client: Client, request: NewUserRequest) -> None:
        assert isinstance(request, NewUserRequest)
        self._result_cls = NewUserResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NewUserResponse(self._request, self._result)


class NicksHandler(CmdHandlerBase):
    _result_cls: type[NicksResult]
    _request: NicksRequest
    _result: NicksResult

    def __init__(self, client: Client, request: NicksRequest) -> None:
        assert isinstance(request, NicksRequest)
        self._result_cls = NicksResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NicksResponse(self._request, self._result)


class OthersHandler(CmdHandlerBase):
    _request: OthersRequest
    _result_cls: type[OthersResult]
    _result: OthersResult

    def __init__(self, client: Client, request: OthersRequest) -> None:
        assert isinstance(request, OthersRequest)
        self._result_cls = OthersResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = OthersResponse(self._result)


class OthersListHandler(CmdHandlerBase):
    _result_cls: type[OthersListResult]
    _result: OthersListResult

    def __init__(self, client: Client, request: OthersListRequest) -> None:
        assert isinstance(request, OthersListRequest)
        self._result_cls = OthersListResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = OthersListResponse(self._result)


class SearchHandler(CmdHandlerBase):
    """
    last one we search with email this may get few profile so we can not return GPErrorCode
    SearchWithEmail(client,dict );
    \\search\\\\sesskey\\0\\profileid\\0\\namespaceid\\1\\partnerid\\0\\nick\\mycrysis\\uniquenick\\xiaojiuwo\\email\\koujiangheng@live.cn\\gamename\\gmtest\\final\\
    \\bsrdone\\more\\<more>\\final\\
    string sendingbuffer = 
    "\\\\bsr\\\\1\\\\nick\\\\mycrysis\\\\uniquenick\\\\1\\\\namespaceid\\\\0\\\\firstname\\\\jiangheng\\\\lastname\\\\kou\\\\email\\\\koujiangheng@live.cn\\\\bsrdone\\\\0\\\\final\\\\";
    client.Stream.SendAsync(sendingbuffer);
    \\more\\<number of items>\\final\\
    \\search\\sesskey\\0\\profileid\\0\\namespaceid\\0\\nick\\gbr359_jordips\\gamename\\gbrome\\final\\
    """
    _result_cls: type[SearchResult]
    _result: SearchResult

    def __init__(self, client: Client, request: SearchRequest) -> None:
        assert isinstance(request, SearchRequest)
        self._result_cls = SearchResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SearchResponse(self._result)


class SearchUniqueHandler(CmdHandlerBase):
    _result_cls: type[SearchUniqueResult]
    _result: SearchUniqueResult

    def __init__(self, client: Client, request: SearchUniqueRequest) -> None:
        assert isinstance(request, SearchUniqueRequest)
        self._result_cls = SearchUniqueResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SearchUniqueResponse(self._result)


class UniqueSearchHandler(CmdHandlerBase):
    _result_cls: type[UniqueSearchResult]
    _request: UniqueSearchRequest
    _result: UniqueSearchResult

    def __init__(self, client: Client, request: UniqueSearchRequest) -> None:
        assert isinstance(request, UniqueSearchRequest)
        self._result_cls = UniqueSearchResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = UniqueSearchResponse(self._request, self._result)


class ValidHandler(CmdHandlerBase):
    _result_cls: type[ValidResult]
    _result: ValidResult
    _request: ValidRequest

    def __init__(self, client: Client, request: ValidRequest) -> None:
        assert isinstance(request, ValidRequest)
        self._result_cls = ValidResult
        super().__init__(client, request)

    def _response_construct(self):
        self._response = ValidResponse(self._request, self._result)
