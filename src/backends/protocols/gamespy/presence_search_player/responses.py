from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.chat.contracts.results import NickResult
from frontends.gamespy.protocols.presence_search_player.contracts.results import CheckResult, NewUserResult, OthersListResult, OthersResult, SearchResult, SearchUniqueResult, UniqueSearchResult, ValidResult


class CheckResponse(DataResponse):
    result: CheckResult


class NewUserResponse(DataResponse):
    result: NewUserResult


class NickResponse(DataResponse):
    result: NickResult


class OthersResponse(DataResponse):
    result: OthersResult


class OthersListResponse(DataResponse):
    result: OthersListResult


class SearchResponse(DataResponse):
    result: SearchResult


class SearchUniqueResponse(DataResponse):
    result: SearchUniqueResult


class ValidResponse(DataResponse):
    result: ValidResult

class UniqueSearchResponse(DataResponse):
    result:UniqueSearchResult