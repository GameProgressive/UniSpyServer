from servers.presence_connection_manager.src.abstractions.handler import CmdHandlerBase
from servers.presence_connection_manager.src.applications.client import Client
from servers.presence_connection_manager.src.contracts.responses.general import NewUserResponse
from servers.presence_search_player.src.contracts.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest
from servers.presence_search_player.src.contracts.responses import CheckResponse, NicksResponse, OthersListResponse, OthersResponse, SearchResponse, SearchUniqueResponse, UniqueSearchResponse, ValidResponse
from servers.presence_search_player.src.contracts.results import CheckResult, NewUserResult, NicksResult



class CheckHandler(CmdHandlerBase):
    _request: CheckRequest
    _result: CheckResult
    _response: CheckResponse

    def __init__(self, client: Client, request: CheckRequest) -> None:
        assert isinstance(request, CheckRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = CheckResponse(self._request, self._result)


class NewUserHandler(CmdHandlerBase):
    _request: NewUserRequest
    _result: NewUserResult
    _response: NewUserResponse

    def __init__(self, client: Client, request: NewUserRequest) -> None:
        assert isinstance(request, NewUserRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NewUserResponse(self._request, self._result)
class NicksHandler(CmdHandlerBase):
    _request: NicksRequest
    _result: NicksResult
    _response: NicksResponse

    def __init__(self, client: Client, request: NicksRequest) -> None:
        assert isinstance(request, NicksRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = NicksResponse(self._request, self._result)

class OthersHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: OthersRequest) -> None:
        assert isinstance(request, OthersRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = OthersResponse(self._result)
        
class OthersListHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: OthersListRequest) -> None:
        assert isinstance(request, OthersListRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = OthersListResponse(self._result)

class SearchHandler(CmdHandlerBase):
    """
    last one we search with email this may get few profile so we can not return GPErrorCode
    SearchWithEmail(client,dict );
    \search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
    \bsrdone\more\<more>\final\
    string sendingbuffer = 
    "\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
    client.Stream.SendAsync(sendingbuffer);
    \more\<number of items>\final\
    \search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\
    """
    def __init__(self, client: Client, request: SearchRequest) -> None:
        assert isinstance(request, SearchRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SearchResponse(self._result)


class SearchUniqueHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: SearchUniqueRequest) -> None:
        assert isinstance(request, SearchUniqueRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = SearchUniqueResponse(self._result)
class UniqueSearchHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: UniqueSearchRequest) -> None:
        assert isinstance(request, UniqueSearchRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = UniqueSearchResponse(self._request, self._result)
class ValidHandler(CmdHandlerBase):
    def __init__(self, client: Client, request: ValidRequest) -> None:
        assert isinstance(request, ValidRequest)
        super().__init__(client, request)

    def _response_construct(self):
        self._response = ValidResponse(self._request, self._result)
