
from typing import final
from frontends.gamespy.protocols.presence_search_player.contracts.requests import CheckRequest, NewUserRequest, NicksRequest, OthersListRequest, OthersRequest, SearchRequest, SearchUniqueRequest, UniqueSearchRequest, ValidRequest

from frontends.gamespy.protocols.presence_search_player.contracts.responses import CheckResponse, NewUserResponse, NicksResponse, OthersListResponse, OthersResponse, SearchResponse, SearchUniqueResponse, UniqueSearchResponse, ValidResponse
from frontends.gamespy.protocols.presence_search_player.contracts.results import CheckResult, NewUserResult, NicksResult, OthersListResult, OthersResult, SearchResult, SearchUniqueResult, UniqueSearchResult, ValidResult

from frontends.gamespy.protocols.presence_search_player.abstractions.handler import CmdHandlerBase
from frontends.gamespy.protocols.presence_search_player.applications.client import Client


@final
class CheckHandler(CmdHandlerBase):
    _request: CheckRequest
    _result: CheckResult
    _response: CheckResponse

    def __init__(self, client: Client, request: CheckRequest) -> None:
        assert isinstance(request, CheckRequest)
        super().__init__(client, request)


@final
class NewUserHandler(CmdHandlerBase):
    _request: NewUserRequest
    _result: NewUserResult
    _response: NewUserResponse

    def __init__(self, client: Client, request: NewUserRequest) -> None:
        assert isinstance(request, NewUserRequest)
        super().__init__(client, request)


@final
class NicksHandler(CmdHandlerBase):
    _request: NicksRequest
    _result: NicksResult
    _response: NicksResponse

    def __init__(self, client: Client, request: NicksRequest) -> None:
        assert isinstance(request, NicksRequest)
        super().__init__(client, request)


@final
class OthersHandler(CmdHandlerBase):
    _request: OthersRequest
    _result: OthersResult
    _response: OthersResponse

    def __init__(self, client: Client, request: OthersRequest) -> None:
        assert isinstance(request, OthersRequest)
        super().__init__(client, request)


@final
class OthersListHandler(CmdHandlerBase):
    _result: OthersListResult
    _response: OthersListResponse

    def __init__(self, client: Client, request: OthersListRequest) -> None:
        assert isinstance(request, OthersListRequest)
        super().__init__(client, request)


@final
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
    _result: SearchResult
    _response: SearchResponse

    def __init__(self, client: Client, request: SearchRequest) -> None:
        assert isinstance(request, SearchRequest)
        super().__init__(client, request)


@final
class SearchUniqueHandler(CmdHandlerBase):
    _result: SearchUniqueResult
    _response: SearchUniqueResponse

    def __init__(self, client: Client, request: SearchUniqueRequest) -> None:
        assert isinstance(request, SearchUniqueRequest)
        super().__init__(client, request)


@final
class UniqueSearchHandler(CmdHandlerBase):
    _request: UniqueSearchRequest
    _result: UniqueSearchResult
    _response: UniqueSearchResponse

    def __init__(self, client: Client, request: UniqueSearchRequest) -> None:
        assert isinstance(request, UniqueSearchRequest)
        super().__init__(client, request)


@final
class ValidHandler(CmdHandlerBase):
    _result: ValidResult
    _request: ValidRequest
    _response: ValidResponse

    def __init__(self, client: Client, request: ValidRequest) -> None:
        assert isinstance(request, ValidRequest)
        super().__init__(client, request)
