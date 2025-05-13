from frontends.gamespy.protocols.presence_search_player.abstractions.contracts import (
    ResponseBase,
)
from frontends.gamespy.protocols.presence_search_player.contracts.requests import (
    CheckRequest,
    NewUserRequest,
    NicksRequest,
    UniqueSearchRequest,
    ValidRequest,
)
from frontends.gamespy.protocols.presence_search_player.contracts.results import (
    CheckResult,
    NewUserResult,
    NicksResult,
    OthersListResult,
    OthersResult,
    SearchResult,
    SearchUniqueResult,
    UniqueSearchResult,
    ValidResult,
)


class CheckResponse(ResponseBase):
    _result: CheckResult

    def __init__(self, request: CheckRequest, result: CheckResult) -> None:
        assert isinstance(request, CheckRequest)
        assert isinstance(result, CheckResult)
        super().__init__(request, result)

    def build(self):
        if self._result.profile_id is None:
            self.sending_buffer = "\\cur\\1\\final\\"
        else:
            self.sending_buffer = f"\\cur\\0\\pid\\{self._result.profile_id}\\final\\"


class NewUserResponse(ResponseBase):
    _result: NewUserResult
    _request: NewUserRequest

    def __init__(self, request: NewUserRequest, result: NewUserResult) -> None:
        assert isinstance(request, NewUserRequest)
        assert isinstance(result, NewUserResult)
        super().__init__(request, result)

    def build(self):
        self.sending_buffer = f"\\nur\\\\pid\\{self._result.profile_id}\\final\\"


class NicksResponse(ResponseBase):
    _request: NicksRequest
    _result: NicksResult

    def __init__(self, request: NicksRequest, result: NicksResult) -> None:
        assert isinstance(request, NicksRequest)
        assert isinstance(result, NicksResult)

        super().__init__(request, result)

    def build(self):
        self.sending_buffer = "\\nr\\"
        for info in self._result.data:
            self.sending_buffer += f"\\nick\\{info.nick}"
            if self._request.is_require_uniquenicks:
                self.sending_buffer += f"\\uniquenick\\{info.uniquenick}"
        self.sending_buffer += "\\ndone\\final\\"


class OthersListResponse(ResponseBase):
    _result: OthersListResult

    def __init__(self, result: OthersListResult) -> None:
        assert isinstance(result, OthersListResult)
        self._result = result

    def build(self):
        self.sending_buffer = "\\otherslist\\"
        for info in self._result.data:
            self.sending_buffer += f"\\o\\{info.profile_id}"
            self.sending_buffer += f"\\uniquenick\\{info.unique_nick}"
        self.sending_buffer += "oldone"


class OthersResponse(ResponseBase):
    _result: OthersResult

    def __init__(self, result: OthersResult) -> None:
        assert isinstance(result, OthersResult)
        self._result = result

    def build(self):
        self.sending_buffer = "\\others\\"
        for info in self._result.data:
            self.sending_buffer += f"\\o\\{info.profile_id}"
            self.sending_buffer += f"\\nick\\{info.nick}"
            self.sending_buffer += f"\\uniquenick\\{info.uniquenick}"
            self.sending_buffer += f"\\first\\{info.firstname}"
            self.sending_buffer += f"\\last\\{info.lastname}"
            self.sending_buffer += f"\\email\\{info.email}"
        self.sending_buffer += "\\odone\\final\\"


class SearchResponse(ResponseBase):
    _result: SearchResult

    def __init__(self, result: SearchResult) -> None:
        assert isinstance(result, SearchResult)
        self._result = result

    def build(self):
        self.sending_buffer = "\\bsr\\"
        for info in self._result.data:
            self.sending_buffer += str(info.profile_id)
            self.sending_buffer += f"\\nick\\{info.nick}"
            self.sending_buffer += f"\\uniquenick\\{info.uniquenick}"
            self.sending_buffer += f"\\namespaceid\\{info.namespace_id}"
            self.sending_buffer += f"\\firstname\\{info.firstname}"
            self.sending_buffer += f"\\lastname\\{info.lastname}"
            self.sending_buffer += f"\\email\\{info.email}"
        self.sending_buffer += "\\bsrdone\\\\more\\0\\final\\"


class SearchUniqueResponse(ResponseBase):
    _result: SearchUniqueResult

    def __init__(self, result: SearchUniqueResult) -> None:
        assert isinstance(result, SearchUniqueResult)
        self._result = result

    def build(self):
        self.sending_buffer = "\\bsr\\"
        for info in self._result.data:
            self.sending_buffer += str(info.profile_id)
            self.sending_buffer += f"\\nick\\{info.nick}"
            self.sending_buffer += f"\\uniquenick\\{info.uniquenick}"
            self.sending_buffer += f"\\namespaceid\\{info.namespace_id}"
            self.sending_buffer += f"\\firstname\\{info.firstname}"
            self.sending_buffer += f"\\lastname\\{info.lastname}"
            self.sending_buffer += f"\\email\\{info.email}"
        self.sending_buffer += "\\bsrdone\\\\more\\0\\final\\"


class ValidResponse(ResponseBase):
    _request: ValidRequest
    _result: ValidResult

    def __init__(self, request: ValidRequest, result: ValidResult) -> None:
        assert isinstance(request, ValidRequest)
        assert isinstance(result, ValidResult)
        super().__init__(request, result)

    def build(self):
        if self._result.is_account_valid:
            self.sendingbuffer = "\\vr\\1\\final\\"
        else:
            self.sendingbuffer = "\\vr\\0\\final\\"


class UniqueSearchResponse(ResponseBase):
    _request: UniqueSearchRequest
    _result: UniqueSearchResult

    def __init__(
        self, request: UniqueSearchRequest, result: UniqueSearchResult
    ) -> None:
        assert isinstance(request, UniqueSearchRequest)
        assert isinstance(result, UniqueSearchResult)
        super().__init__(request, result)

    def build(self):
        if self._result.is_uniquenick_exist:
            self.sending_buffer = "\\us\\1\\nick\\Choose another name\\usdone\\final\\"
        else:
            self.sending_buffer = (
                f"\\us\\1\\nick\\{self._request.preferred_nick}\\usdone\\final\\"
            )
